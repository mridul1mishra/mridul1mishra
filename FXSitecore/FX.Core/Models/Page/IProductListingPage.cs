using FX.Core.Models.Base;
using FX.Core.Models.Components;
using FX.Core.Models.Products;
using FX.Core.Models.Settings;
using Glass.Mapper.Sc.Configuration.Attributes;
using System.Collections.Generic;

namespace FX.Core.Models.Page
{
	public interface IProductListingPage : IPage, IBanner
	{
		#region Content
		IPopupForm EnquiryPage { get; set; }
		#endregion

		#region Filter Settings
		string FilterLabel { get; set; }
		string FilterResetLabel { get; set; }
		string FilterSubmitLabel { get; set; }
		string SortbyLabel { get; set; }
		string LoadMoreLabel { get; set; }
		string SearchResultsText { get; set; }
		string ZeroResultsText { get; set; }

        [SitecoreField("Latest Filter Label")]
        string LatestFilterLabel { get; set; }

        [SitecoreField("Promotion Filter Label")]
        string PromotionFilterLabel { get; set; }

        [SitecoreField("Recommended Filter Label")]
        string RecommendedFilterLabel { get; set; }

        [SitecoreField("Best Seller Filter Label")]
        string BestSellerFilterLabel { get; set; }
		#endregion

		[SitecoreQuery("./*[@@templateid='"+ Templates.PrinterCategory.Id + "' or @@templateid ='" + Templates.SupplyCategory.Id + "' or @@templateid='" + Templates.SoftwareCategory.Id + "']", IsRelative = true, InferType = true)]
		IEnumerable<IProductCategory> ProductCategories { get; set; }

        [SitecoreField("New Label")]
        string NewLabel { get; set; }

        [SitecoreField("Promoted Label")]
        string PromotedLabel { get; set; }

        [SitecoreField("Best Seller Label")]
        string BestSellerLabel { get; set; }

        [SitecoreField("Recommended Label")]
        string RecommendedLabel { get; set; }
    }
}
