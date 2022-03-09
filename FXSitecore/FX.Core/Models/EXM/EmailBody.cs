using FX.Core.Models.Base;
using Glass.Mapper.Sc.Fields;

namespace FX.Core.Models.EXM
{
    public interface EmailBody : ISitecoreItem
    {
        string Heading { get; set; }

        string Content { get; set; }

        Link Link { get; set; }
    }
}
