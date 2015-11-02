using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;

namespace XmlRpcPortable.Models
{
    public class XmlRpcString : XmlRpcValue
    {
        public XmlRpcString(IXmlNode node)
        {
            Value = node.InnerText;
        }

        public XmlRpcString(string value)
        {
            Value = value;
        }
        public string StringValue
        {
            get { return Value.ToString(); }
        }

        
    }
}
