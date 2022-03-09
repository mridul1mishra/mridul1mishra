using Glass.Mapper.Sc;
using FX.Core.Models.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FX.Core.Models.Microsite.Page;

namespace FX.Core
{
	public class FXContextItems
	{
		public struct ItemKeys
		{
			public static readonly string HomePage = "FX.HomePage";
            public static readonly string MicrositeHomePage = "FX.MicrositeHomePage";
        }

		public static IHomePage HomePage
		{
			get
			{
				if (Sitecore.Context.Items[FXContextItems.ItemKeys.HomePage] == null)
				{
					var context = new SitecoreContext();
					if (context != null)
					{
                        Sitecore.Context.Items[FXContextItems.ItemKeys.HomePage] = context.GetHomeItem<IHomePage>();
					}
				}
				return Sitecore.Context.Items[FXContextItems.ItemKeys.HomePage] as IHomePage;
			}
		}

        public static IMicrositeHomePage MicrositeHomePage
        {
            get
            {
                if (Sitecore.Context.Items[FXContextItems.ItemKeys.MicrositeHomePage] == null)
                {
                    var context = new SitecoreContext();
                    if (context != null)
                    {
                        string query = string.Format("./ancestor-or-self::*[@@templateid='{0}']", Core.Keys.Templates.MicrositeHomePage);
                        Sitecore.Context.Items[FXContextItems.ItemKeys.MicrositeHomePage] = context.QuerySingleRelative<IMicrositeHomePage>(query, false, false);
                    }
                }
                return Sitecore.Context.Items[FXContextItems.ItemKeys.MicrositeHomePage] as IMicrositeHomePage;
            }

        }
	}
}
