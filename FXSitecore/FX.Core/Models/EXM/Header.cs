using FX.Core.GlassMapper.DataHandler;
using FX.Core.Models.Base;
using FX.Core.Models.Components;
using FX.Core.Models.Components.Notifications;
using FX.Core.Models.Page;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Sitecore.Data.Items;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace FX.Core.Models.EXM
{
    public interface Header : ISitecoreItem
    {
        Image LogoImage { get; set; }
        
        Link LogoLink { get; set; }
        
        string TopText { get; set; }
        
        string BrowserLinkText { get; set; }

    }
}
