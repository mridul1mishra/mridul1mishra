using ExcelDataReader;
using Sitecore.ContentSearch;
using System.Web.Mvc;
using System.Linq;
using SitecoreProjectMediaLib.Model;
using Sitecore.Data;

namespace SitecoreProjectMediaLib.Controllers
{
    public class MediaLibController : Controller
    {
        // GET: MediaLib
        public ActionResult Index()
        {
            return View();
        }
        public void DetachMedia()
        {
            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var home = db.GetItem("/sitecore/content/Products");
            var children = home.Children;
            for (int childIndex = 0; childIndex < children.Count; childIndex++)
            {
                var child = children[childIndex];
                var homechild = child.Children;
                if(homechild!=null)
                { 
                for (int homechildIndex = 0; homechildIndex < homechild.Count; homechildIndex++)
                {
                    var homechildren = homechild[homechildIndex];
                    homechildren.Fields.ReadAll();
                    if(homechildren.Fields["CopyrightText"] !=null)
                    {
                        if(homechildren.Fields["CopyrightText"]?.Value != "This page includes Fuji Xerox product(s), licensed from Xerox Corporation.<br>\nThe distributor of the product(s) is FUJIFILM Business Innovation Corp.")
                        {
                            using (new Sitecore.SecurityModel.SecurityDisabler())
                            {
                                homechildren.Editing.BeginEdit();
                                homechildren.Fields["CopyrightText"].Value = "This page includes Fuji Xerox product(s), licensed from Xerox Corporation.<br>\nThe distributor of the product(s) is FUJIFILM Business Innovation Corp.";
                                homechildren.Editing.EndEdit();
                            }
                        }                        
                    }
                }
                }
                child.Fields.ReadAll();
                if (child.Fields["CopyrightText"] != null)
                {
                    if (child.Fields["CopyrightText"]?.Value != "This page includes Fuji Xerox product(s), licensed from Xerox Corporation. The distributor of the product(s) is FUJIFILM Business Innovation Corp.")
                    {
                        using (new Sitecore.SecurityModel.SecurityDisabler())
                        {
                            child.Editing.BeginEdit();
                            child.Fields["CopyrightText"].Value = "This page includes Fuji Xerox product(s), licensed from Xerox Corporation. The distributor of the product(s) is FUJIFILM Business Innovation Corp.";
                            child.Editing.EndEdit();
                        }
                    }                    
                }
            }
        }
    }
}