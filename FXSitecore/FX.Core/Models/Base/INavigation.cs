using FX.Core.Models.Settings;
using FX.Core.Utils;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using System.Collections.Generic;
using System.Linq;
using FX.Core.GlassMapper;
namespace FX.Core.Models.Base
{
    [SitecoreType(AutoMap = true)]
    public interface INavigation : ISitecoreItem, ITeaser
    {
        string NavigationTitle { get; set; }
        bool ShowInSideNav { get; set; }
        bool ShowInSitemap { get; set; }
        bool ShowInSitemapXML { get; set; }
        bool ShowInFooter { get; set; }
        ITaxonomyItem NavigationGroup { get; set; }
        string InquiryText { get; set; }
        Link InquiryLink { get; set; }
        string InquiryLinkCustomText { get; set; }
        bool HideInquiry { get; set; }

        [SitecoreParent]
        INavigation Parent { get; set; }

        [SitecoreChildren(InferType = true)]
        IEnumerable<INavigation> Children { get; set; }

        [SitecoreQuery("./*[@" + Templates.Navigation.Fields.ShowInSideNav + "='1']", IsRelative = true)]
        IEnumerable<INavigation> SideNavItems { get; set; }

        [SitecoreQuery("./*[@" + Templates.Navigation.Fields.ShowInFooterNav + "='1']", IsRelative = true, InferType = true)]
        IEnumerable<INavigation> MainNavItems { get; set; }

        Image MainNavThumbnail { get; set; }
    }

    [SitecoreType(TemplateId = Templates.SnSPage.Id, AutoMap = true)]
    public interface ISNSNavigation : INavigation
    {
    
    }

    [SitecoreType(TemplateId = Templates.ProductListingPage.Id, AutoMap = true)]
    public interface IProductListingNavigation : INavigation
    {
        string Column1Title { get; set; }
        IEnumerable<INavigation> Column1Items { get; set; }
        string Column2Title { get; set; }
        IEnumerable<INavigation> Column2Items { get; set; }
        string Column3Title { get; set; }
        IEnumerable<INavigation> Column3Items { get; set; }
    }

    public static class INavigationExtension
    {
        public static INavigation GetSectionParent(this INavigation page)
        {
            if (page == null)
                return null;

            if (page.Parent.IsHomePage())
                return page;

            return GetSectionParent(page.Parent);
        }

        public static IEnumerable<INavigation> GetSideNavItems(this INavigation navi)
        {
            return navi.SideNavItems.Where(i => i.HasNavigationTitle()).ToArray();
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

        public static bool IsHomePage(this INavigation page)
        {
            return page != null && Util.AreEquals(page.TemplateId, Templates.HomePage.Id);
        }

        public static IEnumerable<INavigation> GetBreadcrumbs(this INavigation page)
        {
            var result = new List<INavigation>();

            if (!page.IsHomePage())
            {
                var parentPage = page.Parent;
                while (parentPage != null && !parentPage.IsHomePage())
                {
                    if (!string.IsNullOrEmpty(parentPage.NavigationTitle))
                    {
                        result.Add(parentPage);
                    }
                    parentPage = parentPage.Parent;
                }
                result.Reverse();
            }
            return result.ToArray();
        }

        public static Dictionary<string, INavigation[]> GetGroupedMainNavigation(this INavigation page)
        {
            if (page != null)
            {
                return page.MainNavItems
                    .GroupBy(x => x.NavigationGroup == null ? string.Empty : "$name".Equals(x.NavigationGroup.TaxonomyItemName) ? x.NavigationGroup.GetDisplayName() : x.NavigationGroup.TaxonomyItemName)
                    .ToDictionary(group => group.Key, group => group.ToArray());
            }
            return null;
        }
    }
}
