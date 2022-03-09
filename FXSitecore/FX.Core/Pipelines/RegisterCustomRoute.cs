using Sitecore.Pipelines;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace FX.Core.Pipelines
{
	public class RegisterCustomRoute
	{
		public virtual void Process(PipelineArgs args)
		{
                  
            Register();
		}

		public static void Register()
		{
            
            RouteTable.Routes.MapRoute("FBSGRoute",
				 "fbsg/api/{action}",
				defaults: new { controller = "Api" });
            RouteTable.Routes.MapRoute("FBAURoute",
                "fbau/api/{action}",
                defaults: new { controller = "Api" });
            RouteTable.Routes.MapRoute("FBCARoute",
                "fbca/api/{action}",
                defaults: new { controller = "Api" });
            RouteTable.Routes.MapRoute("FBCNRoute",
                "fbcn/api/{action}",
                defaults: new { controller = "Api" });
            RouteTable.Routes.MapRoute("FBHKRoute",
                "fbhk/api/{action}",
                defaults: new { controller = "Api" });
            RouteTable.Routes.MapRoute("FBKRRoute",
                "fbkr/api/{action}",
                defaults: new { controller = "Api" });
            RouteTable.Routes.MapRoute("FBMYRoute",
                "fbmy/api/{action}",
                defaults: new { controller = "Api" });
            RouteTable.Routes.MapRoute("FBMMRoute",
                "fbmm/api/{action}",
                defaults: new { controller = "Api" });
            RouteTable.Routes.MapRoute("FBNZRoute",
                "fbnz/api/{action}",
                defaults: new { controller = "Api" });
            RouteTable.Routes.MapRoute("FBPHRoute",
                "fbph/api/{action}",
                defaults: new { controller = "Api" });
            RouteTable.Routes.MapRoute("FBTWRoute",
                "fbtw/api/{action}",
                defaults: new { controller = "Api" });
            RouteTable.Routes.MapRoute("FBTHRoute",
                "fbth/api/{action}",
                defaults: new { controller = "Api" });
            RouteTable.Routes.MapRoute("FBVNRoute",
                "fbvn/api/{action}",
                defaults: new { controller = "Api" });
            RouteTable.Routes.MapRoute("FBSGFormProxyRoute",
                "fbsg/form/{action}",
                defaults: new { controller = "Form" });
            RouteTable.Routes.MapRoute("FBTHFormProxyRoute",
                "fbth/form/{action}",
                defaults: new { controller = "Form" });
            RouteTable.Routes.MapRoute("FBAUFormProxyRoute",
                "fbau/form/{action}",
                defaults: new { controller = "Form" });
            RouteTable.Routes.MapRoute("FBNZFormProxyRoute",
                "fbnz/form/{action}",
                defaults: new { controller = "Form" });
            RouteTable.Routes.MapRoute("FBPHFormProxyRoute",
                "fbph/form/{action}",
                defaults: new { controller = "Form" });
            RouteTable.Routes.MapRoute("FBMYFormProxyRoute",
                "fbmy/form/{action}",
                defaults: new { controller = "Form" });
            RouteTable.Routes.MapRoute("FBKRFormProxyRoute",
                "fbkr/form/{action}",
                defaults: new { controller = "Form" });
            RouteTable.Routes.MapRoute("FBTWFormProxyRoute",
                "fbtw/form/{action}",
                defaults: new { controller = "Form" });
            RouteTable.Routes.MapRoute("FBMMFormProxyRoute",
                "fbmm/form/{action}",
                defaults: new { controller = "Form" });
            RouteTable.Routes.MapRoute("FBCAFormProxyRoute",
                "fbca/form/{action}",
                defaults: new { controller = "Form" });
            RouteTable.Routes.MapRoute("FBHKFormProxyRoute",
                "fbhk/form/{action}",
                defaults: new { controller = "Form" });
            RouteTable.Routes.MapRoute("FBVNFormProxyRoute",
                "fbvn/form/{action}",
                defaults: new { controller = "Form" });
        }
	}
}
