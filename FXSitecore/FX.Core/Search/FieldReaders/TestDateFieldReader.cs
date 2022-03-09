using Sitecore.ContentSearch;
using Sitecore.ContentSearch.FieldReaders;
using Sitecore.Data.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Core.Search.FieldReaders
{
    public class TestDateFieldReader : DateFieldReader
    {
        public override object GetFieldValue(IIndexableDataField indexableField)
        {
            Field field = indexableField as SitecoreItemDataField;
            DateField dateField = FieldTypeManager.GetField(field) as DateField;
            if (dateField != null)
            {
                return dateField.DateTime;
            }
            return null;
        }
    }
}
