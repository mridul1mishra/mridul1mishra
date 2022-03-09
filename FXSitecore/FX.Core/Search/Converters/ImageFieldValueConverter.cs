using FX.Core.Search.Fields;
using System;
using System.ComponentModel;

namespace FX.Core.Search.Converters
{
	public class ImageFieldValueConverter : TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(ImageField))
				return true;
			else
				return base.CanConvertFrom(context, sourceType);
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(string))
				return true;
			else
				return base.CanConvertTo(context, destinationType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			ImageField result = new ImageField();

			if (value != null)
			{
				try
				{
					string fieldValue = (string)value;
					string[] args = fieldValue.Split('|');
					result.Url = args[0];
					result.Alt = args[1];
				}
				catch
				{
					//do nothing
				}
			}

			return (object)result;
		}

		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
		{
			ImageField imageField = (ImageField)value;
			if (imageField != null)
			{
				return string.Format("{0}|{1}", imageField.Url, imageField.Alt);
			}
			return string.Empty;
		}
	}
}
