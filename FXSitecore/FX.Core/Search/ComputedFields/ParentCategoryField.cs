using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;

namespace FX.Core.Search.ComputedFields
{
    public class ParentCategoryField : AbstractComputedIndexField
    {
        public List<string> AcceptedTemplates
        {
            get
            {
                return new List<string>() { FX.Core.Templates.SnSStandardPage.Id, FX.Core.Templates.StandardPage.Id, FX.Core.Templates.CareerListingPage.Id };
            }
        }
        private Item GetParent(Item item)
        {
            if (item == null)
                return null;

            if (AcceptedTemplates.Contains(item.TemplateID.ToString()))
                return item;

            return GetParent(item.Parent);
        }

        public override object ComputeFieldValue(IIndexable indexable)
        {
            string teaserTitle = string.Empty;
            try
            {
                Item item = indexable as SitecoreIndexableItem;
                if (item == null)
                {
                    return null;
                }
                var parent = GetParent(item.Parent);
                if (parent != null)
                {
                    teaserTitle = parent["TeaserTitle"];
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error($" An error occured while executing ParentCategoryField -> ComputeFieldValue() method :: {ex.ToString()}", ex, this);
            }

            return teaserTitle;
        }
    }
}
