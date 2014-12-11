using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;

namespace XmlRpcPortable.Models
{
    public class XmlRpcDouble : XmlRpcValue
    {
        public XmlRpcDouble(IXmlNode node)
        {
            double val = -1;

            double.TryParse(node.InnerText, out val);

            Value = val;
        }

        public XmlRpcDouble(double value)
        {
            Value = value;
        }
        public static XmlRpcDouble Parse(string value)
        {
            double val;

            if (double.TryParse(value, out val))
            {
                return new XmlRpcDouble(val);
            }
            return null;
        }


        public double DoubleValue {
            get {
                if (Value is double)
                {
                    return (double)Value;
                }
                else
                {
                    return 0.0;
                }
            }

        }

        public override string ToXml()
        {
            return "<double>" + DoubleValue.ToString() + "</double>";
        }

        public override void BuildXml(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("double");

            writer.WriteString(DoubleValue.ToString());

            writer.WriteEndElement();
        }

    }
}
