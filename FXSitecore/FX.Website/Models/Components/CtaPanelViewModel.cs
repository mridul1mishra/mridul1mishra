using System.Collections.Generic;
using FX.Core.Models.Settings.CTA;
using FX.Core.Models.Base;

namespace FX.Website.Models.Components
{
    public class CtaPanelViewModel
    {
        public List<ICtaAction> CtaActions { get; set; }
        public IPage Page { get; set; }
    }
}