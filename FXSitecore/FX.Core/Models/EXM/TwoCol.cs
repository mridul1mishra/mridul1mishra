using FX.Core.Models.Base;
using Glass.Mapper.Sc.Fields;

namespace FX.Core.Models.EXM
{
    public interface TwoCol : ISitecoreItem
    {
        string MainTitle { get; set; }

        string Title1 { get; set; }

        Image Image1 { get; set; }

        string Text1 { get; set; }

        Link Link1 { get; set; }

        string Title2 { get; set; }

        Image Image2 { get; set; }

        string Text2 { get; set; }

        Link Link2 { get; set; }

    }
}
