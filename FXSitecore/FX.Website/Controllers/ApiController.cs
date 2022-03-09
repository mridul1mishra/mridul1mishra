using FX.Core.Models.DTO;
using FX.Core.Models.Form;
using FX.Core.Utils;
using Glass.Mapper.Sc.Web.Mvc;
using Newtonsoft.Json;
using Sitecore.Data.Items;
using Sitecore.Form.Web.UI.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Services;

namespace FX.Website.Controllers
{
    public class ApiController : GlassController
    {
        public ActionResult GetFilterOptions()
        {
            TaxonomyService taxonomyService = new TaxonomyService(this.SitecoreContext);
            //var filterOptions = taxonomyService.GetFilterOptions();
            var filterOptions = taxonomyService.GetRefinerOptions();
            return Content(filterOptions.ToString(Newtonsoft.Json.Formatting.None), "application/json");
        }

        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        [WebMethod]
        public ActionResult RPFormData(WffmFormParams formObj)
        {
            TaxonomyService taxonomyService = new TaxonomyService(this.SitecoreContext);
            FormDataResponse response = taxonomyService.FormSubmission(formObj);
            return Content(JsonConvert.SerializeObject(response));
        }

        public ActionResult GetFilterResults(string industry, string department, string service, string business)
        {
            TaxonomyService taxonomyService = new TaxonomyService(this.SitecoreContext);
            var results = taxonomyService.GetFilterResults(industry, department, service, business);
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(results), "application/json");
        }

        [AcceptVerbs("Get", "Post")]
        public ActionResult GetProductListing(string categoryField, string[] features = null)
        {
            int offset = 0;
            int pageSize = 0;
            string orderBy = null;
            TaxonomyService taxonomyService = new TaxonomyService(this.SitecoreContext);
            var results = taxonomyService.GetProductListing(categoryField, features, offset, pageSize, orderBy);
            return Content(results.ToString(Newtonsoft.Json.Formatting.None, new DateTimeFormatConverter(FX.Core.Constants.DateTimeFormat.FullDateTime)), "application/json");
        }

        public ActionResult GetNewsByCategory(string category, int year, int page)
        {
            int pageSize = 0;
            string orderBy = null;
            NewsroomService newsroomService = new NewsroomService(this.SitecoreContext);
            var results = newsroomService.GetNewsListing(category, year, page, pageSize, orderBy);
            return Content(results.ToString(Newtonsoft.Json.Formatting.None, new DateTimeFormatConverter(FX.Core.Constants.DateTimeFormat.DateOnly)), "application/json");
        }

        public ActionResult RegisterEvent(string name, string definitionID, string itemID, string data, string text)
        {
            var pageEventService = new PageEventService();
            pageEventService.RegisterPageEvent(name, definitionID, itemID, data, text);
            return Content("");
        }

        [AcceptVerbs("Get", "Post")]
        public ActionResult AddToICalendar(string downloadFileName, string eventId)
        {
            EventService eventService = new EventService(this.SitecoreContext);
            var results = eventService.MakeDayEvent(eventId);
            var bytes = Encoding.UTF8.GetBytes(results);
            return this.File(bytes, "text/calendar", downloadFileName);
        }

        [AcceptVerbs("Get", "Post")]
        public ActionResult GetEventsListing(string country, string industry, string year, string month, int page = 0, int pageSize = 10)
        {
            EventService eventService = new EventService(this.SitecoreContext);
            var results = eventService.GetEventsListing(country, industry, year, month, page, pageSize);
            return Content(results.ToString(Newtonsoft.Json.Formatting.None, new DateTimeFormatConverter(FX.Core.Constants.DateTimeFormat.DateOnly)), "application/json");
        }

        [AcceptVerbs("Get", "Post")]
        public ActionResult GetStoryListing(string country, string solution, string product, string industry, string year, int page = 0, int pageSize = 9)
        {
            SuccessStoryService successstoryService = new SuccessStoryService(this.SitecoreContext);
            var results = successstoryService.GetSuccessStoriesListing(country, solution, product, industry, year, page, pageSize);
            return Content(results.ToString(Newtonsoft.Json.Formatting.None, new DateTimeFormatConverter(FX.Core.Constants.DateTimeFormat.DateOnly)), "application/json");
        }

        [AcceptVerbs("Get", "Post")]
        //public ActionResult GetCareerListing(string[] careerlocations = null, string[] careerspecialisations = null, string[] careerjobtypes = null, string[] careertitles = null, string[] careerpostingdates = null)
        public ActionResult GetCareerListing()
        {
            var request = HttpContext.Request;
            var searchArgs = new CareerSearch();
            searchArgs.Arguments = request.Params.AllKeys
                .Where(k => k.ToLower().Contains("career")).Select(k =>
            new CareerFieldArgument<string>
            {
                Key = k,
                Values = request.Params.GetValues(k)
            });


            //hackish way to segregate the date type filter from the rest

            searchArgs.Arguments = searchArgs.Arguments.Where(a => a.Key != "careerpostingdates");

            searchArgs.DateItemArgument = new CareerFieldArgument<string>()
            {
                Key = "careerpostingdates",
                Values = request.Params.GetValues("careerpostingdates")
            };

            CareerService careerService = new CareerService(this.SitecoreContext);
            //var results = careerService.GetCareerListing(careerlocations, careerspecialisations, careerjobtypes, careertitles, careerpostingdates);
            var results = careerService.GetCareerListing(searchArgs);
            return Content(results.ToString(Newtonsoft.Json.Formatting.None, new DateTimeFormatConverter(FX.Core.Constants.DateTimeFormat.DateOnly)), "application/json");
        }

