using FX.Core.Models.Base;
using FX.Core.Models.Components;
using FX.Core.Models.Page;
using FX.Core.Models.Settings;
using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Sitecore.Data.Items;
using System.Collections.Generic;

namespace FX.Core.Models.Products
{
	public interface IProductPage : IStandardPage
	{
		#region Content
		Image ProductImage { get; set; }
		bool IsPromotedProduct { get; set; }
		bool IsBestsellerProduct { get; set; }
		ISitecoreItem ProductForm { get; set; }
		string ProductFormLinkText { get; set; }
		Link ProductLink { get; set; }
		IEnumerable<ITaxonomyItem> ProductFeatures { get; set; }
        IEnumerable<System.Guid> Countries { get; set; }
        string CopyrightText { get; set; }
        #endregion

        #region Teaser
        string ProductDescription { get; set; }
		Image ProductThumbnail { get; set; }
		#endregion

		#region Legacy content
		string RowId { get; set; }
        #endregion


        [SitecoreChildren]
        [SitecoreQuery("sitecore/Content/Products/*[@@templateid = '{FEC48C76-98D4-411B-B2F5-6DFAA376C870}']")]
        IEnumerable<ISectionFolder> SectionFolders { get; set; }

        //[SitecoreQuery("sitecore/Content/Products//*[@@templateid = '{EE8A10F9-045C-48AE-9485-9F0B6CD771C5}']")]
        //IEnumerable<IResources> Resources { get; set; }
    }

    public interface ISectionFolder : ISitecoreItem
    {
        [SitecoreChildren]
        IEnumerable<IResources> Specifications { get; set; }
    }

    public interface ISubSections : ISitecoreItem
    {
    }

    public static class IProductPageExtension
    {
        public static IProductListingPage GetListingPage(this FX.Core.Models.Products.IProductPage productPage, ISitecoreContext context)
        {
            var homePage = FXContextItems.HomePage;
            return context.Cast<IProductListingPage>(homePage.SitecoreItem.Axes.SelectSingleItem($"./*[@@templateId='{Templates.ProductListingPage.Id}']"));
        }
    }
}
