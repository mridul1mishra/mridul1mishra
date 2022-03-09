using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sitecoreforms.Models
{
    public class Forms
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