using FX.Core.Models.Microsite.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;

namespace FX.Core.Models.Microsite.Components
{
    [SitecoreType(TemplateId = Core.MicrositeTemplates.TwoColumnImageTextContentBlock.ID, AutoMap =true)]
    public interface ITwoColumnImageTextContentBlock : IMicrositeBaseComponentFields
    {
        Image Image { get; set; }
        string RightTitle { get; set; }
        string RightText { get; set; }
        string ButtonText { get; set; }
        Link ButtonLink { get; set; }
    }
}
