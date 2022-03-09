using FX.Core.Models.Base;
using Glass.Mapper.Sc.Fields;

namespace FX.Core.Models.EXM
{
    public interface BarContent : ISitecoreItem
    {
        Image Image { get; set; }

        string Title { get; set; }

        string Text { get; set; }

        Link Link { get; set; }
    }
}
