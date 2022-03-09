using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using System.Collections.Generic;

namespace FX.Core.Models.Settings.SuccessStories
{
    [SitecoreType(TemplateId = Templates.SuccessStoriesCountryFilter.Id, AutoMap = true)]
    public interface ISuccessStoriesCountryFilter : ISitecoreItem
    {
        [SitecoreQuery("./*[@@templateId = '" + Templates.SuccessStoriesFilterItem.Id + "']", IsRelative = true, InferType = true)]
        IEnumerable<ISuccessStoriesFilterItem> SuccessStoriesFilterItems { get; set; }
    }
}
