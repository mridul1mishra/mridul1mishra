using Sitecore.Data.Items;
using Sitecore.Events;
using System;
namespace FX.Core.Pipelines
{
    public class AddFromTemplate
    {
        public void OnItemAdded(object sender, EventArgs args)
        {
            Item targetItem = Event.ExtractParameter(args, 0) as Item;
            if (targetItem == null)
                return;
            if (targetItem.Branch == null)
                return;
            if (targetItem.Branch.InnerItem.Children.Count != 1)
                return;
            Item branchRoot = targetItem.Branch.InnerItem.Children[0];
            new ReferenceReplacementJob(branchRoot, targetItem).StartAsync();
        }
    }
}