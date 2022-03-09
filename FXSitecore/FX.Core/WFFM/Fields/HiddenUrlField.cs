using Sitecore.Form.Core.Attributes;
using Sitecore.Form.Core.Visual;
using System.ComponentModel;
using System.Web;
using Sitecore.Forms.Mvc.ViewModels.Fields;



namespace FX.Core.WFFM.Fields
{
    public class HiddenUrlField : SingleLineTextField
    {
        public override string Value { get; set; }

        public override void Initialize()
        {
            if (!string.IsNullOrWhiteSpace(this.CssClass))
                this.CssClass += " hidden";
            else
                this.CssClass = "hidden";
            this.ShowTitle = false;
            this.Value = HttpContext.Current.Request.Url.AbsolutePath.ToString();
        }
    }
}
