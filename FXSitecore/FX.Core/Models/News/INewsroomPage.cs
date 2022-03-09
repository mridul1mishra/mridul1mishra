using FX.Core.GlassMapper.DataHandler;
using FX.Core.Models.Base;
using FX.Core.Models.Page;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using System;
using System.Collections.Generic;

namespace FX.Core.Models.News
{
    public interface INewsroomPage : IPage
    {
        #region Content
        IEnumerable<IStandardPage> FeaturedNews { get; set; }
        #endregion

        #region Featured Story
        string StoryTitle { get; set; }
        string StoryDescription { get; set; }
        Link StoryLink { get; set; }
        Image StoryBackgroundImage { get; set; }
        #endregion

        #region Featured Video
        string VideoTitle { get; set; }
        string VideoDescription { get; set; }
        DateTime VideoDate { get; set; }
        [SitecoreField("VideoDate")]
        LocalisedDateTimeValue LocalisedVideoDate { get; set; }
        Image VideoThumbnail { get; set; }
        string YoutubeId { get; set; }
        string VideoUrl { get; set; }
        bool ShowFeaturedVideo { get; set; }
        #endregion

        #region Featured Resource
        string ResourceTitle { get; set; }
        string ResourceDescription { get; set; }
        DateTime ResourceDate { get; set; }
        [SitecoreField("ResourceDate")]
        LocalisedDateTimeValue LocalisedResourceDate { get; set; }
        Image ResourceThumbnail { get; set; }
        IMediaFile ResourceLink { get; set; }
        bool ShowFeaturedResource { get; set; }
        #endregion

        #region Filter Settings
        string FilterFromYearLabel { get; set; }
        int FilterFromYear { get; set; }
        INewsCategory FilterSource1 { get; set; }
        INewsCategory FilterSource2 { get; set; }
        string LoadMoreLabel { get; set; }

        [SitecoreField("No More News Label")]
        string NoMoreNewsLabel { get; set; }

        [SitecoreField("Left Column Label")]
        string LeftColumnLabel { get; set; }

        [SitecoreField("Right Column Label")]
        string RightColumnLabel { get; set; }
        
        #endregion
    }
}
