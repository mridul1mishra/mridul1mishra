using Sitecore.Data.Items;
using System;
using System.Collections.Generic;

namespace FX.Core.Events
{
    public class SetParentCategory
    {
        public string Database { get; set; }

        private readonly List<string> _templates = new List<string>();
        public List<string> Templates
        {
            get
            {
                return this._templates;
            }
        }

        protected void OnItemSaving(object sender, EventArgs args)
        {
            /// 
            /// Called when a Sitecore item is saved or renamed.
            /// 
            // Pull the item from the args.
            Item item = Sitecore.Events.Event.ExtractParameter(args, 0) as Item;

            // Do nothing if the item isn't in the master database.
            if (item.Database == null || item.Database.Name != this.Database)
                return;

            // only concerned with configured templates
            var templateMatch = Templates.Contains(item.TemplateID.ToString());

            if (!templateMatch)
                return;

            var parent = GetParent(item.Parent);
            using (new Sitecore.Data.Events.EventDisabler())
            {
                using (new EditContext(item))
                {
                    item["ParentCategory"] = parent != null ? parent["TeaserTitle"] : string.Empty;
                }
            }
        }

        private Item GetParent(Item item)
        {
            if (item == null)
                return null;

            if (Templates.Contains(item.TemplateID.ToString()))
                return item;

            return GetParent(item.Parent);
        }
    }
}
