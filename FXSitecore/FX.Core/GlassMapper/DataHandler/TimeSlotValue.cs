using System;
using System.Text.RegularExpressions;
using System.Web;

namespace FX.Core.GlassMapper.DataHandler
{
	public class TimeSlotValue
	{
		public string RawValue { get; set; }

		public TimeSpan StartTime { get; set; }

		public TimeSpan EndTime { get; set; }

		public TimeSlotValue(string value)
		{
			RawValue = (value ?? string.Empty).Trim();
			ParseTimeSlot();
		}

		protected void ParseTimeSlot()
		{
			StartTime = EndTime = TimeSpan.Zero;
			if (!string.IsNullOrEmpty(RawValue)){
				var match = Regex.Match(RawValue, @"^([01]\d:[0-5][0-9]|2[0-3]:[0-5][0-9])\s?-\s?([01]\d:[0-5][0-9]|2[0-3]:[0-5][0-9])$");
				if (match.Success)
				{
					StartTime = ParseTimeSpan(match.Groups[1].Value);
					EndTime = ParseTimeSpan(match.Groups[2].Value);
				}
			}
		}

		protected TimeSpan ParseTimeSpan(string value)
		{
			return DateTime.ParseExact(value, "HH:mm", System.Globalization.CultureInfo.InvariantCulture).TimeOfDay;
		}

		public bool IsAvailable(TimeSpan currentTime)
		{
			return (currentTime.CompareTo(StartTime) >= 0) &&
				(currentTime.CompareTo(EndTime) <= 0);
		}

		public bool IsAvailable(DateTime currentDate)
		{
			return IsAvailable(currentDate.TimeOfDay);
		}
	}
}
