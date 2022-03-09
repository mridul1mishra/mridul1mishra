// Decompiled with JetBrains decompiler
// Type: Sitecore.Support.Form.Core.Pipelines.ProcessMessage.ProcessMessage
// Assembly: Sitecore.Support.110301.v2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0913C4D1-6254-48A6-A41F-14B986712A44
// Assembly location: D:\Websites\FX Build\Website\bin\Sitecore.Support.110301.v2.dll

using Sitecore.WFFM.Abstractions.Mail;
using System;
using System.Net.Mail;
using Sitecore.Forms.Core.Data;
using Sitecore.Form.Core.Configuration;
using Sitecore.StringExtensions;
using Sitecore.WFFM.Abstractions.Actions;
using System.Web;

namespace Sitecore.Support.Form.Core.Pipelines.ProcessMessage
{
    public class ProcessMessage
    {
        public void SendEmail(ProcessMessageArgs args)
        {
            try
            {
                SmtpClient smtpClient1 = new SmtpClient(args.Host);
                int num = args.EnableSsl ? 1 : 0;
                smtpClient1.EnableSsl = num != 0;
                SmtpClient smtpClient2 = smtpClient1;
                if (args.Port != 0)
                    smtpClient2.Port = args.Port;
                smtpClient2.Credentials = args.Credentials;
                smtpClient2.Send(this.GetMail(args));
            }
            catch(Exception ex)
            {
                HttpCookie userInfo = new HttpCookie("userInfo");
                userInfo["UserName"] = ex.Message;
                
                userInfo.Expires.Add(new TimeSpan(0, 1, 0));
                HttpContext.Current.Response.Cookies.Add(userInfo);
            }
            
        }

        private MailMessage GetMail(ProcessMessageArgs args)
        {
            MailMessage mailMessage = new MailMessage(args.From.Replace(";", ","), args.To.Replace(";", ",").ToString(), args.Subject.ToString(), args.Mail.ToString());
            mailMessage.IsBodyHtml = args.IsBodyHtml;
            foreach (AdaptedControlResult result in args.Fields)
            {
                FieldItem field = new FieldItem(StaticSettings.ContextDatabase.GetItem(result.FieldID));
                args.BCC.Replace(string.Join(string.Empty, new string[] { "[", field.ID.ToString(), "]" }), result.Value);
            }
            if (args.IsBodyHtml)
            {
                mailMessage.Body = System.Web.HttpUtility.HtmlDecode(mailMessage.Body);
            }
            MailMessage mail = mailMessage;
            if (args.CC.Length > 0)
            {
                string str = args.CC.Replace(";", ",").ToString();
                char[] chArray = new char[1] { ',' };
                foreach (string address in str.Split(chArray))
                    mail.CC.Add(new MailAddress(address));
            }
            if (args.BCC.Length > 0)
            {
             // args.BCC.Replace(string.Format("[<label id=\"{0}\">{1}</label>]", args.BCC.ID, field.Title), enteredValue); 
              
                string str = args.BCC.Replace(";", ",").ToString();
                char[] chArray = new char[1] { ',' };
                foreach (string address in str.Split(chArray))
                    mail.Bcc.Add(new MailAddress(address));
            }
            args.Attachments.ForEach((Action<Attachment>)(attachment => mail.Attachments.Add(attachment)));
            return mail;
        }
    }
}