        [HttpPost]
        public ActionResult TriggerGoal(string goalId)
        {
            var goalService = new GoalService();
            goalService.TriggerGoal(goalId);
            return Content("");
        }
        [AcceptVerbs("Get")]
        public ActionResult GetInsights(string path, string category, string type)
        {
            TaxonomyService taxonomyService = new TaxonomyService(this.SitecoreContext);
            var results = taxonomyService.GetInsightsListing(path, category, type);
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(results), "application/json");
        }

        [AcceptVerbs("Get")]
        public ActionResult GetResellers(string Province, string City, string ProductCategory, string ProductName, string ResellerType, string DealerName, bool useCache = true)
        {
            var resellerService = new ResellerService(this.SitecoreContext, useCache);
            var results = resellerService.GetResellers(Province, City, ProductCategory, ProductName, ResellerType, DealerName);
            return Content(results, "application/json");
        }


        [HttpPost]
        [AllowAnonymous]
        public ActionResult validateToken(string token)
        {
            var grecaptcha = new CaptchaService();
            var CaptchaValid = grecaptcha.captchaMessage(token);
            return Content(CaptchaValid);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult FormReporting()
        {
            FormReporting formReportingDataModel = new FormReporting();

            try
            {
                Sitecore.Data.Database web = Sitecore.Configuration.Factory.GetDatabase("web");

                string formRootItemPath = Sitecore.Context.Site.StartPath + "/Content Stores/Forms";
                Item formRootItem = web.GetItem(formRootItemPath);
                if (formRootItem != null)
                {
                    Item[] formItems = formRootItem.GetChildren().Where(x => x.TemplateID.ToString().Equals("{FFB1DA32-2764-47DB-83B0-95B843546A7E}")).ToArray();

                    if (formItems?.Length > 0)
                    {
                        foreach (Item formItem in formItems)
                        {
                            formReportingDataModel.Forms.Add(new SelectListItem() { Text = formItem.Name, Value = formItem.ID.ToString() });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error($"Error occurred while getting the form: {ex.ToString()}", this);
            }

            return View("~/Views/FX/Shared/FormReporting.cshtml", formReportingDataModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult FormReporting(FormReportingParams formReportingParams)
        {
            FormReportingData formReportingDataModel = new FormReportingData();
            try
            {
                TaxonomyService taxonomyService = new TaxonomyService(this.SitecoreContext);
                DataTable customDbFormDataTable = new DataTable();

                if (formReportingParams != null && !string.IsNullOrEmpty(formReportingParams.SelectedFormValue))
                {
                    string _connectionString = Sitecore.Configuration.Settings.GetConnectionString("web");
                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        String query = $"select * from FbFormData where FormId = '{formReportingParams.SelectedFormValue}'";

                        if (!string.IsNullOrEmpty(formReportingParams.FromDateRangeValue) && !string.IsNullOrEmpty(formReportingParams.ToDateRangeValue))
                        {
                            try
                            {
                                string toDate = DateTime.ParseExact(formReportingParams.ToDateRangeValue, "dd/MM/yyyy", new CultureInfo("en-US")).AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss.fff");
                                string fromDate = DateTime.ParseExact(formReportingParams.FromDateRangeValue, "dd/MM/yyyy", new CultureInfo("en-US")).AddSeconds(1).ToString("yyyy-MM-dd HH:mm:ss.fff");

                                query = $"select * from FbFormData where FormId = '{formReportingParams.SelectedFormValue}' and  CreatedDate >= '{fromDate}' and CreatedDate <= '{toDate}'";
                            }
                            catch (Exception ex)
                            {
                                Sitecore.Diagnostics.Log.Error($"Invalid date format: {ex.ToString()}", this);
                            }
                        }

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            using (SqlDataAdapter sda = new SqlDataAdapter())
                            {
                                sda.SelectCommand = command;
                                sda.Fill(customDbFormDataTable);
                                formReportingDataModel.FormDataTable = taxonomyService.FormatFormData(customDbFormDataTable, formReportingParams.SelectedFormValue);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error($"Error occurred while getting the form report from database: {ex.ToString()}", this);
            }
            return View("~/Views/FX/Shared/FormReportingData.cshtml", formReportingDataModel);
        }
        [HttpGet]
        [AllowAnonymous]
        public JsonResult FormMigration()
        {
            List<FormMigration> formReportingDataModel = new List<FormMigration>();

            try
            {
                Sitecore.Data.Database web = Sitecore.Configuration.Factory.GetDatabase("master");

                string formRootItemPath = Sitecore.Context.Site.StartPath + "/Content Stores/Forms";
                Item formRootItem = web.GetItem(formRootItemPath);
                if (formRootItem != null)
                {
                    Item[] formItems = formRootItem.GetChildren().Where(x => x.TemplateID.ToString().Equals("{FFB1DA32-2764-47DB-83B0-95B843546A7E}")).ToArray();

                    if (formItems?.Length > 0)
                    {
                        foreach (Item formItem in formItems)
                        {
                            List<FieldItem> childFields = new List<FieldItem>();
                            if (formItem.GetChildren().Count>1)
                            {
                                foreach (var formdata in formItem.GetChildren().ToArray())
                                {
                                    childFields.Add(new FieldItem() { FieldId = formdata.ID.ToString(), Name = formdata.Name, FieldTitle = formdata["Title"], FieldType = formdata["Field Link"] });
                                }
                            }
                            formReportingDataModel.Add(new FormMigration { Text = formItem.Name, ValueId = formItem.ID.ToString(), child = childFields });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error($"Error occurred while getting the form: {ex.ToString()}", this);
            }

            return Json(formReportingDataModel, JsonRequestBehavior.AllowGet);
        }
    }
}