using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using System.Collections.Generic;

namespace FX.Core.Models.Components
{
    public interface ICategoryBanner : ISitecoreItem
    {
        string BannerTitle { get; set; }
        string BannerDescription { get; set; }
        Link BannerLink { get; set; }

        Image ImageDesktop1 { get; set; }

        Image ImageDesktop2 { get; set; }

        Image ImageDesktop3 { get; set; }

        Image ImageDesktop4 { get; set; }

        Image ImageDesktop5 { get; set; }
        Image ImageMobile1 { get; set; }

        Image ImageMobile2 { get; set; }

        Image ImageMobile3 { get; set; }

        Image ImageMobile4 { get; set; }

        Image ImageMobile5 { get; set; }
    }
}
