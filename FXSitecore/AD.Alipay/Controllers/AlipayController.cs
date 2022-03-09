using AD.Alipay.Models;
using Aop.Api;
using Aop.Api.Domain;
using Aop.Api.Request;
using Aop.Api.Response;
using Sitecore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace AD.Alipay.Controllers
{
    public class AlipayController : SitecoreController
    {
        public ActionResult AlipayForm()
        {
            return View("~/Views/Alipay/AlipayForm.cshtml");
        }

        //[HttpPost]
        //public ActionResult AlipaySubmit(FormCollection formFields)
        //{
        //    List<string> values = new List<string>();

        //    foreach(var key in formFields.AllKeys)
        //    {
        //        values.Add(formFields[key]);
        //    }

        //    /////////////////////////////////////////////////

        //    var out_trade_no = formFields["out_trade_no"];
        //    var companyName = formFields["CompanyName"];
        //    var customerName = formFields["customerName"];
        //    var cutomerPhone = formFields["txtPhone"];
        //    var subject = formFields["hidproduct"];
        //    var fee = formFields["price"];

        //    double doubleFee;

        //    if(!double.TryParse(fee, out doubleFee))
        //    {
        //        return View("~/Views/Alipay/AlipayForm.cshtml");
        //    }



        //    if (string.IsNullOrEmpty(out_trade_no))
        //    {
        //        out_trade_no = DateTime.Now.ToString("yyyyMMddhhmmss");
        //    }

        //    string fullOutTradeNo = customerName + out_trade_no;


        //    string notifyUrl = Config.notify_url;

        //    switch (companyName)
        //    {
        //        case "FX_Industrial":
        //            notifyUrl = Config.notify_url;
        //            break;
        //        case "FX_Shanghai":
        //            notifyUrl = Config.notify_url;
        //            break;
        //        case "FX_Leasing":
        //            notifyUrl = Config.notify_url;
        //            break;
        //        default:
        //            notifyUrl = Config.notify_url;
        //            break;
        //    }

        //    //SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
        //    //sParaTemp.Add("service", Config.service);
        //    //sParaTemp.Add("partner", Config.partner);
        //    //sParaTemp.Add("_input_charset", Config.input_charset.ToLower());
        //    //sParaTemp.Add("notify_url", notifyUrl);
        //    //sParaTemp.Add("return_url", Config.return_url);
        //    //sParaTemp.Add("out_trade_no", fullOutTradeNo);
        //    //sParaTemp.Add("subject", subject);
        //    ////sParaTemp.Add("rmb_fee", doubleFee.ToString());
        //    //sParaTemp.Add("rmb_fee", doubleFee.ToString());
        //    //sParaTemp.Add("product_code", "NEW_OVERSEAS_SELLER");
        //    //sParaTemp.Add("body", subject);
        //    //sParaTemp.Add("currency", "USD");


        //    SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
        //    sParaTemp.Add("service", Config.service);
        //    sParaTemp.Add("partner", Config.partner);
        //    sParaTemp.Add("_input_charset", Config.input_charset.ToLower());
        //    sParaTemp.Add("notify_url", notifyUrl);
        //    sParaTemp.Add("return_url", Config.return_url);
        //    sParaTemp.Add("out_trade_no", fullOutTradeNo);
        //    sParaTemp.Add("subject", subject);
        //    //sParaTemp.Add("rmb_fee", doubleFee.ToString());
        //    sParaTemp.Add("total_fee", doubleFee.ToString());
        //    sParaTemp.Add("payment_type", "1");
        //    sParaTemp.Add("paymethod", "bankPay");
        //    sParaTemp.Add("seller_email", "alipay-fxid@chn.fujixerox.com");
        //    //sParaTemp.Add("Seller_id", Config.partner);
        //    //sParaTemp.Add("Seller_account_name", "alipay-fxid@chn.fujixerox.com");
        //    //sParaTemp.Add("product_code", "FAST_INSTANT_TRADE_PAY");


        //    string sHtmlText = Submit.BuildRequest(sParaTemp, "get", "确认");

        //    var viewModel = new AlipaySubmitModel();

        //    viewModel.HTML_Snippet = sHtmlText;

        //    return View("~/Views/Alipay/AlipaySubmit.cshtml", viewModel);
        //}

        [HttpPost]
        public ActionResult AlipaySubmit(FormCollection formFields)
        {

            DefaultAopClient client = new DefaultAopClient(Config.gatewayUrl, Config.app_id, Config.private_key, "json", "1.0", Config.sign_type, Config.alipay_public_key, Config.charset, true);

            var out_trade_no = formFields["out_trade_no"].Trim();
            var companyName = formFields["CompanyName"];
            var customerName = formFields["customerName"];
            var cutomerPhone = formFields["txtPhone"];
            var subject = formFields["hidproduct"];
            var fee = formFields["price"];

            double doubleFee;

            if (!double.TryParse(fee, out doubleFee))
            {
                return Redirect(Request.UrlReferrer.AbsoluteUri);
            }

            string total_amout = doubleFee.ToString();


            if (string.IsNullOrEmpty(out_trade_no))
            {
                out_trade_no = DateTime.Now.ToString("yyyyMMddhhmmss");
            }

            string fullOutTradeNo = customerName + out_trade_no;


            string notifyUrl = Config.notify_url1;

            switch (companyName)
            {
                case "FX_Industrial":
                    notifyUrl = Config.notify_url1;
                    break;
                case "FX_Shanghai":
                    notifyUrl = Config.notify_url2;
                    break;
                case "FX_Leasing":
                    notifyUrl = Config.notify_url3;
                    break;
                default:
                    notifyUrl = Config.notify_url1;
                    break;
            }


            // 组装业务参数model
            AlipayTradePagePayModel model = new AlipayTradePagePayModel();
            model.Subject = subject;
            model.TotalAmount = total_amout;
            model.OutTradeNo = fullOutTradeNo;
            model.ProductCode = "FAST_INSTANT_TRADE_PAY";

            AlipayTradePagePayRequest request = new AlipayTradePagePayRequest();
            // 设置同步回调地址
            request.SetReturnUrl(Config.return_url);
            // 设置异步通知接收地址
            request.SetNotifyUrl(notifyUrl);
            // 将业务model载入到request
            request.SetBizModel(model);

            AlipayTradePagePayResponse response = null;
            try
            {
                Diagnostics.AlipayLogger.Log.Info("SUBMISSION: " + model.Subject + "/" + model.TotalAmount + "/" + model.OutTradeNo);

                response = client.pageExecute(request, null, "post");

                var viewModel = new AlipaySubmitModel();
                viewModel.HTML_Snippet = response.Body;

                Diagnostics.AlipayLogger.Log.Info("RESPONSE: " + response.Body);

                return View("~/Views/Alipay/AlipaySubmit.cshtml", viewModel);
            }
            catch (Exception exp)
            {
                Diagnostics.AlipayLogger.Log.Error(exp.ToString());
                throw exp;
            }


        }


    }
}