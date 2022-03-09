using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace FX.Core.Models.Settings.SuccessStories
{
    [SitecoreType(TemplateId = Templates.SuccessStoriesFilterFolder.Id, AutoMap = true)]
    public interface ISuccessStoriesFilterFolder : ISitecoreItem
    {
        [SitecoreQuery("./*[@@templateId = '" + Templates.SuccessStoriesCountryFilter.Id + "']", IsRelative = true, InferType = true)]
        ISuccessStoriesCountryFilter SuccessStoriesCountryFilter { get; set; }

        [SitecoreQuery("./*[@@templateId = '" + Templates.SuccessStoriesSolutionFilter.Id + "']", IsRelative = true, InferType = true)]
        ISuccessStoriesSolutionFilter SuccessStoriesSolutionFilter { get; set; }

        [SitecoreQuery("./*[@@templateId = '" + Templates.SuccessStoriesProductFilter.Id + "']", IsRelative = true, InferType = true)]
        ISuccessStoriesProductFilter SuccessStoriesProductFilter { get; set; }

        [SitecoreQuery("./*[@@templateId = '" + Templates.SuccessStoriesIndustryFilter.Id + "']", IsRelative = true, InferType = true)]
        ISuccessStoriesIndustryFilter SuccessStoriesIndustryFilter { get; set; }

        [SitecoreQuery("./*[@@templateId = '" + Templates.SuccessStoriesYearFilter.Id + "']", IsRelative = true, InferType = true)]
        ISuccessStoriesYearFilter SuccessStoriesYearFilter { get; set; }
    }
}
