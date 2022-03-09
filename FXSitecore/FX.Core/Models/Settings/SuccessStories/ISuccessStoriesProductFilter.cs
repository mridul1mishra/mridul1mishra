using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using System.Collections.Generic;

namespace FX.Core.Models.Settings.SuccessStories
{
    [SitecoreType(TemplateId = Templates.SuccessStoriesProductFilter.Id, AutoMap = true)]
    public interface ISuccessStoriesProductFilter : ISitecoreItem
    {
        [SitecoreQuery("./*[@@templateId = '" + Templates.SuccessStoriesFilterItem.Id + "']", IsRelative = true, InferType = true)]
        IEnumerable<ISuccessStoriesFilterItem> SuccessStoriesFilterItems { get; set; }
    }
}
