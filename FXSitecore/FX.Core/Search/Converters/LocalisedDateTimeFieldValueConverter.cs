using FX.Core.GlassMapper.DataHandler;
using Sitecore.ContentSearch.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Core.Search.Converters
{
    public class LocalisedDateTimeFieldValueConverter : AbstractTypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(LocalisedDateTimeValue))
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
            return new LocalisedDateTimeValue((DateTime)value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            string fieldFormat = this.GetFieldFormat(context as IndexFieldConverterContext);
            return (object)((DateTime)value).ToString(fieldFormat, (IFormatProvider)culture);
        }
    }
}
