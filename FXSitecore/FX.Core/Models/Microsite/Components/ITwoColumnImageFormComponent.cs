using FX.Core.Models.Microsite.Base;
using Glass.Mapper.Sc.Fields;

namespace FX.Core.Models.Microsite.Components
{
    public interface ITwoColumnImageFormComponent : IMicrositeBaseComponentFields
    {
        Image Image { get; set; }
    }
}
