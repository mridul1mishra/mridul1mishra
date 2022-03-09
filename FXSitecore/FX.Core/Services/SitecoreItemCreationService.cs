using FX.Core.Serialization;
using Glass.Mapper;
using Glass.Mapper.Sc;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Globalization;
using Sitecore.Publishing;
using Sitecore.SecurityModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Core.Services
{
    public class SitecoreItemCreationService
    {
        private Database __db { get; set; }
        private Database __webdb { get; set; }
        public SitecoreItemCreationService()
        {
            __db = Sitecore.Data.Database.GetDatabase("master");
            __webdb = Sitecore.Data.Database.GetDatabase("web");
        }
        public string VerifyItem(SerializableItem item)
        {
            var existingItem = __db.GetItem(ID.Parse(item.ID), Language.Parse(item.Language.Name));
            if (existingItem != null)
            {
                foreach(var fieldName in item.Fields.Select(f => f.Key))
                {
                    if (existingItem[fieldName] != item.Fields[fieldName])
                        return "Item found, but field values are different. Item ID: " + item.ID.ToString() + " and item path: " + item.Paths.FullPath;
                }
                if (existingItem.Paths.FullPath.ToLower() != item.Paths.FullPath.ToLower())
                    return "Item found, but path is different. Item ID: " + item.ID.ToString() + " and item path: " + item.Paths.FullPath + " and destination path is: " + existingItem.Paths.FullPath;
                return "Item found";
            }
            else
                return "Not found. Item ID: " + item.ID.ToString() + " and item path: " + item.Paths.FullPath;
        }
        public void PublishAllItems(IEnumerable<Item> items)
        {

            foreach (var item in items)
            {
                foreach (var language in item.Languages)
                {
                    PublishOptions options = new PublishOptions(__db, __webdb, PublishMode.Smart, language, DateTime.Now)
                    {
                        RootItem = item,
                        Deep = false
                    };
                    Publisher publisher = new Publisher(options);
                    publisher.PublishAsync();
                }
            }
            //var languages = LanguageManager.GetLanguages(__db);
            //PublishManager.PublishSmart(__db, new Database[] { __webdb }, languages.ToArray());
        }

        public Tuple<Item, string> CreateOrUpdateItem(SerializableItem item)
        {
            string message = "";
            var parentPath = item.Paths.ParentPath;
            var template = __db.GetTemplate(item.TemplateID);

            //We find existing item using ID as identifier, in case the item has been moved out of its original place.
            var existingItem = __db.GetItem(ID.Parse(item.ID), Language.Parse(item.Language.Name));

            //If item doesn't exist we will create it.
            if (existingItem == null)
            {
                using (new LanguageSwitcher(item.Language.Name))
                {
                    var parent = __db.GetItem(parentPath);

                    // check if section folder is missing.
                    if (parent == null && parentPath.ToLower().EndsWith("/sections"))
                    {
                        var grandParent = __db.GetItem(parentPath.ToLower().Replace("/sections", ""));
                        if (grandParent != null)
                            parent = ItemManager.AddFromTemplate("Sections", ID.Parse(Templates.PageSectionFolder.Id), grandParent);
                    }
                    
                    if(parent==null)
                    {
                        throw new Exception("[ALIYUN] Parent is missing. Item path is " + item.Paths.FullPath+"\n");
                    }

                    if (template == null)
                    {
                        message += string.Format("Template Missing for {0}, Template ID: {1}", item.Paths.FullPath, item.TemplateID);
                        //Base Item
                        //template = __db.GetTemplate(ID.Parse("{B174990B-37B1-4A60-9C7D-891B521E1B76}"));
                        Sitecore.Diagnostics.Log.Error("[ALIYUN] Template is missing. Item path is " + item.Paths.FullPath + "\n", this);
                    }

                    var newItem = ItemManager.AddFromTemplate(item.Name, template.ID, parent, ID.Parse(item.ID));

                    newItem.Editing.BeginEdit();

                    try
                    {
                        foreach (var fieldName in item.Fields.Where(f => !string.IsNullOrWhiteSpace(f.Key)).Select(f => f.Key))
                        {
                            if (string.IsNullOrWhiteSpace(fieldName))
                                continue;
                            if (fieldName == "{40E50ED9-BA07-4702-992E-A912738D32DC}")
                            {
                                if (!string.IsNullOrEmpty(item.Fields[fieldName]))
                                {
                                    var bytes = Convert.FromBase64String(item.Fields[fieldName]);
                                    var memoryStream = new MemoryStream(bytes);
                                    newItem.Fields[fieldName].SetBlobStream(memoryStream);
                                }
                            }
                            else
                            {
                                try
                                {
                                    newItem[fieldName] = item.Fields[fieldName];
                                }
                                catch(Exception ex)
                                {
                                    message += string.Format("Field is missing. Item path is {0} Field is {1}", item.Paths.FullPath, fieldName);
                                    Sitecore.Diagnostics.Log.Error("[ALIYUN] Field is missing. Item path is " + item.Paths.FullPath + "Field is " + fieldName + " " + ex.Message, this);
                                }
                            }
                               
                        }
                        
                        newItem.Editing.EndEdit();
                        PublishManager.AddToPublishQueue(newItem, ItemUpdateType.Created);
                        return Tuple.Create(newItem, message);
                    }
                    catch (Exception e)
                    {
                        newItem.Editing.CancelEdit();
                        throw new Exception("Failed to add Item. " + e.Message, e);
                    }

                    
                }
            }
            else
            {
                using (new SecurityDisabler())
                {
                    using (new LanguageSwitcher(item.Language.Name))
                    {
                        // ItemManager.AddVersion(existingItem);
                        //   

                        //Unlock if item is locked
                        if(existingItem.Locking.IsLocked())
                        {
                            existingItem.Locking.Unlock();
                        }

                        var newVer = existingItem.Versions.AddVersion();
                        newVer.Editing.BeginEdit();

                        // We should also try to rename the item if it has changed.
                        if (newVer.Name != item.Name)
                            newVer.Name = item.Name;

                        // Move item if the path is different
                        if (newVer.Paths.ParentPath != item.Paths.ParentPath)
                        {
                            // Try get the parent to see if it exists
                            var parentItem = __db.GetItem(item.Paths.ParentPath);
                            if (parentItem != null)
                            {
                                newVer.MoveTo(parentItem);
                            }
                        }
                        try
                        {
                            foreach (var fieldName in item.Fields.Where(f => !string.IsNullOrWhiteSpace(f.Key)).Select(f => f.Key))
                            {
                                if (string.IsNullOrWhiteSpace(fieldName))
                                    continue;
                                if (fieldName == "{40E50ED9-BA07-4702-992E-A912738D32DC}")
                                {
                                    if (!string.IsNullOrEmpty(item.Fields[fieldName]))
                                    {
                                        var bytes = Convert.FromBase64String(item.Fields[fieldName]);
                                        var memoryStream = new MemoryStream(bytes);
                                        newVer.Fields[fieldName].SetBlobStream(memoryStream);
                                    }
                                }
                                else
                                    newVer[fieldName] = item.Fields[fieldName];
                            }

                            newVer.Editing.EndEdit();
                            PublishManager.AddToPublishQueue(newVer, ItemUpdateType.VersionAdded);
                            return Tuple.Create(newVer, message);
                        }
                        catch (Exception e)
                        {
                            newVer.Editing.CancelEdit();
                            throw new Exception("Failed to update version. " + e.Message, e);
                        }
                    }

                }
            }
            
        }
    }
}
