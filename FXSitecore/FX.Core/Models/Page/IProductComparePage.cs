using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using System.Collections.Generic;
using FX.Core.Models.Products;
using FX.Core.Models.Settings;
using FX.Core.Models.Components;

namespace FX.Core.Models.Page
{
    [SitecoreType(TemplateId = Templates.ProductComparePage.Id, AutoMap = true)]
    public interface IProductComparePage : ISitecoreItem
    {
        string ProductFinderTitle { get; set; }
        string BackLabel { get; set; }
        string StartOverLabel { get; set; }
        string LearnMoreLabel { get; set; }
        string DownloadBrochureLabel { get; set; }
        string ContactSalesTeamLabel { get; set; }

        IEnumerable<ISitecoreItem> FeatureFoldersSelection { get; set; }
    }

    //public interface IFeatureFolder : ISitecoreItem
    //{
    //    [SitecoreQuery("sitecore/content/Global Content Store/Products/Product Finder Printers/*[@@templateid = '{B934F044-C0AE-4E75-9F50-06DEC22C94D1}']")]
    //    IEnumerable<IFeatureType> FeatureTypes { get; set; }
    //}

}
