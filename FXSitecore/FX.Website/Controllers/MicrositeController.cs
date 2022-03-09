using System.Web.Mvc;

namespace FX.Website.Controllers
{
    public class MicrositeController : Controller
    {
        // GET: Microsite
        public ActionResult Head()
        {
            return View("~/Views/Microsite/Shared/Head.cshtml");
        }

        public ActionResult Header()
        {
            return View("~/Views/Microsite/Component/Header.cshtml", FX.Core.FXContextItems.MicrositeHomePage.SiteSettings);
        }

        public ActionResult Footer()
        {
            return View("~/Views/Microsite/Component/Footer.cshtml", FX.Core.FXContextItems.MicrositeHomePage.SiteSettings);
        }

        public ActionResult MicrositeContactForm()
        {
            return View("~/Views/Microsite/Component/ContactForm.cshtml", FX.Core.FXContextItems.MicrositeHomePage.SiteSettings);
        }

        public ActionResult MicrositeNavigation()
        {
            return View("~/Views/Microsite/Shared/MicrositeNavigation.cshtml");
        }

        public ActionResult MicrositeSubNavigation()
        {
            return View("~/Views/Microsite/Shared/MicrositeSubNavigation.cshtml");
        }

        public ActionResult MicrositeBreadcrumb()
        {
            return View("~/Views/Microsite/Shared/MicrositeBreadcrumb.cshtml");
        }

        public ActionResult MainContent()
        {
            return View("~/Views/Microsite/Component/MainContent.cshtml");
        }

        public ActionResult HeroBanner()
        {
            return View("~/Views/Microsite/Component/HeroBanner.cshtml");
        }

        public ActionResult Introduction()
        {
            return View("~/Views/Microsite/Component/Introduction.cshtml");
        }

        public ActionResult PowerfulFeatures()
        {
            return View("~/Views/Microsite/Component/PowerfulFeatures.cshtml");
        }

        public ActionResult PowerfulFeaturesThreeColumn()
        {
            return View("~/Views/Microsite/Component/PowerfulFeaturesThreeColumn.cshtml");
        }

        public ActionResult MicrositeCarousel()
        {
            return View("~/Views/Microsite/Component/MicrositeCarousel.cshtml");
        }

        public ActionResult FeaturedRows()
        {
            return View("~/Views/Microsite/Component/FeaturedRows.cshtml");
        }

        public ActionResult FeaturedContent()
        {
            return View("~/Views/Microsite/Component/FeaturedContent.cshtml");
        }

        public ActionResult TwoColumnContentBlock()
        {
            return View("~/Views/Microsite/Component/TwoColumnContentBlock.cshtml");
        }

        public ActionResult TwoColumnImageTextContentBlock()
        {
            return View("~/Views/Microsite/Component/TwoColumnImageTextContentBlock.cshtml");
        }

        public ActionResult Discover()
        {
            return View("~/Views/Microsite/Component/Discover.cshtml");
        }

        public ActionResult Listings()
        {
            return View("~/Views/Microsite/Component/Listings.cshtml");
        }


        public ActionResult MicrositeResourcesCarousel()
        {
            return View("~/Views/Microsite/Component/MicrositeVideoCarousel.cshtml");
        }

        public ActionResult MicrositeBrochureCarousel()
        {
            return View("~/Views/Microsite/Component/MicrositeBrochureCarousel.cshtml");
        }

        public ActionResult MicrositeArticleCarousel()
        {
            return View("~/Views/Microsite/Component/MicrositeArticleCarousel.cshtml");
            
        }

        public ActionResult MicrositeFormComponent()
        {
            return View("~/Views/Microsite/Component/MicrositeFormComponent.cshtml");

        }

        public ActionResult TwoColumnTextFormComponent()
        {
            return View("~/Views/Microsite/Component/TwoColumnTextFormComponent.cshtml");
        }

        public ActionResult TwoColumnImageFormComponent()
        {
            return View("~/Views/Microsite/Component/TwoColumnImageFormComponent.cshtml");
        }

        public ActionResult IFrameMicrositeComponent()
        {
            return View("~/Views/Microsite/Component/IFrameMicrositeComponent.cshtml");
        }
    }
}