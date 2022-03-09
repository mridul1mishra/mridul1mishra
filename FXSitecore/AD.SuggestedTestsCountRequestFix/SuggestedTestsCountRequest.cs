using Sitecore.ContentTesting;
using Sitecore.ContentTesting.ContentSearch.Models;
using Sitecore.ContentTesting.Data;
using Sitecore.ContentTesting.Requests.ExperienceEditor;
using Sitecore.Diagnostics;
using Sitecore.ExperienceEditor.Speak.Server.Responses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AD.SuggestedTestsCountRequestFix
{
    public class SuggestedTestsCountRequest : PipelineProcessorRequestBase
    {
        private readonly IContentTestStore contentTestStore;

        public SuggestedTestsCountRequest() : this(ContentTestingFactory.Instance.ContentTestStore)
        {
        }

        public SuggestedTestsCountRequest(IContentTestStore contentTestStore)
        {
            Assert.ArgumentNotNull(contentTestStore, "contentTestStore");
            this.contentTestStore = contentTestStore;
        }

        public override PipelineProcessorResponseValue ProcessRequest()
        {
            IEnumerable<SuggestedTestSearchResultItem> suggestedTests = this.contentTestStore.GetSuggestedTests(null, null);
            return new PipelineProcessorResponseValue { Value = suggestedTests.Count<SuggestedTestSearchResultItem>() };
        }

        [Obsolete("Use ProcessRequest() instead")]
        public override PipelineProcessorResponseValue Process()
        {
            return this.ProcessRequest();
        }
    }
}
