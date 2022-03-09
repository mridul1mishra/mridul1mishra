using FX.Core.Models.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FX.Website.Models.Events
{
    public class EventsFilterItem
    {
        public string Title { get; set; }
        public string Value { get; set; }
    }
    public class EventsFilter
    {
        public string Title { get; set; }
        public string Name { get; set; }
        public IEnumerable<EventsFilterItem> Items { get; set; }
    }
    public class EventsListingViewModel
    {
        public IEventListingPage SitecoreModel { get; set; }
        public EventsFilter LocationFilter { get; set; }
        public EventsFilter IndustryFilter { get; set; }
        public EventsFilter YearFilter { get; set; }
        public EventsFilter MonthFilter { get; set; }
        public bool ShowBannerCheckBox { get; set; }
    }
}