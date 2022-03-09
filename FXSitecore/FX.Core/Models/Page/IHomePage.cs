using FX.Core.Models.Base;
using FX.Core.Models.Components.Announcements;
using FX.Core.Models.Settings;
using Glass.Mapper.Sc.Configuration.Attributes;
using System.Collections.Generic;

namespace FX.Core.Models.Page
{
	public interface IHomePage : IPage
	{
        ISiteSettings SiteSettings { get; set; }

        [SitecoreQuery("./Content Stores/*/*[@@templateid='" + Templates.AnnouncementFolder.Id + "']", IsRelative = true)]
        IEnumerable<IAnnouncementFolder> AnnouncementFolder { get; set; }

        [SitecoreQuery(".//*[@@templateid='" + Templates.NewResellerLocator.Id + "']", IsRelative = true, IsLazy = false)]
        IEnumerable<INewResellerLocator> NewResellerLocator { get; set; }
	}
}
