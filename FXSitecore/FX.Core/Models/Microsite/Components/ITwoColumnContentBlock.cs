using FX.Core.Models.Microsite.Base;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace FX.Core.Models.Microsite.Components
{
    [SitecoreType(TemplateId = MicrositeTemplates.TwoColumnContentBlock.ID, AutoMap = true)]
    public interface ITwoColumnContentBlock : IMicrositeBaseComponentFields
    {
        string LeftTitle { get; set; }
        string LeftText { get; set; }
        string RightText { get; set; }
    }
}
