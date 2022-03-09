using FX.Core.Models.Microsite.Base;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace FX.Core.Models.Microsite.Components
{
    [SitecoreType(TemplateId = MicrositeTemplates.IntructionComponent.ID, AutoMap = true)]
    public interface IIntroduction : IMicrositeBaseComponentFields
    {
        string Title { get; set; }
        string Text { get; set; }

        bool HideBar { get; set; }
    }
}
