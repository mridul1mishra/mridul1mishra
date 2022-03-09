using FX.Core.Models.Base;
using FX.Core.Serialization;
using FX.Core.Tasks.Models;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FX.Core.Services
{

    public class ItemWebAPIService
    {
        private HttpClient __httpClient { get; set; }
        private string __endPoint { get; set; }
        public ItemWebAPIService() { }
        public ItemWebAPIService(HttpClient httpClient, string endPoint)
        {
            __endPoint = endPoint;
            __httpClient = httpClient;
        }
        public DataSyncResult PushBulkItems(IEnumerable<Item> sitecoreItems, int maxItemsPush)
        {
            var dataSyncResult = new DataSyncResult();
            System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            var runs = Math.Ceiling((decimal)sitecoreItems.Count() / (decimal)Math.Max(0, maxItemsPush));
            var currentTry = 0;
            var success = true;
            do
            {
                for (int i = 0; i < runs; i++)
                {
                    try
                    {
                        var pagedItems = sitecoreItems.Skip(i * maxItemsPush).Take(Math.Max(0, maxItemsPush))
                            .Select(item => new SerializableItem()
                            {
                                ID = item.ID.ToGuid(),
                                TemplateID = item.TemplateID,
                                Fields = item.Fields.Select(f =>
                                {
                                    if (f.ID == ID.Parse("{40E50ED9-BA07-4702-992E-A912738D32DC}"))
                                    {
                                        var fileStream = ((FileField)f).InnerField.GetBlobStream();
                                        if (fileStream != null)
                                        {
                                            var memoryStream = new MemoryStream();
                                            fileStream.CopyTo(memoryStream);
                                            var bytes = memoryStream.ToArray();
                                            var value = Convert.ToBase64String(bytes);
                                            return new KeyValuePair<string, string>(f.ID.ToString(), value);
                                        }
                                        else
                                            return new KeyValuePair<string, string>(f.ID.ToString(), "");
                                    }
                                    else
                                        return new KeyValuePair<string, string>(f.ID.ToString(), f.Value);
                                }).ToDictionary(p => p.Key, p => p.Value),
                                Name = item.Name,
                                Paths = new SerializablePath()
                                {
                                    FullPath = item.Paths.FullPath,
                                    ParentPath = item.Paths.ParentPath
                                },
                                Language = new SerializableLanguage()
                                {
                                    Name = item.Language.Name
                                }
                            });



                        var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(pagedItems);
                        var task = Task.Run(() => __httpClient.PostAsync(__endPoint, new StringContent(jsonString, Encoding.UTF8, "application/json")));
                        var result = task.Result;
                        //if any of the runs fail, we break the operation and re-try everything.
                        if (!result.IsSuccessStatusCode)
                        {
                            success = false;
                            dataSyncResult.HttpStatusCode = result.StatusCode.ToString();
                            var body = result.Content.ReadAsStringAsync().Result;
                            dataSyncResult.ResponseMessage.Add(body);
                            break;
                        }
                        else
                        {
                            var body = result.Content.ReadAsStringAsync().Result;
                            if (body != null && body.Contains("Failed Items"))
                            {
                                dataSyncResult.ResponseMessage.Add(body);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Sitecore.Diagnostics.Log.Error("Error generating the current run: " + ex.Message + ex.StackTrace, this);
                        continue;
                    }
                }
                if (success)
                    break;
                currentTry++;
            } while (currentTry < 3 && !success);
            dataSyncResult.Success = success;
            return dataSyncResult;
        }
    }
}
