using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using System.Collections.Generic;
using FX.Core.Models.Products;
using FX.Core.Models.Settings;

namespace FX.Core.Models.Page
{
    [SitecoreType(TemplateId = Templates.ProductFinderPage.Id, AutoMap = true)]
    public interface IProductFinderPage : ISitecoreItem
    {
        string ProductFinderTitle { get; set; }
        string PrinterCountLabel { get; set; }
        string BackLabel { get; set; }
        string StartOverLabel { get; set; }
        string FindPrintersLabel { get; set; }
        string CompareLabel { get; set; }
        string AddToCompareLabel { get; set; }
        string LearnMoreLabel { get; set; }
        string MaxPrinterCompareText { get; set; }
        string MaxPrinterCompareAlert { get; set; }
        string ApologyMessage { get; set; }
        string ContactUsLabel { get; set; }
        string ContactUsLink { get; set; }

        [SitecoreChildren]
        IEnumerable<IProductFinderQuestion> ProductFinderQuestions { get; set; }

        IEnumerable<IPrinterFolder> ProductsForComparison { get; set; }
    }

    public interface IPrinterFolder : ISitecoreItem
    {
        [SitecoreChildren]  
        //[SitecoreQuery("sitecore/Content/Products//*[@@templateid = '{6CC8C747-F8B0-43DD-A597-090CB9E74594}']")]
        IEnumerable <IPrinterPage> Printers { get; set; }
    }

    [SitecoreType(TemplateId = Templates.ProductFinderQuestion.Id, AutoMap = true)]
    public interface IProductFinderQuestion : ISitecoreItem
    {
        string StepNumber { get; set; }
        string StepLabel { get; set; }
        string Question { get; set; }   
        string QuestionDescription { get; set; }

        //set by backend. user unable to change this
        string QuestionType { get; set; }

        [SitecoreChildren]
        IEnumerable<IProductFinderOption> ProductFinderOptions { get; set; }
    }

    [SitecoreType(TemplateId = Templates.ProductFinderOption.Id, AutoMap = true)]
    public interface IProductFinderOption : ISitecoreItem
    {
        string OptionLabel { get; set; }
        Image OptionImage { get; set; }

        //set by backend. user unable to change this
        string OptionName { get; set; }

        //set by backend. user unable to change this
        string OptionHint { get; set; }

        IEnumerable<ITaxonomyItem> OptionFilter { get; set; }       
    }
 
}
