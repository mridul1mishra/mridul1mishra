using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Core.Models.Components.Announcements
{
    [SitecoreType(
        EnforceTemplate = Glass.Mapper.Sc.Configuration.SitecoreEnforceTemplate.TemplateAndBase,
        TemplateId = Templates.AnnouncementFolder.Id)]
    public interface IAnnouncementFolder : ISitecoreItem
    {
        [SitecoreField("Heading Title")]
        string HeadingTitle { get; set; }

        [SitecoreChildren]
        IEnumerable<IAnnouncement> Children { get; set; }
    }
}
