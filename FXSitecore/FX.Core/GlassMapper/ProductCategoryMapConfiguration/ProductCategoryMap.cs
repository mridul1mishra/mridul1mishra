using FX.Core.Models.Base;
using FX.Core.Models.Products;
using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Maps;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Links;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FX.Core.GlassMapper.ProductCategoryMapConfiguration
{
    public class ProductCategoryMap : SitecoreGlassMap<IProductCategory>
    {
        
        public override void Configure()
        {
            Map(m => m.AutoMap(),
                m => m.Delegate(c => c.CategoryNavName).GetValue(c => c.Item.Template.BaseTemplates.Any(t => t.ID == ID.Parse(Templates.BaseProductCategory.Id)) ?
                    c.Item["CategoryName"] :                    
                    c.Item["NavigationTitle"]),
                m => m.Delegate(c => c.Link).GetValue(c => c.Item.Template.BaseTemplates.Any(t => t.ID == ID.Parse(Templates.BaseProductCategory.Id)) ?
                    string.Format("{0}?listing={1}", LinkManager.GetItemUrl(c.Item.Parent), HttpUtility.UrlEncode(c.Item["CategoryName"])).ToLower():
                    c.Item.Fields["NavigationTitle"] != null ? 
                        ((LinkField)c.Item.Fields["RedirectLink"]).GetFriendlyUrl() :
                        LinkManager.GetItemUrl(c.Item)),
                m => m.Delegate(c => c.IsCategory).GetValue(c => c.Item.Template.BaseTemplates.Any(t => t.ID == ID.Parse(Templates.BaseProductCategory.Id)))
            );
           
        }
    }
}
