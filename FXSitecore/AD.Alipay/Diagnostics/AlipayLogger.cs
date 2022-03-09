using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;

namespace AD.Alipay.Diagnostics
{
    public class AlipayLogger
    {
            private static ILog log;
            public static ILog Log
            {
                get
                {
                    return log ?? (log = log4net.LogManager.GetLogger("FX.AlipayLogger"));
                }
            }
    }
}