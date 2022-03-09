using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Mvc.Pipelines.Response.GetXmlBasedLayoutDefinition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace FX.Website.Mvc.Pipelines
{
    public class GetFromLayoutFieldFix : GetFromLayoutField
    {
        protected override XElement GetFromField(Item item)
        {
            if (Sitecore.Context.Site.SiteInfo.EnableItemLanguageFallback
                && Sitecore.Context.Site.SiteInfo.Name.ToLower() != Sitecore.Constants.ShellSiteName
                && item.Fields[FieldIDs.FinalLayoutField] != null
                && !item.Fields[FieldIDs.FinalLayoutField].HasValue)
            {
                var item2 = item.GetFallbackItem();
                item = item2 != null ? item2 : item;
            }
            return base.GetFromField(item);
        }
    }
}