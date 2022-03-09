using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
namespace FX.Core.Pipelines
{
    public class ItemReferenceReplacer
    {
        private readonly Func<Field, bool> _fieldFilter;
        private readonly ICollection<ItemPair> _itemPairs = new HashSet<ItemPair>();

        public IEnumerable<Item> Items
        {
            get { return _itemPairs.Select(pair => pair.Item).ToArray(); }
        }

        public IEnumerable<Item> OtherItems
        {
            get { return _itemPairs.Select(pair => pair.OtherItem).ToArray(); }
        }

        public ItemReferenceReplacer(Func<Field, bool> fieldFilter)
        {
            Assert.ArgumentNotNull(fieldFilter, "fieldFilter");
            _fieldFilter = fieldFilter;
        }

        public void AddItemPair(Item item, Item otherItem)
        {
            Assert.ArgumentNotNull(item, "item");
            Assert.ArgumentNotNull(otherItem, "otherItem");
            _itemPairs.Add(new ItemPair(item, otherItem));
        }

        public void ReplaceItemReferences(Item item)
        {
            IEnumerable<Field> fields = GetFieldsToProcess(item);
            foreach (Field field in fields)
            {
                foreach (Item itemVersion in GetVersionsToProcess(item))
                {
                    Field itemVersionField = itemVersion.Fields[field.ID];
                    ProcessField(itemVersionField);
                }
            }
        }

        private IEnumerable<Field> GetFieldsToProcess(Item item)
        {
            item.Fields.ReadAll();
            return item.Fields.Where(_fieldFilter).ToArray();
        }

        private IEnumerable<Item> GetVersionsToProcess(Item item)
        {
            return item.Versions.GetVersions(true);
        }

        private void ProcessField(Field field)
        {
            string initialValue = GetInitialFieldValue(field);
            if (string.IsNullOrEmpty(initialValue))
                return;

            StringBuilder value = new StringBuilder(initialValue);
            foreach (ItemPair itemPair in _itemPairs)
            {
                ReplaceID(itemPair.Item, itemPair.OtherItem, value);
                ReplaceShortID(itemPair.Item, itemPair.OtherItem, value);
                ReplaceFullPath(itemPair.Item, itemPair.OtherItem, value);
                ReplaceContentPath(itemPair.Item, itemPair.OtherItem, value);
            }
            UpdateFieldValue(field, initialValue, value);
        }

        private string GetInitialFieldValue(Field field)
        {
            return field.GetValue(true, true);
        }

        private void ReplaceID(Item item, Item otherItem, StringBuilder value)
        {
            value.Replace(item.ID.ToString(), otherItem.ID.ToString());
        }

        private void ReplaceShortID(Item item, Item otherItem, StringBuilder value)
        {
            value.Replace(item.ID.ToShortID().ToString(), otherItem.ID.ToShortID().ToString());
        }

        private void ReplaceFullPath(Item item, Item otherItem, StringBuilder value)
        {
            value.Replace(item.Paths.FullPath, otherItem.Paths.FullPath);
        }

        private void ReplaceContentPath(Item item, Item otherItem, StringBuilder value)
        {
            if (item.Paths.IsContentItem)
                value.Replace(item.Paths.ContentPath, otherItem.Paths.ContentPath);
        }

        private void UpdateFieldValue(Field field, string initialValue, StringBuilder value)
        {
            if (initialValue.Equals(value.ToString()))
                return;

            using (new EditContext(field.Item))
            {
                field.Value = value.ToString();
            }
        }

        [DebuggerDisplay("Item: \"{Item.Paths.Path}\", OtherItem: \"{OtherItem.Paths.Path}\"")]
        private class ItemPair
        {
            public Item Item { get; private set; }
            public Item OtherItem { get; private set; }

            public ItemPair(Item item, Item otherItem)
            {
                Assert.ArgumentNotNull(item, "item");
                Assert.ArgumentNotNull(otherItem, "otherItem");
                Item = item;
                OtherItem = otherItem;
            }

            public override bool Equals(object instance)
            {
                return instance is ItemPair && instance.GetHashCode() == GetHashCode();
            }

            public override int GetHashCode()
            {
                return string.Concat(Item.ID, OtherItem.ID).GetHashCode();
            }
        }
    }
}