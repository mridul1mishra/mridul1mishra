using FX.Core.Models.Base;
﻿using FX.Core.GlassMapper.DataHandler;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using System;

namespace FX.Core.Models.Page
{
    public interface IEventRegistrationPage : IBanner, IPage
    {
        [SitecoreField("Show Banner")]
        bool ShowBannerCheckbox { get; set; }

        //Event Details
        [SitecoreField("Event Detail Section Title")]
        string EDSectionTitle { get; set; }

        [SitecoreField("Location")]
        MultiLineText Location { get; set; }

        [SitecoreField("Start Date")]
        DateTime StartDate { get; set; }

        [SitecoreField("Start Date")]
        LocalisedDateTimeValue LocalisedStartDate { get; set; }

        [SitecoreField("End Date")]
        DateTime EndDate { get; set; }

        [SitecoreField("End Date")]
        LocalisedDateTimeValue LocalisedEndDate { get; set; }

        [SitecoreField("Start End Time")]
        string StartEndTime { get; set; }

        [SitecoreField("Add To Calendar Label")]
        string AddToCalendarLabel { get; set; }

        [SitecoreField("View Details Label")]
        string ViewDetailsLabel { get; set; }

        [SitecoreField("View Details Link")]
        Link ViewDetailsLink { get; set; }

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
    }
}
