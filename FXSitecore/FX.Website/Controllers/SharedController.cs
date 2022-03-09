using FX.Core.Models.Base;
using FX.Website.Models.Shared;
using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using Sitecore.Data.Items;
using System.Web.Services;
using System.Data;

namespace FX.Website.Controllers
{
    public class SharedController : GlassController
    {
        public ActionResult Head()
        {
            var dataModel = SitecoreContext.GetCurrentItem<IPage>();

            var viewModel = new HeadViewModel()
            {
                MetaDescription = dataModel.MetaDescription,
                MetaTitle = dataModel.MetaTitle,
                MetaKeywords = dataModel.MetaKeywords,
                MainTitle = dataModel.MainTitle,
                CanonicalUrl = Sitecore.Links.LinkManager.GetItemUrl(dataModel.SitecoreItem, new Sitecore.Links.UrlOptions() { EncodeNames = true, AlwaysIncludeServerUrl = true, LanguageEmbedding = Sitecore.Links.LanguageEmbedding.Always }),
                SocialMediaTags = new List<SocialMediaMetaTag>()
                  {
                      new SocialMediaMetaTag()
                      { Property = "og:title", Content = dataModel.FBTitle },
                      new SocialMediaMetaTag()
                      { Property = "og:description", Content = dataModel.FBDescription },
                      new SocialMediaMetaTag()
                      {
                          Property = "og:image",
                          Content = dataModel.FBImage != null && SitecoreContext.GetItem<ISitecoreItem>( dataModel.FBImage.MediaId)!=null ?
                          Sitecore.Resources.Media.MediaManager.GetMediaUrl(SitecoreContext.GetItem<ISitecoreItem>( dataModel.FBImage.MediaId).SitecoreItem,
                          new Sitecore.Resources.Media.MediaUrlOptions() { AlwaysIncludeServerUrl = true }): ""
                      },
                      new SocialMediaMetaTag()
                      { Property = "twitter:title", Content = dataModel.TwitterTitle },
                      new SocialMediaMetaTag()
                      { Property = "twitter:description", Content = dataModel.TwitterDescription },
                      new SocialMediaMetaTag()
                      {
                          Property = "twitter:image",
                          Content = dataModel.TwitterImage != null && SitecoreContext.GetItem<ISitecoreItem>( dataModel.TwitterImage.MediaId) !=null?
                          Sitecore.Resources.Media.MediaManager.GetMediaUrl(SitecoreContext.GetItem<ISitecoreItem>( dataModel.TwitterImage.MediaId).SitecoreItem,
                          new Sitecore.Resources.Media.MediaUrlOptions() { AlwaysIncludeServerUrl = true }): ""
                      },
                      new SocialMediaMetaTag()
                      {
                          Property = "twitter:card",
                          Content = "summary"
                      }
                  },
                Language = dataModel.Language
            };


            return View("~/Views/FX/Shared/Head.cshtml", viewModel);
        }

        public ActionResult MainNavigation()
        {
            return View("~/Views/FX/Shared/MainNavigation.cshtml");
        }

        public ActionResult Breadcrumb()
        {
            return View("~/Views/FX/Shared/Breadcrumb.cshtml");
        }

        public ActionResult TopBanner()
        {
            return View("~/Views/FX/Shared/TopBanner.cshtml");
        }

        public ActionResult SideMenu()
        {
            return View("~/Views/FX/Shared/SideMenu.cshtml", FX.Core.FXContextItems.HomePage.SiteSettings);
        }

        public ActionResult SiblingNavigation()
        {
            return View("~/Views/FX/Shared/SiblingNavigation.cshtml");
        }

        public ActionResult Bottom()
        {
            return View("~/Views/FX/Shared/Bottom.cshtml");
        }

        public ActionResult EnquiryForm()
        {
            return View("~/Views/FX/Shared/EnquiryForm.cshtml", FX.Core.FXContextItems.HomePage.SiteSettings);
        }

        public ActionResult SideBarNotification()
        {
            return View("~/Views/FX/Shared/SideBarNotification.cshtml", FX.Core.FXContextItems.HomePage.SiteSettings);
        }

        public ActionResult LinkData()
        {
            return View("~/Views/FX/Shared/LinkData.cshtml", Sitecore.Context.Item);
        }        
    }
}