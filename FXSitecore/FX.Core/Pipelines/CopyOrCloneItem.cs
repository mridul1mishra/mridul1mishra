using System.Linq;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Shell.Framework.Pipelines;
namespace FX.Core.Pipelines
{
    public class CopyOrCloneItem : CopyItems
    {
        public virtual void ProcessFieldValues(CopyItemsArgs args)
        {
            Item sourceRoot = GetItems(args).FirstOrDefault();
            Assert.IsNotNull(sourceRoot, "sourceRoot is null.");

            Item copyRoot = args.Copies.FirstOrDefault();
            Assert.IsNotNull(copyRoot, "copyRoot is null.");

            new ReferenceReplacementJob(sourceRoot, copyRoot).StartAsync();
        }
    }
}