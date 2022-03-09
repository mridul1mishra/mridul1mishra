using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using System;
using System.Collections.Generic;

namespace FX.Core.Models.Components.Events
{
    [SitecoreType(TemplateId = Templates.EventAgendaFolder.Id, AutoMap = true)]
    public interface IEventAgendaFolder : ISitecoreItem
    {
        [SitecoreField("Event Agenda Tab Label")]
        string EventAgendaTabLabel { get; set; }

        [SitecoreQuery("./*[@@templateId = '" + Templates.EventAgendaItem.Id + "']", IsRelative = true, InferType = true)]
        IEnumerable<IEventAgendaItem> IEventAgendaItems { get; set; }
    }
}

