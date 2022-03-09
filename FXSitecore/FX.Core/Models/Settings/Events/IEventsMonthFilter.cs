using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using System.Collections.Generic;

namespace FX.Core.Models.Settings.Events
{
    [SitecoreType(TemplateId = Templates.EventsMonthFilter.Id, AutoMap = true)]
    public interface IEventsMonthFilter : ISitecoreItem
    {
        [SitecoreQuery("./*[@@templateId = '" + Templates.EventsFilterItem.Id + "']", IsRelative = true, InferType = true)]
        IEnumerable<IEventsFilterItem> EventsFilterItems { get; set; }
    }
}
