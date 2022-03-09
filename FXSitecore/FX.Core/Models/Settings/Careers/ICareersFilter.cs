using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Core.Models.Settings.Careers
{
    [SitecoreType(
        EnforceTemplate = Glass.Mapper.Sc.Configuration.SitecoreEnforceTemplate.TemplateAndBase, 
        TemplateId = Templates.CareerFilter.Id)]
    public interface ICareersFilter : ISitecoreItem
    {
        [SitecoreChildren]
        IEnumerable<ICareersFilterItem> FilterItems { get; set; }

        [SitecoreField("Filter Name")]
        string FilterName { get; set; }

        [SitecoreField("Filter Key")]
        string FilterKey { get; set; }

        [SitecoreField("AllFiltersLabel")]
        string AllFiltersLabel { get; set; }
    }
}
