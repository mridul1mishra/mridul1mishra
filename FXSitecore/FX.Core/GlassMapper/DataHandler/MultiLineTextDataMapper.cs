using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Configuration;

namespace FX.Core.GlassMapper.DataHandler
{
	public class MultiLineTextDataMapper : Glass.Mapper.Sc.DataMappers.AbstractSitecoreFieldMapper
	{
		public MultiLineTextDataMapper()
			: base(new[] { typeof(MultiLineText) })
		{
		}
		public override string SetFieldValue(object value, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
		{
			MultiLineText encoded = value as MultiLineText;
			return encoded == null ? null : encoded.RawValue;
		}

		public override object GetFieldValue(string fieldValue, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
		{
			return new MultiLineText(fieldValue);
		}
	}
}
