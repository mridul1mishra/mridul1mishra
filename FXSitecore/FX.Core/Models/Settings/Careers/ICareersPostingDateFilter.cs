using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using System.Collections.Generic;

namespace FX.Core.Models.Settings.Careers
{
    [SitecoreType(
        TemplateId = Templates.CareerTitleFilter.Id,
        AutoMap = true,
        EnforceTemplate = Glass.Mapper.Sc.Configuration.SitecoreEnforceTemplate.TemplateAndBase)]
    public interface ICareersPostingDateFilter : ICareersFilter
    {
        [SitecoreQuery("./*[@@templateId = '" + Templates.CareerDateFilterItem.Id + "']", IsRelative = true, InferType = true)]
        IEnumerable<ICareersDateFilterItem> CareersDateFilterItems { get; set; }
    }
}
