using Sitecore.Forms.Mvc.Controllers.Filters;
using Sitecore.Forms.Mvc.Controllers.ModelBinders;
using Sitecore.Forms.Mvc.Interfaces;
using Sitecore.Forms.Mvc.Models;
using Sitecore.Forms.Mvc.ViewModels;
using Sitecore.Mvc.Controllers;
using Sitecore.WFFM.Abstractions.Dependencies;
using Sitecore.WFFM.Abstractions.Shared;
using System;
using System.IO;
using System.Web.Mvc;
using Sitecore.Configuration;
using Sitecore.Forms.Mvc.Controllers;
using Sitecore.Diagnostics;
using System.Web;

namespace FX.Website.Controllers
{
    [ModelBinder(typeof(FormModelBinder))]
    public class RPFormController: SitecoreController
    {
        private readonly IAnalyticsTracker analyticsTracker;

        public RPFormController()
      : this((IRepository<FormModel>)Factory.CreateObject(Sitecore.Forms.Mvc.Constants.FormRepository, true), (IAutoMapper<FormModel, FormViewModel>)Factory.CreateObject(Sitecore.Forms.Mvc.Constants.FormAutoMapper, true), (IFormProcessor<FormModel>)Factory.CreateObject(Sitecore.Forms.Mvc.Constants.FormProcessor, true), DependenciesManager.AnalyticsTracker)
        {
        }

        
        public RPFormController(
      IRepository<FormModel> repository,
      IAutoMapper<FormModel, FormViewModel> mapper,
      IFormProcessor<FormModel> processor,
      IAnalyticsTracker analyticsTracker)
        {
            Assert.ArgumentNotNull((object)repository, nameof(repository));
            Assert.ArgumentNotNull((object)mapper, nameof(mapper));
            Assert.ArgumentNotNull((object)processor, nameof(processor));
            Assert.ArgumentNotNull((object)analyticsTracker, nameof(analyticsTracker));
            this.FormRepository = repository;
            this.Mapper = mapper;
            this.FormProcessor = processor;

            this.analyticsTracker = analyticsTracker;
        }
        public IRepository<FormModel> FormRepository { get; private set; }

        public IAutoMapper<FormModel, FormViewModel> Mapper { get; private set; }

        public IFormProcessor<FormModel> FormProcessor { get; private set; }


        
        [Sitecore.Forms.Mvc.Attributes.FormErrorHandler]
        [WffmValidateAntiForgeryToken]
        [HttpPost]
        [SubmittedFormHandler]
        public virtual ActionResult Index([ModelBinder(typeof(FormModelBinder))] FormViewModel formViewModel)
        {
            HttpCookie userInfo = new HttpCookie("userInfo");
            userInfo["UserName"] = "This is a test userinfo on the RPform";

            userInfo.Expires.Add(new TimeSpan(0, 1, 0));
            HttpContext.Response.Cookies.Add(userInfo);
            this.analyticsTracker.InitializeTracker();
            return (ActionResult)this.ProcessedForm(formViewModel);

        }
        public virtual Sitecore.Forms.Mvc.Controllers.ProcessedFormResult<FormModel, FormViewModel> ProcessedForm(
      FormViewModel viewModel,
      string viewName = "")
        {
            
            Log.Warn("This is a log from mridul index form", typeof(RPFormController));
            ProcessedFormResult<FormModel, FormViewModel> processedFormResult1 = new ProcessedFormResult<FormModel, FormViewModel>(this.FormRepository, this.Mapper, this.FormProcessor, viewModel);
            processedFormResult1.ViewData = this.ViewData;
            processedFormResult1.TempData = this.TempData;
            processedFormResult1.ViewEngineCollection = this.ViewEngineCollection;
            Sitecore.Forms.Mvc.Controllers.ProcessedFormResult<FormModel, FormViewModel> processedFormResult2 = processedFormResult1;
            if (!string.IsNullOrEmpty(viewName))
                processedFormResult2.ViewName = viewName;
            return processedFormResult2;
        }
    }
}