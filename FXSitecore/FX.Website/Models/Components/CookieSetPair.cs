using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FX.Website.Models.Components
{
    public class CookieSetPair
    {
        public string CookieKey { get; set; }
        public string CookieValue { get; set; }
        public string CookieExpirationDate { get; set; }
        public string CookieCategory { get; set; }
    }
}