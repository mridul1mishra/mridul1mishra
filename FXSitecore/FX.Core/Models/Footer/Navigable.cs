using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using FX.Core.Models.Base;

namespace FX.Core.Models.Footer
{    
        [SitecoreType(TemplateId = Constants.Footer.NavigableTemplateId)]
        public interface Navigable : ISitecoreItem
        {
            [SitecoreField(FieldId = Constants.Footer.TitleId)]
            string Title { get; set; }
            [SitecoreField(FieldId = Constants.Footer.LinkId)]
            Link Link { get; set; }
            [SitecoreField(FieldId = Constants.Footer.LevelId)]
            string Level { get; set; }
            [SitecoreChildren]
            IEnumerable<Navigable> Child { get; set; }
        }
    
}
