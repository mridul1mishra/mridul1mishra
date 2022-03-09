using FX.Core.Serialization;
using FX.Core.Services;
using Sitecore.Data.Items;
using Sitecore.Publishing;
using Sitecore.SecurityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Description;
using static Sitecore.Configuration.Settings;
using http = System.Web.Http;

namespace FX.Website.Controllers
{
    public class DataSyncApiController : http.ApiController
    {
        private SitecoreItemCreationService __service { get; set; }
        public DataSyncApiController()
        {
            __service = new SitecoreItemCreationService();
        }
        [ResponseType(typeof(string))]
        public HttpResponseMessage CreateItems(IEnumerable<SerializableItem> items)
        {
            Sitecore.Diagnostics.Log.Info("[ALIYUN] Data Sync Started. Items being added: " + items.Count(), this);
            var failedItems = new List<string>();
            List<Item> createdItems = new List<Item>();
            using (new SecurityDisabler())
            {
                foreach (var item in items)
                {
                    try
                    {
                        Sitecore.Diagnostics.Log.Info("[ALIYUN] Attempting to add item: " + item.Name, this);
                        var itemTuple = __service.CreateOrUpdateItem(item);

                        createdItems.Add(itemTuple.Item1);

                        if(!string.IsNullOrEmpty(itemTuple.Item2))
                        {
                            failedItems.Add(itemTuple.Item2);
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        var errorMsg = "[ALIYUN] Error adding item: " + item.Paths.FullPath + ex.Message + ex.StackTrace;
                        Sitecore.Diagnostics.Log.Error(errorMsg, this);
                        failedItems.Add(item.Paths.FullPath);
                    }
                }
                Sitecore.Diagnostics.Log.Info("[ALIYUN] Data Sync Complete. Publishing all items", this);
                __service.PublishAllItems(createdItems);
            }
            if (failedItems.Any())
                Sitecore.Diagnostics.Log.Error("[ALIYUN] Failed Items : " + failedItems.Count() + "\n" + string.Join("\n", failedItems), this);
            
            //Sitecore.Diagnostics.Log.Info("Current json received: " + Newtonsoft.Json.JsonConvert.SerializeObject(items), this);
            Sitecore.Diagnostics.Log.Info("[ALIYUN] Data Sync Ended", this);
            return (failedItems.Any()) ? Request.CreateResponse<string>(HttpStatusCode.OK, "[ALIYUN] Failed Items: " + failedItems.Count() + "\n" + string.Join("\n", failedItems))
                : Request.CreateResponse<string>(HttpStatusCode.OK, "OK");
        }
        [ResponseType(typeof(string))]
        public HttpResponseMessage VerifyItem(SerializableItem item)
        {
            try
            {
                var result = __service.VerifyItem(item);
                return Request.CreateResponse<string>(HttpStatusCode.OK, result);
            }
            catch(Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("[ALIYUN] Verify item failed due to: " + ex.Message + ex.StackTrace, this);
                return Request.CreateResponse<string>(HttpStatusCode.OK, "[ALIYUN] Verify item failed due to: " + ex.Message + ex.StackTrace);
            }
            //return Request.CreateResponse<string>(HttpStatusCode.OK, "");
        }
    }
}