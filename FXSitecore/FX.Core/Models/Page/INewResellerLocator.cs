using FX.Core.Models.Base;
using FX.Core.Models.Reseller;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Newtonsoft.Json;
using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
namespace FX.Core.Models.Page
{
    [SitecoreType(TemplateId = "{2790B110-7131-4F88-B36B-D2FAA05823A9}", AutoMap = true)]
    public interface INewResellerLocator : IPage
    {

        #region Reseller Locator Labels
        string ProvinceLabel { get; set; }
        string SelectAProvinceLabel { get; set; }
        string SelectAProvinceError { get; set; }

        string CityLabel { get; set; }
        string SelectACityLabel { get; set; }
        string SelectACityError { get; set; }

        string ResellerTypeLabel { get; set; }
        string SelectAResellerTypeLabel { get; set; }
        string SelectAResellerTypeError { get; set; }

        string ProductCategoryLabel { get; set; }
        string SelectAProductCategoryLabel { get; set; }
        string SelectAProductCategoryError { get; set; }

        string ProductNameLabel { get; set; }
        string SelectAProductLabel { get; set; }
        string SelectAProductError { get; set; }

        string DealerLabel { get; set; }
        string DealerError { get; set; }

        string SearchLabel { get; set; }
        string ResultsLabel { get; set; }
        string SwitchViewLabel { get; set; }

        string MedalLegendLabel { get; set; }
        bool HideMap { get; set; }
        bool HideResellerType { get; set; }
        #endregion

        #region Reseller Locator API
        IResellerMapType ResellerMapType { get; set; }
        #endregion

        #region Reseller Locator Results Label
        string ResellerNameLabel { get; set; }
        string AddressLabel { get; set; }
        string ContactLabel { get; set; }
        string FaxLabel { get; set; }
        string EmailLabel { get; set; }
        string WebsiteLabel { get; set; }
        string OtherInfoLabel { get; set; }
        string ShowOnMapLabel { get; set; }
        string RequestAQuoteLabel { get; set; }
        #endregion

        [SitecoreChildren]
        IEnumerable<IProvince> Provinces { get; set; }

        [SitecoreChildren]
        IEnumerable<IProductFolder> ProductFolder {get;set;}

        [SitecoreChildren]
        IEnumerable<IResellerTypeFolder> ResellerTypeFolder { get; set; }

        [SitecoreChildren]
        IEnumerable<IMedalFolder> MedalFolder { get; set; }
    }

    public static class INewResellerLocatorExtension
    {
        public static string GetProvinces(this INewResellerLocator resellerLocator)
        {
            return JsonConvert.SerializeObject(from p in resellerLocator.Provinces
                                               select new
                                               {
                                                   name = p.ProvinceName,
                                                   value = ShortID.Parse(p.Id.ToString()).ToString(),
                                                   cities = from c in p.Cities
                                                            select new
                                                            {
                                                                name = c.CityName,
                                                                value = ShortID.Parse(c.Id.ToString()).ToString()
                                                            }
                                               });
        }

        public static string GetProducts(this INewResellerLocator resellerLocator)
        {
            var productFolder = resellerLocator.ProductFolder.FirstOrDefault();

            if (productFolder == null)
                return "";

            return JsonConvert.SerializeObject(from c in productFolder.Categories
                                               select new
                                            {
                                                name = c.CategoryName,
                                                value = ShortID.Parse(c.Id.ToString()).ToString(),
                                                products = from p in c.Products
                                                           select new
                                                           {
                                                               name = p.ProductName,
                                                               value = ShortID.Parse(p.Id.ToString()).ToString()
                                                           }
                                            });
        }

        public static IEnumerable<IProductCategoryFolder> GetProductCategories(this INewResellerLocator resellerLocator)
        {
            var folder = resellerLocator.ProductFolder.FirstOrDefault();
            if (folder == null)
                return new List<IProductCategoryFolder>();

            return folder.Categories;
        }
        public static IEnumerable<IResellerType> GetResellerTypes(this INewResellerLocator resellerLocator)
        {
            var folder = resellerLocator.ResellerTypeFolder.FirstOrDefault();
            if(folder==null)
                return new List<IResellerType>();

            return folder.ResellerTypes;
        }

        public static string MapTypeSnippet(this INewResellerLocator resellerLocator)
        {
            return resellerLocator.ResellerMapType != null ? resellerLocator.ResellerMapType.CodeSnippet : "";
        }

        public static string MapClass(this INewResellerLocator resellerLocator)
        {
            return resellerLocator.ResellerMapType != null ? resellerLocator.ResellerMapType.MapClass : "";
        }
    }

