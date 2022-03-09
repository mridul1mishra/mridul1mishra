using FX.Core.Models.Base;
using FX.Core.Models.Page;
using FX.Core.Models.Settings;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using System;
using System.Collections.Generic;

namespace FX.Core.Models.Products
{
    [SitecoreType(TemplateId = Templates.BaseProductCategory.Id, AutoMap = true)]
	public interface IProductCategory : IPage, IBanner

	{
        string CategoryNavName { get; set; }
        string Link { get; set; }
        bool IsCategory { get; set; }
		#region Details
		string CategoryName { get; set; }
		IEnumerable<IProductFilter> FilterItems { get; set; }
        Guid CategorySource { get; set; }

        [SitecoreField("Category Description")]
        string CategoryDescription { get; set; }
        #endregion

        #region Advanced Filters
        string AdvFilterLabel { get; set; }
		IEnumerable<IProductFilter> AdvFilterItems { get; set; }
        #endregion

        [SitecoreQuery("./ancestor-or-self::*[@@templateid='" + Templates.ProductListingPage.Id+ "']", IsRelative = true)]
        IProductListingPage ProductListingPage { get; set; }
	}
}
