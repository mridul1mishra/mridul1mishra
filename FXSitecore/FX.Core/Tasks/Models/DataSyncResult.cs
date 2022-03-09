using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Core.Tasks.Models
{
    public class DataSyncResult
    {
        private List<string> __responseMessage { get; set; }
        public List<string> ResponseMessage
        {
            get
            {
                if (__responseMessage == null)
                    __responseMessage = new List<string>();
                return __responseMessage;
            }
        }
        public bool Success { get; set; }
        public string HttpStatusCode { get; set; }
    }
}
