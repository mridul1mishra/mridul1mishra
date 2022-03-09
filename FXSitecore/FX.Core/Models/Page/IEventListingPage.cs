using FX.Core.Models.Base;
using FX.Core.Models.Settings;
using FX.Core.Models.Settings.Events;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace FX.Core.Models.Page
{
    public interface IEventListingPage : IBanner, IPage
    {
        [SitecoreField("Show Banner")]
        bool ShowBannerCheckbox { get; set; }

        [SitecoreField("Location Filter Label")]
        string LocationFilterLabel { get; set; }
        
        [SitecoreField("Industry Filter Label")]
        string IndustryFilterLabel { get; set; }

        [SitecoreField("Year Filter Label")]
        string YearFilterLabel { get; set; }

        [SitecoreField("Month Filter Label")]
        string MonthFilterLabel { get; set; }

        [SitecoreField("View Details Label")]
        string ViewDetailsLabel { get; set; }

        [SitecoreField("Reset Button Label")]
        string ResetButtonLabel { get; set; }

        [SitecoreField("Load More Label")]
        string LoadMoreLabel { get; set; }

        [SitecoreField("Zero Results Text")]
        string ZeroResultsText { get; set; }

        [SitecoreQuery("./ancestor::*/Content Stores/*[@@templateid='" + Templates.EventsFilterFolder.Id + "']", IsRelative = true, InferType = true)]
        IEventsFilterFolder EventsFilterFolder { get; set; }
    }
}
