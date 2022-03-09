using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using System.Collections.Generic;

namespace FX.Core.Models.Microsite.Base
{
    public interface IMicrositeNavigation : ISitecoreItem
    {
        string NavigationTitle { get; set; }
        bool ShowInNavigation { get; set; }
        bool ShowInSitemap { get; set; }
        bool ShowInSitemapXML { get; set; }
        [SitecoreParent]
        IMicrositeNavigation Parent { get; set; }

        [SitecoreChildren(InferType = true)]
        IEnumerable<IMicrositeNavigation> Children { get; set; }
    }
}
