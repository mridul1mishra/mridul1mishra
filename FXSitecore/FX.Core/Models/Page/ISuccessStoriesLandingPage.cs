using FX.Core.Models.Base;
using FX.Core.Models.Settings.SuccessStories;
using Glass.Mapper.Sc.Configuration.Attributes;
using System.Collections.Generic;

namespace FX.Core.Models.Page
{
    public interface ISuccessStoriesLandingPage : IBanner, IPage
    {
        [SitecoreField("Show Banner")]
        bool ShowBannerCheckbox { get; set; }

        [SitecoreField("Introduction Text")]
        string IntroductionText { get; set; }

        [SitecoreField("Filter Reset Label")]
        string FilterResetLabel { get; set; }

        [SitecoreField("Country Filter Label")]
        string CountryFilterLabel { get; set; }
        
        [SitecoreField("Solution Filter Label")]
        string SolutionFilterLabel { get; set; }

        [SitecoreField("Product Filter Label")]
        string ProductFilterLabel { get; set; }

        [SitecoreField("Industry Filter Label")]
        string IndustryFilterLabel { get; set; }

        [SitecoreField("Year Filter Label")]
        string YearFilterLabel { get; set; }

        [SitecoreField("Load More Label")]
        string LoadMoreLabel { get; set; }

        [SitecoreField("Zero Results Text")]
        string ZeroResultsText { get; set; }

        [SitecoreField("Default Country Filter")]
        ISuccessStoriesFilterItem DefaultFilterItem { get; set; }

        [SitecoreQuery("./*[@@templateId = '" + Templates.SuccessStoryDetailPage.Id + "']", IsRelative = true, InferType = true)]
        IEnumerable<ISuccessStoryDetailPage> SuccessStoryDetailPages { get; set; }
                             
        [SitecoreQuery("./ancestor::*/Content Stores/*[@@templateid='"+ Templates.SuccessStoriesFilterFolder.Id + "']", IsRelative = true, InferType = true)]
        ISuccessStoriesFilterFolder SuccessStoriesFilterFolder { get; set; }
    }
}
