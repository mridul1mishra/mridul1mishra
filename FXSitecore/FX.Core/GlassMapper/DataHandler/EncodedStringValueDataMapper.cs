using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Configuration;

namespace FX.Core.GlassMapper.DataHandler
{
	public class EncodedStringValueDataMapper: Glass.Mapper.Sc.DataMappers.AbstractSitecoreFieldMapper
    {
		public EncodedStringValueDataMapper()
			: base(new[] { typeof(EncodedStringValue) })
        {
        }
        public override string SetFieldValue(object value, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
			EncodedStringValue encoded = value as EncodedStringValue;
            return encoded == null ? null : encoded.RawValue;
        }

        public override object GetFieldValue(string fieldValue, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
			return new EncodedStringValue(fieldValue);
        }
    }
}
