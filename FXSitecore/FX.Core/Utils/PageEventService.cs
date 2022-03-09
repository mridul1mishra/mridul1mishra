using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Analytics;
using Sitecore.Analytics.Data;

namespace FX.Core.Utils
{
    public class PageEventService
    {
        public void RegisterPageEvent(string name, string definitionID, string itemID, string data, string text)
        {
            Guid definitionGuid;
            Guid.TryParse(definitionID, out definitionGuid);

            Guid itemGuid;
            Guid.TryParse(itemID, out itemGuid);

            RegisterPageEvent(name, definitionGuid, itemGuid, data, text);
        }

        private void RegisterPageEvent(string name, Guid definitionId, Guid itemId, string data, string text)
        {
            if (!string.IsNullOrEmpty(name) && definitionId != null && itemId != null)
            {
                if (TrackerEnabled())
                {
                    var interaction = Tracker.Current.Session.Interaction;
                    var pageEventData = new PageEventData(name, definitionId)
                    {
                        ItemId = itemId,
                        Data = data,
                        Text = text
                    };
                    interaction.CurrentPage.Register(pageEventData);
                }
            }
        }

        private static bool TrackerEnabled()
        {
            return Tracker.IsActive && Tracker.Current.Session != null && Tracker.Current.Session.Interaction != null;
        }
    }
}
