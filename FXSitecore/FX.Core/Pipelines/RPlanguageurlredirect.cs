using Sitecore.Globalization;
using Sitecore.Pipelines.HttpRequest;
using System;
using System.Web;
using System.Web.Mvc;

namespace FX.Core.Pipelines
{
    public class RPlanguageurlredirect : HttpRequestProcessor
    {
        public override void Process(HttpRequestArgs args)
        {
            try
            {
                string opco = FX.Core.Utils.Util.OpcoType();
                string opcolang = FX.Core.Utils.Util.OpcoLang().ToLower();
                string currentUrl = HttpContext.Current.Request.Url.PathAndQuery.ToLower();
                string url = string.Empty;
                Language language = null;
                string[] urlArr = currentUrl.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                if (currentUrl.Contains($"{opco}/{opcolang}") && !string.IsNullOrEmpty(opcolang))
                {
                    url = currentUrl.Replace($"{opco}/{opcolang}", $"{opcolang}/{opco}");
                    Sitecore.Context.Language = Sitecore.Globalization.Language.Parse(opcolang);
                    HttpContext.Current.Server.TransferRequest(url);
                }
                else if (currentUrl.Contains($"{opco}/en"))
                {
                    if (urlArr?.Length > 1 && Language.TryParse(urlArr[1], out language))
                    {
                        url = currentUrl.Replace($"/{opco}/en", $"/en/{opco}");
                    }

                    HttpContext.Current.Server.TransferRequest(url);
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Reverse Proxy Language Resolver Module" + ex.Message + "\n" + ex.StackTrace, this);
            }

        }
    }
}
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public sealed class ValidateJsonAntiForgeryTokenAttribute : FilterAttribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationContext filterContext)
    {
        if (filterContext.Controller.ControllerContext.RequestContext.HttpContext.Session.IsNewSession)
        {
        }
    }
}