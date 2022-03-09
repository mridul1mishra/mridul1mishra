using FX.Core.Models.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FX.Website.Models.Careers
{
    public class CareersFilterItem
    {
        public string Value { get; set; }
        public string ID { get; set; }
        public string Title { get; set; }
    }
    public class CareersFilter
    {
        public string Name { get; set; }
        public string ID { get; set; }
        public string Title { get; set; }
        public IEnumerable<CareersFilterItem> Items { get; set; }
        public string AllFiltersLabel { get; set; }
    }
    public class CareersListingViewModel
    {
        public ICareerListingPage SitecoreModel { get; set; }
        public string SearchResultsText { get; set; }
        public IEnumerable<CareersFilter> Filters { get; set; }
        

    }
}