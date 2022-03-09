using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FX.Core.Utils
{
    public class CaptchaService
    {
        public bool CaptchaValidate(string grecaptchaResponse)
        {

            var secretKey = System.Configuration.ConfigurationManager.AppSettings["RecaptchaSecretKey"];
            var recaptchaScore = System.Configuration.ConfigurationManager.AppSettings["recaptchaScore"];

            HttpClient httpClient = new HttpClient();
            var res = httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={grecaptchaResponse}").Result;

            if (res.StatusCode != HttpStatusCode.OK)
                return false;

            string JSONres = res.Content.ReadAsStringAsync().Result;
            dynamic JSONdata = JObject.Parse(JSONres);

            if (JSONdata.success != "true" || JSONdata.score <= recaptchaScore)
                return false;

            return true;



        }

        public string captchaMessage(string token)
        {
            var isCaptchaValid = CaptchaValidate(token);
            var captchaitem = Sitecore.Context.Database.GetItem(Constants.CaptchaConstant.recaptchaItemId);
            if (isCaptchaValid)
            {
                var successmsg = captchaitem != null ? captchaitem.Fields["Success"].Value : string.Empty;
                return string.IsNullOrEmpty(successmsg) ? string.Empty : successmsg;
            }
            else
            {
                var Failmsg = captchaitem != null ? captchaitem.Fields["Failure"].Value : string.Empty;
                return string.IsNullOrEmpty(Failmsg) ? string.Empty : Failmsg;
            }
        }

    }
}
