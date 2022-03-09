using FX.Core.GlassMapper.DataHandler;
using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;

namespace FX.Core.Models.Components
{
    [SitecoreType(TemplateId = Templates.VideoBoxComponent.Id, AutoMap = true)]
    public interface IVideoBox : IBasePageComponentFields
    {
        [SitecoreField("YoutubeID")]
        string YoutubeID { get; set; }

        [SitecoreField("Company Description Text")]
        MultiLineText CompanyDescriptionText { get; set; }

        [SitecoreField("Company Logo")]
        Image CompanyLogo { get; set; }

        [SitecoreField("VideoEmbedCode")]
        string VideoEmbedCode { get; set; }
    }
}

