using FX.Core.Models.Base;
using Glass.Mapper.Sc.Fields;

namespace FX.Core.Models.EXM
{
    public interface BannerImage : ISitecoreItem
    {
        Image Image { get; set; }

    }
}
