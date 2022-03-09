using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using System.Collections.Generic;

namespace FX.Core.Models.Components.Events
{
    [SitecoreType(TemplateId = Templates.EventSponsorFolder.Id, AutoMap = true)]
    public interface IEventSponsorFolder : ISitecoreItem
    {
        [SitecoreField("Sponsor Category Title")]
        string SponsorCategoryTitle { get; set; }

        [SitecoreQuery("./*[@@templateId = '" + Templates.EventSponsorItem.Id + "']", IsRelative = true, InferType = true)]
        IEnumerable<IEventSponsorItem> SponsorItems { get; set; }
    }
}

