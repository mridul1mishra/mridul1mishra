using Sitecore.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Routing;

namespace FX.Website.App_Start
{
    public class FXChinaRoutes
    {
        public void Process(PipelineArgs args)
        {
            GlobalConfiguration.Configuration.Routes.Add("fxapi", new HttpRoute("api/{controller}/{action}"));

            Type dependsOnThisTypeOfAssembly = typeof(AD.Alipay.Core);
        }
    }
}