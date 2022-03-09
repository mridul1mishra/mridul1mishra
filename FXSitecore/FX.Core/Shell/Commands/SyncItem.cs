using FX.Core.Services;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Shell.Framework.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Core.Shell.Commands
{
    public class SyncItem : Command
    {
        private Database __db { get; set; }
        public SyncItem()
        {
            __db = Sitecore.Data.Database.GetDatabase("master");
        }
        public override void Execute(CommandContext context)
        {
            var selectedItem = context.Items.FirstOrDefault();
            if (selectedItem == null)
                return;
            var settingsItem = __db.GetItem("/sitecore/content/china/content stores/data sync");
            if (settingsItem == null)
                return;

            List<Item> items = new List<Item>();
            foreach(var language in selectedItem.Languages)
            {
                var tempItem = __db.GetItem(selectedItem.ID, language);
                if (tempItem.Versions.Count > 0)
                    items.Add(tempItem);
            }
            foreach(var descendant in selectedItem.Axes.GetDescendants())
            {
                foreach(var language in descendant.Languages)
                {
                    var tempItem = __db.GetItem(descendant.ID, language);
                    if (tempItem.Versions.Count > 0)
                        items.Add(tempItem);
                }
            }
            int maxItemsPerWave = int.TryParse(settingsItem["MaxItemsPerWave"], out maxItemsPerWave) ? maxItemsPerWave : 5;
            items = items.OrderBy(i => i.Paths.FullPath).ToList();
            var parentItem = items.FirstOrDefault();
            var apiService = new ItemWebAPIService(new System.Net.Http.HttpClient(), settingsItem["EndPoint"]);

            if (Sitecore.Context.User != null && parentItem.Name != null)
                Sitecore.Diagnostics.Log.Audit(string.Format("{0} has started a manual sync on {1}", Sitecore.Context.User.Name, parentItem.Name), this);
            apiService.PushBulkItems(items, maxItemsPerWave);
            Sitecore.Context.ClientPage.ClientResponse.Alert("The data sync has completed.");
        }
    }
}
