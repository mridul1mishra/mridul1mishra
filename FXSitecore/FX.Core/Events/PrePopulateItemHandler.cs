using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Events;
using Sitecore.Globalization;
using System;

namespace FX.Core.Events
{
	public class PrePopulateItemHandler
	{
        public void OnVersionAdded(object sender, EventArgs args)
        {
            //get item of the new version added
            var item = Event.ExtractParameter(args, 0) as Item;
            try
            {
                if (item != null && item.Version.Number == 1)
                {
                    //copy fields from fallback item Language english
                    var fallbackItem = item.Database.GetItem(item.ID, Language.Parse("en"));

                    item.Editing.BeginEdit();
                    fallbackItem.Fields.ReadAll();

                    //copy only __Final Renderings fields from source to target language
                    foreach (Sitecore.Data.Fields.Field field in fallbackItem.Fields)
                    {
                        if (field.Name.StartsWith("__Final Renderings") && field.Name.Trim() != "")
                        {
                            item.Fields[field.Name].SetValue(field.Value, true);
                        }
                    }
                    item.Editing.EndEdit();
                    item.Editing.AcceptChanges();
                }
            }
            catch (Exception exception)
            {
                Log.Error("PrePopulateItemHandler Error. Cancel Edit.", exception, this);
                if (item != null)
                {
                    item.Editing.CancelEdit();
                }
            }
        }
	}
}
