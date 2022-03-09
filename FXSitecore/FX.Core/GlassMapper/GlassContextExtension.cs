using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Fields;
using FX.Core.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FX.Core.Utils;
using Sitecore.Data.Items;

namespace FX.Core.GlassMapper
{
    public static class GlassContextExtension
    {
        public static string GetDisplayName(this ISitecoreItem item)
        {
			if (item == null){
				return string.Empty;
			}
            string result = item.DisplayName;
            if (string.IsNullOrWhiteSpace(result))
            {
                result = item.Name;
            }
            return result;
        }

        public static bool HasNavigationTitle(this INavigation navigation)
        {
			if (navigation == null)
			{
				return false;
			}

            if (navigation.NavigationTitle != null)
            {
                return !string.IsNullOrWhiteSpace(navigation.NavigationTitle);
            }
            return false;
        }

        public static bool HasTeaserTitle(this IPage page)
        {
            return page != null && !string.IsNullOrWhiteSpace(page.TeaserTitle);
        }

        public static bool IsValidImage(this Image image)
        {
            return image != null && !string.IsNullOrEmpty(image.Src);
        }

        

        public static string LocalDateFormat(this IPage page)
        {
            var siteSettings = FXContextItems.HomePage.SiteSettings;

            return siteSettings.DateRangeStartFormat;
        }

        public static string DateOnlyFirst(this IPage page)
        {
            var siteSettings = FXContextItems.HomePage.SiteSettings;

            return siteSettings.DayMonthYearFormat;
        }

        

        public static IEnumerable<INavigation> GetMainNavigationItems(this INavigation navi)
        {
            return navi.MainNavItems;
        }

		

        public static bool HasContent(this FX.Core.Models.Components.TitleAndText.ITitleAndTextItem item)
        {
			if (item == null)
			{
				return false;
			}
            return !string.IsNullOrEmpty(item.Title) ||
                !string.IsNullOrEmpty(item.Text);
        }

        public static IEnumerable<INextStepFields> GetNextSteps(this FX.Core.Models.Components.TakeTheNextStep.ITakeTheNextStep item)
        {
            return item.NextSteps.Take(4);
        }

        public static bool HasContent(this FX.Core.Models.Components.Accordion.IAccordionItem item)
        {
			if (item == null)
			{
				return false;
			}
            return !string.IsNullOrEmpty(item.Title) ||
                !string.IsNullOrEmpty(item.MainContent);
        }

        public static int GetCarouselTimer(this FX.Core.Models.Components.ICarousel item)
        {
            int result = Math.Abs(item.CarouselTimer);
            if (result == 0)
            {
                result = 3000;
            }
            return result;
        }

		public static bool HasFilter(this FX.Core.Models.Products.IProductCategory category)
		{
			return category != null && category.FilterItems != null && category.FilterItems.Any();
		}

		public static bool HasAdvancedFilter(this FX.Core.Models.Products.IProductCategory category)
		{
			return category != null && category.AdvFilterItems != null && category.AdvFilterItems.Any();
		}

		public static FX.Core.Models.Products.EnquiryLink GetEnquiryLink(this FX.Core.Models.Products.IProductPage productPage)
		{
			return new Models.Products.EnquiryLink(productPage);
		}

		public static IEnumerable<FX.Core.Models.News.INewsCategory> GetNewsCategories(this FX.Core.Models.News.INewsroomPage newsroomPage)
		{
			var result = new List<FX.Core.Models.News.INewsCategory>();
			if (newsroomPage != null)
			{
				if (newsroomPage.FilterSource1 != null)
				{
					result.Add(newsroomPage.FilterSource1);
				}
				if (newsroomPage.FilterSource2 != null)
				{
					result.Add(newsroomPage.FilterSource2);
				}
			}
			return result.ToArray();
		}

        public static FX.Core.Models.Microsite.Page.IMicrositePage GetBanner(this FX.Core.Models.Microsite.Page.IMicrositePage micrositePage)
        {
            if (micrositePage == null)
                return null;

            if (micrositePage.Banner != null)
                return micrositePage;

            return GetBanner(micrositePage.ParentPage);
        }

        public static bool HasChildPages(this Models.Microsite.Base.IMicrositeNavigation page)
        {
            return page.Children.Where(i => i.ShowInNavigation).Count() > 0;
        }
    }

}

