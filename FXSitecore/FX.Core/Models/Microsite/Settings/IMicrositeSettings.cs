using FX.Core.Models.Base;
using Glass.Mapper.Sc.Fields;

namespace FX.Core.Models.Microsite.Settings
{
    public interface IMicrositeSettings : ISitecoreItem
    {
        #region Header Details
        Image Logo { get; set; }
        Link HeaderLink1 { get; set; }
        Link HeaderLink2 { get; set; }
        Link HeaderLink3 { get; set; }
        Link HeaderLink4 { get; set; }
        #endregion

        #region Footer Details
        Link FooterLink1 { get; set; }
        Link FooterLink2 { get; set; }
        Link FooterLink3 { get; set; }
        Link FooterLink4 { get; set; }
        string FooterText1 { get; set; }
        string FooterText2 { get; set; }
        #endregion

        string ContactFormSnippet { get; set; }
        bool HideForm { get; set; }
    }
}
