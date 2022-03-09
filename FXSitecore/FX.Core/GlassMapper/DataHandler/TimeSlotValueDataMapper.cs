using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Configuration;

namespace FX.Core.GlassMapper.DataHandler
{
	public class TimeSlotValueDataMapper : Glass.Mapper.Sc.DataMappers.AbstractSitecoreFieldMapper
	{
		public TimeSlotValueDataMapper()
			: base(new[] { typeof(TimeSlotValue) })
		{
		}
		public override string SetFieldValue(object value, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
		{
			var timeSlot = value as TimeSlotValue;
			return timeSlot == null ? string.Empty : timeSlot.RawValue;
		}

		public override object GetFieldValue(string fieldValue, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
		{
			return new TimeSlotValue(fieldValue);
		}
	}
}
