using FX.Core.GlassMapper.DataHandler;
using FX.Core.Models.Base;
using FX.Core.Models.Settings;
using Glass.Mapper.Sc.Configuration.Attributes;
using System;
using System.Collections.Generic;

namespace FX.Core.Models.Page
{
    public interface ICareerDetailPage : IPage, IBanner
    {
        [SitecoreQuery("../*[@@templateId = '" + Templates.CareerDetailPage.Id + "']", IsRelative = true, InferType = true)]
        IEnumerable<ICareerDetailPage> RelatedPages { get; set; }

        IEnumerable<ITaxonomyItem> Related { get; set; }

        [SitecoreField("Show Banner")]
        bool ShowBannerCheckbox { get; set; }

        [SitecoreField("Job Title")]
        string JobTitle { get; set; }

        [SitecoreField("Overview Label")]
        string OverviewLabel { get; set; }

        [SitecoreField("Posting Date Label")]
        string PostingDateLabel { get; set; }

        [SitecoreField("Close Date Label")]
        string CloseDateLabel { get; set; }

        [SitecoreField("JobPostingDate")]
        DateTime JobPostingDate { get; set; }

        [SitecoreField("JobPostingDate")]
        LocalisedDateTimeValue LocalisedJobPostingDate { get; set; }

        [SitecoreField("JobClosingDate")]
        DateTime JobClosingDate { get; set; }

        [SitecoreField("JobClosingDate")]
        LocalisedDateTimeValue LocalisedJobClosingDate { get; set; }
        [SitecoreField("Career Text")]
        string CareerText { get; set; }
        [SitecoreField("Table Label")]
        string TableLabel { get; set; }
        [SitecoreField("Specialization Label")]
        string SpecializationLabel { get; set; }
        [SitecoreField("Specialization")]
        string Specialization { get; set; }
        [SitecoreField("Job Type Label")]
        string JobTypeLabel { get; set; }
        [SitecoreField("Job Type")]
        string JobType { get; set; }
        [SitecoreField("Minimum Experience Label")]
        string MinimumExperienceLabel { get; set; }
        [SitecoreField("Minimum Experience")]
        string MinimumExperience { get; set; }
        [SitecoreField("Location Label")]
        string LocationLabel { get; set; }
        [SitecoreField("CareerLocation")]
        string CareerLocation { get; set; }
        
    }
}
