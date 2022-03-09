using Sitecore.Data;
using Sitecore.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FX.Core.Serialization
{
    public class SerializablePath
    {
        public string FullPath { get; set; }
        public string ParentPath { get; set; }

    }
    public class SerializableLanguage
    {
        public string Name { get; set; }
    }
    public class SerializableItem
    {
        public Guid ID { get; set; }
        public ID TemplateID { get; set; }
        public SerializablePath Paths { get; set; }
        public string Name { get; set; }
        public SerializableLanguage Language { get; set; }
        public Dictionary<string, string> Fields { get; set; }
    }
}
