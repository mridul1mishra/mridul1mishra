using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace FX.Core.Models.Settings.CTA
{
    [SitecoreType(TemplateId = Templates.CtaPopupForm.Id, AutoMap = true)]
    public interface ICtaPopupAction : ICtaAction
    {
        string Form { get; set; }
    }
}
