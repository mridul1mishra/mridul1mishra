using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.FacebookTools.Models
{
    public class Posts
    {
        public List<Post> data { get; set; }
        public Paging Paging { get; set; }
    }

    public class Paging
    {
        public Cursor Cursors { get; set; }
        public string Next { get; set; }
    }

    public class Cursor
    {
        public string Before { get; set; }
        public string After { get; set; }
    }

    public class Post
    {
        public string ID { get; set; }
        public string Message { get; set; }
        public DateTime Created_Time { get; set; }
    }
}
