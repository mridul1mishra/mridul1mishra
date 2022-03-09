using System;
using System.Linq;
using System.Text;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Data.Fields;
using Sitecore.Data.Templates;
using Sitecore.Search.Crawlers.FieldCrawlers;
using System.Collections.Generic;

namespace FX.Core.Search.ComputedFields
{
    public class ComponentContentSearch : AbstractComputedIndexField
    {
        private List<string> _textFieldTypes = new List<string>(new string[]
        {
            "Rich Text",
            "Multi-Line Text",
            "rich text",
            "html"
        });

        public override object ComputeFieldValue(IIndexable indexable)
        {
            Assert.ArgumentNotNull(indexable, "indexable");
            try
            {
                Item item = indexable as SitecoreIndexableItem;

                // This field only works for items uder /sitecore/content that have a layout
                if (item == null
                    || item.Visualization.Layout == null
                    || !item.Paths.FullPath.StartsWith(
                            Sitecore.Constants.ContentPath,
                            StringComparison.InvariantCultureIgnoreCase))
                {
                    return null;
                }

                var section = item.Database.SelectItems(string.Format("{0}/sections", item.Paths.Path));
                if (section.Count() < 1)
                    return null;

                var result = new StringBuilder();
                result.Append(item["MainTitle"]);
                // Get all renderings in sectionPlaceholder
                var references = item.Visualization.GetRenderings(Sitecore.Data.Items.DeviceItem.ResolveDevice(item.Database), false)
                    .Where(x => x.Placeholder.Contains("sectionPlaceholder"));
                foreach (var reference in references)
                {
                    // Get the source item
                    if (reference.RenderingItem != null && !string.IsNullOrEmpty(reference.Settings.DataSource))
                    {
                        var source = item.Database.GetItem(reference.Settings.DataSource);
                        if (source != null)
                        {
                            // Go through all fields
                            foreach (Field field in source.Fields)
                            {
                                result.Append(GetFieldValue(field));
                                result.Append(" ");
                            }
                        }
                    }
                }

                string content = result.ToString();
                return content;
            }
            catch (Exception exc)
            {
                Log.Error(string.Format("An error occurred when indexing {0}: {1}", indexable.Id, exc.Message), exc, this);
            }
            return null;
        }

        private string GetFieldValue(Field field)
        {
            if (IsTextField(field))
            {
                FieldCrawlerBase fieldCrawler = FieldCrawlerFactory.GetFieldCrawler(field);
                if (fieldCrawler != null)
                {
                    return fieldCrawler.GetValue();
                }
            }

            return string.Empty;
        }

        protected virtual bool IsTextField(Field field)
        {
            Assert.ArgumentNotNull(field, "field");
            if (!this._textFieldTypes.Contains(field.Type))
            {
                return false;
            }
            TemplateField templateField = field.GetTemplateField();
            return templateField == null || !templateField.ExcludeFromTextSearch;
        }
    }
}
