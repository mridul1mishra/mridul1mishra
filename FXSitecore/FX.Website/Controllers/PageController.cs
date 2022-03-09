using FX.Core;
using FX.Core.GlassMapper;
using FX.Core.Models.Base;
using FX.Core.Models.Page;
using FX.Core.Models.News;
using FX.Core.Utils;
using FX.Website.Models;
using FX.Website.Models.Careers;
using FX.Website.Models.Events;
using FX.Website.Models.Pages;
using FX.Website.Models.Shared;
using FX.Website.Models.SuccessStories;
using Glass.Mapper.Sc.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FX.Core.Models.Components;
using Newtonsoft.Json;
using FX.Core.Models.Products;

namespace FX.Website.Controllers
{
    public class PageController : GlassController
    {
        public ActionResult Home()
        {
            var viewModel = new HomeViewModel();
            viewModel.SitecoreModel = SitecoreContext.GetCurrentItem<IHomePage>();
            return View("~/Views/FX/Page/Home.cshtml", viewModel);
        }

        public ActionResult StandardPage()
        {
            return View("~/Views/FX/Page/StandardPage.cshtml");
        }

        public ActionResult SnSPage()
        {
            return View("~/Views/FX/Page/SnSPage.cshtml");
        }

        public ActionResult ProductListingPage()
        {
            return View("~/Views/FX/Page/ProductListingPage.cshtml");
        }

        public ActionResult ProductPage()
        {
            return View("~/Views/FX/Page/ProductPage.cshtml");
        }

        public ActionResult NewsroomPage()
        {
            var viewModel = new NewsroomModel();
            viewModel.SitecoreModel = SitecoreContext.GetCurrentItem<INewsroomPage>();
            viewModel.LeftColumnLabel = viewModel.SitecoreModel.LeftColumnLabel;
            viewModel.RightColumnLabel = viewModel.SitecoreModel.RightColumnLabel;
            return View("~/Views/FX/Page/NewsroomPage.cshtml", viewModel);
        }

        public ActionResult LandingPage()
        {
            return View("~/Views/FX/Page/LandingPage.cshtml");
        }

        public ActionResult PopupForm(Guid? formId, string productName)
        {
            var model = new PopupForm();

            if (formId.HasValue)
            {
                var formItem = SitecoreContext.GetItem<FX.Core.Models.Base.ISitecoreItem>(formId.Value);
                if (formItem != null)
                {
                    model.FormId = formItem.Id;
                }
            }

            model.ProductName = productName;
            return View("~/Views/FX/Page/PopupForm.cshtml", model);
        }

        public ActionResult SearchPage()
        {
            return View("~/Views/FX/Page/SearchPage.cshtml");
        }

        public ActionResult FullWidthPage()
        {
            return View("~/Views/FX/Page/FullWidthPage.cshtml");
        }

        public ActionResult SitemapPage()
        {
            /*
            var viewModel = new SitemapViewModel();
            viewModel.SitecoreItem = SitecoreContext.GetCurrentItem<IStandardPage>();
            viewModel.Sections = FXContextItems.HomePage.Children.Where(i => i.ShowInSitemap).Select(p => new Section()
            {
                SectionLink = new NavigationLink() { Url = p.URL(), Text = p.NavigationTitle },
                SubSections = new Func<INavigation, IEnumerable<Section>>((i) =>
              {
                  if (Util.AreEquals(i.TemplateId, FX.Core.Templates.SnSPage.Id))
                  {
                      int x = 0;
                      var groups = from s in i.Children.Where(c => c.ShowInSitemap)
                                   let num = x++
                                   group s by num / 4 into g
                                   select g;
                      return groups.Select(g => new Section());

                  }
                  else if (Util.AreEquals(i.TemplateId, FX.Core.Templates.ProductListingPage.Id))
                  {

                  }
                  else
                  {

                  }

                  return new List<Section>();
              })(p)
            });
            */
            return View("~/Views/FX/Page/SitemapPage.cshtml");
        }

