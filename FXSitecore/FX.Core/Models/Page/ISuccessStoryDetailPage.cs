using FX.Core.Models.Base;
using FX.Core.Models.Settings;
using Glass.Mapper.Sc.Configuration.Attributes;
using System.Collections.Generic;

namespace FX.Core.Models.Page
{
    public interface ISuccessStoryDetailPage : IBanner, IPage, IArticleDate
    {
        [SitecoreQuery("../*[@@templateId = '" + Templates.SuccessStoryDetailPage.Id + "']", IsRelative = true, InferType = true)]
        IEnumerable<ISuccessStoryDetailPage> RelatedPages { get; set; }

        [SitecoreField("Show Banner")]
        bool ShowBannerCheckbox { get; set; }

        IEnumerable<ITaxonomyItem> Related { get; set; }
        
    }
}
