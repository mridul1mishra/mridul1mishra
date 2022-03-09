using System;

namespace FX.Core.GlassMapper.DataHandler
{
	public class MultiLineText
	{
		public string RawValue { get; set; }

		public MultiLineText(string value)
		{
			RawValue = value;
		}

		public static implicit operator string(MultiLineText value)
		{
			if (value == null)
				return string.Empty;
			return value.ToString();
		}

		public static implicit operator MultiLineText(string value)
		{
			return new MultiLineText(value);
		}

		public override string ToString()
		{
			if (string.IsNullOrEmpty(RawValue))
			{
				return string.Empty;
			}
			return RawValue.Replace(Environment.NewLine, "<br />").Replace("\n", "<br />");
		}
	}
}
