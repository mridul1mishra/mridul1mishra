using Sitecore.Form.Core.Configuration;
using Sitecore.Forms.Core.Data;
using Sitecore.StringExtensions;
using Sitecore.WFFM.Abstractions.Actions;
using Sitecore.WFFM.Abstractions.Mail;
using System.Collections.Generic;

namespace AD.DroplistTextValueSwap.ProcessMessage
{
    public class DroplistValue
    {
        private List<string> templates;

        public DroplistValue()
        {
            templates = new List<string>();
        }

        public void AddTemplate(string template)
        {
            templates.Add(template);
        }

        public void Process(ProcessMessageArgs args)
        {
            foreach (AdaptedControlResult result in args.Fields)
            {
                FieldItem field = new FieldItem(StaticSettings.ContextDatabase.GetItem(result.FieldID));

                if (!templates.Contains(field.TypeID.ToString()))
                    continue;

                string str = result.Value;

                if (args.MessageType == MessageType.Sms)
                {
                    args.Mail.Replace("[{0}]".FormatWith(new object[] { field.FieldDisplayName }), str ?? string.Empty);
                    args.Mail.Replace("[{0}]".FormatWith(new object[] { field.Name }), str ?? string.Empty);
                }
                else
                {
                    args.Mail.Replace("[<label id=\"{0}\">{1}</label>]".FormatWith(new object[] { field.ID.ToString(), field.Name }), str ?? string.Empty);
                    args.Mail.Replace("[<label id=\"{0}\">{1}</label>]".FormatWith(new object[] { field.ID.ToString(), field.FieldDisplayName }), str ?? string.Empty);
                }
                args.To.Replace(string.Join(string.Empty, new string[] { "[", field.ID.ToString(), "]" }), result.Value);
                args.CC.Replace(string.Join(string.Empty, new string[] { "[", field.ID.ToString(), "]" }), result.Value);
                args.BCC.Replace(string.Join(string.Empty, new string[] { "[", field.ID.ToString(), "]" }), result.Value);
                args.Subject.Replace(string.Join(string.Empty, new string[] { "[", field.ID.ToString(), "]" }), result.Value);

                args.To.Replace(string.Join(string.Empty, new string[] { "[", result.FieldName, "]" }), result.Value);
                args.CC.Replace(string.Join(string.Empty, new string[] { "[", result.FieldName, "]" }), result.Value);
                args.BCC.Replace(string.Join(string.Empty, new string[] { "[", result.FieldName, "]" }), result.Value);
                args.Subject.Replace(string.Join(string.Empty, new string[] { "[", result.FieldName, "]" }), result.Value);
            }
        }
    }
}
