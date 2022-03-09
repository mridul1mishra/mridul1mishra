using FX.Core.Models.Base;
using FX.Core.Models.Settings;
using FX.Core.Models.Settings.Careers;
using Glass.Mapper.Sc.Configuration.Attributes;
using System.Collections.Generic;

namespace FX.Core.Models.Page
{
    public interface ICareerListingPage : IBanner, IPage
    {
        [SitecoreField("Show Banner")]
        bool ShowBannerCheckbox { get; set; }

        [SitecoreField("Introduction Text")]
        string IntroductionText { get; set; }

        [SitecoreField("Filter Label")]
        string FilterLabel { get; set; }

        [SitecoreField("Filter Reset Label")]
        string FilterResetLabel { get; set; }

        [SitecoreField("Filter Submit Label")]
        string FilterSubmitLabel { get; set; }

        [SitecoreField("Search Results Text")]
        string SearchResultsText { get; set; }

        [SitecoreField("Zero Results Text")]
        string ZeroResultsText { get; set; }

        [SitecoreField("View Job Label")]
        string ViewJobLabel { get; set; }

        [SitecoreField("Load More Label")]
        string LoadMoreLabel { get; set; }

        [SitecoreQuery("./*[@@templateId = '" + Templates.CareerDetailPage.Id + "']", IsRelative = true, InferType = true)]
        IEnumerable<ICareerDetailPage> CareerDetailPages { get; set; }

        [SitecoreQuery("./ancestor::*/Content Stores/*[@@templateid='" + Templates.CareerFilterFolder.Id + "']", IsRelative = true, InferType = true)]
        ICareersFilterFolder CareersFilterFolder { get; set; }
    }
}
