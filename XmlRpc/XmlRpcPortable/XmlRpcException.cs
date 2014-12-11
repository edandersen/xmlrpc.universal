using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using XmlRpcPortable.Models;
using XmlRpcPortable.Utilities;

namespace XmlRpcPortable
{
    public class XmlRpcException : Exception
    {
        public XmlRpcException() { }

        public int XmlRpcCode { private set; get; }
        public string XmlRpcMessage { private set; get; }

        public XmlRpcException(IXmlNode node)
        {
            var val = XmlRpcParser.Parse(node);

            if (val != null && val is XmlRpcStruct)
            {
                var data = val as XmlRpcStruct;

                if (data.StructValue.ContainsKey("faultCode"))
                {
                    XmlRpcCode = ((XmlRpcInt)data.StructValue["faultCode"]).IntValue;
                }

                if (data.StructValue.ContainsKey("faultString"))
                {

                    XmlRpcMessage = ((XmlRpcString)data.StructValue["faultString"]).StringValue;
                }
            }
        }
    }
}
