using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using System;
using System.Collections.Generic;

namespace FX.Core.Models.Components.Events
{
    [SitecoreType(TemplateId = Templates.EventFeaturedSpeakerItem.Id, AutoMap = true)]
    public interface IEventFeaturedSpeakerItem : ISitecoreItem
    {
        [SitecoreField("Speaker Name")]
        string SpeakerName { get; set; }

        [SitecoreField("Designation")]
        string Designation { get; set; }

        [SitecoreField("Company")]
        string Company { get; set; }

        [SitecoreField("Summary")]
        string Summary { get; set; }

        [SitecoreField("Speaker Image")]
        Image SpeakerImage { get; set; }
    }
}

