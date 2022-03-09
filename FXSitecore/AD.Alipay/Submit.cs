using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Aop;

namespace AD.Alipay
{
    public class Submit
    {
        #region 字段
        //The Alipay gateway provided to merchants
        //The Alipay gateway of sandbox environment.		
        private static string GATEWAY_NEW = "https://openapi.alipaydev.com/gateway.do?";
        //private static string GATEWAY_NEW = "https://openapi.alipay.com/gateway.do?";
        //The Alipay gateway of production environment.(pls use the below line instead if you were in production environment)
        //private static string GATEWAY_NEW = "https://mapi.alipay.com/gateway.do?";

        private static string _private_key = "";
        //charset
        private static string _input_charset = "";
        private static string _sign_type = "RSA2";
        #endregion

        static Submit()
        {
            _private_key = Config.private_key;
            _input_charset = Config.input_charset.Trim().ToLower();
            _sign_type = Config.sign_type.Trim().ToUpper();
        }

        /// <summary>
        /// Generate the sign
        /// </summary>
        /// <param name="sPara">Parameters to sign</param>
        /// <returns>sign generated</returns>
        private static string BuildRequestMysign(Dictionary<string, string> sPara)
        {
            //Rearrange parameters in the data set alphabetically and connect rearranged parameters with & like "parametername=value"
            string prestr = Core.CreateLinkString(sPara);

            //get the sign
            string mysign = "";
            switch (_sign_type)
            {
                case "RSA":
                    Sitecore.Diagnostics.Log.Error(prestr + "-" + _private_key + "-" + _input_charset, "MySign");
                    mysign = RSAFromPkcs8.sign(prestr, _private_key, _input_charset);
                    break;
                case "RSA2":
                    mysign = Aop.Api.Util.AlipaySignature.RSASignCharSet(prestr, _private_key, _input_charset, "RSA2");
                    break;
                default:
                    mysign = Aop.Api.Util.AlipaySignature.RSASignCharSet(prestr, _private_key, _input_charset, "RSA2");
                    break;
            }

            return mysign;
        }

        /// <summary>
		/// Generate a set of parameters need in the request of Alipay
        /// </summary>
        /// <param name="sParaTemp">Pre-sign string</param>
        /// <returns>parameters need to be in the request</returns>
        private static Dictionary<string, string> BuildRequestPara(SortedDictionary<string, string> sParaTemp)
        {
            //params to be signed
            Dictionary<string, string> sPara = new Dictionary<string, string>();
            //sign generated
            string mysign = "";

            //filter the parameters
            sPara = Core.FilterPara(sParaTemp);

            //Generate the sign
            mysign = BuildRequestMysign(sPara);

            //Add the sign and sign_type into the sPara
            sPara.Add("sign", mysign);
            sPara.Add("sign_type", _sign_type);

            return sPara;
        }

        /// <summary>
		/// Generate a set of parameters need in the request of Alipay
        /// </summary>
        /// <param name="sParaTemp">Pre-sign string</param>
        /// <param name="code">charset</param>
        /// <returns>string need to be in the request</returns>
        private static string BuildRequestParaToString(SortedDictionary<string, string> sParaTemp, Encoding code)
        {
            //params to be signed
            Dictionary<string, string> sPara = new Dictionary<string, string>();
            sPara = BuildRequestPara(sParaTemp);

            //connect the parameters with "&" like "parameter=value",and do urlencode to the value
            string strRequestData = Core.CreateLinkStringUrlencode(sPara, code);

            return strRequestData;
        }

        /// <summary>
		/// Build the request,costruct in the format of HTML form
        /// </summary>
        /// <param name="sParaTemp">the request params</param>
        /// <param name="strMethod">request form.support two types:post and get</param>
        /// <param name="strButtonValue">The text of confirmation button</param>
        /// <returns>the text of requested HTML form</returns>
        public static string BuildRequest(SortedDictionary<string, string> sParaTemp, string strMethod, string strButtonValue)
        {
            //pre-request params

            Dictionary<string, string> dicPara = new Dictionary<string, string>();
            dicPara = BuildRequestPara(sParaTemp);

            StringBuilder sbHtml = new StringBuilder();

            sbHtml.Append("<form id='alipaysubmit' name='alipaysubmit' action='" + GATEWAY_NEW + "_input_charset=" + _input_charset + "' method='" + strMethod.ToLower().Trim() + "'>");

            foreach (KeyValuePair<string, string> temp in dicPara)
            {
                sbHtml.Append("<input type='hidden' name='" + temp.Key + "' value='" + temp.Value + "'/>");
            }

            //Pls don't set name attribute for the submit button 
            sbHtml.Append("<input type='submit' value='" + strButtonValue + "' style='display:none;'></form>");

            sbHtml.Append("<script>document.forms['alipaysubmit'].submit();</script>");

            return sbHtml.ToString();
        }



        /// <summary>
		///	Used to anti-phishing，use interface "query_timestamp" to get the function to get the timestamp
        /// note：If you get error when parsing XML from remote services,it is related to sever setting like whether the server supports SSL etc.
        /// </summary>
        /// <returns>String of timestamp</returns>
        public static string Query_timestamp()
        {
            string url = GATEWAY_NEW + "service=query_timestamp&partner=" + Config.partner + "&_input_charset=" + Config.input_charset;
            string encrypt_key = "";

            XmlTextReader Reader = new XmlTextReader(url);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Reader);

            encrypt_key = xmlDoc.SelectSingleNode("/alipay/response/timestamp/encrypt_key").InnerText;

            return encrypt_key;
        }
    }
}