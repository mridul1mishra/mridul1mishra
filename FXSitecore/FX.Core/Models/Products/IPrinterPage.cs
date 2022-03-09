using FX.Core.Models.Components;
using FX.Core.Models.Settings;
using Glass.Mapper.Sc.Configuration.Attributes;
using System.Collections.Generic;

namespace FX.Core.Models.Products
{
    public interface IPrinterPage : IProductPage
    {
        IEnumerable<ITaxonomyItem> PrinterFeatures { get; set; }

        string PrinterShortDescription1 { get; set; }
        string PrinterShortDescription2 { get; set; }

        string PrintSpeedMonochromeText { get; set; }
        string PrintSpeedColourText { get; set; }
        string FirstPageOutputTimeText1 { get; set; }
        string FirstPageOutputTimeText2 { get; set; }
        string ScanSpeedText { get; set; }
        string PrintingResolutionText { get; set; }
        string PrintingInputCapacityText1 { get; set; }
        string PrintingInputCapacityText2 { get; set; }
        string MaxPaperSizeText { get; set; }
        string UserInterfaceText { get; set; }
        bool NetworkConnectivity { get; set; }

    }
}
