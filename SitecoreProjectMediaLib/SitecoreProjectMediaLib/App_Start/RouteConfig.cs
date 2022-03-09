using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SitecoreProjectMediaLib
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(name: "DetachMedia", url: "MediaLibController/DetachMedia",
            defaults: new { controller = "MediaLibController", action = "DetachMedia", id = UrlParameter.Optional });
        }
    }
}

