using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using System.Collections.Generic;

namespace FX.Core.Models.Components.Events
{
    [SitecoreType(TemplateId = Templates.EventAgendaComponent.Id, AutoMap = true)]
    public interface IEventAgenda : IBasePageComponentFields
    {
        [SitecoreField("Main Title")]
        string MainTitle { get; set; }

        [SitecoreField("Time Label")]
        string TimeLabel { get; set; }

        [SitecoreField("Session Label")]
        string SessionLabel { get; set; }

        [SitecoreField("Synopsis Label")]
        string SynopsisLabel { get; set; }

        [SitecoreField("Download Agenda Label")]
        string DownloadAgendaLabel { get; set; }

        [SitecoreField("Download Agenda Link")]
        Link DownloadAgendaLink { get; set; }

        [SitecoreQuery("./*[@@templateId = '" + Templates.EventAgendaFolder.Id + "']", IsRelative = true, InferType = true)]
        IEnumerable<IEventAgendaFolder> EventAgendaFolders { get; set; }
    }
}

