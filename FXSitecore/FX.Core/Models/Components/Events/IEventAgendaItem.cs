using FX.Core.GlassMapper.DataHandler;
using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using System;

namespace FX.Core.Models.Components.Events
{
    [SitecoreType(TemplateId = Templates.EventAgendaItem.Id, AutoMap = true)]
    public interface IEventAgendaItem : ISitecoreItem
    {
        [SitecoreField("Time")]
        DateTime Time { get; set; }

        [SitecoreField("Time")]
        LocalisedDateTimeValue LocalisedTime { get; set; }

        [SitecoreField("Description")]
        string Description { get; set; }

        [SitecoreField("Synopsis")]
        string Synopsis { get; set; }
    }
}

