using FX.Core.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Core.Models.SalesForce
{
    public class SalesForceEntry
    {
        public SalesForceEntry()
        {
            __fields = new NameValueCollection();
        }
        public SalesForceEntry(NameValueCollection fields)
        {
            __fields = fields;
        }
        public SalesForceEntry(SalesForceService sfService)
        {
            __sfService = sfService;
        }
        public SalesForceEntry(SalesForceService sfService, NameValueCollection fields)
        {
            __sfService = sfService;
            __fields = fields;
        }
        private SalesForceService __sfService { get; set; }
        private NameValueCollection __fields { get; set; }
        public NameValueCollection Fields
        {
            get
            {
                return __fields;
            }
        }
        public string EntryXml
        {
            get
            {
                //implement xml serializer
                return "";
            }
        }
        public IEnumerable<KeyValuePair<string,string>> KeyValuePairs
        {
            get
            {
                return __fields.AllKeys.Select(k => new KeyValuePair<string,string>( k, __fields[k] )); 
            }
        }
        public string EntryQueryString
        {
            get
            {
                return string.Join("&", __fields.AllKeys.Select(k => string.Format("{0}={1}", k, __fields[k])));
            }
        }
        public string EntryJson
        {
            get
            {
                //implement json serializer
                return "";
            }
        }
    }
}
