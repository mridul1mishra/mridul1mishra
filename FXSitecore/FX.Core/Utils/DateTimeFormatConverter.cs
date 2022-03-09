using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Core.Utils
{
	public class DateTimeFormatConverter : IsoDateTimeConverter
	{
		public DateTimeFormatConverter(string format)
		{
			DateTimeFormat = format;
		}
	}
}
