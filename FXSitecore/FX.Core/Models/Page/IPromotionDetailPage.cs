using FX.Core.GlassMapper.DataHandler;
using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using System;

namespace FX.Core.Models.Page
{
    public interface IPromotionDetailPage : IBanner, IPage, IMainContent
    {
        [SitecoreField("Show Banner")]
        bool ShowBannerCheckbox { get; set; }

        [SitecoreField("Show All Promotions Button Checkbox")]
        bool ShowAllPromotionsButtonCheckbox { get; set; }

        [SitecoreField("Promotions Start Date")]
        DateTime PromotionsStartDate{ get; set; }

        [SitecoreField("Promotions Start Date")]
        LocalisedDateTimeValue LocalisedPromotionsStartDate { get; set; }

        [SitecoreField("Promotions End Date")]
        DateTime PromotionsEndDate{ get; set; }

        [SitecoreField("Promotions End Date")]
        LocalisedDateTimeValue LocalisedPromotionsEndDate { get; set; }

        [SitecoreField("View All Promotions Label")]
        string ViewAllPromotionsLabel{ get; set; }

        [SitecoreField("Promotion Title")]
        string PromotionTitle { get; set; }

        [SitecoreField("Promotion Title Description")]
        string PromotionTitleDescription { get; set; }
    }
}
