using FX.Core.GlassMapper;
using FX.Core.Models.Form;
using FX.Core.Models.Page;
using FX.Core.Models.Settings;
using FX.Core.Search.Models;
using FX.Core.Search.Models.Taxonomy;
using Glass.Mapper.Sc;
using Newtonsoft.Json.Linq;
using Sitecore.Configuration;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Form.Core.Data;
using Sitecore.Form.Web.UI.Controls;
using Sitecore.WFFM.Abstractions.Actions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace FX.Core.Utils
{
    public class TaxonomyService
    {
        public ISitecoreContext Context { get; private set; }

        protected Guid IndustriesId = new Guid(Keys.Items.Industries);
        protected Guid DepartmentsId = new Guid(Keys.Items.Departments);
        protected Guid ServicesId = new Guid(Keys.Items.Services);
        protected Guid BusinessTypesId = new Guid(Keys.Items.BusinessTypes);

        public TaxonomyService() : this(null) { }

        public TaxonomyService(ISitecoreContext context)
        {
            if (context == null)
            {
                context = new SitecoreContext();
            }
            this.Context = context;
        }

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

        public IEnumerable<ITaxonomyItem> GetTaxonomyItems(Guid id)
        {
            var source = Context.GetItem<ITaxonomyFolder>(id);
            if (source != null)
            {
                return source.TaxonomyItems.ToArray();
            }
            return new ITaxonomyItem[0];
        }

        protected string GetItemId(ITaxonomyItem item)
        {
            return Util.GetNormalizedId(item.Id);
        }

        public JObject GetFilterOptions()
        {
            Dictionary<string, IEnumerable<ITaxonomyItem>> items = new Dictionary<string, IEnumerable<ITaxonomyItem>>();
            items.Add("industries", GetTaxonomyItems(IndustriesId));
            items.Add("departments", GetTaxonomyItems(DepartmentsId));
            items.Add("services", GetTaxonomyItems(ServicesId));
            items.Add("businesses", GetTaxonomyItems(BusinessTypesId));

            var result = JObject.FromObject(new
            {
                industries = from i in items["industries"]
                             select new
                             {
                                 id = GetItemId(i),
                                 name = i.GetDisplayName(),
                                 departments = from d in items["departments"]
                                               select new
                                               {
                                                   id = GetItemId(d),
                                                   name = d.GetDisplayName(),
                                                   services = from s in items["services"]
                                                              select new
                                                              {
                                                                  id = GetItemId(s),
                                                                  name = s.GetDisplayName(),
                                                                  businesses = from b in items["businesses"]
                                                                               select new
                                                                               {
                                                                                   id = GetItemId(b),
                                                                                   name = b.GetDisplayName()
                                                                               }
                                                              }
                                               }
                             }
            });
            return result;
        }

        protected ID TryParseId(string value)
        {
            ID result = ID.Null;
            if (string.IsNullOrEmpty(value))
            {
                return result;
            }

            ShortID sId;
            if (ShortID.TryParse(value, out sId))
            {
                result = sId.ToID();
            }

            return result;
        }

        protected string GetFilterByFieldId(string fieldName, ID filterValue)
        {
            if (ID.IsNullOrEmpty(filterValue))
            {
                return string.Empty;
            }
            return string.Format("{0}='%{1}%'", fieldName, filterValue);
        }

        protected IEnumerable<FXResultItem> GetTaggedItems(ID listingID,
            string industry, string department, string service, string business)
        {
            var result = new List<FXResultItem>();
            if (ID.IsNullOrEmpty(listingID))
            {
                return result.ToArray();
            }

            var whereClause = PredicateBuilder.True<FXResultItem>();
            //whereClause = whereClause.And(x => x.Paths.Contains(listingID));
            var grandParentClause = PredicateBuilder.True<FXResultItem>();
            grandParentClause = grandParentClause.Or(x => x.Parent == listingID);
            grandParentClause = grandParentClause.Or(x => x.GrandParent == listingID);
            whereClause = whereClause.And(x => x.Language == Sitecore.Context.Language.Name);
            whereClause = whereClause.And(grandParentClause);


            var taxonomyClause = PredicateBuilder.True<FXResultItem>();

            ID industryId = TryParseId(industry);
            if (!industryId.IsNull)
                taxonomyClause = taxonomyClause.And(x => x.Industries.Contains(industryId));

            ID departmentId = TryParseId(department);
            if (!departmentId.IsNull)
                taxonomyClause = taxonomyClause.And(x => x.Departments.Contains(departmentId));

            ID serviceId = TryParseId(service);
            if (!serviceId.IsNull)
                taxonomyClause = taxonomyClause.And(x => x.Services.Contains(serviceId));

            ID businessId = TryParseId(business);
            if (!businessId.IsNull)
                taxonomyClause = taxonomyClause.And(x => x.BusinessTypes.Contains(businessId));

            whereClause = whereClause.And(taxonomyClause);

            var searchParam = new SearchParam<FXResultItem>(this.Context.Database);
            searchParam.AddTemplateRestriction(new ID(Templates.SnSStandardPage.Id));
            searchParam.OrderBy = results => results.OrderByDescending(x => x.ArticleDate);


            searchParam.WhereClause = whereClause;

            int hitsCount = 0;
            var searchResults = SearchHelper.Search<FXResultItem>(searchParam, out hitsCount);

            return searchResults;
        }


        public IEnumerable<JObject> GetFilterResults(string industry, string department, string service, string business)
        {
            var opco = FX.Core.Utils.Util.OpcoType();
            var list = new List<JObject>();

            if (string.IsNullOrEmpty(industry))
            {
                department = service = business = industry;
            }
            else if (string.IsNullOrEmpty(department))
            {
                service = business = department;
            }
            else if (string.IsNullOrEmpty(service))
            {
                business = service;
            }

            try
            {
                DateTime now = DateTime.Now;

                var listingPage = HomePage.SiteSettings.SnSPage;
                var listingItem = this.Context.Database.GetItem(listingPage.SitecoreItem.ID);
                int daysLimit = FX.Core.FXContextItems.HomePage.SiteSettings.NewArticleAge;

                if (daysLimit <= 0)
                {
                    daysLimit = 0;
                }

                if (listingPage == null)
                {
                    return list;
                }

                var items = GetTaggedItems(listingPage.SitecoreItem.ID, industry, department, service, business);

                if (items != null && items.Any())
                {
                    var categories = listingPage.Pages;
                    foreach (var r in items)
                    {
                        list.Add(JObject.FromObject(new
                        {
                            image = r.TeaserImage != null ? "/" + opco + r.TeaserImage.Url : string.Empty,
                            //sub = Util.IsNewArticle(daysLimit, r.ArticleDate.ToLocalTime(), now) ? "New" : "",
                            title = r.TeaserTitle,
                            text = r.TeaserDescription,
                            category = GetSNSCategory(categories, r.Parent.Guid),
                            // andy requested date be removed https://redmine.adelphi.digital/issues/77590
                            //date = r.LocalisedArticleDate.DayMonthYearString,
                            url = $"/{opco}{r.Url}"
                        })
                        );
                    }
                }

            }
            catch (Exception ex)
            {
                Log.Error(string.Format("Failed to get filter results: Industry('{0}'), Department('{1}'), Service('{2}'), Business('{3}')",
                    industry, department, service, business), ex);

                return list;
            }

            return list;
        }

        private string GetSNSCategory(IEnumerable<Models.Base.IPage> pages, Guid id)
        {
            var match = pages.FirstOrDefault(p => p.Id == id);
            return match != null ? match.MainTitle : string.Empty;
        }
        protected IEnumerable<Sitecore.Data.Items.Item> FilterItems(ISnSPage listingPage,
            string industry,
            string department,
            string service,
            string business)
        {
            if (listingPage == null)
            {
                return new Sitecore.Data.Items.Item[0];
            }

            ID industryId = TryParseId(industry);
            ID departmentId = TryParseId(department);
            ID serviceId = TryParseId(service);
            ID businessId = TryParseId(business);

            var searchParam = new SearchParam<FXResultItem>(Context.Database);
            searchParam.AddTemplateRestriction(new ID(Templates.SnSStandardPage.Id));

            var listingPageID = new ID(listingPage.Id);
            var whereClause = PredicateBuilder.True<FXResultItem>();
            whereClause = whereClause.And(x => x.Paths.Contains(listingPageID));
            whereClause = whereClause.And(x => x.Language == Sitecore.Context.Language.Name);

            if (!ID.IsNullOrEmpty(industryId))
            {
                whereClause = whereClause.And(x => x.Industries.Contains(industryId));
            }
            if (!ID.IsNullOrEmpty(departmentId))
            {
                whereClause = whereClause.And(x => x.Departments.Contains(departmentId));
            }
            if (!ID.IsNullOrEmpty(serviceId))
            {
                whereClause = whereClause.And(x => x.Services.Contains(serviceId));
            }
            if (!ID.IsNullOrEmpty(businessId))
            {
                whereClause = whereClause.And(x => x.BusinessTypes.Contains(businessId));
            }
            searchParam.WhereClause = whereClause;

            int hitsCount = 0;
            var searchResults = SearchHelper.Search<FXResultItem>(searchParam, out hitsCount);
            return searchResults.Select(x => x.GetItem()).ToArray();
        }

        public JObject FilterItems(string industry, string department, string service, string business)
        {
            var array = new List<JObject>();
            if (string.IsNullOrEmpty(industry))
            {
                department = service = business = industry;
            }
            else if (string.IsNullOrEmpty(department))
            {
                service = business = department;
            }
            else if (string.IsNullOrEmpty(service))
            {
                business = service;
            }

            try
            {
                DateTime now = DateTime.Now;

                int daysLimit = Math.Max(0, HomePage.SiteSettings.NewArticleAge);
                var listingPage = HomePage.SiteSettings.SnSPage;
                var opco = FX.Core.Utils.Util.OpcoType();
                var items = FilterItems(listingPage, industry, department, service, business);
                if (items != null && items.Any())
                {
                    var resultPages = items.Select(i => Context.Cast<ISnSStandardPage>(i))
                        .OrderByDescending(i => i.ArticleDate)
                        .GroupBy(i => i.Parent.Id);

                    listingPage.Pages.ToList().ForEach(p =>
                    {
                        var temp = (resultPages.FirstOrDefault(x => x.Key == p.Id));
                        if (temp != null)
                        {
                            var category = JObject.FromObject(new
                            {
                                thumbnail = (p.TeaserImage != null && p.TeaserImage.IsValidImage()) ? $"/{opco}{p.TeaserImage.Src}" : string.Empty,
                                title = p.TeaserTitle.Left(FX.Core.Constants.MaxLength.Title),
                                url = $"/{opco}{p.Url}",
                                description = p.TeaserDescription.RawValue,
                                results = temp.Select(r => new
                                {
                                    title = r.TeaserTitle.Left(FX.Core.Constants.MaxLength.Title),
                                    url = $"{opco}{r.Url}",
                                    @new = Util.IsNewArticle(daysLimit, r.ArticleDate, now),
                                }).ToArray()
                            });
                            array.Add(category);
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("Failed to get filter items: Industry('{0}'), Department('{1}'), Service('{2}'), Business('{3}')",
                    industry, department, service, business), ex);
            }

            return JObject.FromObject(new
            {
                categories = array
            });
        }

        private static TreeNode<ITaxonomyNode> CreateTreeNode(string value, IDictionary<string, string> dict = null)
        {
            string text = value;
            if (dict != null)
            {
                dict.TryGetValue(value.ToLower(), out text);
            }
            return new TreeNode<ITaxonomyNode>(new TaxonomyNode(value, text));
        }

        public static TreeNode<ITaxonomyNode> CreateTreeNode(FacetItemGroup<FXResultItem> facetResult,
            IDictionary<string, string> dict)
        {
            Assert.ArgumentNotNull(facetResult, "facetResult");
            Assert.ArgumentNotNull(dict, "dict");

            var rootNode = CreateTreeNode(facetResult.Caption);
            if (facetResult.Values != null && facetResult.Values.Any())
            {
                foreach (var facetItem in facetResult.Values)
                {
                    string[] taxonomies = facetItem.Value.Split(new string[] { Constants.SnSTaxonomy.TaxonomySeparator }, StringSplitOptions.None);
                    foreach (var taxonomy in taxonomies)
                    {
                        var parent = rootNode;

                        string[] paths = taxonomy.Split(new string[] { Constants.SnSTaxonomy.TagSeparator }, StringSplitOptions.None);
                        foreach (string path in paths)
                        {
                            TreeNode<ITaxonomyNode> item = parent.FindNodeByValue(path);
                            if (item == null)
                            {
                                item = CreateTreeNode(path, dict);
                                parent.Add(item);
                            }
                            parent = item;
                        }
                    }
                }
            }
            return rootNode;
        }

        public TreeNode<ITaxonomyNode> GetRefiners(IDictionary<string, string> dict)
        {
            const string categoryName = "Taxonomy";
            var searchParam = new SearchParam<FXResultItem>(Context.Database);
            searchParam.AddTemplateRestriction(new ID(Templates.SnSStandardPage.Id));
            searchParam.Page = 0;
            searchParam.PageSize = 1;

            var homePageId = new ID(HomePage.Id);
            var whereClause = PredicateBuilder.True<FXResultItem>();
            whereClause = whereClause.And(x => x.Paths.Contains(homePageId));
            searchParam.WhereClause = whereClause;

            searchParam.AddFacet(new FacetItemGroup<FXResultItem>(x => x.SnsTaxonomy, categoryName, null));

            int hitsCount = 0;
            var searchResults = SearchHelper.Search<FXResultItem>(searchParam, out hitsCount);
            return CreateTreeNode(searchParam.Facets.FirstOrDefault(), dict);
        }

        public FormDataResponse FormSubmission(WffmFormParams formdata)
        {
            FormDataResponse formDataResponse = new FormDataResponse();
            try
            {
                Sitecore.Globalization.Language language = null;
                if (formdata != null && !string.IsNullOrEmpty(formdata.Language) && Sitecore.Globalization.Language.TryParse(formdata.Language, out language))
                {
                    Sitecore.Context.Language = language;
                }

                Item formItem = Sitecore.Context.Database.GetItem(formdata.strings[2].value, Sitecore.Context.Language);
                Database web = Factory.GetDatabase("web");

                if (formItem != null && web != null && formdata?.strings?.Count > 0)
                {
                    var watch = System.Diagnostics.Stopwatch.StartNew();
                    List<ControlResult> results = GetFormFieldsFromSitecoreExcludingCounter(formItem, formdata);
                    watch.Stop();
                    Log.Info($"time taken to get results collection in FormSubmission() method: {watch.ElapsedMilliseconds}", this);

                    var watch1 = System.Diagnostics.Stopwatch.StartNew();

                    var simpleForm = new SitecoreSimpleForm(formItem);
                    var saveActionXml = simpleForm.FormItem.SaveActions;
                    var actionList = Sitecore.Form.Core.ContentEditor.Data.ListDefinition.Parse(saveActionXml);                    
                    var SubmitActionManager = (IActionExecutor)Factory.CreateObject("wffm/wffmActionExecutor", false);
                    var actionDefinitions = new List<ActionDefinition>();

                    actionDefinitions.AddRange(actionList.Groups.SelectMany(x => x.ListItems).Select(li => new ActionDefinition(li.ItemID, li.Parameters) { UniqueKey = li.Unicid }));
                    Sitecore.Form.Core.WffmActionEvent sessionID = new Sitecore.Form.Core.WffmActionEvent();// SessionIDGuid
                    var submissionResult = SubmitActionManager.ExecuteSaving(formItem.ID, results.ToArray(), actionDefinitions.ToArray(), true, ID.Parse(sessionID.SessionIDGuid));

                    string successMode = formItem.Fields["Success Mode"]?.Value;

                    watch1.Stop();
                    Log.Info($"time taken to save form data in FormSubmission() method: {watch1.ElapsedMilliseconds}", this);

                    var watch2 = System.Diagnostics.Stopwatch.StartNew();

                    if (!string.IsNullOrEmpty(successMode) && successMode.Contains("{F4D50806-6B89-4F2D-89FE-F77FC0A07D48}"))
                    {
                        Sitecore.Data.Fields.LinkField linkField = (Sitecore.Data.Fields.LinkField)formItem.Fields["Success Page"];
                        string redirectUrl = string.Empty;
                        if (linkField != null)
                        {
                            if (linkField.LinkType.Equals(Constants.LinkTypeConstant.Internal))
                            {
                                Item targetItem = web.GetItem(linkField.Url, Sitecore.Context.Language);
                                if (targetItem != null)
                                {
                                    redirectUrl = Sitecore.Links.LinkManager.GetItemUrl(targetItem);
                                }
                            }
                            else if (linkField.LinkType.Equals(Constants.LinkTypeConstant.External))
                            {
                                redirectUrl = linkField.Url;
                            }
                        }

                        formDataResponse.ReturnToUrl = redirectUrl;

                    }
                    else
                    {
                        formDataResponse.SummaryText = formItem.Fields["Success Message"]?.Value;
                    }

                    watch2.Stop();
                    Log.Info($"time taken to get the thank you page in FormSubmission() method: {watch2.ElapsedMilliseconds}", this);

                    var watch3 = System.Diagnostics.Stopwatch.StartNew();

                    SaveFbFormData(formdata, formItem);

                    watch3.Stop();
                    Log.Info($"time taken to save the data in custom table in FormSubmission() method: {watch3.ElapsedMilliseconds}", this);

                }

            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error($"Error occurred in method FormSubmission() : {ex.ToString()}", this);
            }

            return formDataResponse;
        }

        public void SaveFbFormData(WffmFormParams formdata, Item formItem)
        {
            try
            {
                if (formdata != null && !string.IsNullOrEmpty(formdata.url) && formdata.strings?.Count > 0 && formItem != null && formItem.Fields["Save Form Data To Storage"] != null && formItem.Fields["Save Form Data To Storage"].Value.Equals("1"))
                {

                    string formDataInXml = ToXML(formdata);
                    string country = Sitecore.Context.Site.Name;
                    string language = Sitecore.Context.Language.Name;
                    string formId = formdata.strings[2].value;
                    string _connectionString = Sitecore.Configuration.Settings.GetConnectionString("web");
                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        String query = "insert into FbFormData(FormId, FormData, Country, Langauge, FormName) values(@formId, @formData, @country, @language, @formName)";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@formId", formId);
                            command.Parameters.AddWithValue("@formData", formDataInXml);
                            command.Parameters.AddWithValue("@country", country);
                            command.Parameters.AddWithValue("@language", language);
                            command.Parameters.AddWithValue("@formName", formItem.Name);

                            connection.Open();
                            int result = command.ExecuteNonQuery();

                            if (result < 0)
                            {
                                Sitecore.Diagnostics.Log.Info($"Data is not saved in FbFormDataTable", this);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error($"Error occurred while saving the data in FbFormDataTable :: {ex.ToString()}", this);
            }
        }

        public string ToXML(Object oObject)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlSerializer xmlSerializer = new XmlSerializer(oObject.GetType());
            using (MemoryStream xmlStream = new MemoryStream())
            {
                xmlSerializer.Serialize(xmlStream, oObject);
                xmlStream.Position = 0;
                xmlDoc.Load(xmlStream);
                return xmlDoc.InnerXml;
            }
        }

        public JObject GetRefinerOptions()
        {
            try
            {
                List<ITaxonomyItem> list = new List<ITaxonomyItem>();
                list.AddRange(GetTaxonomyItems(IndustriesId));
                list.AddRange(GetTaxonomyItems(DepartmentsId));
                list.AddRange(GetTaxonomyItems(ServicesId));
                list.AddRange(GetTaxonomyItems(BusinessTypesId));

                var refiners = GetRefiners(list.ToDictionary(x => Util.GetNormalizedId(x.Id), x => { return "$name".Equals(x.TaxonomyItemName) ? x.GetDisplayName() : x.TaxonomyItemName; }));
                var result = JObject.FromObject(new
                {
                    industries = from i in refiners.Nodes.OrderBy(x => x.Text)
                                 select new
                                 {
                                     id = i.Value,
                                     name = i.Text,
                                     departments = from d in i.Nodes.OrderBy(x => x.Text)
                                                   select new
                                                   {
                                                       id = d.Value,
                                                       name = d.Text,
                                                       services = from s in d.Nodes.OrderBy(x => x.Text)
                                                                  select new
                                                                  {
                                                                      id = s.Value,
                                                                      name = s.Text,
                                                                      businesses = from b in s.Nodes.OrderBy(x => x.Text)
                                                                                   select new
                                                                                   {
                                                                                       id = b.Value,
                                                                                       name = b.Text,
                                                                                   }
                                                                  }
                                                   }
                                 }
                });
                return result;
            }
            catch (Exception ex)
            {
                Log.Error("Failed to get refiners", ex);

                var array = new List<JObject>();
                return JObject.FromObject(new
                {
                    industries = array
                });
            }
        }

        public IEnumerable<FXResultItem> GetRelatedCareerPages(ICareerDetailPage page, int pageSize, ID[] includeTemplates = null)
        {
            if (page == null || page.Related == null || page.Related.Count() == 0)
            {
                return new FXResultItem[0];
            }

            var searchParam = new SearchParam<FXResultItem>(this.Context.Database);
            if (includeTemplates != null && includeTemplates.Any())
            {
                includeTemplates.ToList().ForEach(templateId =>
                {
                    searchParam.AddTemplateRestriction(templateId);
                });
            }
            searchParam.PageSize = pageSize;

            // make sure not to get current item
            var pageID = new Sitecore.Data.ID(page.Id);
            var boolClause = PredicateBuilder.True<FXResultItem>();
            boolClause = boolClause.And(x => x.ItemId != pageID);

            // check if current site
            var homeId = new Sitecore.Data.ID(HomePage.Id);
            boolClause = boolClause.And(x => x.Paths.Contains(homeId));

            // match the current related tags
            var whereClause = PredicateBuilder.True<FXResultItem>();
            foreach (var t in page.Related)
            {
                var tID = new Sitecore.Data.ID(t.Id);
                whereClause = whereClause.Or(x => x.Related.Contains(tID));
            }

            whereClause = whereClause.And(x => x.Language == Sitecore.Context.Language.Name);

            boolClause = boolClause.And(whereClause);
            searchParam.WhereClause = boolClause;

            int count = 0;
            var results = SearchHelper.Search<FXResultItem>(searchParam, out count);

            return results.ToArray();
        }

        public IEnumerable<FXResultItem> GetRelatedPages(IStandardPage page, int pageSize, ID[] includeTemplates = null)
        {
            if (page == null || page.Related == null || page.Related.Count() == 0)
            {
                return new FXResultItem[0];
            }

            var searchParam = new SearchParam<FXResultItem>(this.Context.Database);
            if (includeTemplates != null && includeTemplates.Any())
            {
                includeTemplates.ToList().ForEach(templateId =>
                {
                    searchParam.AddTemplateRestriction(templateId);
                });
            }
            searchParam.PageSize = pageSize;

            // make sure not to get current item
            var pageID = new Sitecore.Data.ID(page.Id);
            var boolClause = PredicateBuilder.True<FXResultItem>();
            boolClause = boolClause.And(x => x.ItemId != pageID);

            // check if current site
            var homeId = new Sitecore.Data.ID(HomePage.Id);
            boolClause = boolClause.And(x => x.Paths.Contains(homeId));

            // match the current related tags
            var whereClause = PredicateBuilder.True<FXResultItem>();
            foreach (var t in page.Related)
            {
                var tID = new Sitecore.Data.ID(t.Id);
                whereClause = whereClause.Or(x => x.Related.Contains(tID));
            }

            whereClause = whereClause.And(x => x.Language == Sitecore.Context.Language.Name);

            boolClause = boolClause.And(whereClause);
            searchParam.WhereClause = boolClause;

            int count = 0;
            var results = SearchHelper.Search<FXResultItem>(searchParam, out count);

            return results.ToArray();
        }

        public IEnumerable<FXResultItem> GetRelatedProducts(IStandardPage page, int pageSize, ID[] includeTemplates = null)
        {
            if (page == null || page.Related == null || page.Related.Count() == 0)
            {
                return new FXResultItem[0];
            }

            var searchParam = new SearchParam<FXResultItem>(this.Context.Database);
            if (includeTemplates != null && includeTemplates.Any())
            {
                includeTemplates.ToList().ForEach(templateId =>
                {
                    searchParam.AddTemplateRestriction(templateId);
                });
            }
            searchParam.PageSize = pageSize;

            // make sure not to get current item
            var pageID = new Sitecore.Data.ID(page.Id);
            var boolClause = PredicateBuilder.True<FXResultItem>();
            boolClause = boolClause.And(x => x.ItemId != pageID);

            // get items that is tagged to the country
            var homeId = new Sitecore.Data.ID(HomePage.Id);
            boolClause = boolClause.And(x => x.Countries.Contains(homeId));

            // match the current related tags
            var whereClause = PredicateBuilder.True<FXResultItem>();
            foreach (var t in page.Related)
            {
                var tID = new Sitecore.Data.ID(t.Id);
                whereClause = whereClause.Or(x => x.Related.Contains(tID));
            }

            whereClause = whereClause.And(x => x.Language == Sitecore.Context.Language.Name);

            boolClause = boolClause.And(whereClause);
            searchParam.WhereClause = boolClause;

            int count = 0;
            var results = SearchHelper.Search<FXResultItem>(searchParam, out count);

            return results.ToArray();
        }

        public IEnumerable<FXResultItem> GetRelatedProducts(Guid id, IEnumerable<ITaxonomyItem> related, int pageSize, ID[] includeTemplates = null)
        {
            if (related == null || related.Count() == 0)
            {
                return new FXResultItem[0];
            }

            var searchParam = new SearchParam<FXResultItem>(this.Context.Database);
            if (includeTemplates != null && includeTemplates.Any())
            {
                includeTemplates.ToList().ForEach(templateId =>
                {
                    searchParam.AddTemplateRestriction(templateId);
                });
            }
            searchParam.PageSize = pageSize;

            // make sure not to get current item
            var pageID = new Sitecore.Data.ID(id);
            var boolClause = PredicateBuilder.True<FXResultItem>();
            boolClause = boolClause.And(x => x.ItemId != pageID);

            // get items that is tagged to the country
            var homeId = new Sitecore.Data.ID(HomePage.Id);
            boolClause = boolClause.And(x => x.Countries.Contains(homeId));

            // match the current related tags
            var whereClause = PredicateBuilder.True<FXResultItem>();
            foreach (var t in related)
            {
                var tID = new Sitecore.Data.ID(t.Id);
                whereClause = whereClause.Or(x => x.Related.Contains(tID));
            }

            whereClause = whereClause.And(x => x.Language == Sitecore.Context.Language.Name);

            boolClause = boolClause.And(whereClause);
            searchParam.WhereClause = boolClause;

            int count = 0;
            var results = SearchHelper.Search<FXResultItem>(searchParam, out count);

            return results.ToArray();
        }

        public IEnumerable<FXResultItem> GetRelatedServices(Guid id, IEnumerable<ITaxonomyItem> related, int pageSize, ID[] includeTemplates = null)
        {
            if (related == null || related.Count() == 0)
            {
                return new FXResultItem[0];
            }

            var searchParam = new SearchParam<FXResultItem>(this.Context.Database);
            if (includeTemplates != null && includeTemplates.Any())
            {
                includeTemplates.ToList().ForEach(templateId =>
                {
                    searchParam.AddTemplateRestriction(templateId);
                });
            }
            searchParam.PageSize = pageSize;

            // make sure not to get current item
            var pageID = new Sitecore.Data.ID(id);
            var boolClause = PredicateBuilder.True<FXResultItem>();
            boolClause = boolClause.And(x => x.ItemId != pageID);

            // check if current site
            var homeId = new Sitecore.Data.ID(HomePage.Id);
            boolClause = boolClause.And(x => x.Paths.Contains(homeId));

            // match the current related tags
            var whereClause = PredicateBuilder.True<FXResultItem>();
            foreach (var t in related)
            {
                var tID = new Sitecore.Data.ID(t.Id);
                whereClause = whereClause.Or(x => x.Related.Contains(tID));
            }

            whereClause = whereClause.And(x => x.Language == Sitecore.Context.Language.Name);

            boolClause = boolClause.And(whereClause);
            searchParam.WhereClause = boolClause;

            int count = 0;
            var results = SearchHelper.Search<FXResultItem>(searchParam, out count);

            return results.ToArray();
        }

        public IEnumerable<FXResultItem> GetRelatedCareerPages(ICareerDetailPage page, int pageSize)
        {
            var templateIds = new List<ID>();
            templateIds.Add(new ID(Templates.CareerDetailPage.Id));
            return GetRelatedCareerPages(page, pageSize, templateIds.ToArray());
        }

        public IEnumerable<FXResultItem> GetRelatedPages(IStandardPage page, int pageSize)
        {
            var templateIds = new List<ID>();
            templateIds.Add(new ID(Templates.StandardPage.Id));
            templateIds.Add(new ID(Templates.SnSStandardPage.Id));
            templateIds.Add(new ID(Templates.LandingPage.Id));
            return GetRelatedPages(page, pageSize, templateIds.ToArray());
        }

        public IEnumerable<FXResultItem> GetRelatedProducts(IStandardPage page, int pageSize)
        {
            var templateIds = new List<ID>();
            templateIds.Add(new ID(Templates.PrinterPage.Id));
            templateIds.Add(new ID(Templates.SupplyPage.Id));
            templateIds.Add(new ID(Templates.SoftwarePage.Id));
            return GetRelatedProducts(page, pageSize, templateIds.ToArray());
        }

        public IEnumerable<FXResultItem> GetRelatedProductsAndServices(Guid id, IEnumerable<ITaxonomyItem> related, int pageSize)
        {
            int servicesCount = 0;
            int productsCount = 0;
            var templateIds = new List<ID>();
            templateIds.Add(new ID(Templates.SnSStandardPage.Id));
            IEnumerable<FXResultItem> services = GetRelatedServices(id, related, pageSize, templateIds.ToArray());
            servicesCount = services.Count();

            templateIds.Clear();
            templateIds.Add(new ID(Templates.PrinterPage.Id));
            templateIds.Add(new ID(Templates.SupplyPage.Id));
            templateIds.Add(new ID(Templates.SoftwarePage.Id));
            IEnumerable<FXResultItem> products = GetRelatedProducts(id, related, pageSize, templateIds.ToArray());
            productsCount = products.Count();

            IEnumerable<FXResultItem> productsAndServices = Enumerable.Empty<FXResultItem>();
            if (servicesCount == 0 && productsCount > 0)
            {
                //take 2 from products since there are no services fetched
                return products.Take(2);
            }
            else if (servicesCount > 0 && productsCount == 0)
            {
                //take 2 from services since there are no products fetched
                return services.Take(2);
            }
            else
            {
                //take 1 from each
                productsAndServices = productsAndServices.Concat(products.Take(1)).Concat(services.Take(1));
                return productsAndServices;
            }
        }

        public IEnumerable<FXResultItem> GetProducts(string category, string[] features, int offset, int pageSize, string orderBy, out int hitsCount)
        {
            pageSize = Math.Abs(pageSize);
            int page = pageSize == 0 ? 1 : (offset / pageSize) + 1;
            ID homeID = new ID(HomePage.Id);
            var whereClause = PredicateBuilder.True<FXResultItem>();
            whereClause = whereClause.And(x => x.Paths.Contains(TryParseId(category)) && x.Countries.Contains(homeID));
            whereClause = whereClause.And(x => x.Language == Sitecore.Context.Language.Name);

            if (features != null && features.Any())
            {
                string prefix = category + "_";
                var featureIdsGroup = features
                                    .Where(feature => feature.StartsWith(prefix))
                                    .GroupBy(feature => feature.Substring(prefix.Length, category.Length), feature => TryParseId(feature.Substring(prefix.Length + prefix.Length)));
                foreach (var featureIds in featureIdsGroup)
                {
                    var clause = PredicateBuilder.True<FXResultItem>();
                    foreach (var featureId in featureIds)
                    {
                        clause = clause.Or(x => x.ProductFeatures.Contains(featureId));
                    }
                    whereClause = whereClause.And(clause);
                }
            }

            var searchParam = new SearchParam<FXResultItem>(this.Context.Database);
            searchParam.AddTemplateRestriction(new ID(Templates.PrinterPage.Id));
            searchParam.AddTemplateRestriction(new ID(Templates.SupplyPage.Id));
            searchParam.AddTemplateRestriction(new ID(Templates.SoftwarePage.Id));
            searchParam.Page = page;
            searchParam.PageSize = pageSize;
            searchParam.WhereClause = whereClause;
            if (orderBy != null)
            {
                switch (orderBy.ToLower())
                {
                    case "best seller":
                    case "recommended":
                    default:
                        searchParam.OrderBy = results => results.OrderByDescending(x => x.ArticleDate);
                        break;
                }
            }

            hitsCount = 0;
            var searchResults = SearchHelper.Search<FXResultItem>(searchParam, out hitsCount);
            return searchResults.ToArray();
        }

        public JObject GetProductListing(string category, string[] features, int offset, int pageSize, string orderBy)
        {
            try
            {
                int hitsCount;
                var products = GetProducts(category, features, offset, pageSize, orderBy, out hitsCount);
                var opco = FX.Core.Utils.Util.OpcoType();
                DateTime now = DateTime.Now;
                int daysLimit = Math.Max(0, HomePage.SiteSettings.NewArticleAge);
                var result = JObject.FromObject(new
                {
                    results = from p in products
                                  //let productPage = Context.Cast<FX.Core.Models.Products.IProductPage>(p.GetItem())
                                  //let enquiry = new FX.Core.Models.Products.EnquiryLink(productPage)
                              let enquiry = new FX.Core.Models.Products.EnquiryLink(p)
                              select new
                              {
                                  thumbnail = p.TeaserImage == null ? string.Empty : Sitecore.Resources.Media.HashingUtils.ProtectAssetUrl(string.Format($"/{opco}{p.TeaserImage.Url}?mw=124")),
                                  name = p.TeaserTitle,
                                  url = $"/{opco}{p.Url}",
                                  date = p.ArticleDate.ToLocalTime().ToString("MMM dd yyyy hh:mm:ss"),
                                  buttonlink = enquiry.Url,
                                  buttonlabel = enquiry.Label,
                                  buttontype = enquiry.ButtonType,
                                  desc = p.ProductDescription, //productPage.ProductDescription,
                                  price = 0,
                                  promoted = p.IsPromotedProduct,
                                  bestseller = p.IsBestSellerProduct,
                                  newitem = Util.IsNewArticle(daysLimit, p.ArticleDate.ToLocalTime(), now),
                                  productid = p.ItemId.ToShortID().ToString(),
                              }
                });
                return result;
            }
            catch (Exception ex)
            {
                Log.Error("Failed to get product listing", ex);

                var array = new List<JObject>();
                return JObject.FromObject(new
                {
                    results = array,
                });
            }
        }

        private IEnumerable<FXResultItem> GetInsights(string path, string category, string type, out int hitsCount)
        {
            var whereClause = PredicateBuilder.True<FXResultItem>();
            var pathID = TryParseId(path);
            whereClause = whereClause.And(x => x.Paths.Contains(pathID));
            whereClause = whereClause.And(x => x.ItemId != pathID);
            whereClause = whereClause.And(x => x.Language == Sitecore.Context.Language.Name);

            var catID = TryParseId(category);

            if (!catID.IsNull)
                whereClause = whereClause.And(x => x.InsightTags.Contains(catID));

            var typeID = TryParseId(type);
            if (!typeID.IsNull)
                whereClause = whereClause.And(x => x.InsightTags.Contains(typeID));


            var searchParam = new SearchParam<FXResultItem>(this.Context.Database);
            searchParam.AddTemplateRestriction(new ID(Templates.StandardPage.Id));
            searchParam.AddTemplateRestriction(new ID(Templates.TopicPage.Id));
            searchParam.WhereClause = whereClause;

            hitsCount = 0;
            var searchResults = SearchHelper.Search<FXResultItem>(searchParam, out hitsCount);
            return searchResults.ToArray();
        }

        public IEnumerable<JObject> GetInsightsListing(string path, string category, string type)
        {
            int hitCount;
            var insights = GetInsights(path, category, type, out hitCount).OrderByDescending(x => x.LocalisedArticleDate.DateTime);
            var opco = FX.Core.Utils.Util.OpcoType();
            List<JObject> list = new List<JObject>();

            foreach (var r in insights)
            {
                list.Add(JObject.FromObject(new
                {
                    image = r.TeaserImage != null ? $"/{opco}{r.TeaserImage.Url}" : string.Empty,
                    title = r.TeaserTitle,
                    text = r.TeaserDescription,
                    url = $"/{opco}{r.Url}",
                    category = r.InsightType,
                    date = r.LocalisedArticleDate.DayMonthYearString
                })
                );
            }

            return list;
        }

        public DataTable FormatFormData(DataTable customDbFormDataTable, string selectedFormValue)
        {
            CultureInfo culture = new CultureInfo("en-US");
            DataTable formattedDataTable = new DataTable();
            Database web = Factory.GetDatabase("web");
            List<NameValuePair> fieldsNameValuePair = null;

            try
            {
                if (customDbFormDataTable != null && customDbFormDataTable.Rows?.Count > 0 && !string.IsNullOrEmpty(selectedFormValue))
                {
                    string formLanguage = customDbFormDataTable.Rows[0].ItemArray[4].ToString();
                    fieldsNameValuePair = GetFormFieldsFromSitecore(selectedFormValue, web, formLanguage);

                    ///Add column to datatable
                    if (fieldsNameValuePair?.Count > 0)
                    {
                        foreach (NameValuePair nameValuePair in fieldsNameValuePair)
                        {
                            formattedDataTable.Columns.Add(nameValuePair.name, typeof(String));
                        }

                        formattedDataTable.Columns.Add($"CreatedDate", typeof(String));
                    }

                    foreach (DataRow dataRow in customDbFormDataTable.Rows)
                    {
                        DateTime dateTime = dataRow.ItemArray[2] != null && !string.IsNullOrEmpty(dataRow.ItemArray[2].ToString()) ? Convert.ToDateTime(dataRow.ItemArray[2].ToString(), culture) : new DateTime();
                        string language = dataRow.ItemArray[4] != null && !string.IsNullOrEmpty(dataRow.ItemArray[4].ToString()) ? dataRow.ItemArray[4].ToString() : string.Empty;

                        FormDataFormat formDataFormat = new FormDataFormat(selectedFormValue, dateTime, language);

                        formDataFormat.WffmFormData = LoadFromXMLString(dataRow.ItemArray[1].ToString());

                        if (formDataFormat.WffmFormData?.strings?.Count > 0)
                        {
                            PopulateDataTable(formattedDataTable, formDataFormat.WffmFormData.strings, fieldsNameValuePair, formDataFormat.CreatedDate);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error($"Error occurred while creating the form report in method FormatFormData() : {ex.ToString()}", this);
            }

            return formattedDataTable;
        }

        public void PopulateDataTable(DataTable dataTable, List<NameValuePair> nameValuePairsFromDb, List<NameValuePair> nameValuePairsFromSitecore, DateTime createdDate)
        {
            try
            {
                if (dataTable != null && nameValuePairsFromDb?.Count > 0 && nameValuePairsFromSitecore?.Count > 0)
                {

                    DataRow dataRow = dataTable.NewRow();
                    foreach (NameValuePair nameValuePairSitecoreFormField in nameValuePairsFromSitecore)
                    {
                        NameValuePair keyFieldNameValuePair = nameValuePairsFromDb.Where(x => !string.IsNullOrEmpty(x.value) && x.value.Contains(nameValuePairSitecoreFormField.value))?.FirstOrDefault();
                        string value = string.Empty;

                        if (keyFieldNameValuePair != null && !string.IsNullOrEmpty(keyFieldNameValuePair.name))
                        {
                            string key = keyFieldNameValuePair.name.Substring(0, keyFieldNameValuePair.name.Length - 3);
                            List<NameValuePair> nameValuePairsData = nameValuePairsFromDb.Where(x => !string.IsNullOrEmpty(x.name) && x.name.Contains(key))?.ToList();

                            nameValuePairsData = nameValuePairsData?.Where(x => !x.name.Equals(keyFieldNameValuePair.name))?.ToList();

                            if (nameValuePairsData?.Count > 0)
                            {
                                foreach (NameValuePair nameValue in nameValuePairsData)
                                {
                                    value = value + nameValue.value + ";";
                                }

                                value = value.Substring(0, value.Length - 1);
                            }
                        }

                        dataRow[nameValuePairSitecoreFormField.name] = value;
                    }

                    dataRow["CreatedDate"] = createdDate.ToString();

                    dataTable.Rows.Add(dataRow);
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error($"Error occurred in method PopulateDataTable() : {ex.ToString()}", this);
            }
        }

        public List<NameValuePair> RemoveUnnecessayFields(List<NameValuePair> WffmFormData)
        {
            List<NameValuePair> nameValuePairsList = new List<NameValuePair>();

            try
            {
                if (WffmFormData?.Count > 0)
                {
                    NameValuePair[] nameValuePairsArray = WffmFormData.ToArray();
                    for (int i = 3; i < nameValuePairsArray.Length; i = i + 2)
                    {
                        NameValuePair nameValuePairs = new NameValuePair();
                        nameValuePairs.value = nameValuePairsArray[i + 1].value;
                        nameValuePairs.name = nameValuePairsArray[i].value;

                        nameValuePairsList.Add(nameValuePairs);
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error($"Error occurred in method RemoveUnnecessayFields() : {ex.ToString()}", this);
            }

            return nameValuePairsList;
        }

        public List<NameValuePair> GetFormFieldsFromSitecore(string selectedFormValue, Database database, string formLanguage)
        {
            List<NameValuePair> formFieldsNameValuePair = new List<NameValuePair>();
            try
            {
                if (database != null && !string.IsNullOrEmpty(selectedFormValue))
                {
                    Item formItem = database.GetItem(Sitecore.Data.ID.Parse(selectedFormValue), Sitecore.Globalization.Language.Parse(formLanguage));

                    Item[] fieldItems = formItem.Axes.GetDescendants().Where(x => x.TemplateID.ToString().Equals("{C9E1BF85-800A-4247-A3A3-C3F5DFBFD6AA}")).ToArray();

                    if (fieldItems?.Length > 0)
                    {
                        int count = 0;
                        foreach (Item fieldItem in fieldItems)
                        {
                            count = count + 1;
                            NameValuePair nameValuePair = new NameValuePair();

                            nameValuePair.name = fieldItem.Fields["Title"] != null && !string.IsNullOrEmpty(fieldItem.Fields["Title"].Value) ? $"{fieldItem.Fields["Title"].Value} - {count}" : $"{fieldItem.Name} - {count}";
                            nameValuePair.value = fieldItem.ID.ToString();

                            string fieldLink = fieldItem.Fields["Field Link"].Value;

                            ///exclude captcha field
                            if (!string.IsNullOrEmpty(fieldLink) && !fieldLink.Contains("{7FB270BE-FEFC-49C3-8CB4-947878C099E5}"))
                            {
                                formFieldsNameValuePair.Add(nameValuePair);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error($"Error occurred in method GetFormFieldsFromSitecore() : {ex.ToString()}", this);
            }

            return formFieldsNameValuePair;
        }


        public List<ControlResult> GetFormFieldsFromSitecoreExcludingCounter(Item formItem, WffmFormParams formdata)
        {
            List<ControlResult> results = new List<ControlResult>();
            try
            {
                Item[] fieldItems = formItem?.Axes.GetDescendants().Where(x => x.TemplateID.ToString().Equals("{C9E1BF85-800A-4247-A3A3-C3F5DFBFD6AA}")).ToArray();

                if (fieldItems?.Length > 0)
                {
                    foreach (Item fieldItem in fieldItems)
                    {
                        NameValuePair nameValuePair = new NameValuePair();

                        nameValuePair.name = fieldItem.Fields["Title"] != null && !string.IsNullOrEmpty(fieldItem.Fields["Title"].Value) ? fieldItem.Fields["Title"].Value : fieldItem.Name;
                        nameValuePair.value = fieldItem.ID.ToString();

                        string fieldLink = fieldItem.Fields["Field Link"].Value;

                        if (!string.IsNullOrEmpty(fieldLink) && !fieldLink.Contains("{7FB270BE-FEFC-49C3-8CB4-947878C099E5}"))
                        {
                            //code here
                            NameValuePair keyFieldNameValuePair = formdata.strings.Where(x => !string.IsNullOrEmpty(x.value) && x.value.Contains(nameValuePair.value))?.FirstOrDefault();
                            string value = string.Empty;

                            if (keyFieldNameValuePair != null && !string.IsNullOrEmpty(keyFieldNameValuePair.name))
                            {
                                string key = keyFieldNameValuePair.name.Substring(0, keyFieldNameValuePair.name.Length - 3);

                                List<NameValuePair> nameValuePairsData = formdata.strings.Where(x => !string.IsNullOrEmpty(x.name) && x.name.Contains(key))?.ToList();

                                nameValuePairsData = nameValuePairsData?.Where(x => !x.name.Equals(keyFieldNameValuePair.name))?.ToList();

                                NameValuePair nameValuePairFormData = nameValuePairsData?.FirstOrDefault();

                                value = nameValuePairFormData != null && !string.IsNullOrEmpty(nameValuePairFormData.value) ? nameValuePairFormData.value : string.Empty;
                            }

                            results.Add(new ControlResult(nameValuePair.value, fieldItem.Name, value, string.Empty));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error($"Error occurred in method GetFormFieldsFromSitecore() : {ex.ToString()}", this);
            }

            return results;
        }

        public string RemoveAllNamespaces(string xmlDocument)
        {
            XElement xmlDocumentWithoutNs = RemoveAllNamespaces(XElement.Parse(xmlDocument));

            return xmlDocumentWithoutNs.ToString();
        }

        //Core recursion function
        private XElement RemoveAllNamespaces(XElement xmlDocument)
        {
            if (!xmlDocument.HasElements)
            {
                XElement xElement = new XElement(xmlDocument.Name.LocalName);
                xElement.Value = xmlDocument.Value;

                foreach (XAttribute attribute in xmlDocument.Attributes())
                    xElement.Add(attribute);

                return xElement;
            }
            return new XElement(xmlDocument.Name.LocalName, xmlDocument.Elements().Select(el => RemoveAllNamespaces(el)));
        }

        public WffmFormParams LoadFromXMLString(string xmlText)
        {
            try
            {
                if (!string.IsNullOrEmpty(xmlText))
                {
                    xmlText = RemoveAllNamespaces(xmlText);
                    using (var stringReader = new System.IO.StringReader(xmlText))
                    {
                        var serializer = new XmlSerializer(typeof(WffmFormParams));
                        return serializer.Deserialize(stringReader) as WffmFormParams;
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error($"Error occurred in method LoadFromXMLString() : {ex.ToString()}", this);
            }

            return null;
        }
    }
}
