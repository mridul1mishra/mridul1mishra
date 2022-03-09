using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using FX.Core.Models.Components.Announcements;

namespace FX.Website.Models.Components
{
    public class Announcement
    {
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public string StartEndDate { get; set; }
        public string Url { get; set; }
    }

    public class AnnouncementFolder
    {
        public string HeadingTitle { get; set; }
        public IEnumerable<Announcement> Announcement { get; set; }
    }

    public class AnnouncementViewModel
    {
        public IAnnouncementFolder SitecoreModel { get; set; }
        public AnnouncementFolder AnnouncementFolder { get; set; }
        public int AnnouncementItemCount { get; set; }
    }
}