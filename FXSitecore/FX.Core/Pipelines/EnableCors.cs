using Sitecore.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace FX.Core.Pipelines
{
    class EnableCors
    {
        public virtual void Process(PipelineArgs args)
        {
            System.Web.Http.GlobalConfiguration.Configuration.EnableCors(new EnableCorsAttribute("https://www-fbau.fujifilm.com", "*", "*"));
        }
    }
}
