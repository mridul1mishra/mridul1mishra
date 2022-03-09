using Sitecore.WFFM.Abstractions.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Data;
using Glass.Mapper.Sc;
using FX.Core.Models.Settings;
using FX.Core.Models.Base;
using FX.Core.Models.Page;
using FX.Core.Services;
using System.Collections.Specialized;
using FX.Core.Models.SalesForce;
using Sitecore.WFFM.Actions.Base;
using System.Web;

namespace FX.Core.WFFM.SaveActions
{
    public class SalesForceSaveAction : WffmSaveAction
    {
        //another service to be inserted here for the salesforce stuff
        private SalesForceService __sfService { get; set; }
        private ISitecoreContext __scContext { get; set; }
        private ID __actionID { get; set; }
        private string __uniqueKey { get; set; }
        public SalesForceSaveAction()
        {
            __scContext = SitecoreContext.GetFromHttpContext();
        }
        public SalesForceSaveAction(ISitecoreContext scContext)
        {
            __scContext = scContext;
        }

        public override void Execute(ID formId, AdaptedResultList adaptedFields, ActionCallContext actionCallContext = null, params object[] data)
        {
            var homeItem = __scContext.GetHomeItem<IHomePage>();
            var settings = homeItem.SiteSettings;
            var productName = HttpContext.Current.Request.Params[0].ToString();
            NameValueCollection nvc = new NameValueCollection();
            //extract all the defined mapping values from the form to be sent to salesforce
            nvc = settings.FieldMappings.AllKeys.Aggregate(nvc, (n, s) =>
            {
                n = settings.FieldMappings[s].Split('|').Aggregate(n, (nn, ss) =>
                {
                    if (adaptedFields.GetEntryByName(ss) != null)
                            nn.Add(s, adaptedFields.GetEntryByName(ss).Value);
                    return nn;
                });
                return n;
            });
            //extract all the predefined values to be sent
            nvc = settings.PredefinedFieldValues.AllKeys.Aggregate(nvc, (n, s) =>
            {
                n.Add(s, settings.PredefinedFieldValues[s]);
                return n;
            });
            //extract the http cookies based on the defined mappings in site settings
            nvc = settings.CookieFields.AllKeys.Aggregate(nvc, (n, s) =>
            {
                
                if (System.Web.HttpContext.Current != null
                    && System.Web.HttpContext.Current.Request.Cookies != null
                    && System.Web.HttpContext.Current.Request.Cookies[settings.CookieFields[s]] != null
                    && s == "LeadSource")
                    n.Add(s, System.Web.HttpContext.Current.Request.Cookies[settings.CookieFields[s]].Value.Split('=').Last().Trim('"'));
                if (System.Web.HttpContext.Current != null
                    && HttpContext.Current.Request.Params["product name"]?.ToString() != null
                    && s == "00N9000000FDrwg")
                    n.Add(s, HttpContext.Current.Request.Params["product name"].ToString());
                if (System.Web.HttpContext.Current != null
                    && System.Web.HttpContext.Current.Request.Cookies != null
                    && System.Web.HttpContext.Current.Request.Cookies[settings.CookieFields[s]] != null
                    && s == "Campaign_ID"
                    && string.IsNullOrEmpty(n["Campaign_ID"]))
                    n.Add(s, System.Web.HttpContext.Current.Request.Cookies[settings.CookieFields[s]].Value.Split('&').First().Split('=').Last().Trim('"'));
                return n;
            });
            __sfService = new SalesForceService(new System.Net.Http.HttpClient(), settings.SalesForceEndPoint);
            var entry = new SalesForceEntry(__sfService, nvc);

            bool result = false;

            var retries = 3;
            while (!result && retries >= 0)
            {
                retries -= 1;
                result = __sfService.SaveEntry(entry);
                if (result)
                    Sitecore.Diagnostics.Log.Info("Sales Force Successfully Sent", this);
                else
                    Sitecore.Diagnostics.Log.Info("Sales Force Unsuccessfull", this);
            }
        }
    }
}
