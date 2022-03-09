using FX.Core.Models.Base;
using FX.Core.Models.Components;
using FX.Core.Models.Components.Events;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using System.Collections.Generic;
using System;
using FX.Core.GlassMapper.DataHandler;
using FX.Core.Models.Settings;
using FX.Core.Models.Settings.Events;

namespace FX.Core.Models.Page
{
    public interface IEventDetailPage : IBanner, IPage
    {
        [SitecoreQuery("./*[@@templateId = '" + Templates.EventSponsorFolder.Id + "']", IsRelative = true, InferType = true)]
        IEnumerable<IEventSponsorFolder> SponsorFolders { get; set; }

        [SitecoreField("Show Banner")]
        bool ShowBannerCheckbox { get; set; }

        [SitecoreField("View All Events Label")]
        string ViewAllEventsLabel { get; set; }

        [SitecoreField("Register Now Label")]
        string RegisterNowLabel { get; set; }

        [SitecoreField("Register Now Link")]
        Link RegisterNowLink { get; set; }

        //Event Details
        [SitecoreField("Event Detail Section Title")]
        string EDSectionTitle { get; set; }

        [SitecoreField("EventLocation")]
        MultiLineText EventLocation { get; set; }

        [SitecoreField("EventStartDate")]
        DateTime EventStartDate { get; set; }

        [SitecoreField("EventStartDate")]
        LocalisedDateTimeValue LocalisedEventStartDate { get; set; }

        [SitecoreField("EventEndDate")]
        DateTime EventEndDate { get; set; }

        [SitecoreField("EventEndDate")]
        LocalisedDateTimeValue LocalisedEventEndDate { get; set; }

        [SitecoreField("Start End Time")]
        string StartEndTime { get; set; }

        [SitecoreField("Add To Calendar Label")]
        string AddToCalendarLabel { get; set; }

        [SitecoreField("Show Event Details Checkbox")]
        bool ShowEventDetailsCheckbox { get; set; }

        //Map
        [SitecoreField("Map Section Title")]
        string MapSectionTitle { get; set; }

        [SitecoreField("Google Maps Embed Code")]
        string GoogleMapsEmbedCode { get; set; }

        [SitecoreField("Show Map Checkbox")]
        bool ShowMapCheckbox { get; set; }

        //Social Media
        [SitecoreField("Social Media Section Title")]
        string SMSectionTitle { get; set; }

        [SitecoreField("Facebook Link")]
        Link FacebookLink { get; set; }

        [SitecoreField("Instagram Link")]
        Link InstagramLink { get; set; }

        [SitecoreField("LinkedIn Link")]
        Link LinkedInLink { get; set; }

        [SitecoreField("Twitter Link")]
        Link TwitterLink { get; set; }

        [SitecoreField("Show Social Media Checkbox")]
        bool ShowSocialMediaCheckbox { get; set; }

        //Sponsors
        [SitecoreField("Sponsor Section Title")]
        string SponsorSectionTitle { get; set; }

        [SitecoreField("Show Sponsors Checkbox")]
        bool ShowSponsorsCheckbox { get; set; }
    }
}
