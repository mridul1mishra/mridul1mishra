using Sitecore.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SitecoreProjectMediaLib.Pipeline
{
    public class RegisterMediaLibCustomRoutes   
    {
        public virtual void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapRoute("MediaImage",
               "api/Sitecore/MediaLibController/DetachMedia",
               new
               {
                   controller = "MediaLibController",
                   action = "DetachMedia"
               });
        }
    }
}