        public ActionResult SuccessStoriesLandingPage()
        {
            var viewModel = new SuccessStoriesLandingViewModel();
            viewModel.SitecoreModel = SitecoreContext.GetCurrentItem<ISuccessStoriesLandingPage>();
            var defaultCountry = viewModel.SitecoreModel.DefaultFilterItem;
            viewModel.CountryFilter = new SuccessStoriesFilter()
            {
                Items = viewModel.SitecoreModel.SuccessStoriesFilterFolder.SuccessStoriesCountryFilter.SuccessStoriesFilterItems.Select(i => new SuccessStoriesFilterItem()
                {
                    Value = Util.GetNormalizedId(i.Id),
                    Title = i.SuccessStoryFilterItemName,
                    SelectedAttribute = defaultCountry != null && defaultCountry.Id == i.Id ? " selected=\"selected\" " : ""
                })
            };
            viewModel.SolutionFilter = new SuccessStoriesFilter()
            {
                Items = viewModel.SitecoreModel.SuccessStoriesFilterFolder.SuccessStoriesSolutionFilter.SuccessStoriesFilterItems.Select(i => new SuccessStoriesFilterItem()
                {
                    Value = Util.GetNormalizedId(i.Id),
                    Title = i.SuccessStoryFilterItemName
                })
            };
            viewModel.ProductFilter = new SuccessStoriesFilter()
            {
                Items = viewModel.SitecoreModel.SuccessStoriesFilterFolder.SuccessStoriesProductFilter.SuccessStoriesFilterItems.Select(i => new SuccessStoriesFilterItem()
                {
                    Value = Util.GetNormalizedId(i.Id),
                    Title = i.SuccessStoryFilterItemName
                })
            };
            viewModel.IndustryFilter = new SuccessStoriesFilter()
            {
                Items = viewModel.SitecoreModel.SuccessStoriesFilterFolder.SuccessStoriesIndustryFilter.SuccessStoriesFilterItems.Select(i => new SuccessStoriesFilterItem()
                {
                    Value = Util.GetNormalizedId(i.Id),
                    Title = i.SuccessStoryFilterItemName
                })
            };
            viewModel.YearFilter = new SuccessStoriesFilter()
            {
                Items = viewModel.SitecoreModel.SuccessStoriesFilterFolder.SuccessStoriesYearFilter.SuccessStoriesFilterItems.Select(i => new SuccessStoriesFilterItem()
                {
                    Value = Util.GetNormalizedId(i.Id),
                    Title = i.SuccessStoryFilterItemName
                })
            };
            viewModel.ShowBannerCheckBox = viewModel.SitecoreModel.ShowBannerCheckbox;
            return View("~/Views/FX/Page/SuccessStoriesLandingPage.cshtml", viewModel);
        }

        public ActionResult SuccessStoryDetailPage()
        {
            return View("~/Views/FX/Page/SuccessStoryDetailPage.cshtml");
        }

        public ActionResult CareerDetailPage()
        {
            return View("~/Views/FX/Page/CareerDetailPage.cshtml");
        }

        public ActionResult CareerListingPage()
        {
            var viewModel = new CareersListingViewModel();
            viewModel.SitecoreModel = SitecoreContext.GetCurrentItem<ICareerListingPage>();
            viewModel.Filters = viewModel.SitecoreModel.CareersFilterFolder.Filters.Select(f => new CareersFilter()
            {
                ID = Util.GetNormalizedId(f.Id),
                Name = f.FilterKey,
                Title = f.FilterName,
                Items = f.FilterItems.Select(fi => new CareersFilterItem()
                {
                    ID = Util.GetNormalizedId(fi.Id),
                    Value = Util.GetNormalizedId(fi.Id),
                    Title = fi.CareerFilterItemName,
                }),
                AllFiltersLabel = f.AllFiltersLabel

            });
            viewModel.SearchResultsText = viewModel.SitecoreModel.SearchResultsText.Replace("{0}", "<span class=\"total-results\">0</span>");
            return View("~/Views/FX/Page/CareerListingPage.cshtml", viewModel);
        }

        public ActionResult PromotionListingPage()
        {
            return View("~/Views/FX/Page/PromotionListingPage.cshtml");
        }

        public ActionResult PromotionDetailPage()
        {
            return View("~/Views/FX/Page/PromotionDetailPage.cshtml");
        }

        public ActionResult EventDetailPage()
        {
            return View("~/Views/FX/Page/EventDetailPage.cshtml");
        }

