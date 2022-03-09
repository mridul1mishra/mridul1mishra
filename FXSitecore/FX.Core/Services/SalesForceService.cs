using FX.Core.Models.SalesForce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FX.Core.Services
{
    public class SalesForceService
    {
        private HttpClient __httpClient { get; set; }
        private string __endPoint { get; set; }
        public SalesForceService() { }
        public SalesForceService(HttpClient httpClient)
        {
            __httpClient = httpClient;
        }
        public SalesForceService(HttpClient httpClient, string endPoint)
        {
            __httpClient = httpClient;
            __endPoint = endPoint;
        }
        public bool SaveEntry(SalesForceEntry entry)
        {
            System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            var encodedContent = new FormUrlEncodedContent(entry.KeyValuePairs);
            var task = Task.Run(() => __httpClient.PostAsync(__endPoint, encodedContent));

            //This line should be removed if we switch to using dependency injection
            Sitecore.Diagnostics.Log.Info("Sales Force content sent: " + encodedContent.ReadAsStringAsync().Result, this);

            var result = task.Result;
            if (result != null)
                Sitecore.Diagnostics.Log.Info(result.Content.ReadAsStringAsync().Result, this);
            return result != null ? (result.StatusCode == HttpStatusCode.OK ? true : false) : false;
        }
    }
}
