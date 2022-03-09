using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace FX.Core.Models.Settings.Events
{
    [SitecoreType(TemplateId = Templates.EventsFilterItem.Id, AutoMap = true)]
    public interface IEventsFilterItem : ISitecoreItem
    {
        [SitecoreField("Event Filter Item Name")]
        string EventFilterItemName { get; set; }
    }
}
