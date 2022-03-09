using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AD.Alipay
{
    public class Notify
    {
        #region 字段
        private string _partner = "";               
        private string alipay_public_key = "";      
        private string _input_charset = "";         
        private string _sign_type = "";             
                                                                                   
        //The URL of verification of Alipay notification.
        //The verification URL of Alipay notification,sandbox environment.
        private string Https_veryfy_url = "https://openapi.alipaydev.com/gateway.do?service=notify_verify&";
        //The verification URL of Alipay notification,production environment.(pls use the below line instead if you were in production environment)	
        //private string Https_veryfy_url = "https://mapi.alipay.com/gateway.do?service=notify_verify&";
        #endregion


        /// <summary>
        /// constructor
        /// Initialize variables from configuration
        /// </summary>
        /// <param name="inputPara">The params sent back from notification</param>
        /// <param name="notify_id">ID the id of notification</param>
        public Notify()
        {
            //Initialize the basic configuration information
            _partner = Config.partner.Trim();
            alipay_public_key = getPublicKeyStr(Config.alipay_public_key.Trim());
            _input_charset = Config.input_charset.Trim().ToLower();
            _sign_type = Config.sign_type.Trim().ToUpper();
        }

        /// <summary>
        ///Get public key from file and transform to a string
        /// </summary>
        /// <param name="Path">the path of public key</param>
        public static string getPublicKeyStr(string Path)
        {
            StreamReader sr = new StreamReader(Path);
            string pubkey = sr.ReadToEnd();
            sr.Close();
            if (pubkey != null)
            {
                pubkey = pubkey.Replace("-----BEGIN PUBLIC KEY-----", "");
                pubkey = pubkey.Replace("-----END PUBLIC KEY-----", "");
                pubkey = pubkey.Replace("\r", "");
                pubkey = pubkey.Replace("\n", "");
            }
            return pubkey;
        }

        public bool VerifyReturn(SortedDictionary<string, string> inputPara, string sign)
        {
            //get the verification result of the return
            bool isSign = GetSignVeryfy(inputPara, sign);
            return isSign;
        }

        /// <summary>
        ///  Verify whether it's a legal notification sent from Alipay
        /// </summary>
        /// <param name="inputPara">The response from notification</param>
        /// <param name="notify_id">ID notify_id</param>
        /// <param name="sign">the sign generated from Alipay</param>
        /// <returns>verification result</returns>
        public bool Verify(SortedDictionary<string, string> inputPara, string notify_id, string sign)
        {
            //get the result of verification
            bool isSign = GetSignVeryfy(inputPara, sign);
            Sitecore.Diagnostics.Log.Error("is sign " + isSign.ToString(), this);
            //check whether the notification is from Alipay
            string responseTxt = "false!";
            if (notify_id != null && notify_id != "") { responseTxt = GetResponseTxt(notify_id); }

            Sitecore.Diagnostics.Log.Error(responseTxt, this);

            //write the log
            //string sWord = "responseTxt=" + responseTxt + "\n isSign=" + isSign.ToString() + "\n Params：" + GetPreSignStr(inputPara) + "\n ";
            //Core.LogResult(sWord);

            //judge whether responsetTxt and isSign is true
            //if responsetTxt is not true,the cause might be related to sever setting,merchant account and expiration time of notify_id(one minute).
            //if isSign is not true，the cause might be related to sign,charset and format of request str(eg:request with custom parameter etc.) 
            if (responseTxt == "true" && isSign)//verification succeed
            {
                return true;
            }
            else//verification failed
            {
                return false;
            }
        }

        /// <summary>
		///get the pre-sign string (for debug)
        /// </summary>
        /// <param name="inputPara">params from notification</param>
        /// <returns>pre-sign string</returns>
        private string GetPreSignStr(SortedDictionary<string, string> inputPara)
        {
            Dictionary<string, string> sPara = new Dictionary<string, string>();

            //filter the blank,sign and sign_type
            sPara = Core.FilterPara(inputPara);

            //get the pre-sign string
            string preSignStr = Core.CreateLinkString(sPara);

            return preSignStr;
        }

        /// <summary>
		/// get the result of verification of returned notification
        /// </summary>
        /// <param name="inputPara">the params from the feedback notification</param>
        /// <param name="sign">the sign to be compared</param>
        /// <returns>the result of verification</returns>
        private bool GetSignVeryfy(SortedDictionary<string, string> inputPara, string sign)
        {
            Dictionary<string, string> sPara = new Dictionary<string, string>();

            //Filter parameters with null value ,sign and sign_type
            sPara = Core.FilterPara(inputPara);

            //Generate the pre-sign string
            string preSignStr = Core.CreateLinkString(sPara);

            Sitecore.Diagnostics.Log.Error("sign_type: " + _sign_type, this);

            //get the result of verification
            bool isSgin = false;
            if (sign != null && sign != "")
            {
                switch (_sign_type)
                {
                    case "RSA":
                        isSgin = RSAFromPkcs8.verify(preSignStr, sign, alipay_public_key, _input_charset);
                        break;
                    default:
                        break;
                }
            }

            return isSgin;
        }

        /// <summary>
		/// check whether the notification is sent from Alipay sever
        /// </summary>
        /// <param name="notify_id">ID</param>
        /// <returns>the result of notification</returns>
        private string GetResponseTxt(string notify_id)
        {
            Sitecore.Diagnostics.Log.Error(Https_veryfy_url + "partner=" + _partner + "&notify_id=" + notify_id, this);
            string veryfy_url = Https_veryfy_url + "partner=" + _partner + "&notify_id=" + notify_id;

            //Get the remote server ATN result,verify whether it's from Alipay
            string responseTxt = Get_Http(veryfy_url, 120000);

            return responseTxt;
        }

        /// <summary>
		/// Get the remote server ATN result
        /// </summary>
        /// <param name="strUrl">specified URL value</param>
        /// <param name="timeout">timeout</param>
        /// <returns>Server ATN result</returns>
        private string Get_Http(string strUrl, int timeout)
        {
            string strResult;
            try
            {
                HttpWebRequest myReq = (HttpWebRequest)HttpWebRequest.Create(strUrl);
                myReq.Timeout = timeout;
                HttpWebResponse HttpWResp = (HttpWebResponse)myReq.GetResponse();
                Stream myStream = HttpWResp.GetResponseStream();
                StreamReader sr = new StreamReader(myStream, Encoding.Default);
                StringBuilder strBuilder = new StringBuilder();
                while (-1 != sr.Peek())
                {
                    strBuilder.Append(sr.ReadLine());
                }

                strResult = strBuilder.ToString();
            }
            catch (Exception exp)
            {
                strResult = "error：" + exp.Message;
            }

            Sitecore.Diagnostics.Log.Error(strResult, this);

            return strResult;
        }
    }
}