using FX.Core.Search.Fields;
using Sitecore.Data.Fields;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Core.Search.Converters
{
	public class ButtonLinkFieldValueConverter : TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(ButtonLinkField))
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
			ButtonLinkField result = new ButtonLinkField();
			if (value != null)
			{
				try
				{
					string fieldValue = (string)value;
					string[] args = fieldValue.Split('|');
					result.Url = args[0];
					result.Target = args[1];
					result.Text = args[2];
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
			ButtonLinkField field = (ButtonLinkField)value;
			if (field != null)
			{
				return string.Format("{0}|{1}|{2}", field.Url, field.Target, field.Text);
			}
			return string.Empty;
		}
	}
}
