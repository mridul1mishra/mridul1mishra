using System.Web;

namespace FX.Core.GlassMapper.DataHandler
{
	public class EncodedStringValue
	{
		public string RawValue { get; set; }

		public EncodedStringValue(string value)
		{
			RawValue = value;
		}

		public static implicit operator string(EncodedStringValue encoded)
		{
			if (encoded == null)
				return string.Empty;
			return encoded.ToString();
		}

		public static implicit operator EncodedStringValue(string value)
		{
			return new EncodedStringValue(value);
		}

		public override string ToString()
		{
			if (string.IsNullOrEmpty(RawValue))
			{
				return string.Empty;
			}
			return HttpUtility.HtmlEncode(RawValue);
		}
	}
}
