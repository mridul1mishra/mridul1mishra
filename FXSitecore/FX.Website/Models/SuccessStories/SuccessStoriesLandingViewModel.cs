using FX.Core.Models.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FX.Website.Models.SuccessStories
{
    public class SuccessStoriesFilterItem
    {
        public string Title { get; set; }
        public string Value { get; set; }
        public string SelectedAttribute { get; set; }
    }
    public class SuccessStoriesFilter
    {
        public string Title { get; set; }
        public string Name { get; set; }
        public IEnumerable<SuccessStoriesFilterItem> Items { get; set; }
    }
    public class SuccessStoriesLandingViewModel
    {
        public ISuccessStoriesLandingPage SitecoreModel { get; set; }
        public SuccessStoriesFilter CountryFilter { get; set; }
        public SuccessStoriesFilter SolutionFilter { get; set; }
        public SuccessStoriesFilter ProductFilter { get; set; }
        public SuccessStoriesFilter IndustryFilter { get; set; }
        public SuccessStoriesFilter YearFilter { get; set; }
        public bool ShowBannerCheckBox { get; set; }
    }
}