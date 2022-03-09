using FX.Core.Models.Base;
using FX.Core.Models.Page;
using FX.Core.Models.Products;
using Glass.Mapper.Sc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FX.Website.Models.Pages
{
    public class ProductCompareViewModel
    {
        public IProductComparePage SitecoreModel { get; set; }

        public List<IPrinterPage> QueryItems { get; set; }

        public string QueryItemsString { get; set; }

        public string GetProductFinderPageItemName()
        {
            var context = new SitecoreContext();
            var printerPage = context.GetCurrentItem<IProductFinderPage>();
            var parent = printerPage.SitecoreItem.Parent.Paths.ContentPath;

            var comparePage = context.Database.SelectItems("/sitecore/content" + parent + " /*[@@templateid='{AB6C433D-461A-41B1-B9B8-C7FFB8583CC6}']").FirstOrDefault();
            
            if (comparePage != null)
            {
                var url = Sitecore.Links.LinkManager.GetItemUrl(comparePage);
                return url;
            }
            else
            {
                return "";
            }
        }
    }


}