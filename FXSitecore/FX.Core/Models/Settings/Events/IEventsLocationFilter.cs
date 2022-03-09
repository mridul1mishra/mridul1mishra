using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using System.Collections.Generic;

namespace FX.Core.Models.Settings.Events
{
    [SitecoreType(TemplateId = Templates.EventsLocationFilter.Id, AutoMap = true)]
    public interface IEventsLocationFilter : ISitecoreItem
    {
        [SitecoreQuery("./*[@@templateId = '" + Templates.EventsFilterItem.Id + "']", IsRelative = true, InferType = true)]
        IEnumerable<IEventsFilterItem> EventsFilterItems { get; set; }
    }
}
