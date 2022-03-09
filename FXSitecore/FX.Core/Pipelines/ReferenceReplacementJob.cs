using Sitecore;
using Sitecore.Configuration;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Data.Query;
using Sitecore.Diagnostics;
using Sitecore.Jobs;
using System.Collections.Generic;
using System.Linq;
namespace FX.Core.Pipelines
{
    public class ReferenceReplacementJob
    {
        private readonly Item _source;
        private readonly Item _target;
        private int TotalItems = 0;
        private int ProcessedItems = 0;


        public ReferenceReplacementJob(Item source, Item target)
        {
            Assert.ArgumentNotNull(source, "source");
            Assert.ArgumentNotNull(target, "target");
            _source = source;
            _target = target;
        }

        public void StartAsync()
        {
            string jobCategory = typeof(ReferenceReplacementJob).Name;
            string siteName = Context.Site == null ? "No Site Context" : Context.Site.Name;
            JobOptions jobOptions = new JobOptions(GetJobName(), jobCategory, siteName, this, "Start");
            JobManager.Start(jobOptions);
        }

        private string GetJobName()
        {
            return string.Format("Resolving item references between source {0} and target {1}.", AuditFormatter.FormatItem(_source), AuditFormatter.FormatItem(_target));
        }

        public void Start()
        {
            Sitecore.Diagnostics.Log.Info("replacement started", this);
            
            ItemPathTranslator translator = new ItemPathTranslator(_source, _target);
            IEnumerable<Item> sourceDescendants = GetDescendantsAndSelf(_source);
            TotalItems = sourceDescendants.Count();
            ItemReferenceReplacer replacer = InitializeReplacer(sourceDescendants, translator);
            foreach (Item equivalentTarget in replacer.OtherItems)
            {
                replacer.ReplaceItemReferences(equivalentTarget);
            }
            Sitecore.Diagnostics.Log.Info("replacement ended", this);
        }

        private IEnumerable<Item> GetDescendantsAndSelf(Item source)
        {
            List<Item> items = new List<Item>();
            var webIndex = ContentSearchManager.GetIndex("sitecore_master_index");
            using (var context = webIndex.CreateSearchContext())
            {
                var results = context.GetQueryable<SearchResultItem>()
                    .Where(i => i.Paths.Contains(source.ID) && i["_latestversion"]=="1");
                items = results.Select(x => x.GetItem()).ToList();
            }
            return items;
        }

        private ItemReferenceReplacer InitializeReplacer(IEnumerable<Item> sourceDescendants, ItemPathTranslator translator)
        {
            ItemReferenceReplacer replacer = new ItemReferenceReplacer(ExcludeStandardSitecoreFieldsExceptLayout);
            foreach (Item sourceDescendant in sourceDescendants)
            {
                if (!translator.CanTranslatePath(sourceDescendant))
                    continue;

                Item equivalentTarget = sourceDescendant.Database.GetItem(translator.TranslatePath(sourceDescendant));
                if (equivalentTarget == null)
                    continue;

                replacer.AddItemPair(sourceDescendant, equivalentTarget);
            }
            return replacer;
        }

        private bool ExcludeStandardSitecoreFieldsExceptLayout(Field field)
        {
            Assert.ArgumentNotNull(field, "field");
            return field.ID == FieldIDs.LayoutField || field.ID == FieldIDs.FinalLayoutField || !field.Name.StartsWith("__");
        }
    }
}