        public ActionResult EventListingPage()
        {
            var viewModel = new EventsListingViewModel();
            viewModel.SitecoreModel = SitecoreContext.GetCurrentItem<IEventListingPage>();
            viewModel.LocationFilter = new EventsFilter()
            {
                Items = viewModel.SitecoreModel.EventsFilterFolder.EventsLocationFilter.EventsFilterItems.Select(i => new EventsFilterItem()
                {
                    Value = Util.GetNormalizedId(i.Id),
                    Title = i.EventFilterItemName
                })
            };
            viewModel.IndustryFilter = new EventsFilter()
            {
                Items = viewModel.SitecoreModel.EventsFilterFolder.EventsIndustryFilter.EventsFilterItems.Select(i => new EventsFilterItem()
                {
                    Value = Util.GetNormalizedId(i.Id),
                    Title = i.EventFilterItemName
                })
            };
            viewModel.YearFilter = new EventsFilter()
            {
                Items = viewModel.SitecoreModel.EventsFilterFolder.EventsYearFilter.EventsFilterItems.Select(i => new EventsFilterItem()
                {
                    Value = Util.GetNormalizedId(i.Id),
                    Title = i.EventFilterItemName
                })
            };
            viewModel.MonthFilter = new EventsFilter()
            {
                Items = viewModel.SitecoreModel.EventsFilterFolder.EventsMonthFilter.EventsFilterItems.Select(i => new EventsFilterItem()
                {
                    Value = Util.GetNormalizedId(i.Id),
                    Title = i.EventFilterItemName
                })
            };
            return View("~/Views/FX/Page/EventListingPage.cshtml", viewModel);
        }

        public ActionResult EventRegistrationPage()
        {
            return View("~/Views/FX/Page/EventRegistrationPage.cshtml");
        }

        public ActionResult BlankPage()
        {
            return View("~/Views/FX/Page/BlankPage.cshtml");
        }

        public ActionResult TopicPage()
        {
            return View("~/Views/FX/Page/TopicPage.cshtml");
        }

        public ActionResult ResellerLocatorPage()
        {
            var Model = SitecoreContext.GetCurrentItem<IResellerLocatorPage>();

            if (Model.XMLFile != null)
            {
                Model.Resellers = Core.Models.Reseller.Resellers.GetResellers(new Sitecore.Data.ID(Model.XMLFile.Id));
            }

            return View("~/Views/FX/Page/ResellerLocator.cshtml", Model);
        }

        public ActionResult InsightPage()
        {
            var Model = SitecoreContext.GetCurrentItem<IInsightPage>();

            var filters = Model.CareersFilterFolder.FirstOrDefault();
            var subFolders = filters.SubFolders;
            if (subFolders.Any())
            {
                Model.FirstFilter = subFolders.FirstOrDefault();
            }
            if (subFolders.Count() > 1)
            {
                Model.SecondFilter = subFolders.Skip(1).Take(1).FirstOrDefault();
            }

            return View("~/Views/FX/Page/InsightPage.cshtml", Model);
        }

        public ActionResult NewResellerLocatorPage()
        {
            return View("~/Views/FX/Page/NewResellerLocatorPage.cshtml");
        }

        public ActionResult ProductFinderPage()
        {

            var data = new ProductFinderViewModel()
            {
                SitecoreModel = SitecoreContext.GetCurrentItem<IProductFinderPage>()
            };

            return View("~/Views/FX/Page/ProductFinderPage.cshtml", data);
        }


        public ActionResult ProductComparePage(string id)
        {
            var listOfIds = new List<string>();

            if (id.Contains(','))
            {
                foreach (var item in id.Split(','))
                {
                    var longId = new Sitecore.Data.ShortID(item).ToID().ToString();
                    listOfIds.Add(longId);
                }
            }
            else
            {
                var longId = new Sitecore.Data.ShortID(id).ToID().ToString();
                listOfIds.Add(longId);
            }

            var data = new ProductCompareViewModel();

            data.SitecoreModel = SitecoreContext.GetCurrentItem<IProductComparePage>();
            data.QueryItemsString = id;

            var printerList = new List<IPrinterPage>();

            for (int i = 0; i < listOfIds.Count; i++)
            {
                printerList.Add(SitecoreContext.GetItem<IPrinterPage>(listOfIds[i].ToString()));
            }

            data.QueryItems = printerList;

            return View("~/Views/FX/Page/ProductComparePage.cshtml", data);
        }
    }
}