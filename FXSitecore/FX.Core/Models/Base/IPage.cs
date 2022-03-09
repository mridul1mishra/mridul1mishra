using FX.Core.GlassMapper.DataHandler;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using System.Collections.Generic;

namespace FX.Core.Models.Base
{
    public interface IPage : IMeta, INavigation, ITeaser, ICta
    {
        string MainTitle { get; set; }

        [SitecoreParent]
        IPage ParentPage { get; set; }

        [SitecoreChildren(InferType = true)]
        IEnumerable<IPage> Pages { get; set; }

        string OpCo { get; set; }
    }


}
