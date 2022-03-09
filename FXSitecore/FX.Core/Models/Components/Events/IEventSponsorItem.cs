using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;

namespace FX.Core.Models.Components.Events
{
    [SitecoreType(TemplateId = Templates.EventSponsorItem.Id, AutoMap = true)]
    public interface IEventSponsorItem : ISitecoreItem
    {
        [SitecoreField("Sponsor Title")]
        string SponsorTitle { get; set; }

        [SitecoreField("Sponsor Logo")]
        Image SponsorLogo { get; set; }
    }
}

