using FX.Core.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FX.Website.Models
{
    public class PopupForm
    {
        public Guid FormId { get; set; }
        public string ProductName { get; set; }

        public bool IsValid
        {
            get { return FormId != Guid.Empty; }
        }

        public bool IsEnquiryForm
        {
            get
            {
                Guid enquiryFormID;
                if (Guid.TryParse(Core.FXContextItems.HomePage.SiteSettings.EnquiryForm, out enquiryFormID))
                    return Guid.Equals(enquiryFormID, FormId);


                return false;
            }
        }

        public ISiteSettings SiteSettings
        {
            get
            {
                return FX.Core.FXContextItems.HomePage.SiteSettings;
            }
        }

        public bool IsCaptchaView
        {
            get
            {
                try
                {
                    var siteInfoList = Sitecore.Configuration.Factory.GetSiteInfoList();
                    var url = System.Web.HttpContext.Current.Request.Url;
                    var query = Sitecore.Configuration.Settings.Sites.Where(n => n.HostName.Contains(url.Host));
                    if (query.Count() == 1)
                    {
                        var siteName = query.First().Name;
                        var recaptchaSiteKeys = System.Configuration.ConfigurationManager.AppSettings["ShowProductCaptchaSite"];
                        string[] recaptchaSitearr = recaptchaSiteKeys?.Split(',');
                        if (recaptchaSitearr?.Length > 0)
                        {
                            if (recaptchaSitearr.Contains(siteName))
                            {
                                return true;
                            }
                        }
                    }

                    return false;
                }
                catch (Exception ex)
                {
                    Sitecore.Diagnostics.Log.Error("Error at IsCaptchaView: " + ex.Message + ex.StackTrace, this);
                    return false;
                }
            }
        }
    }
}