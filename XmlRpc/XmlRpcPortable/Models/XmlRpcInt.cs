using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;

namespace XmlRpcPortable.Models
{
    public class XmlRpcInt : XmlRpcValue
    {
        public XmlRpcInt(IXmlNode node)
        {
            int val = -1;

            int.TryParse(node.InnerText, out val);

            Value = val;
        }

        public static XmlRpcInt Parse(string value)
        {
            int val;

            if (int.TryParse(value, out val))
            {
                return new XmlRpcInt(val);
            }
            return null;
        }
        public XmlRpcInt(int value)
        {
            Value = value;
        }
        public int IntValue {

            get
            {
                if (Value is int)
                {
                    return (int)Value;
                }
                else
                {
                    return 0;
                }
            }
        }

        public override string ToXml()
        {
            return "<int>" + IntValue.ToString() + "</int>";
        }

        public override void BuildXml(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("int");

            writer.WriteString(IntValue.ToString());

            writer.WriteEndElement();
        }
    }
}
