using Newtonsoft.Json;
using Sitecore.Data.Items;
using Sitecore.SecurityModel;
using sitecoreforms.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace sitecoreforms.Controllers
{
    public class Formscontroller : Controller
    {
        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public async Task<string> FormsMigration()
        {
            Sitecore.Data.Database masterDB = Sitecore.Configuration.Factory.GetDatabase("master");
            Item ParentItem = masterDB.GetItem("/sitecore/Forms/australia");
            var client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("http://local-staging.fujixerox.com.au/fbau/api/formmigration");

            if (response.IsSuccessStatusCode)
            {

                string str = await response.Content.ReadAsStringAsync();
                var migrationForm = JsonConvert.DeserializeObject<Forms[]>(str);
                foreach (var form in migrationForm)
                {
                    string name = "Comment_" + Sitecore.DateUtil.IsoNow;
                    TemplateItem template = masterDB.GetTemplate("{6ABEE1F2-4AB4-47F0-AD8B-BDB36F37F64C}");
                    TemplateItem templatechild = masterDB.GetTemplate("{0908030B-4564-42EA-A6FA-C7A5A2D921A8}"); ;
                    using (new SecurityDisabler())
                    {
                        ParentItem.Add(form.Text, template);
                        foreach (var child in form.child)
                        {
                            try
                            {
                                var lastID = form.child.Select(c => c.FieldId).LastOrDefault();
                                if (child.FieldId.Equals(lastID))
                                {
                                    child.Name = child.Name.Replace("1.", "");
                                    child.Name = child.Name.Replace("2.", "");
                                    var formItem = masterDB.GetItem($"/sitecore/Forms/australia/{form.Text}");
                                    using (new Sitecore.SecurityModel.SecurityDisabler())
                                    {
                                        Item newItem = formItem.Add(child.Name, templatechild);
                                        if (newItem != null)
                                        {
                                            newItem.Editing.BeginEdit();
                                            newItem.Fields["Title"].Value = child.FieldTitle;
                                            if (child.FieldType == "{002E5FD5-8B12-4913-BA52-BCC5FEAD2785}")
                                            {
                                                newItem.Fields["Field Type"].SetValue("{DF74F55B-47E6-4D1C-92F8-B0D46A7B2704}", true);
                                            }
                                            if (child.FieldType == "{84ABDA34-F9B1-4D3A-A69B-E28F39697069}")
                                            {
                                                newItem.Fields["Field Type"].SetValue("{04C39CAC-8976-4910-BE0D-879ED3368429}", true);

                                            }
                                            newItem.Editing.EndEdit();
                                            var SubmitButtonTemplate = masterDB.GetTemplate("{94A46D66-B1B8-405D-AAE4-7B5A9CD61C5E}");
                                            Item submitButton = formItem.Add("Submit Button", SubmitButtonTemplate);
                                            var SubmitActionsTemplate = masterDB.GetTemplate("{A87A00B1-E6DB-45AB-8B54-636FEC3B5523}");
                                            Item SubmitActions = submitButton.Add("SubmitActions", SubmitActionsTemplate);
                                            var SaveDataTemplate = masterDB.GetTemplate("{05FE45D4-B9C7-40DE-B767-7C5ABE7119F9}");
                                            Item SaveData = SubmitActions.Add("Save Data", SaveDataTemplate);
                                            SaveData.Editing.BeginEdit();
                                            SaveData.Fields["Submit Action"].SetValue("{0C61EAB3-A61E-47B8-AE0B-B6EBA0D6EB1B}", true);
                                            SaveData.Editing.EndEdit();


                                        }
                                    }
                                }
                                else
                                {
                                    child.Name = child.Name.Replace("1.", "");
                                    child.Name = child.Name.Replace("2.", "");
                                    var formItem = masterDB.GetItem($"/sitecore/Forms/australia/{form.Text}");
                                    using (new Sitecore.SecurityModel.SecurityDisabler())
                                    {
                                        Item newItem = formItem.Add(child.Name, templatechild);
                                        if (newItem != null)
                                        {
                                            newItem.Editing.BeginEdit();
                                            newItem.Fields["Title"].Value = child.FieldTitle;
                                            if (child.FieldType == "{002E5FD5-8B12-4913-BA52-BCC5FEAD2785}")
                                            {
                                                newItem.Fields["Field Type"].SetValue("{DF74F55B-47E6-4D1C-92F8-B0D46A7B2704}", true);
                                            }
                                            if (child.FieldType == "{84ABDA34-F9B1-4D3A-A69B-E28F39697069}")
                                            {
                                                newItem.Fields["Field Type"].SetValue("{04C39CAC-8976-4910-BE0D-879ED3368429}", true);

                                            }
                                            newItem.Editing.EndEdit();
                                        }
                                    }
                                }
                                
                            }
                            catch(Exception ex)
                            {
                                var message = ex.Message;
                            }
                        }
                    }
                }
                return str;
            }
            else
            {
                return response.ToString();
            }



        }
    }
}