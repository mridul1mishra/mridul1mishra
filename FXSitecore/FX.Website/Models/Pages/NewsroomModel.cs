using FX.Core;
using FX.Core.Models.Base;
using FX.Core.Models.News;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FX.Website.Models.Pages
{
    public class NewsroomModel
    {
        public INewsroomPage SitecoreModel { get; set; }
        public string LeftColumnLabel { get; set; }
        public string RightColumnLabel { get; set; }
    }
}