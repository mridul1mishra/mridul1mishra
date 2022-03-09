using FX.Core.Models.Base;
using FX.Core.Models.Microsite.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using System.Collections.Generic;

namespace FX.Core.Models.Microsite.Page
{
    public interface IMicrositePage: IMicrositeNavigation, IMeta, ITeaser
    {
        string MainTitle { get; set; }
        string MainContent { get; set; }
        Image Banner { get; set; }
        string BannerTitle { get; set; }
        string BannerDescription { get; set; }
        [SitecoreParent]
        IMicrositePage ParentPage { get; set; }

        [SitecoreChildren(InferType = true)]
        IEnumerable<IMicrositePage> Pages { get; set; }
    }
}
