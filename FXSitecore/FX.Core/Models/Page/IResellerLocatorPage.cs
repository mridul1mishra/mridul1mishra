using FX.Core.Models.Base;
using FX.Core.Models.Reseller;

namespace FX.Core.Models.Page
{
    public interface IResellerLocatorPage: IPage, IBanner
    {
        string LocationLabel { get; set; }
        string LocationDefaultValueLabel { get; set; }
        string StoresNearMeLabel { get; set; }
        string StoresNearMeLink { get; set; }
        string SearchLabel { get; set; }
        string ResultsLabel { get; set; }
        string NoResultsLabel { get; set; }
        string LocationErrorLabel { get; set; }
        string AddressLabel { get; set; }
        string ContactLabel { get; set; }
        string FaxLabel { get; set; }
        string EmailLabel { get; set; }
        string WebsiteLabel { get; set; }
        string OtherInfoLabel { get; set; }
        string ShowOnMapLabel { get; set; }
        string RequestAQuoteLabel { get; set; }
        string APIEndpoint { get; set; }
        string APIKey { get; set; }

        ISitecoreItem XMLFile { get; set; }
        int Radius { get; set; }

        Resellers Resellers { get; set; }
    }
}
