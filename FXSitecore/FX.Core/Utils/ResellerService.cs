using FX.Core.Models.Page;
using Glass.Mapper.Sc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace FX.Core.Utils
{
    public class ResellerService
    {
        private string CacheKey = "ResellerLocator";
        public ISitecoreContext Context { get; private set; }
        private MemoryCache memCache { get; set; }
        private INewResellerLocator ResellerLocator { get; set; }
        bool useCache = false;
        public ResellerService(ISitecoreContext context, bool useCache = true)
        {
            if (context == null)
                context = new SitecoreContext();

            this.Context = context;
            this.useCache = useCache;
            memCache = MemoryCache.Default;
        }

        private INewResellerLocator GetResellerLocator()
        {
            var homePage = Context.GetHomeItem<IHomePage>();
            return homePage.NewResellerLocator.FirstOrDefault();
        }

        private string FormatAddress(string address)
        {
            var split = address.Split('\n');
            string result = "";
            foreach (string s in split)
            {
                result += $"<p>{s}</p>";
            }

            return result;
        }

        private Guid GetGuidFromShortID(string s)
        {
            if (!ShortID.IsShortID(s))
                return Guid.Empty;

            return ShortID.Parse(s).Guid;
        }

        private Object GetFromMemCache(string key)
        {
            return memCache[key];
        }

        private void AddToMemCache(string key, Object o)
        {
            memCache.Add(key, o, new CacheItemPolicy() { AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(5) });
        }

        private IProvince GetProvince(string Province)
        {
            IProvince provinceItem = GetFromMemCache(Province) as IProvince;

            if (provinceItem == null)
            {
                var provinceID = GetGuidFromShortID(Province);
                provinceItem = Context.GetItem<IProvince>(provinceID);

                if (provinceItem != null)
                    AddToMemCache(Province, provinceItem);
            }

            return provinceItem;
        }

        private ICity GetCity(string City)
        {
            ICity cityItem = GetFromMemCache(City) as ICity;

            if(cityItem == null)
            {
                var cityID = GetGuidFromShortID(City);
                cityItem = Context.GetItem<ICity>(cityID);

                if (cityItem != null)
                    AddToMemCache(City, cityItem);
            }

            return cityItem;
        }

        private IProductCategoryFolder GetCategoryFolder(Guid id)
        {
            return Context.GetItem<IProductCategoryFolder>(id);
        }

        public string GetResellers(string Province, string City, string ProductCategory, string ProductName, string ResellerType, string DealerName)
        {
            // invalid search
            if (string.IsNullOrEmpty(Province) && string.IsNullOrEmpty(City))
                return "Invalid query";

            IEnumerable<IReseller> resellers = new List<IReseller>();

            // search by province only
            if (!string.IsNullOrEmpty(Province) && string.IsNullOrEmpty(City))
            {
                var provinceID = GetGuidFromShortID(Province);

                IProvince provinceItem = GetProvince(Province);
                if (provinceItem == null)
                    return "province item not found";

                resellers = (from cities in provinceItem.Cities
                             from reseller in cities.Resellers
                             select reseller);
            }

            // search by city only
            if (!string.IsNullOrEmpty(City))
            {
                var cityItem = GetCity(City);

                if (cityItem == null)
                    return "city item not found";

                resellers = cityItem.Resellers;
            }

            // filter by reseller type
            if (!string.IsNullOrEmpty(ResellerType) && ShortID.IsShortID(ResellerType))
            {
                var resellerTypeID = ShortID.Parse(ResellerType).Guid;
                resellers = resellers.Where(x => x.ResellerType.Contains(resellerTypeID));
            }

            //search by product category only
            if (!string.IsNullOrEmpty(ProductCategory) && string.IsNullOrEmpty(ProductName))
            {
                if (ShortID.IsShortID(ProductCategory))
                {

                    var categoryID = GetGuidFromShortID(ProductCategory);

                    IProductCategoryFolder categoryFolder = GetCategoryFolder(categoryID);
                    if (categoryFolder != null)
                    {
                        List<IReseller> newList = new List<IReseller>();
                        foreach (var product in categoryFolder.Products)
                        {
                            newList.AddRange(resellers.Where(x => x.Products.Contains(product.Id)));
                        }

                        resellers = newList.Distinct();
                    }
                }
            }

            //filter by product
            if (!string.IsNullOrEmpty(ProductName) && ShortID.IsShortID(ProductName))
            {
                var productID = ShortID.Parse(ProductName).Guid;
                resellers = resellers.Where(x => x.Products.Contains(productID));
            }

            //filter by reseller name
            if(!string.IsNullOrEmpty(DealerName))
            {
                resellers = resellers.Where(x => x.DealerName.ToLower().Contains(DealerName.ToLower()));
            }

            resellers = resellers.OrderBy(x => x.Medal!=null ? x.Medal.SortValue : int.MaxValue);

            var result = JsonConvert.SerializeObject(
                 resellers.Select(r => new
                 {
                     name = r.DealerName,
                     address = FormatAddress(r.Address),
                     contact = r.Contact,
                     fax = r.Fax,
                     email = r.Email,
                     webURL = r.Website,
                     formURL = r.FormUrl,
                     otherInfo = r.OtherInfo,
                     lat = r.Latitude,
                     @long = r.Longitude,
                     medal = GetMedalUrl(r)
                 }
                )
            );


            return result;

        }

        private string GetMedalUrl(IReseller reseller)
        {
            if (reseller.Medal != null && reseller.Medal.Icon != null)
                return reseller.Medal.Icon.Src;

            return string.Empty;
        }

        
    }
}
