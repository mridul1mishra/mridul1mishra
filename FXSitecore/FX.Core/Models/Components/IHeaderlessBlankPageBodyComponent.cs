using FX.Core.Models.Base;
using FX.Core.Models.Components;
using FX.Core.Models.Settings;
using Glass.Mapper.Sc.Configuration.Attributes;
using System.Collections.Generic;

namespace FX.Core.Models.Components
{
    public interface IHeaderlessBlankPageBodyComponent

    {
        string Body { get; set; }
    }
}
