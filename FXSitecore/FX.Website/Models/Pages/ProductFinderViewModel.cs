using FX.Core.Models.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Glass.Mapper.Sc.Web.Mvc;
using System.Web.Mvc;
using Glass.Mapper.Sc;
using FX.Core.Models.Products;
using FX.Core.Models.Base;
using FX.Core.Models.Settings;

namespace FX.Website.Models.Pages
{
    public class ProductFinderViewModel
    {
        public IProductFinderPage SitecoreModel { get; set; }

        public List<string> QueryItemsString { get; set; }

        public string GetJson()
        {
            var context = new SitecoreContext();
            var printerPage = context.GetCurrentItem<IProductFinderPage>();

            //list of printer folders
            var filters = printerPage.ProductsForComparison.ToList();
            //var printers = filters.Printers;

            var printerList = new List<IPrinterPage>();
            foreach (var item in filters)
            {
                printerList.AddRange(item.Printers);
            }


            //get question1's list of options
            var listofQuestions = SitecoreModel.ProductFinderQuestions.ToList();
            var listOfOptionsQ1 = listofQuestions[0].ProductFinderOptions.ToList();

            //get question2's list of options
            var listOfOptionsQ2 = listofQuestions[1].ProductFinderOptions.ToList();

            //get question3's list of options
            var listOfOptionsQ3 = listofQuestions[2].ProductFinderOptions.ToList();

            //get question4a's list of options
            var listOfOptionsQ4 = listofQuestions[3].ProductFinderOptions.ToList();
            //get question4b's list of options
            var listOfOptionsQ4b = listofQuestions[4].ProductFinderOptions.ToList();

            //get question4's list of options
            listOfOptionsQ4.AddRange(listOfOptionsQ4b);

            //get question5's list of options
            var listOfOptionsQ5 = listofQuestions[5].ProductFinderOptions.ToList();


            var jsonData = JsonConvert.SerializeObject(printerList.Select(x =>
            {
                var maxPrintVolName = x.PrinterFeatures.Count() != 0 &&
                x.PrinterFeatures.Where(y => Int32.TryParse(y.TaxonomyItemName, out int intMaxPrintVol)).Count() != 0 ? x.PrinterFeatures.Where(y => Int32.TryParse(y.TaxonomyItemName, out int intMaxPrintVol)).
                OrderByDescending(y => Int32.Parse(y.TaxonomyItemName)).FirstOrDefault().TaxonomyItemName : "";

                var printVolList = x.PrinterFeatures.Count() != 0 &&
                x.PrinterFeatures.Where(y => Int32.TryParse(y.TaxonomyItemName, out int intPrintVol)).Count() != 0 ? x.PrinterFeatures.Where(y => Int32.TryParse(y.TaxonomyItemName, out int intPrintVol)).
                OrderBy(y => Int32.Parse(y.TaxonomyItemName)).ToList() : new List<ITaxonomyItem>();

                //for printer with multiple print volumes. printer can fall into multiple product finder quantity options.
                var printVolStringList = new List<string>();
                foreach (var item in printVolList)
                {
                    if (listOfOptionsQ4[0].OptionFilter.Any(g => g.TaxonomyItemName == item.TaxonomyItemName) && x.PrinterFeatures.Any(k => listOfOptionsQ1[0].OptionFilter.Any(g => g.Id == k.Id)))
                    {
                        printVolStringList.Add(listOfOptionsQ4[0].OptionName);
                    }
                    else if (listOfOptionsQ4[1].OptionFilter.Any(g => g.TaxonomyItemName == item.TaxonomyItemName) && x.PrinterFeatures.Any(k => listOfOptionsQ1[0].OptionFilter.Any(g => g.Id == k.Id)))
                    {
                        printVolStringList.Add(listOfOptionsQ4[1].OptionName);
                    }
                    else if (listOfOptionsQ4[2].OptionFilter.Any(g => g.TaxonomyItemName == item.TaxonomyItemName) && x.PrinterFeatures.Any(k => listOfOptionsQ1[0].OptionFilter.Any(g => g.Id == k.Id)))
                    {
                        printVolStringList.Add(listOfOptionsQ4[2].OptionName);
                    }
                    else if (listOfOptionsQ4[3].OptionFilter.Any(g => g.TaxonomyItemName == item.TaxonomyItemName) && x.PrinterFeatures.Any(k => listOfOptionsQ1[0].OptionFilter.Any(g => g.Id == k.Id)))
                    {
                        printVolStringList.Add(listOfOptionsQ4[3].OptionName);
                    }
                    else if (listOfOptionsQ4[4].OptionFilter.Any(g => g.TaxonomyItemName == item.TaxonomyItemName) && x.PrinterFeatures.Any(k => listOfOptionsQ1[1].OptionFilter.Any(g => g.Id == k.Id)))
                    {
                        printVolStringList.Add(listOfOptionsQ4[4].OptionName);
                    }
                    else if (listOfOptionsQ4[5].OptionFilter.Any(g => g.TaxonomyItemName == item.TaxonomyItemName) && x.PrinterFeatures.Any(k => listOfOptionsQ1[1].OptionFilter.Any(g => g.Id == k.Id)))
                    {
                        printVolStringList.Add(listOfOptionsQ4[5].OptionName);
                    }
                    else if (listOfOptionsQ4[6].OptionFilter.Any(g => g.TaxonomyItemName == item.TaxonomyItemName) && x.PrinterFeatures.Any(k => listOfOptionsQ1[1].OptionFilter.Any(g => g.Id == k.Id)))
                    {
                        printVolStringList.Add(listOfOptionsQ4[6].OptionName);
                    }
                    else if (listOfOptionsQ4[7].OptionFilter.Any(g => g.TaxonomyItemName == item.TaxonomyItemName) && x.PrinterFeatures.Any(k => listOfOptionsQ1[1].OptionFilter.Any(g => g.Id == k.Id)))
                    {
                        printVolStringList.Add(listOfOptionsQ4[7].OptionName);
                    }
                }

                return
                new
                {
                    id = new Sitecore.Data.ShortID(x.Id).ToString(),
                    name = x.MainTitle,
                    desc = x.PrinterShortDescription1,
                    desc2 = x.PrinterShortDescription2,
                    thumbnail = x.ProductImage == null ? "" : x.ProductImage.Src,
                    url = x.Url,

                    q1 = x.PrinterFeatures.Any(k => listOfOptionsQ1[0].OptionFilter.Any(g => g.Id == k.Id)) ? listOfOptionsQ1[0].OptionName
                    : x.PrinterFeatures.Any(k => listOfOptionsQ1[1].OptionFilter.Any(g => g.Id == k.Id)) ? listOfOptionsQ1[1].OptionName : "",

                    q2 = x.PrinterFeatures.Any(k => listOfOptionsQ2[0].OptionFilter.Any(g => g.Id == k.Id)) ? listOfOptionsQ2[0].OptionName
                    : x.PrinterFeatures.Any(k => listOfOptionsQ2[1].OptionFilter.Any(g => g.Id == k.Id)) ? listOfOptionsQ2[1].OptionName : "",

                    q3 = x.PrinterFeatures.Any(k => listOfOptionsQ3[0].OptionFilter.Any(g => g.Id == k.Id)) ? listOfOptionsQ3[0].OptionName
                    : x.PrinterFeatures.Any(k => listOfOptionsQ3[1].OptionFilter.Any(g => g.Id == k.Id)) ? listOfOptionsQ3[1].OptionName : "",

                    q4 = printVolStringList.Distinct().ToList(),

                    q5 = new List<string>()
                    {
                        x.PrinterFeatures.Any(k => listOfOptionsQ5[0].OptionFilter.Any(g => g.Id == k.Id)) ? listOfOptionsQ5[0].OptionName : null,
                        x.PrinterFeatures.Any(k => listOfOptionsQ5[1].OptionFilter.Any(g => g.Id == k.Id)) ? listOfOptionsQ5[1].OptionName : null,
                        x.PrinterFeatures.Any(k => listOfOptionsQ5[2].OptionFilter.Any(g => g.Id == k.Id)) ? listOfOptionsQ5[2].OptionName : null,
                        x.PrinterFeatures.Any(k => listOfOptionsQ5[3].OptionFilter.Any(g => g.Id == k.Id)) ? listOfOptionsQ5[3].OptionName : null,
                    }

                    ////original q4
                    //(
                    //    //if a printer is tagged with a few print volumes, assign the corresponding option for the largest value to the printer
                    //    //for the first 4: if any of the printer features contains the selected option for "medium" option
                    //    //for the next 4: if any of the printer features contains the selected option for "large" option
                    //    listOfOptionsQ4[0].OptionFilter.Any(g => g.TaxonomyItemName == maxPrintVolName) && x.PrinterFeatures.Any(k => listOfOptionsQ1[0].OptionFilter.Any(g => g.Id == k.Id)) ? listOfOptionsQ4[0].OptionName :
                    //    listOfOptionsQ4[1].OptionFilter.Any(g => g.TaxonomyItemName == maxPrintVolName) && x.PrinterFeatures.Any(k => listOfOptionsQ1[0].OptionFilter.Any(g => g.Id == k.Id)) ? listOfOptionsQ4[1].OptionName :
                    //    listOfOptionsQ4[2].OptionFilter.Any(g => g.TaxonomyItemName == maxPrintVolName) && x.PrinterFeatures.Any(k => listOfOptionsQ1[0].OptionFilter.Any(g => g.Id == k.Id)) ? listOfOptionsQ4[2].OptionName :
                    //    listOfOptionsQ4[3].OptionFilter.Any(g => g.TaxonomyItemName == maxPrintVolName) && x.PrinterFeatures.Any(k => listOfOptionsQ1[0].OptionFilter.Any(g => g.Id == k.Id)) ? listOfOptionsQ4[3].OptionName :
                    //    listOfOptionsQ4[4].OptionFilter.Any(g => g.TaxonomyItemName == maxPrintVolName) && x.PrinterFeatures.Any(k => listOfOptionsQ1[1].OptionFilter.Any(g => g.Id == k.Id)) ? listOfOptionsQ4[4].OptionName :
                    //    listOfOptionsQ4[5].OptionFilter.Any(g => g.TaxonomyItemName == maxPrintVolName) && x.PrinterFeatures.Any(k => listOfOptionsQ1[1].OptionFilter.Any(g => g.Id == k.Id)) ? listOfOptionsQ4[5].OptionName :
                    //    listOfOptionsQ4[6].OptionFilter.Any(g => g.TaxonomyItemName == maxPrintVolName) && x.PrinterFeatures.Any(k => listOfOptionsQ1[1].OptionFilter.Any(g => g.Id == k.Id)) ? listOfOptionsQ4[6].OptionName :
                    //    listOfOptionsQ4[7].OptionFilter.Any(g => g.TaxonomyItemName == maxPrintVolName) && x.PrinterFeatures.Any(k => listOfOptionsQ1[1].OptionFilter.Any(g => g.Id == k.Id)) ? listOfOptionsQ4[7].OptionName :
                    //    ""
                    //)
                };
            }).ToList());

            //return "{" + string.Format("\"data\":{0}", jsonData) + "}";
            return jsonData;
        }

        public string GetComparePageItemName()
        {
            var context = new SitecoreContext();
            var printerPage = context.GetCurrentItem<IProductFinderPage>();
            var parent = printerPage.SitecoreItem.Parent.Paths.ContentPath;

            var comparePage = context.Database.SelectItems("/sitecore/content" + parent + " /*[@@templateid='{12265FE4-9E2F-4C29-9B60-25D8A991AC1A}']").FirstOrDefault();

            if (comparePage != null)
            {
                var url = Sitecore.Links.LinkManager.GetItemUrl(comparePage);
                return url;
            }
            else
            {
                return "";
            }
        }
    }
}