    [SitecoreType(TemplateId = "{469C48E3-9BD0-4C66-9ECA-5E7A48E54409}", AutoMap = true, EnforceTemplate = SitecoreEnforceTemplate.Template)]
    public interface IProvince : ISitecoreItem
    {
        [SitecoreId]
        ID SitecoreID { get; set; }
        string ProvinceName { get; set; }

        [SitecoreChildren]
        IEnumerable<ICity> Cities { get; set; }

        [SitecoreParent]
        INewResellerLocator ResellerLocator { get; set; }

    }

    [SitecoreType(TemplateId = "{A25CF041-3B40-4937-8111-C260853FFF41}", AutoMap = true, EnforceTemplate = SitecoreEnforceTemplate.Template)]
    public interface ICity : ISitecoreItem
    {
        [SitecoreId]
        ID SitecoreID { get; set; }
        string CityName { get; set; }

        [SitecoreChildren]
        IEnumerable<IReseller> Resellers { get; set; }

        [SitecoreParent]
        IProvince Province { get; set; }
    }

    [SitecoreType(TemplateId = "{A2DB1A82-3543-410E-89A5-5B631F5E7EBE}", AutoMap = true, EnforceTemplate = SitecoreEnforceTemplate.Template)]
    [JsonObject]
    public interface IReseller : ISitecoreItem
    {
        [JsonIgnore]
        [SitecoreParent]
        ICity City { get; set; }

        [JsonProperty("name")]
        string DealerName { get; set; }
        [JsonProperty("address")]
        string Address { get; set; }
        [JsonProperty("contact")]
        string Contact { get; set; }
        [JsonProperty("fax")]
        string Fax { get; set; }
        [JsonProperty("email")]
        string Email { get; set; }
        [JsonProperty("webURL")]
        string Website { get; set; }
        [JsonProperty("formURL")]
        string FormUrl { get; set; }
        [JsonProperty("otherInfo")]
        string OtherInfo { get; set; }
        [JsonProperty("long")]
        string Longitude { get; set; }
        [JsonProperty("lat")]
        string Latitude { get; set; }
        [JsonIgnore]
        IEnumerable<Guid> Products { get; set; }
        [JsonIgnore]
        IEnumerable<Guid> ResellerType { get; set; }
        [JsonIgnore]
        IMedal Medal { get; set; }
    }

    [SitecoreType(TemplateId = "{00BCF55D-5A0D-4A4F-A902-401A1D0F564D}", AutoMap = true, EnforceTemplate = SitecoreEnforceTemplate.Template)]
    public interface IProductFolder : ISitecoreItem
    {
        [SitecoreChildren]
        IEnumerable<IProductCategoryFolder> Categories { get; set; }
    }

    [SitecoreType(TemplateId = "{11247EA4-2EAF-44D1-A53E-8BDBD233A754}", AutoMap = true, EnforceTemplate = SitecoreEnforceTemplate.Template)]
    public interface IProductCategoryFolder : ISitecoreItem
    {
        [SitecoreId]
        ID SitecoreID { get; set; }
        string CategoryName { get; set; }
        [SitecoreChildren]
        IEnumerable<IProduct> Products { get; set; }
    }

    [SitecoreType(TemplateId = "{E5366EAD-CF01-46B8-9381-37DD4C0BB60D}", AutoMap = true, EnforceTemplate = SitecoreEnforceTemplate.Template)]
    public interface IProduct : ISitecoreItem
    {
        [SitecoreId]
        ID SitecoreID { get; set; }
        string ProductName { get; set; }
    }

    [SitecoreType(TemplateId = "{1B485E81-BFE0-4EFD-BED7-84DBA8984152}", AutoMap = true, EnforceTemplate = SitecoreEnforceTemplate.Template)]
    public interface IResellerTypeFolder : ISitecoreItem
    {
        [SitecoreChildren]
        IEnumerable<IResellerType> ResellerTypes { get; set; }
    }

    [SitecoreType(TemplateId = "{602EC43D-FAB0-435F-8A98-83CCB08633ED}", AutoMap = true, EnforceTemplate = SitecoreEnforceTemplate.Template)]
    public interface IResellerType : ISitecoreItem
    {
        [SitecoreId]
        ID SitecoreID { get; set; }
        string ResellerType { get; set; }
    }

    public interface IResellerMapType : ISitecoreItem
    {
        string CodeSnippet { get; set; }
        string MapClass { get; set; }
    }

    public interface IMedal : ISitecoreItem
    {
        Image Icon { get; set; }
        string Text { get; set; }
        [SitecoreField("__Sortorder")]
        int SortValue { get; set; }
    }

    [SitecoreType(TemplateId = "{0788D877-3671-4680-BEAA-472C18062F11}", AutoMap = true, EnforceTemplate = SitecoreEnforceTemplate.Template)]
    public interface IMedalFolder : ISitecoreItem
    {
        [SitecoreChildren]
        IEnumerable<IMedal> Medals { get; set; }
    }
}
