using FX.Core.GlassMapper.DataHandler;
using FX.Core.Models.Base;
using FX.Core.Models.Components;
using FX.Core.Models.Components.Notifications;
using FX.Core.Models.Page;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Sitecore.Data.Items;
using System.Collections.Generic;
using System.Collections.Specialized;
using FX.Core.Models.Footer;

namespace FX.Core.Models.Settings
{
    public interface ISiteSettings : ISitecoreItem
    {
        #region Site Settings
        string SiteName { get; set; }
        Link AllSitesLink { get; set; }
        #endregion
        
        #region Header Details
        Image CorporateLogo { get; set; }
        string CompanyName { get; set; }
        Link HeaderLink1 { get; set; }
        Link HeaderLink2 { get; set; }
        Link HeaderLink3 { get; set; }
        Link HeaderLink4 { get; set; }
        Link HeaderLink5 { get; set; }
        Link HeaderLink6 { get; set; }
        Link MainNavigationHeaderLink { get; set; }
        string MetaTitleSuffixText { get; set; }
        #endregion

        #region Social Links
        string BannerLabel { get; set; }
        Link SocialPageLink1 { get; set; }
        Link SocialPageLink2 { get; set; }
        Link SocialPageLink3 { get; set; }
        Link SocialPageLink4 { get; set; }
        Link SocialPageLink5 { get; set; }
        Image SocialPageQRCode { get;set;}
        
        Image SocialPageIcon1 { get; set; }
        Image SocialPageIcon2 { get; set; }
        Image SocialPageIcon3 { get; set; }
        Image SocialPageIcon4 { get; set; }
        Image SocialPageIcon5 { get; set; }

        #endregion

      
        #region Footer Details
        Link FooterLink1 { get; set; }
        Link FooterLink2 { get; set; }
        Link FooterLink3 { get; set; }
        Link FooterLink4 { get; set; }
        Link FooterLink5 { get; set; }
        string CopyrightText { get; set; }

        string CommonNote { get; set; }

        Image AffiliatedCompanyLogo1 { get; set; }
        Link AffiliatedCompanyLink1 { get; set; }
        Image AffiliatedCompanyLogo2 { get; set; }
        Link AffiliatedCompanyLink2 { get; set; }
        Image SNSLogo1 { get; set; }
        Link SNSLink1 { get; set; }
        Image SNSLogo2 { get; set; }
        Link SNSLink2 { get; set; }
        Image SNSLogo3 { get; set; }
        Link SNSLink3 { get; set; }
        Image SNSLogo4 { get; set; }
        Link SNSLink4 { get; set; }
        Image SNSLogo5 { get; set; }
        Link SNSLink5 { get; set; }
        Image SNSLogo6 { get; set; }
        Link SNSLink6 { get; set; }
        Link LinkToMainPages { get; set; }
        string AffiliatedCompanyTitle { get; set; }
        string LicenceLabel { get; set; }
        #endregion

        #region Footer Main Navigation
        [SitecoreField(FieldId = Constants.Footer.FooterMainNavigationId)]
        IEnumerable<Navigable> FooterMainNavigation { get; set; }
        #endregion

        #region Sidebar
        string NotificationsTitle { get; set; }
        string NoNotificationsMessage { get; set; }
        INotificationFolder NotificationsFolder { get; set; }
        string EnquiryTitle { get; set; }
        string EnquiryMessage { get; set; }
        string EnquiryRequiredFieldsText { get; set; }
        string EnquiryForm { get; set; }
        bool EnableShare { get; set; }
        string ShareTitle { get; set; }
        string ShareEmbedCode { get; set; }
        string ChatEmbedCode { get; set; }
        bool EnableChat { get; set; }
        TimeSlotValue TimeSlot { get; set; }
        string ChatOfflineMessage { get; set; }

        [SitecoreField("Notification Goal")]
        string NotificationGoal { get; set; }

        [SitecoreField("ContactUs Goal")]
        string ContactUsGoal { get; set; }

        [SitecoreField("Chat Goal")]
        string ChatGoal { get; set; }

        [SitecoreField("ShareThisPage Goal")]
        string ShareThisPageGoal { get; set; }
        #endregion

        #region Other Settings
        int NewArticleAge { get; set; }
        string RelatedPagesTitle { get; set; }

        [SitecoreField("Search Label")]
        string SearchLabel { get; set; }

        [SitecoreField("Follow Us Label")]
        string FollowUsLabel { get; set; }

        [SitecoreField("Home Breadcrumb Label")]
        string HomeBreadcrumbLabel { get; set; }

        string TagManagerSnippet { get; set; }
        ISnSPage SnSPage { get; set; }
		IGeneralLinks RelatedProductLink { get; set; }

        string AnalyticsSnippet { get; set; }
        string BackToTopLabel { get; set; }
        #endregion

        #region Languages

        IEnumerable<ILanguage> SelectedLanguages { get; set; }

        #endregion

        #region Sales Force

        NameValueCollection FieldMappings { get; set; }
        NameValueCollection PredefinedFieldValues { get; set; }
        NameValueCollection CookieFields { get; set; }

        string SalesForceEndPoint { get; set; }

        #endregion

        #region Resources Component

        string PdfLabel { get; set; }
        string DocLabel { get; set; }
        string XlsLabel { get; set; }
        string ZipLabel { get; set; }
        string FileSizeLabel { get; set; }
        string DocumentTypeLabel { get; set; }
        #endregion        
        
        #region Time Settings
        string TimeZone { get; set; }
        string DayMonthYearFormat { get; set; }
        string TimeFormat { get; set; }
        string DateRangeStartFormat { get; set; } 
        string DateRangeEndFormat { get; set; }
        #endregion

        #region Collapsible Form Fields
        string AddField { get; set; }
        string RemoveField { get; set; }
        #endregion
		
        #region AddThis Settings
        string AddThisScriptSource { get; set; }
        string AddThisHTMLSnippet { get; set; }
        bool AddThisEnabled { get; set; }
        #endregion
    }
    
}
