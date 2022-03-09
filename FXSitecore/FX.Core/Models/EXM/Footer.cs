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
    public interface Footer : ISitecoreItem
    {
        string ConnectText { get; set; }

        string FooterText { get; set; }

        string CopyrightTextOne { get; set; }

        string CopyrightTextTwo { get; set; }

        Image Icon1 { get; set; }
        Link Link1 { get; set; }

        Image Icon2 { get; set; }
        Link Link2 { get; set; }

        Image Icon3 { get; set; }
        Link Link3 { get; set; }

        Image Icon4 { get; set; }
        Link Link4 { get; set; }

        Image Icon5 { get; set; }
        Link Link5 { get; set; }

        Image Icon6 { get; set; }
        Link Link6 { get; set; }

        Link FooterLink1 { get; set; }

        Link FooterLink2 { get; set; }

        Link FooterLink3 { get; set; }

        Link FooterLink4 { get; set; }
    }
}
