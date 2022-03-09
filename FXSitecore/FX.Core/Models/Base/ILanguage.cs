using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Sitecore.Globalization;

namespace FX.Core.Models.Base
{
    public interface ILanguage : ISitecoreItem
    {
        string Charset { get; set; }

        [SitecoreField("Code page")]
        string CodePage { get; set; }

        string Dictionary { get; set; }
        string Encoding { get; set; }

        [SitecoreField("Fallback Language")]
        ILanguage FallbackLanguage { get; set; }

        string Iso { get; set; }

        [SitecoreField("Regional Iso Code")]
        string RegionalIsoCode { get; set; }

        [SitecoreField("WorldLingo Language Identifier")]
        string WorldLinoLanguageIdentifier { get; set; }

        [SitecoreField("__Icon")]
        string Icon { get; set; }

        Language AssociatedLanguage { get; set; }
        string IconUrl { get; set; }

    }
}
