using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.TwitterTools.Interfaces
{
    public interface ISearchSettings
    {
        string SearchFormat { get; set; }
        string SearchQuery { get; set; }
        string SearchUrl { get; }
    }
}
