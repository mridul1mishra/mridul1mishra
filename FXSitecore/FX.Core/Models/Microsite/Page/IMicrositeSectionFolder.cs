using FX.Core.Models.Microsite.Base;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace FX.Core.Models.Microsite.Page
{
    [SitecoreType(TemplateId = MicrositeTemplates.MicrositeSectionFolder.ID, AutoMap = true)]
    public interface IMicrositeSectionFolder : IMicrositeNavigation
    {
    }
}
