using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace FX.Core.Models.Settings.CTA
{
    [SitecoreType(TemplateId = Templates.CtaCallContact.Id, AutoMap = true)]
    public interface ICtaPhoneContactAction : ICtaAction
    {
        string ContactNumber { get; set; }
    }
}
