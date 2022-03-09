using FX.Core.Models.Base;
using FX.Core.Models.Components;
using FX.Core.Models.Settings;
using Glass.Mapper.Sc.Configuration.Attributes;
using System.Collections.Generic;

namespace FX.Core.Models.Page
{
	public interface IStandardPage : IPage, IBanner, IArticleDate
	{
		bool IsSectionListingEnabled { get; set; }

		[SitecoreQuery("./*[@@templateId != '" + Templates.PageSectionFolder.Id + "']", IsRelative = true, InferType = true)]
		IEnumerable<IStandardPage> ArticlePages { get; set; }

		[SitecoreQuery("./Sections/*", IsRelative = true, InferType = true)]
		IEnumerable<IBasePageComponentFields> Sections { get; set; }

        IEnumerable<ITaxonomyItem> Related { get; set; }

        string ParentCategory { get; set;}
    }
}
