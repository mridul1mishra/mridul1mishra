using FX.Core.Models.Microsite.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Core.Models.Microsite.Components.FeaturedLinks
{
    [SitecoreType(TemplateId = MicrositeTemplates.FeaturedRowComponent.ID, AutoMap = true)]
    public interface IFeaturedRow : IMicrositeBaseComponentFields
    {
        Image Icon1 { get; set; }
        Image Icon2 { get; set; }
        Image Icon3 { get; set; }

        string Title1 { get; set; }
        string Title2 { get; set; }
        string Title3 { get; set; }

        string Description1 {get;set;}
        string Description2 { get; set; }
        string Description3 { get; set; }

        Link Link1 { get; set; }
        Link Link2 { get; set; }
        Link Link3 { get; set; }

        string LinkText1 { get; set; }
        string LinkText2 { get; set; }
        string LinkText3 { get; set; }
    }
}
