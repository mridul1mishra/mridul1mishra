using FX.Core.GlassMapper.DataHandler;
using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;

namespace FX.Core.Models.Components.TakeTheNextStep
{
    [SitecoreType(TemplateId = Templates.FormEmbedLinkStep.Id, AutoMap = true)]
    public interface IFormEmbedStep : INextStepFields
    {
        string Form{ get; set; }
    }
}
