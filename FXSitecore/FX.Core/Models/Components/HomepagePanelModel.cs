using FX.Core.Models.Base;
using Glass.Mapper.Sc.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FX.Core.Models.Components
{
    public interface HomepagePanelModel : IBasePageComponentFields
    {
        Image ImageOne { get; set; }
        string TitleOne { get; set; }
        string DescOne { get; set; }
        Link UrlOne { get; set; }
        Image ImageTwo { get; set; }
        string TitleTwo { get; set; }
        string DescTwo { get; set; }
        Link UrlTwo { get; set; }
        Image ImageThree { get; set; }
        string TitleThree { get; set; }
        string DescThree { get; set; }
        Link UrlThree { get; set; }

        Image ImageFour { get; set; }
        string TitleFour { get; set; }
        string DescFour { get; set; }
        Link UrlFour { get; set; }
        Image ImageFive { get; set; }
        string TitleFive { get; set; }
        string DescFive { get; set; }
        Link UrlFive { get; set; }
        Image ImageSix { get; set; }
        string TitleSix { get; set; }
        string DescSix { get; set; }
        Link UrlSix { get; set; }





    }
}