using FX.Core.Serialization;
using Sitecore.Data;
using Sitecore.Data.Events;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.SecurityModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace FX.Website
{
    public partial class DataSyncVerification : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            pruneBtn.Click += PruneClick;
        }
        public void PruneClick(object sender, EventArgs e)
        {
            var hostname = url.Text;

            using (new SecurityDisabler())
            {
                using (new DatabaseCacheDisabler())
                {

                    using (new BulkUpdateContext())
                    {
                        var httpClient = new HttpClient();
                        var db = Sitecore.Data.Database.GetDatabase("master");
                        var home = db.GetItem("/sitecore/content/china");
                        var products = db.SelectItems("/sitecore/content/products/*[contains(@@name,'CL ')]//*");

                        var chinaChildren = home.Axes.GetDescendants();
                        
                        var items = products.Concat(chinaChildren);
                        itemsnum.Text = items.Count().ToString();
                        Task.Run(() =>
                        {
                            foreach (var mainitem in items)
                            {
                                foreach (var language in mainitem.Languages)
                                {
                                    var item = db.GetItem(mainitem.ID, language);
                                    //if (item == null)
                                    //    continue;
                                    var serializable = new SerializableItem()
                                    {
                                        ID = item.ID.ToGuid(),
                                        TemplateID = item.TemplateID,
                                        Fields = item.Fields.Select(f =>
                                        {
                                            if (f.ID == Sitecore.Data.ID.Parse("{40E50ED9-BA07-4702-992E-A912738D32DC}"))
                                            {
                                                var fileStream = ((FileField)f).InnerField.GetBlobStream();
                                                var memoryStream = new MemoryStream();
                                                fileStream.CopyTo(memoryStream);
                                                var bytes = memoryStream.ToArray();
                                                var value = Convert.ToBase64String(bytes);
                                                return new KeyValuePair<string, string>(f.ID.ToString(), value);
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
                                    };
                                    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(serializable);
                                    var task =  httpClient.PostAsync(hostname + "/api/datasyncapi/VerifyItem", new StringContent(jsonString, Encoding.UTF8, "application/json"));
                                    var result = task.Result;
                                    var logger = Sitecore.Diagnostics.LoggerFactory.GetLogger("datasync");

                                    var body = result.Content.ReadAsStringAsync().Result;
                                     if (body != "\"Item found\"")
                                    //  {
                                        logger.Info(body);
                                    //  }
                                }
                            }
                        });
                    }

                }
            }


        }

    }

}