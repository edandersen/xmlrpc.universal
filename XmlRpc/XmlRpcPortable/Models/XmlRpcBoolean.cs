using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;

namespace XmlRpcPortable.Models
{
    public class XmlRpcBoolean : XmlRpcValue
    {
        public XmlRpcBoolean(IXmlNode node)
        {
            Value = node.InnerText.Trim() == "1";
        }

        public XmlRpcBoolean(bool value)
        {
            Value = value;
        }

        public static XmlRpcBoolean Parse(string value)
        {
            bool val;

            if (bool.TryParse(value, out val))
            {
                return new XmlRpcBoolean(val);
            }
            return null;
        }

        public bool BoolValue { 
            get {
                if (Value is bool)
                {
                    return (bool)Value;
                }
                else
                {
                    return false;
                }
            }
        }

        public override string ToXml()
        {
            return "<boolean>" + (BoolValue ? "1" : "0") + "</boolean>";
        }

        public override void BuildXml(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("boolean");

            writer.WriteString(BoolValue ? "1" : "0");

            writer.WriteEndElement();
        }

    }
}
