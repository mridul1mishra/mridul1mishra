using System;
using System.Xml.Serialization;

namespace Aop.Api.Response
{
    /// <summary>
    /// AlipayZdataassetsEasyserviceResponse.
    /// </summary>
    public class AlipayZdataassetsEasyserviceResponse : AopResponse
    {
        /// <summary>
        /// θΏεη»ζ
        /// </summary>
        [XmlElement("result")]
        public string Result { get; set; }
    }
}
