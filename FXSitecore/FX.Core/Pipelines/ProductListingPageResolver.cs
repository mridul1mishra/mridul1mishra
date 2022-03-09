using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Pipelines.HttpRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Core.Pipelines
{
    public class ProductListingPageResolver : HttpRequestProcessor
    {
        public override void Process(HttpRequestArgs args)
        {
            if (Context.Item != null || Context.Database == null || args.Url.ItemPath.Length == 0)
            {
                return;
            }

            string text = MainUtil.DecodeName(args.Url.ItemPath);
            Item item = args.GetItem(text);

            if (item != null && item.Name.Equals("*"))
            {
                Context.Items.Add("starItem", item);
            }
        }
    }
}
