using FX.Core.Services;
using Sitecore;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace FX.Core.Tasks
{
    public class SCItemComparer : IEqualityComparer<Item>
    {
        public bool Equals(Item x, Item y)
        {
            return x.ID.Equals(y.ID) && x.Language.Equals(y.Language);
        }

        public int GetHashCode(Item obj)
        {
            return obj.ID.GetHashCode();
        }
    }
    public class DataSynchronizationTask
    {
        public void Execute(Item[] items, Sitecore.Tasks.CommandItem command, Sitecore.Tasks.ScheduleItem schedule)
        {
            var db = Sitecore.Data.Database.GetDatabase("master");
            var settingsItem = db.GetItem("/sitecore/content/china/content stores/data sync");
            if (settingsItem == null)
                return;

            var fromTime = ((DateField)settingsItem.Fields["LastSuccessfulRun"]).DateTime;
            var runTime = DateTime.UtcNow;
            var lastSaved = db
                .Engines.HistoryEngine.GetHistory(fromTime, runTime)
                //we are only interested in the saved action, which denotes there are changes to items.
                .Where(h => h.Action == Sitecore.Data.Engines.HistoryAction.Saved || h.Action == Sitecore.Data.Engines.HistoryAction.Created)
                .Where(h => h.ItemId != command.ID && h.ItemId != schedule.ID)
                .Where(h => !h.ItemPath.ToLower().StartsWith("/sitecore/templates"))
                .Where(h => !h.ItemPath.ToLower().StartsWith("/sitecore/system"))
                .Where(h => !h.ItemPath.ToLower().StartsWith("/sitecore/social"))
                .Where(h => !h.ItemPath.ToLower().StartsWith("/sitecore/layout"))
                .Select(h => db.GetItem(h.ItemId, h.ItemLanguage))
                .Where(i => i != null)
                //exclude the settings item.
                .Where(i => i.TemplateID != settingsItem.TemplateID)
                .Distinct(new SCItemComparer());

            if (!lastSaved.Any())
                return;
            lastSaved = lastSaved.OrderBy(i => i.Paths.FullPath);

            int maxItemsPerWave = int.TryParse(settingsItem["MaxItemsPerWave"], out maxItemsPerWave) ? maxItemsPerWave : 5;
            var apiService = new ItemWebAPIService(new System.Net.Http.HttpClient(), settingsItem["EndPoint"]);
            var result = apiService.PushBulkItems(lastSaved, maxItemsPerWave);
            if (result.Success)
            {
                settingsItem.Editing.BeginEdit();
                settingsItem["LastSuccessfulRun"] = DateUtil.ToIsoDate(runTime);
                settingsItem.Editing.AcceptChanges();
                settingsItem.Editing.EndEdit();
                
            }
            else
            {
                //Inform the admins that their task has failed.
                try
                {
                    Sitecore.Diagnostics.Log.Error(string.Join("\n", result.ResponseMessage), this);
                    var mailMessage = new MailMessage(settingsItem["AlertEmailAddress"], settingsItem["AlertEmailAddress"], "Sync task failure", string.Join("\n", result.ResponseMessage));
                    MainUtil.SendMail(mailMessage);
                }
                catch (Exception e)
                {
                    Sitecore.Diagnostics.Log.Error(e.StackTrace, this);
                }
            }
            //Inform the admins that their task has finished running
            try
            {
                MailMessage mailMessage;
                if (result.ResponseMessage.Any())
                {
                    mailMessage = new MailMessage(settingsItem["AlertEmailAddress"], settingsItem["AlertEmailAddress"], "Sync task finished", string.Join("\n", result.ResponseMessage));
                }
                else
                    mailMessage = new MailMessage(settingsItem["AlertEmailAddress"], settingsItem["AlertEmailAddress"], "Sync task finished", "The data sync task has finished running");
                MainUtil.SendMail(mailMessage);
            }
            catch (Exception e)
            {
                Sitecore.Diagnostics.Log.Error(e.StackTrace, this);
            }
        }
    }
}
