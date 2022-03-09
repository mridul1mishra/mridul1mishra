using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.TwitterTools.Interfaces
{
    public interface IAuthenticateSettings
    {
        string OAuthConsumerKey { get; set; }
        string OAuthConsumerSecret { get; set; }
        string OAuthUrl { get; set; }
    }
}
