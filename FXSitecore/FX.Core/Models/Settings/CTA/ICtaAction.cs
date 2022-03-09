using Glass.Mapper.Sc.Fields;
using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace FX.Core.Models.Settings.CTA
{
    public interface ICtaAction : ISitecoreItem
    {
        Image Image { get; set; }

        Image HoverImage { get; set; }

        string DisplayText { get; set; }

        Link Link { get; set; }
    }
}