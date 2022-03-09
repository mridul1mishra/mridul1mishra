using Sitecore.Globalization;
using Sitecore.SecurityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FX.Website
{

    public partial class MovedItems : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.startBtn.Click += (s, ev) =>
            {
                using (new SecurityDisabler())
                {
                    var db = Sitecore.Data.Database.GetDatabase("master");
                    var languages = new[] { "ko-KR", "zh-HK", "th-TH", "zh-TW" };
                    List<string> items = new List<string>();
                    foreach (var language in languages)
                    {
                        var opco = db.GetItem("/sitecore/content/singapore", Language.Parse(language));
                        items.AddRange(opco.Axes.GetDescendants().Where(i => i.Statistics.Updated > new DateTime(2017, 9, 12)).Select(i => i.Paths.FullPath));
                        var products = db.GetItem("/sitecore/content/products", Language.Parse(language));
                        var countries = new[] { "au ", "ph ", "nz ", "my ", "vn ", "mm " };
                        foreach (var country in countries)
                        {
                            if (products != null)
                            {
                                items.AddRange(products.Axes.GetDescendants().Where(i => i.Paths.FullPath.ToLower().Contains(country) && i.Statistics.Updated > new DateTime(2017, 9, 12)).Select(i => i.Paths.FullPath));
                            }
                        }
                    }
                    listOfPages.ItemDataBound += (x, y) =>
                    {
                        var item = (string)y.Item.DataItem;
                        var path = (Literal)y.Item.FindControl("path");
                        path.Text = item;
                    };
                    listOfPages.DataSource = items.Distinct();
                    listOfPages.DataBind();
                }
            };
            this.findMediaBtn.Click += (s, ev) =>
            {
                using (new SecurityDisabler())
                {
                    var db = Sitecore.Data.Database.GetDatabase("master");
                    var languages = new[] { "ko-KR", "zh-HK", "th-TH", "zh-TW", "en" };
                    List<string> items = new List<string>();
                    foreach (var language in languages)
                    {
                        var kr = db.GetItem("/sitecore/content/korea", Language.Parse(language));
                        var hk = db.GetItem("/sitecore/content/hongkong", Language.Parse(language));
                        var tw = db.GetItem("/sitecore/content/taiwan", Language.Parse(language));
                        var th = db.GetItem("/sitecore/content/thailand", Language.Parse(language));

                        var products = db.GetItem("/sitecore/content/products", Language.Parse(language));

                        items.AddRange(kr.Axes.GetDescendants()
                            .Where(i => !string.IsNullOrEmpty(i["teaserimage"]))
                            .Where(i => db.GetItem(((Sitecore.Data.Fields.ImageField)i.Fields["teaserimage"]).MediaID) == null)
                            .Select(i => ((Sitecore.Data.Fields.ImageField)i.Fields["teaserimage"]).MediaID.ToString()));
                        items.AddRange(hk.Axes.GetDescendants()
                            .Where(i => !string.IsNullOrEmpty(i["teaserimage"]))
                            .Where(i => db.GetItem(((Sitecore.Data.Fields.ImageField)i.Fields["teaserimage"]).MediaID) == null)
                            .Select(i => ((Sitecore.Data.Fields.ImageField)i.Fields["teaserimage"]).MediaID.ToString()));
                        items.AddRange(tw.Axes.GetDescendants()
                            .Where(i => !string.IsNullOrEmpty(i["teaserimage"]))
                            .Where(i => db.GetItem(((Sitecore.Data.Fields.ImageField)i.Fields["teaserimage"]).MediaID) == null)
                            .Select(i => ((Sitecore.Data.Fields.ImageField)i.Fields["teaserimage"]).MediaID.ToString()));
                        items.AddRange(th.Axes.GetDescendants()
                            .Where(i => !string.IsNullOrEmpty(i["teaserimage"]))
                            .Where(i => db.GetItem(((Sitecore.Data.Fields.ImageField)i.Fields["teaserimage"]).MediaID) == null)
                            .Select(i => ((Sitecore.Data.Fields.ImageField)i.Fields["teaserimage"]).MediaID.ToString()));

                        items.AddRange(products.Axes.GetDescendants()
                            .Where(i => !string.IsNullOrEmpty(i["teaserimage"]))
                            .Where(i => db.GetItem(((Sitecore.Data.Fields.ImageField)i.Fields["teaserimage"]).MediaID) == null)
                            .Select(i => ((Sitecore.Data.Fields.ImageField)i.Fields["teaserimage"]).MediaID.ToString()));

                        items.AddRange(kr.Axes.GetDescendants()
                            .Where(i => !string.IsNullOrEmpty(i["bannerimage"]))
                            .Where(i => db.GetItem(((Sitecore.Data.Fields.ImageField)i.Fields["bannerimage"]).MediaID) == null)
                            .Select(i => ((Sitecore.Data.Fields.ImageField)i.Fields["bannerimage"]).MediaID.ToString()));
                        items.AddRange(hk.Axes.GetDescendants()
                            .Where(i => !string.IsNullOrEmpty(i["bannerimage"]))
                            .Where(i => db.GetItem(((Sitecore.Data.Fields.ImageField)i.Fields["bannerimage"]).MediaID) == null)
                            .Select(i => ((Sitecore.Data.Fields.ImageField)i.Fields["bannerimage"]).MediaID.ToString()));
                        items.AddRange(tw.Axes.GetDescendants()
                            .Where(i => !string.IsNullOrEmpty(i["bannerimage"]))
                            .Where(i => db.GetItem(((Sitecore.Data.Fields.ImageField)i.Fields["bannerimage"]).MediaID) == null)
                            .Select(i => ((Sitecore.Data.Fields.ImageField)i.Fields["bannerimage"]).MediaID.ToString()));
                        items.AddRange(th.Axes.GetDescendants()
                            .Where(i => !string.IsNullOrEmpty(i["bannerimage"]))
                            .Where(i => db.GetItem(((Sitecore.Data.Fields.ImageField)i.Fields["bannerimage"]).MediaID) == null)
                            .Select(i => ((Sitecore.Data.Fields.ImageField)i.Fields["bannerimage"]).MediaID.ToString()));
                        
                        //   var countries = new[] { "au ", "ph ", "nz ", "my ", "vn ", "mm " };

                    }
                    listOfPages2.ItemDataBound += (x, y) =>
                    {
                        var item = (string)y.Item.DataItem;
                        var path = (Literal)y.Item.FindControl("path");
                        path.Text = item;
                    };
                    listOfPages2.DataSource = items.Distinct();
                    listOfPages2.DataBind();
                }
            };
        }
    }
}