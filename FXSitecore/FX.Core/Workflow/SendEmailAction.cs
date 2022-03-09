using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Workflows.Simple;
using System;
using System.Net.Mail;

namespace FX.Core.Workflow
{
    /// <summary>
	/// Represents an Email Action.
	/// </summary>
	public class EmailAction
    {
        /// <summary>
        /// Runs the processor.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public void Process(WorkflowPipelineArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            ProcessorItem processorItem = args.ProcessorItem;
            if (processorItem == null)
            {
                return;
            }
            Item innerItem = processorItem.InnerItem;
            string fullPath = innerItem.Paths.FullPath;
            string text = this.GetText(innerItem, "from", args);
            string text2 = this.GetText(innerItem, "to", args);
            string text3 = this.GetText(innerItem, "mail server", args);
            string text4 = this.GetText(innerItem, "subject", args);
            string text5 = this.GetText(innerItem, "message", args);
            Error.Assert(text2.Length > 0, "The 'To' field is not specified in the mail action item: " + fullPath);
            Error.Assert(text.Length > 0, "The 'From' field is not specified in the mail action item: " + fullPath);
            Error.Assert(text4.Length > 0, "The 'Subject' field is not specified in the mail action item: " + fullPath);
            //Error.Assert(text3.Length > 0, "The 'Mail server' field is not specified in the mail action item: " + fullPath);
            System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage(text, text2);
            mailMessage.Subject = text4;
            mailMessage.Body = text5;
            System.Net.Mail.SmtpClient smtpClient = !string.IsNullOrEmpty(text3) ? new System.Net.Mail.SmtpClient(text3) : new System.Net.Mail.SmtpClient();
            smtpClient.Send(mailMessage);
        }
        /// <summary>
        /// Gets the text.
        /// </summary>
        /// <param name="commandItem">The command item.</param>
        /// <param name="field">The field.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        private string GetText(Item commandItem, string field, WorkflowPipelineArgs args)
        {
            string text = commandItem[field];
            if (text.Length > 0)
            {
                return this.ReplaceVariables(text, args);
            }
            return string.Empty;
        }
        /// <summary>
        /// Replaces the variables.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        private string ReplaceVariables(string text, WorkflowPipelineArgs args)
        {
            text = text.Replace("$itemPath$", args.DataItem.Paths.FullPath);
            text = text.Replace("$itemLanguage$", args.DataItem.Language.ToString());
            text = text.Replace("$itemVersion$", args.DataItem.Version.ToString());
            string comments = "";
            foreach (string s in args.CommentFields.Values)
            {
                comments += s + "\n";
            }
            text = text.Replace("$comments$", comments);

            return text;
        }
    }
}
