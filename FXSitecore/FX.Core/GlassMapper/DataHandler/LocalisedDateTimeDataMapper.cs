using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Core.GlassMapper.DataHandler
{
    public class LocalisedDateTimeDataMapper : Glass.Mapper.Sc.DataMappers.AbstractSitecoreFieldMapper
    {
        public LocalisedDateTimeDataMapper()
			: base(new[] { typeof(LocalisedDateTimeValue) })
		{
        }
        public override string SetFieldValue(object value, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            var localDateTimeValue = value as LocalisedDateTimeValue;
            return localDateTimeValue == null ? string.Empty : localDateTimeValue.RawValue;
        }

        public override object GetFieldValue(string fieldValue, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            return new LocalisedDateTimeValue(fieldValue);
        }
    }
}
