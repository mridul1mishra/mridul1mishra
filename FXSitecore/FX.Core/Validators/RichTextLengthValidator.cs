using System.Runtime.Serialization;
using Sitecore.Data.Validators;
using System;
using System.Text;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Configuration;
using Sitecore.Data.Items;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;

namespace FX.Core.Validators
{
    [Serializable]
    public class RichTextLengthValidator : StandardValidator
    {
        public RichTextLengthValidator(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }

        public RichTextLengthValidator()
        {

        }

        public override string Name
        {
            get { return "RichTextLengthValidator"; }
        }

        private int Length
        {
            get
            {
                int length = int.MaxValue;
                if (Parameters.ContainsKey("length"))
                {
                    if (!int.TryParse(Parameters["length"], out length))
                        length = int.MaxValue;
                }
                return length;
            }
        }

        protected override ValidatorResult Evaluate()
        {
            ItemUri itemUri = ItemUri;

            if (itemUri == null)
                return ValidatorResult.Valid;

            Field field = GetField();
            if (field == null)
                return ValidatorResult.Valid;

            string str1 = field.Value;
            if (string.IsNullOrEmpty(str1))
                return ValidatorResult.Valid;

            //if (!(fieldItem.Fields["Maximum Length"] != null 
            //    && !string.IsNullOrWhiteSpace(fieldItem.Fields["Maximum Length"].Value)
            //    && int.TryParse(fieldItem.Fields["Maximum Length"].Value, out maxLength) && maxLength > 0))
            //    return ValidatorResult.Valid;

            if (StripHtml(str1).Length > Length)
            {
                Text = GetText("Character count '{0}' exceeded the limit '{1}' for field \"{2}\".", new[]
                {
                    StripHtml(str1).Length.ToString(CultureInfo.InvariantCulture),
                    Length.ToString(CultureInfo.InvariantCulture),
                    field.DisplayName
                });
                return GetFailedResult(ValidatorResult.CriticalError);
            }

            return ValidatorResult.Valid;
        }

        protected override ValidatorResult GetMaxValidatorResult()
        {
            return GetFailedResult(ValidatorResult.CriticalError);
        }

        public static string StripHtml(string str)
        {
            return HttpUtility.HtmlDecode(Regex.Replace(str, "<.*?>", string.Empty));
        }
    }
}
