using FX.Core.Models.Page;
using FX.Core.Models.Components;
using FX.Core.Models.Components.Announcements;
using FX.Core.Utils;
using FX.Website.Models.Components;
using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sitecore.Mvc.Presentation;
using FX.Core.Models.Base;
using FX.Core.Models.Components.SocialMedia;

namespace FX.Website.Controllers
{
    public class ExmController : GlassController
    {
        public ActionResult Header()
        {
            return View("~/Views/FX/EXM/Component/Header.cshtml");
        }

        public ActionResult Footer()
        {
            return View("~/Views/FX/EXM/Component/Footer.cshtml");
        }

        public ActionResult BannerImage()
        {
            return View("~/Views/FX/EXM/Component/BannerImage.cshtml");
        }

        public ActionResult EmailBody()
        {
            return View("~/Views/FX/EXM/Component/EmailBody.cshtml");
        }

        public ActionResult CtaButton()
        {
            return View("~/Views/FX/EXM/Component/CtaButton.cshtml");
        }

        public ActionResult StripImage()
        {
            return View("~/Views/FX/EXM/Component/StripImage.cshtml");
        }

        public ActionResult TwoCol()
        {
            return View("~/Views/FX/EXM/Component/TwoCol.cshtml");
        }

        public ActionResult BarContentRight()
        {
            return View("~/Views/FX/EXM/Component/BarContentRight.cshtml");
        }

        public ActionResult BarContentLeft()
        {
            return View("~/Views/FX/EXM/Component/BarContentLeft.cshtml");
        }
    }
}