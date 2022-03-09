using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using System.Collections.Generic;

namespace FX.Core.Models.Components
{
    public interface IQuickLinks : IBasePageComponentFields
    {
        [SitecoreChildren(InferType = true)]
        IEnumerable<ILinkItem> QuickLinkItems { get; set; }
    }
}
