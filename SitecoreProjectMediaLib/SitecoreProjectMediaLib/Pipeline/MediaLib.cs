using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Pipelines;
using System.Web.Routing;
using Sitecore.Resources.Media;
using System.IO;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.Data;

namespace SitecoreProjectMediaLib.Pipeline
{
    public class MediaLib
    {

        public virtual void Process(PipelineArgs args)
        {

            var fullpath = "/sitecore/media library/Default Website/Image";
            var myFile = @"C:\temp\img.png";

            byte[] fileContent = null;
            System.IO.FileStream fs = new System.IO.FileStream(myFile, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            System.IO.BinaryReader binaryReader = new System.IO.BinaryReader(fs);
            long byteLength = new System.IO.FileInfo(myFile).Length;
            fileContent = binaryReader.ReadBytes((Int32)byteLength);
            fs.Close();
            fs.Dispose();
            AddMediaItem(fileContent, fullpath, "img.png", Sitecore.Context.Language);
        }



        public static Item AddMediaItem(byte[] fileBuffer, string fullMediaPath, string fileNameWithExtension, Language language)
        {
            try
            {
                var db = Sitecore.Configuration.Factory.GetDatabase("master");
                var options = new MediaCreatorOptions();
                options.FileBased = false;
                options.IncludeExtensionInItemName = false;
                options.OverwriteExisting = true;
                options.Versioned = true;
                options.Destination = fullMediaPath;
                options.Database = db;
                options.Language = language;



                var creator = new MediaCreator();
                var fileStream = new MemoryStream(fileBuffer);



                var pdfItem = db.GetItem(fullMediaPath, language);
                if (pdfItem != null)
                {
                    using (new Sitecore.SecurityModel.SecurityDisabler())
                    {
                        var updatedItem = creator.AttachStreamToMediaItem(fileStream, fullMediaPath, fileNameWithExtension,
                        options);
                        updatedItem.Editing.BeginEdit();
                        updatedItem.Editing.EndEdit();
                        return updatedItem;
                    }
                }
                else
                {
                    //Create a new item
                    var newItem = creator.CreateFromStream(fileStream, fileNameWithExtension, options);
                    newItem.Editing.BeginEdit();
                    newItem.Editing.EndEdit();
                    return newItem;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}