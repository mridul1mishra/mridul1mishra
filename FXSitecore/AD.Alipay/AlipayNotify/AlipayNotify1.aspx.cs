using Aop.Api.Util;
using FX.Core;
using Sitecore;
using Sitecore.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AD.Alipay
{
    public partial class AlipayNotify1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var alipayPubKey = Config.alipay_public_key.Trim();

            Dictionary<string, string> sArray = GetRequestPost();

            if (sArray.Count != 0)
            {
                string notifyId = string.Empty;
                string sign = string.Empty;
                string signType = "RSA2";

                sArray.TryGetValue("notify_id", out notifyId);
                sArray.TryGetValue("sign", out sign);
                sArray.TryGetValue("sign_type", out signType);

                bool flag = CheckNotify(notifyId, sign, signType);

                if (flag)
                {

                    string out_trade_no = Request.Form["out_trade_no"];

                    //支付宝交易号
                    //trade_no

                    string trade_no = Request.Form["trade_no"];

                    //交易状态
                    //trade_status
                    string trade_status = Request.Form["trade_status"];

                    string trade_time = Request.Form["notify_time"];

                    string total_fee = Request.Form["total_amount"];

                    try
                    {
                        var emailTo = FXContextItems.HomePage.SiteSettings.SitecoreItem["NotifyEmail1"];

                        MailMessage message = new MailMessage(Config.alipay_from_email, emailTo);
                        message.Subject = out_trade_no;
                        message.IsBodyHtml = true;
                        message.Body = string.Format("<table><tbody><tr><td>商户订单号</td><td>{0}</td></tr><tr><td>支付日期</td><td>{1}</td></tr><tr><td>付款总金额</td><td>{2}</td></tr><tr><td>收款公司</td><td>{3}</td></tr><tr><td>支付方式</td><td>支付宝</td></tr></tbody></table>", out_trade_no, trade_time, total_fee, "富士施乐实业发展（中国）有限公司");

                        MainUtil.SendMail(message);

                    }
                    catch (Exception ex)
                    {
                        Sitecore.Diagnostics.Log.Error(ex.Message, this);
                    }

                    Response.Write("success");
                }
                else
                {
                    Response.Write("fail");
                }
            }

        }

        public Dictionary<string, string> GetRequestPost()
        {
            int i = 0;
            Dictionary<string, string> sArray = new Dictionary<string, string>();
            NameValueCollection coll;
            coll = Request.Form;
            String[] requestItem = coll.AllKeys;
            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.Form[requestItem[i]]);
            }
            return sArray;
        }

        public bool CheckNotify(string notifyId, string sign, string signType)
        {
            Diagnostics.AlipayLogger.Log.Info("NOTIFY: " + notifyId + "/" + sign + "/" + signType);

            var request = HttpWebRequest.Create(Config.gatewayUrl);

            var postData = String.Format("service={0}&partner={1}&notify_id={2}&sign={3}&sign_type={4}", "notify_verify", Config.partner, notifyId, sign, signType);

            var data = Encoding.UTF8.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            
            Diagnostics.AlipayLogger.Log.Info("NOTIFY_RESPONSE: " + responseString);

            try
            {
                var result = System.Convert.ToBoolean(responseString);

                Diagnostics.AlipayLogger.Log.Info("NOTIFY_RESULT: " + result);

                return result;
            }
            catch
            {
                return false;
            }
        }
    }
}
