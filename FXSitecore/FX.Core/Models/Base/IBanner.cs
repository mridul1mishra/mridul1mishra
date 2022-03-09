using FX.Core.GlassMapper.DataHandler;
using FX.Core.Models.Components;
using FX.Core.Models.Settings;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using System.Collections.Generic;

namespace FX.Core.Models.Base
{
	public interface IBanner : ISitecoreItem
	{
		Image BannerImage { get; set; }
		string BannerTitle { get; set; }
        string BannerTitle2 { get; set; }
        MultiLineText BannerDescription { get; set; }
        [SitecoreParent]
        IBanner ParentBanner { get; set; }

        Link BannerLink { get; set; }
        Link ExtraLink1 { get; set; }
        Link ExtraLink2 { get; set; }
        Link ExtraLink3 { get; set; }

        bool LightTheme { get; set; }
        bool RightAlign { get; set; }
        bool ShowFollowUs { get; set; }
        bool NoText { get; set; }
        bool Narrow { get; set; }
        bool Clear { get; set; }
        bool Blur { get; set; }
        bool Product { get; set; }
        File Video { get; set; }

        [SitecoreField("Show Banner Blue Box")]
        bool ShowBannerBlueBox { get; set; }

        [SitecoreQuery("./*[@@templateid='" + Templates.LinkItem.Id + "']", IsRelative = true, InferType = true) ]
        IEnumerable<ILinkItem> LinkItems { get; set; }
        bool ShowBanner { get; set; }

        ISiteSettings SiteSettings { get; set; }
    }

    public static class IBannerExtension
    {
        public static bool HasMultipleLinks(this IBanner banner)
        {
            return (banner.ExtraLink1 != null || banner.ExtraLink2 != null || banner.ExtraLink3 != null);
        }
    }
}
