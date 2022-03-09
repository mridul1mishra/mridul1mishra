using Newtonsoft.Json;
using Sitecore.Forms.Mvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FX.Core.Models.Form
{
    public partial class WffmFormParams // You can choose some better class names.
    {
        [JsonProperty("strings")]
        public List<NameValuePair> strings { get; set; }

        [JsonProperty("url")]
        public string url { get; set; }
        [JsonProperty("__RequestVerificationToken")]
        public string __RequestVerificationToken { get; set; }
        [JsonProperty("language")]
        public string Language { get; set; }

    }
    public class NameValuePair
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class FormDataResponse
    {
        public string ReturnToUrl { get; set; }
        public string SummaryText { get; set; }

        public FormDataResponse()
        {
            ReturnToUrl = string.Empty;
            SummaryText = string.Empty;
        }
    }
}