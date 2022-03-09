using FX.Core.Models.Microsite.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Core.Models.Microsite.Page
{
    public interface IMicrositeHomePage : IMicrositePage
    {
        IMicrositeSettings SiteSettings { get; set; }
    }
}
