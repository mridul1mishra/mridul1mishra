using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace FX.Core.Models.Settings.Events
{
    [SitecoreType(TemplateId = Templates.EventsFilterFolder.Id, AutoMap = true)]
    public interface IEventsFilterFolder : ISitecoreItem
    {
        [SitecoreQuery("./*[@@templateId = '" + Templates.EventsLocationFilter.Id + "']", IsRelative = true, InferType = true)]
        IEventsLocationFilter EventsLocationFilter { get; set; }

        [SitecoreQuery("./*[@@templateId = '" + Templates.EventsIndustryFilter.Id + "']", IsRelative = true, InferType = true)]
        IEventsIndustryFilter EventsIndustryFilter { get; set; }

        [SitecoreQuery("./*[@@templateId = '" + Templates.EventsYearFilter.Id + "']", IsRelative = true, InferType = true)]
        IEventsYearFilter EventsYearFilter { get; set; }

        [SitecoreQuery("./*[@@templateId = '" + Templates.EventsMonthFilter.Id + "']", IsRelative = true, InferType = true)]
        IEventsMonthFilter EventsMonthFilter { get; set; }
    }
}
