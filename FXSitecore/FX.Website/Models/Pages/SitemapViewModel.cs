using FX.Core.Models.Page;
using FX.Website.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FX.Website.Models.Pages
{
    public class Section
    {
        public IEnumerable<Section> SubSections { get; set; }
        public NavigationLink SectionLink { get; set; }
        public int Ordinal { get; set; }
    }
    public class SitemapViewModel
    {
        public IStandardPage SitecoreItem { get; set; }
        public IEnumerable<Section> Sections { get; set; }
    }
}