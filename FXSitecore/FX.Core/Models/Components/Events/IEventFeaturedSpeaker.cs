using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using System;
using System.Collections.Generic;

namespace FX.Core.Models.Components.Events
{
    [SitecoreType(TemplateId = Templates.EventFeaturedSpeaker.Id, AutoMap = true)]
    public interface IEventFeaturedSpeaker : IBasePageComponentFields
    {
        [SitecoreField("Main Title")]
        string MainTitle { get; set; }

        [SitecoreField("View Profile Label")]
        string ViewProfileLabel { get; set; }

        [SitecoreQuery("./*[@@templateId = '" + Templates.EventFeaturedSpeakerItem.Id + "']", IsRelative = true, InferType = true)]
        IEnumerable<IEventFeaturedSpeakerItem> FeaturedSpeakerItems { get; set; }
    }
}

