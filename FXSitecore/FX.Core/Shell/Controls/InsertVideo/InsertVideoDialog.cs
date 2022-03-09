using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.IO;
using Sitecore.Resources.Media;
using Sitecore.Shell.Controls.RichTextEditor.InsertImage;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.Sheer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FX.Core.Shell.Controls.InsertVideo
{
    public class InsertVideoDialog : InsertImageForm
    {
        protected Edit ExternalLink;
        protected Edit VideoText;
        protected override void OnOK(object sender, EventArgs args)
        {
            Assert.ArgumentNotNull(sender, "sender");
            Assert.ArgumentNotNull(args, "args");
            string text = this.Filename.Value;
            string originalText = text;
            var externalLink = this.ExternalLink.Value;
            if (text.Length == 0 && string.IsNullOrEmpty(externalLink))
            {
                SheerResponse.Alert("Select or link a media item", new string[0]);
                return;
            }
            Item root = this.DataContext.GetRoot();
            if (root != null)
            {
                Item rootItem = root.Database.GetRootItem();
                if (rootItem != null && root.ID != rootItem.ID)
                {
                    string path = root.Paths.Path;
                    text = FileUtil.MakePath(path, text, '/');
                }
            }
            MediaItem mediaItem = !string.IsNullOrEmpty(originalText) ? this.DataContext.GetItem(text, this.ContentLanguage, Sitecore.Data.Version.Latest) : null;
       
            if (mediaItem == null && string.IsNullOrEmpty(externalLink))
            {
                SheerResponse.Alert("The media item could not be found.", new string[0]);
                return;
            }
            MediaUrlOptions shellOptions = MediaUrlOptions.GetShellOptions();
            shellOptions.Language = this.ContentLanguage;
            string text2 = VideoText.Value;

            var returnValue =
                "<a data-fancybox=\"data-fancybox\" href=\"#{0}\" class=\"red-link\">{1}</a>\r\n\r\n<div id=\"{0}\" style=\"padding: 15px; display: none;\">\r\n<video src=\"{2}\" width=\"640\" height=\"320\" controls=\"controls\"></video>\r\n</div>";

            var regex = new Regex("[^a-zA-Z0-9]");

            var htmlid = mediaItem != null ? "video-" + mediaItem.ID.ToShortID().ToString() : "video-" + regex.Replace(externalLink, "");

            returnValue = string.Format(returnValue, htmlid, text2, mediaItem != null ? MediaManager.GetMediaUrl(mediaItem, shellOptions) : externalLink);
            if (this.Mode == "webedit")
            {
                SheerResponse.SetDialogValue(StringUtil.EscapeJavascriptString(returnValue));
                base.OnOK(sender, args);
            }
            else
            {
                SheerResponse.Eval("scClose(" + StringUtil.EscapeJavascriptString(returnValue) + ")");
            }
            this.SaveFolderToRegistry();
        }
    }
}
