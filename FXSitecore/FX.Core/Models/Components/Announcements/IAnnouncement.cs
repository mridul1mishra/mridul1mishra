using FX.Core.GlassMapper.DataHandler;
using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Core.Models.Components.Announcements
{
    [SitecoreType(
        EnforceTemplate = Glass.Mapper.Sc.Configuration.SitecoreEnforceTemplate.TemplateAndBase,
        TemplateId = Templates.Announcement.Id)]
    public interface IAnnouncement : ISitecoreItem
    {
        [SitecoreField("Announcement Title")]
        string AnnouncementTitle { get; set; }

        [SitecoreField("Announcement Start Date")]
        DateTime AnnouncementStartDate { get; set; }

        [SitecoreField("Announcement End Date")]
        DateTime AnnouncementEndDate { get; set; }

        [SitecoreField("Announcement Start Date")]
        LocalisedDateTimeValue LocalisedAnnouncementStartDate { get; set; }

        [SitecoreField("Announcement End Date")]
        LocalisedDateTimeValue LocalisedAnnouncementEndDate { get; set; }

        [SitecoreField("Announcement Link")]
        Link AnnouncementLink { get; set; }
    }
}
