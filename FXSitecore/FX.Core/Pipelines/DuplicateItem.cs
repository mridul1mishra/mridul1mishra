using Sitecore;
using Sitecore.Buckets.Extensions;
using Sitecore.Buckets.Managers;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Events;
using Sitecore.Data.Items;
using Sitecore.Data.LanguageFallback;
using Sitecore.Data.Managers;
using Sitecore.Diagnostics;
using Sitecore.Events;
using Sitecore.Globalization;
using Sitecore.Web.UI.Sheer;
using System;

namespace FX.Core.Pipelines
{
    public class DuplicateItem : Sitecore.Buckets.Pipelines.UI.ItemDuplicate
    {
        private Item _itemToDuplicate;

        public new void Execute(ClientPipelineArgs args)
        {

            Sitecore.Diagnostics.Log.Info("duplicate started", this);
            Item copy = Duplicate(args);
            if (copy == null)
                return;

            if (_itemToDuplicate == null)
                return;

            Sitecore.Diagnostics.Log.Info("replacement started", this);
            new ReferenceReplacementJob(_itemToDuplicate, copy).StartAsync();

            
            //args.AbortPipeline();
        }

        private Item Duplicate(ClientPipelineArgs args)
        {
            using (new LanguageFallbackItemSwitcher(new bool?(false)))
            {
                Assert.ArgumentNotNull(args, "args");
                Database database = Factory.GetDatabase(args.Parameters["database"]);
                Assert.IsNotNull(database, args.Parameters["database"]);
                string itemPath = args.Parameters["id"];
                Item item = database.Items[itemPath];
                _itemToDuplicate = item;

                if (item == null)
                {
                    SheerResponse.Alert(Translate.Text("Item not found."), new string[0]);
                    args.AbortPipeline();
                }
                else
                {
                    Item parent = item.Parent;
                    if (parent == null)
                    {
                        SheerResponse.Alert(Translate.Text("Cannot duplicate the root item."), new string[0]);
                        args.AbortPipeline();
                    }
                    else
                    {
                        if (parent.Access.CanCreate())
                        {
                            Log.Audit(this, "Duplicate item: {0}", new string[]
                            {
                            AuditFormatter.FormatItem(item)
                            });
                            Item parentBucketItemOrSiteRoot = item.GetParentBucketItemOrSiteRoot();
                            if (BucketManager.IsBucket(parentBucketItemOrSiteRoot) && BucketManager.IsBucketable(item))
                            {
                                if (!EventDisabler.IsActive)
                                {
                                    EventResult eventResult = Event.RaiseEvent("item:bucketing:duplicating", new object[]
                                    {
                                    args,
                                    this
                                    });
                                    if (eventResult != null && eventResult.Cancel)
                                    {
                                        Log.Info(string.Format("Event {0} was cancelled", "item:bucketing:duplicating"), this);
                                        args.AbortPipeline();
                                        return null;
                                    }
                                }
                                Item item2 = Context.Workflow.DuplicateItem(item, args.Parameters["name"]);
                                Item destination = BucketManager.Provider.CreateAndReturnBucketFolderDestination(parentBucketItemOrSiteRoot, DateUtil.ToUniversalTime(DateTime.Now), item);
                                if (!IsBucketTemplateCheck(item))
                                {
                                    destination = parentBucketItemOrSiteRoot;
                                }
                                ItemManager.MoveItem(item2, destination);
                                if (!EventDisabler.IsActive)
                                {
                                    Event.RaiseEvent("item:bucketing:duplicated", new object[]
                                    {
                                    args,
                                    this
                                    });
                                }
                            }
                            else
                            {
                                return Context.Workflow.DuplicateItem(item, args.Parameters["name"]);
                            }
                        }
                        else
                        {
                            SheerResponse.Alert(Translate.Text("You do not have permission to duplicate \"{0}\".", new object[]
                            {
                            item.DisplayName
                            }), new string[0]);
                            args.AbortPipeline();
                            return null;
                        }
                    }
                }
                args.AbortPipeline();
                return null;
            }
        }

        private Item GetItemToDuplicate(ClientPipelineArgs args)
        {
            Language language;
            Database database = Factory.GetDatabase(args.Parameters["database"]);
            Assert.IsNotNull(database, args.Parameters["database"]);
            string str = args.Parameters["id"];
            if (!Language.TryParse(args.Parameters["language"], out language))
            {
                language = Context.Language;
            }
            return database.GetItem(ID.Parse(str), language);
        }

        internal static bool IsBucketTemplateCheck(Item item)
        {
            if (item != null)
            {
                if (item.Fields[Sitecore.Buckets.Util.Constants.IsBucket] != null)
                {
                    return item.Fields[Sitecore.Buckets.Util.Constants.BucketableField].Value.Equals("1");
                }
                if (item.Paths.FullPath.StartsWith("/sitecore/templates"))
                {
                    TemplateItem templateItem = (item.Children[0] != null) ? item.Children[0].Template : null;
                    if (templateItem != null)
                    {
                        TemplateItem templateItem2 = new TemplateItem(templateItem);
                        if (templateItem.StandardValues != null && templateItem2.StandardValues[Sitecore.Buckets.Util.Constants.BucketableField] != null)
                        {
                            return templateItem2.StandardValues[Sitecore.Buckets.Util.Constants.BucketableField].Equals("1");
                        }
                    }
                }
            }
            return false;
        }
    }
}