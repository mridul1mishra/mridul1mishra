using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.Links;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System;

namespace FX.Core.Pipelines
{
    class CustomLinkProvider : LinkProvider
    {
        public override string GetItemUrl(Sitecore.Data.Items.Item item,
         Sitecore.Links.UrlOptions options)
        {
            try
            {
                var opco = FX.Core.Utils.Util.OpcoType();
                var opcolang = FX.Core.Utils.Util.OpcoLang();
                string url = "";

                if (CreateLinkBuilder(options).GetItemUrl(item).Contains($"en/{opco}"))
                {
                    url = CreateLinkBuilder(options).GetItemUrl(item).Replace($"en/{opco}", $"{opco}/en");
                    return url;
                }
                else if (CreateLinkBuilder(options).GetItemUrl(item).Contains($"{opcolang}/{opco}"))
                {
                    url = CreateLinkBuilder(options).GetItemUrl(item).Replace($"{opcolang}/{opco}", $"{opco}/{opcolang}");
                    return url;
                }
                return base.GetItemUrl(item, options);
            }
            catch(Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("CustomLinkProvider Module" + ex.Message + "\n" + ex.StackTrace, this);
                return ex.Message;
            }
            
        }
    }
}

