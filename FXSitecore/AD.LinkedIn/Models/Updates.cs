
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.LinkedIn.Models
{

    public class Updates
    {
        public int _total { get; set; }
        public Value[] values { get; set; }
    }

    public class Value
    {
        public bool isCommentable { get; set; }
        public bool isLikable { get; set; }
        public bool isLiked { get; set; }
        public int numLikes { get; set; }
        public long timestamp { get; set; }
        public Updatecomments updateComments { get; set; }
        public Updatecontent updateContent { get; set; }
        public string updateKey { get; set; }
        public string updateType { get; set; }
    }

    public class Updatecomments
    {
        public int _total { get; set; }
    }

    public class Updatecontent
    {
        public Company company { get; set; }
        public Companystatusupdate companyStatusUpdate { get; set; }
    }

    public class Company
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Companystatusupdate
    {
        public Share share { get; set; }
    }

    public class Share
    {
        public string comment { get; set; }
        public string id { get; set; }
        public Source source { get; set; }
        public long timestamp { get; set; }
        public Visibility visibility { get; set; }
    }

    public class Source
    {
        public Serviceprovider serviceProvider { get; set; }
        public string serviceProviderShareId { get; set; }
    }

    public class Serviceprovider
    {
        public string name { get; set; }
    }

    public class Visibility
    {
        public string code { get; set; }
    }

}