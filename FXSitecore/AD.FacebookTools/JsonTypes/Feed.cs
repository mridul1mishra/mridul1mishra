using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.FacebookTools.JsonTypes
{
    public class Feed
    {
        public IEnumerable<Data> data;
        public Paging paging;
    }

    public class Data
    {
        public string message;
        public string created_time;
        public string id;
        public string full_picture;
        public string link;
        public From from;
    }

    public class From
    {
        public string name;
        public string id;
    }
    public class Paging
    {
        public string previous;
        public string next;
    }
}
