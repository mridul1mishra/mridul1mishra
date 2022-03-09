using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sitecore.Data;
using Sitecore.Resources.Media;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace FX.Core.Models.Reseller
{
    [XmlRoot(ElementName = "resellers", Namespace ="")]
    public class Resellers
    {
        public Resellers() { AllResellers = new List<Reseller>(); Regions = new List<string>(); }

        [XmlElement("reseller")]
        public List<Reseller> AllResellers { get; set; }
        
        public List<string> Regions { get; set; }

        public static Resellers GetResellers(ID id)
        {
            try
            {
                var mediaItem = new Sitecore.Data.Items.MediaItem(Sitecore.Context.Database.GetItem(id));
                var media = MediaManager.GetMedia(mediaItem);

                var doc = new XmlDocument();
                doc.Load(new StreamReader(media.GetStream().Stream));

                var deserializer = new XmlSerializer(typeof(Resellers));
                Resellers resellers = (Resellers)deserializer.Deserialize(media.GetStream().Stream);
                resellers.AllResellers.ForEach(x => x.Address = FormatAddress(x.Address));

                resellers.Regions = resellers.AllResellers.GroupBy(x => x.Region).Select(x => x.Key).ToList();

                return resellers;
            }
            catch (Exception) { }


            return new Resellers();
        }

        public static string FormatAddress(string address)
        {
            var split = address.Split(',');
            string result = "";
            foreach (string s in split)
            {
                result += $"<p>{s}</p>";
            }

            return result;
        }

        public string GetJson()
        {
            var regions = this.AllResellers.ToLookup(x => x.Region);

            return JsonConvert.SerializeObject(regions, new LookupSerializer());
        }
    }
    
    public class Reseller
    {
        [XmlElement("region")]
        [JsonIgnore]
        public string Region { get; set; }
        [XmlElement("name")]
        [JsonProperty("name")]
        public string Name { get; set; }
        [XmlElement("address")]
        [JsonProperty("address")]
        public string Address { get; set; }
        [XmlElement("contact")]
        [JsonProperty("contact")]
        public string Contact { get; set; }
        [XmlElement("fax")]
        [JsonProperty("fax")]
        public string Fax { get; set; }
        [XmlElement("email")]
        [JsonProperty("email")]
        public string Email { get; set; }
        [XmlElement("web")]
        [JsonProperty("webURL")]
        public string Web { get; set; }
        [XmlElement("otherInfo")]
        [JsonProperty("otherInfo")]
        public string OtherInfo { get; set; }
        [XmlElement("latitude")]
        [JsonProperty("lat")]
        public string Latitude { get; set; }
        [XmlElement("longitude")]
        [JsonProperty("long")]
        public string Longitude { get; set; }
        [XmlElement("formURL")]
        [JsonProperty("formURL")]
        public string FormURL { get; set; }
    }
    

    public class LookupSerializer : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            var result = objectType.GetInterfaces().Any(a => a.IsGenericType
                && a.GetGenericTypeDefinition() == typeof(ILookup<,>));
            return result;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var obj = new JObject();
            var enumerable = (IEnumerable)value;

            foreach (object kvp in enumerable)
            {
                // TODO: caching
                var keyProp = kvp.GetType().GetProperty("Key");
                var keyValue = keyProp.GetValue(kvp, null);

                obj.Add(keyValue.ToString(), JArray.FromObject((IEnumerable)kvp));
            }

            obj.WriteTo(writer);
        }
    }
}
