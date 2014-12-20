using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;

namespace XmlRpcPortable.Models
{
    public class XmlRpcDateTime : XmlRpcValue
    {
        public XmlRpcDateTime(IXmlNode node)
        {
            DateTime val = DateTime.MinValue;

            if (!DateTime.TryParse(node.InnerText, out val))
            {
                // Fix for non UTC
                var arr = node.InnerText.Split(new char[] { 'T' });

                if (arr.Length > 1)
                {
                    if (arr[0].Length == 8)
                    {
                        var newVal = string.Format("{0}-{1}-{2}T{3}", arr[0].Substring(0, 4), arr[0].Substring(4, 2), arr[0].Substring(6, 2), arr[1]);

                        DateTime.TryParse(newVal, out val);
                    }
                }
            }

            Value = val;
        }

        public XmlRpcDateTime(DateTime value)
        {
            Value = value;
        }

        public static XmlRpcDateTime Parse(string value)
        {
            DateTime val;

            if (DateTime.TryParse(value, out val))
            {
                return new XmlRpcDateTime(val);
            }
            return null;
        }

        public DateTime DateTimeValue
        {
            get
            {
                if (Value is DateTime)
                {
                    return (DateTime)Value;
                }
                else
                {
                    return DateTime.MinValue;
                }
            }
        }

        public override string ToXml()
        {
            return "<datetime.iso8601>" + DateTimeValue.ToUniversalTime() + "</datetime.iso8601>";
        }

        public override void BuildXml(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("datetime.iso8601");

            writer.WriteString(DateTimeValue.ToString("yyyy-MM-ddThh:mm:ss"));

            writer.WriteEndElement();
        }


    }
}
