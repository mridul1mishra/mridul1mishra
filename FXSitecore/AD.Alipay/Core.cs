using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AD.Alipay
{
    public class Core
    {
        public Core()
        {
        }

        /// <summary>
		/// Remove 1 the blank ,sign and sign_type,and rearrange alphabetical from a to z
        /// </summary>
        /// <param name="dicArrayPre">the array to be filter</param>
        /// <returns>Array filtered</returns>
        public static Dictionary<string, string> FilterPara(SortedDictionary<string, string> dicArrayPre)
        {
            Dictionary<string, string> dicArray = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> temp in dicArrayPre)
            {
                if (temp.Key.ToLower() != "sign" && temp.Key.ToLower() != "sign_type1" && temp.Value != "" && temp.Value != null)
                {
                    dicArray.Add(temp.Key, temp.Value);
                }
            }

            return dicArray;
        }

        /// <summary>
		/// connect all parameters with & like "parameter name=value"
        /// </summary>
        /// <param name="sArray">parameters need to be connected </param>
        /// <returns>pre-sign string</returns>
        public static string CreateLinkString(Dictionary<string, string> dicArray)
        {
            StringBuilder prestr = new StringBuilder();
            foreach (KeyValuePair<string, string> temp in dicArray)
            {
                prestr.Append(temp.Key + "=" + temp.Value + "&");
            }

            //remove the last &
            int nLen = prestr.Length;
            prestr.Remove(nLen - 1, 1);

            return prestr.ToString();
        }

        /// <summary>
        /// urlencode
		/// connect all parameters with & like "parameter name=value",and do urlencode to the value of parameters
        /// </summary>
        /// <param name="sArray">parameters need to be connected</param>
        /// <param name="code">charset</param>
        /// <returns>String connected</returns>
        public static string CreateLinkStringUrlencode(Dictionary<string, string> dicArray, Encoding code)
        {
            StringBuilder prestr = new StringBuilder();
            foreach (KeyValuePair<string, string> temp in dicArray)
            {
                prestr.Append(temp.Key + "=" + HttpUtility.UrlEncode(temp.Value, code) + "&");
            }

            //remove the last &
            int nLen = prestr.Length;
            prestr.Remove(nLen - 1, 1);

            return prestr.ToString();
        }

        /// <summary>
        /// Write the log
        /// <param name="sWord">The string to be written in your log</param>
        public static void LogResult(string sWord)
        {
            string strPath = Config.log_path + "\\" + "alipay_log_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
            StreamWriter fs = new StreamWriter(strPath, false, System.Text.Encoding.Default);
            fs.Write(sWord);
            fs.Close();
        }
        
    }
}