using FX.Core.Models.Page;
using FX.Core.Models.Components;
using FX.Core.Models.Components.Announcements;
using FX.Core.Models.Settings.CTA;
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
    public class ComponentController : GlassController
    {
        public ActionResult Header()
        {
            return View("~/Views/FX/Component/Header.cshtml", FX.Core.FXContextItems.HomePage.SiteSettings);
        }

        public ActionResult Footer()
        {
            return View("~/Views/FX/Component/Footer.cshtml", FX.Core.FXContextItems.HomePage.SiteSettings);
        }

        public ActionResult Carousel()
        {
            return View("~/Views/FX/Component/Carousel.cshtml");
        }

        public ActionResult MainBanner()
        {
            return View("~/Views/FX/Component/MainBanner.cshtml");
        }

        public ActionResult CategoryBanner()
        {
            return View("~/Views/FX/Component/CategoryBanner.cshtml");
        }

        public ActionResult FeaturedContent()
        {
            return View("~/Views/FX/Component/FeaturedContent.cshtml");
        }

        public ActionResult LatestEvents()
        {
            var viewModel = new LatestEventsViewModel();
            viewModel.SitecoreModel = SitecoreContext.GetItem<ILatestEvents>(RenderingContext.Current.Rendering.DataSource);

            EventService eventService = new EventService(this.SitecoreContext);
            int startIndex = 0;
            int countOfItems = 4;
            var results = eventService.GetEventsListing(startIndex, countOfItems);
            viewModel.Events = results.Select(i => new Event()
            {
                EventTitle = i.EventTitle,
                EventStartDate = i.LocalisedEventStartDate.DateTime,
                EventEndDate = i.LocalisedEventEndDate.DateTime,
                EventUrl = i.EventUrl
            });

            return View("~/Views/FX/Component/LatestEvents.cshtml", viewModel);
        }

        public ActionResult LatestNews()
        {
            return View("~/Views/FX/Component/LatestNews.cshtml");
        }

        public ActionResult RichText()
        {
            return View("~/Views/FX/Component/RichText.cshtml");
        }

        public ActionResult TitleAndText()
        {
            return View("~/Views/FX/Component/TitleAndText.cshtml");
        }

        public ActionResult Accordion()
        {
            return View("~/Views/FX/Component/Accordion.cshtml");
        }

        public ActionResult ChildCards()
        {
            return View("~/Views/FX/Component/ChildCards.cshtml");
        }

        public ActionResult RelatedPages()
        {
            return View("~/Views/FX/Component/RelatedPages.cshtml");
        }

        public ActionResult Resources()
        {
            return View("~/Views/FX/Component/Resources.cshtml");
        }

        public ActionResult TakeTheNextStep()
        {
            return View("~/Views/FX/Component/TakeTheNextStep.cshtml");
        }

        public ActionResult FormPlaceholder()
        {
            return View("~/Views/FX/Component/FormPlaceholder.cshtml");
        }

        public ActionResult GeneralLinks()
        {
            return View("~/Views/FX/Component/GeneralLinks.cshtml", FX.Core.FXContextItems.HomePage.SiteSettings.RelatedProductLink);
        }

        public ActionResult IFrame()
        {
            return View("~/Views/FX/Component/IFrame.cshtml");
        }

        public ActionResult RelatedProducts()
        {
            return View("~/Views/FX/Component/RelatedProducts.cshtml");
        }

        public ActionResult RelatedProductsAndSolutions()
        {
            return View("~/Views/FX/Component/RelatedProductsAndSolutions.cshtml");
        }

        public ActionResult ProductIntro()
        {
            var dataSource = SitecoreContext.GetItem<IProductIntro>(RenderingContext.Current.Rendering.DataSource);

            if (dataSource != null)
            {
                dataSource.ProductPage = SitecoreContext.GetCurrentItem<Core.Models.Products.IProductPage>();
            }

            return View("~/Views/FX/Component/ProductIntro.cshtml", dataSource);
        }

        public ActionResult RecommendedProducts()
        {
            return View("~/Views/FX/Component/RecommendedProducts.cshtml");
        }

        public ActionResult TestimonyBox()
        {
            return View("~/Views/FX/Component/TestimonyBox.cshtml");
        }

        public ActionResult VideoBox()
        {
            return View("~/Views/FX/Component/VideoBox.cshtml");
        }

        public ActionResult CareerApplyNow()
        {
            return View("~/Views/FX/Component/CareerApplyNow.cshtml");
        }

        public ActionResult EventOverview()
        {
            return View("~/Views/FX/Component/EventOverview.cshtml");
        }

        public ActionResult EventAgenda()
        {
            return View("~/Views/FX/Component/EventAgenda.cshtml");
        }

        public ActionResult EventFeaturedSpeaker()
        {
            return View("~/Views/FX/Component/EventFeaturedSpeaker.cshtml");
        }

        public ActionResult Announcement()
        {
            var viewModel = new AnnouncementViewModel();
            viewModel.SitecoreModel = SitecoreContext.GetItem<IAnnouncementFolder>(RenderingContext.Current.Rendering.DataSource);
            int announcementItemCount = 5;

            bool isAnnouncementItemCountInteger = RenderingContext.Current.Rendering?.Parameters != null && !string.IsNullOrEmpty(RenderingContext.Current.Rendering.Parameters[FX.Core.Constants.AnnouncementConstant.AnnouncementItemCountFieldName]) ? int.TryParse(RenderingContext.Current.Rendering.Parameters[FX.Core.Constants.AnnouncementConstant.AnnouncementItemCountFieldName], out announcementItemCount) : false;

            viewModel.AnnouncementItemCount = announcementItemCount;

            if (viewModel.SitecoreModel != null)
            {
                viewModel.AnnouncementFolder = (new AnnouncementFolder()
                {
                    HeadingTitle = viewModel.SitecoreModel.HeadingTitle,
                    Announcement = viewModel.SitecoreModel.Children.Select(i => new Announcement()
                    {
                        Title = i.AnnouncementTitle,
                        StartDate = i.LocalisedAnnouncementStartDate.DateTime,
                        StartEndDate = i.LocalisedAnnouncementStartDate.DateRange(i.LocalisedAnnouncementStartDate.DateTime, i.LocalisedAnnouncementEndDate.DateTime),
                        Url = i.AnnouncementLink != null ? i.AnnouncementLink.Url : ""
                    }).OrderByDescending(i => i.StartDate)
                });
            }

            return View("~/Views/FX/Component/Announcement.cshtml", viewModel);
        }

        public ActionResult IHeaderlessBlankPageBodyComponent()
        {
            return View("~/Views/FX/Component/HeaderlessBlankPageBodyComponent.cshtml");
        }

        public ActionResult TwoColumnFormPlaceholder()
        {
            return View("~/Views/FX/Component/TwoColumnFormPlaceholder.cshtml");
        }

        public ActionResult HeroBanner()
        {
            IBanner dataSource = SitecoreContext.GetItem<IBanner>(RenderingContext.CurrentOrNull.Rendering.DataSource);

            // no datasource, this is an inpage banner
            if (dataSource == null)
            {
                // take the datasource from the page instead of the component.
                dataSource = SitecoreContext.GetCurrentItem<IBanner>();
                if (!Sitecore.Context.PageMode.IsExperienceEditor)
                {

                    //if the current page has no banner image, get from the parent
                    IBanner temp = dataSource;
                    while (temp.BannerImage == null)
                    {
                        temp = temp.ParentBanner;
                        if (temp == null)
                        {
                            temp = dataSource;
                            break;
                        }
                    }
                    dataSource.BannerImage = temp.BannerImage;
                }
            }

            dataSource.SiteSettings = Core.FXContextItems.HomePage.SiteSettings;

            return View("~/Views/FX/Component/HeroBanner.cshtml", dataSource);
        }

        public ActionResult ProductHeroBanner()
        {
            var dataSource = SitecoreContext.GetCurrentItem<IBanner>();


            if (!Sitecore.Context.PageMode.IsExperienceEditor)
            {
                if (dataSource.BannerImage == null)
                {
                    var productPage = SitecoreContext.GetCurrentItem<Core.Models.Products.IProductPage>();

                    var listingPage = Core.Models.Products.IProductPageExtension.GetListingPage(productPage, SitecoreContext);

                    if (listingPage != null)
                    {
                        var temp = SitecoreContext.GetItem<IBanner>(listingPage.Path);


                        //if the current page has no banner image, get from the parent

                        while (temp.BannerImage == null)
                        {
                            temp = temp.ParentBanner;
                            if (temp == null)
                            {
                                temp = dataSource;
                                break;
                            }
                        }
                        dataSource.BannerImage = temp.BannerImage;
                    }
                }
            }

            dataSource.SiteSettings = Core.FXContextItems.HomePage.SiteSettings;

            return View("~/Views/FX/Component/HeroBanner.cshtml", dataSource);
        }

        public ActionResult SocialFeed()
        {
            return View("~/Views/FX/Component/SocialFeed.cshtml");
        }

        public ActionResult Panel()
        {
            return View("~/Views/FX/Component/HomepagePanel.cshtml");
        }

        public ActionResult QuickLinks()
        {
            return View("~/Views/FX/Component/QuickLinks.cshtml");
        }

        public ActionResult ChatWindow()
        {
            return View("~/Views/FX/Component/ChatWindow.cshtml", FX.Core.FXContextItems.HomePage.SiteSettings);
        }

        public ActionResult TwoColumnCarousel()
        {
            return View("~/Views/FX/Component/TwoColumnCarousel.cshtml");
        }

        public ActionResult InquiryComponent()
        {
            return View("~/Views/FX/Component/InquiryComponent.cshtml");
        }

        public ActionResult FullWidthVideo()
        {
            return View("~/Views/FX/Component/FullWidthVideo.cshtml");
        }

        public ActionResult TopCarousel()
        {
            return View("~/Views/FX/Component/TopCarousel.cshtml");
        }

        public ActionResult ThreeQuarterCarousel()
        {
            return View("~/Views/FX/Component/ThreeQuarterCarousel.cshtml");
        }

        public ActionResult AsidePanel()
        {
            return View("~/Views/FX/Component/AsidePanel.cshtml");
        }

        public ActionResult TabbedCarousel()
        {
            return View("~/Views/FX/Component/TabbedCarousel.cshtml");
        }

        public ActionResult CtaPanel()
        {
            var ViewModel = new CtaPanelViewModel
            {
                Page = SitecoreContext.GetCurrentItem<IPage>(),
                CtaActions = new List<ICtaAction>()
            };

            if (ViewModel.Page.EnableCtaPanel)
            {
                if (!string.IsNullOrEmpty(ViewModel.Page.CtaDataSource))
                {
                    var ctaFolderItem = Sitecore.Context.Database.GetItem(ViewModel.Page.CtaDataSource);
                    var ctaActionItems = ctaFolderItem.GetChildren();
                    foreach (Sitecore.Data.Items.Item ctaAction in ctaActionItems)
                    {
                        if (ctaAction.TemplateID.ToString() == FX.Core.Templates.CtaCallContact.Id)
                            ViewModel.CtaActions.Add(SitecoreContext.Cast<ICtaPhoneContactAction>(ctaAction));
                        else if (ctaAction.TemplateID.ToString() == FX.Core.Templates.CtaPopupForm.Id)
                            ViewModel.CtaActions.Add(SitecoreContext.Cast<ICtaPopupAction>(ctaAction));
                    }
                    return View("~/Views/FX/Component/CtaPanel.cshtml", ViewModel);
                }
                else
                {
                    return new EmptyResult();
                }
            }
            else
            {
                return new EmptyResult();
            }
        }
    }
}