﻿using FX.Core.Models.Base;
using Glass.Mapper.Sc.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Core.Models.Components.TwoColumnCarousel
{
    public interface ITwoColumnCarouselItem : ISitecoreItem
    {
        string Title { get; set; }

        string Description { get; set; }

        Image Image { get;  set; }

        string ButtonText { get; set; }

        Link ButtonLink { get; set; }
    }
}
