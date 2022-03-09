using FX.Core.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Core.Models.Base
{
    public interface IInsightTagFields
    {
        IEnumerable<ITaxonomyItem> InsightTags { get; set; }
    }
}
