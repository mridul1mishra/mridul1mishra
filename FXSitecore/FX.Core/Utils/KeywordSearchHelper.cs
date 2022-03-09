using FX.Core.Models.Page;
using FX.Core.Search.Models;
using Glass.Mapper.Sc;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace FX.Core.Utils
{
    public class KeywordSearchHelper
    {

        private IHomePage _homePage;

        public IHomePage HomePage
        {
            get
            {
                if (this._homePage == null)
                {
                    this._homePage = Context.GetHomeItem<IHomePage>();
                }
                return this._homePage;
            }
        }

        public ISitecoreContext Context { get; private set; }

        public KeywordSearchHelper(ISitecoreContext context)
        {
            if (context == null)
            {
                context = new SitecoreContext();
            }
            this.Context = context;
        }

        public IEnumerable<FXResultItem> Search(string query, int page, int pageSize, out int count)
        {
            if (string.IsNullOrEmpty(query))
            {
                count = 0;
                return new FXResultItem[0];
            }

            var boolClause = PredicateBuilder.True<FXResultItem>();
            var homeId = new Sitecore.Data.ID(HomePage.Id);
            var productFolderID = new Sitecore.Data.ID(Keys.Items.ProductsFolder);
            boolClause = boolClause.And(x => x.Paths.Contains(homeId) || (x.Paths.Contains(productFolderID) && x.Countries.Contains(homeId)));
            var regexObj = new Regex("^[a-z0-9_-]+$");

            var queries = query.Split(' ');
            var whereClause = PredicateBuilder.True<FXResultItem>();
            whereClause = whereClause.And(x => x.Language == Sitecore.Context.Language.Name);

            if (regexObj.Match(query).Success)
            {
                foreach (string q in queries)
                {
                    whereClause = whereClause.Or(x => x.ComponentContent.Contains(q.ToLowerInvariant()) || x.Content.Contains(q.ToLowerInvariant()));
                }
            }
            else
            {
                foreach (string q in queries)
                {
                    whereClause = whereClause.Or(x => x.ComponentContent.Equals(q.ToLower()) || x.Content.Equals(q.ToLower()));
                }
            }







            boolClause = boolClause.And(whereClause);

            var searchParam = new SearchParam<FXResultItem>(this.Context.Database);
            searchParam.AddTemplateRestriction(new ID(Templates.StandardPage.Id));
            searchParam.AddTemplateRestriction(new ID(Templates.PrinterPage.Id));
            searchParam.AddTemplateRestriction(new ID(Templates.SnSPage.Id));
            searchParam.AddTemplateRestriction(new ID(Templates.SnSStandardPage.Id));
            searchParam.AddTemplateRestriction(new ID(Templates.SoftwarePage.Id));
            searchParam.AddTemplateRestriction(new ID(Templates.SupplyPage.Id));
            searchParam.Page = page;
            searchParam.PageSize = pageSize;
            searchParam.WhereClause = boolClause;

            var results = SearchHelper.Search<FXResultItem>(searchParam, out count);

            return results;
        }
    }
}
