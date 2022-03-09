using Newtonsoft.Json;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FX.Core.Models.Form
{
    public class FormMigration
    {
        public String Text { get; set; }
        public string ValueId { get; set; }
        public List<FieldItem> child { get; set; }        
    }
    public class FieldItem
    {
        public string Name { get; set; }
        public string FieldId { get; set; }
        public string FieldTitle { get; set; }
        public string FieldType { get; set; }
    }
   
}
