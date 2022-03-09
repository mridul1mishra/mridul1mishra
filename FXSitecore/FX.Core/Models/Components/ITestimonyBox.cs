using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;

namespace FX.Core.Models.Components
{
    [SitecoreType(TemplateId = Templates.TestimonyBoxComponent.Id, AutoMap = true)]
    public interface ITestimonyBox : IBasePageComponentFields
    {
        [SitecoreField("Testimony Image")]
        Image TestimonyImage { get; set; }

        [SitecoreField("Testimony Content")]
        string TestimonyContent { get; set; }

        [SitecoreField("Testimony Footer")]
        string TestimonyFooter { get; set; }
    }
}

