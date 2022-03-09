using FX.Core.Models.Microsite.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Core.Models.Microsite.Components
{
    [SitecoreType(TemplateId = MicrositeTemplates.PowerFeatures.ID, AutoMap =true)]
    public interface IPowerfulFeatures : IMicrositeBaseComponentFields, IMicrositeHoverColoursField
    {
        string Title { get; set; }
        Image Icon1 { get; set; }
        Image Icon2 { get; set; }
        Image Icon3 { get; set; }
        Image Icon4 { get; set; }
        Link Link1 { get; set; }
        Link Link2 { get; set; }
        Link Link3 { get; set; }
        Link Link4 { get; set; }
        string Text1 { get; set; }
        string Text2 { get; set; }
        string Text3 { get; set; }
        string Text4 { get; set; }
    }
}
