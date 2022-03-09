using System.Runtime.Serialization;
using Sitecore.Data.Validators;
using System;
using System.Text;

namespace FX.Core.Validators
{
    [Serializable]
    public class ImageDimensionValidator : Sitecore.Data.Validators.StandardValidator
    {
        public ImageDimensionValidator()
        {
        }

        public ImageDimensionValidator(SerializationInfo info, StreamingContext context)
        : base(info, context)
        {
        }

        public override string Name
        {
            get { return ("ImageDimensionValidator"); }
        }

        protected override ValidatorResult GetMaxValidatorResult()
        {
            if (Parameters.ContainsKey("Result"))
            {
                switch (Parameters["Result"])
                {
                    case "Valid": return ValidatorResult.Valid;
                    case "Suggestion": return ValidatorResult.Suggestion;
                    case "Warning": return ValidatorResult.Warning;
                    case "Error": return ValidatorResult.Error;
                    case "CriticalError": return ValidatorResult.CriticalError;
                    case "FatalError": return ValidatorResult.FatalError;
                    default: return ValidatorResult.Valid;
                }
            }
            return ValidatorResult.Unknown;
        }

        protected override ValidatorResult Evaluate()
        {
            Result = ValidatorResult.Valid;
            Sitecore.Data.Fields.ImageField img = GetField();
            if (img.Value == "") return Result;
            
            StringBuilder txt = new StringBuilder(this.GetFieldDisplayName() + ": The image must be ");

            if (Parameters.ContainsKey("w"))
                if (img.Width != Parameters["w"])
                {
                    txt.Append(string.Format("{0} pixels wide", Parameters["w"]));
                    Result = GetMaxValidatorResult();
                }

            if (Parameters.ContainsKey("h"))
                if (img.Height != Parameters["h"])
                {
                    if (Result == GetMaxValidatorResult())
                        txt.Append(" and ");
                    txt.Append(string.Format("{0} pixels tall", Parameters["h"]));
                    Result = GetMaxValidatorResult();
                }

            if (Result == GetMaxValidatorResult())
            {
                txt.Append(".");
                Text = txt.ToString();
            }

            return Result;
        }
    }
}
