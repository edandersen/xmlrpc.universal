using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlRpcPortable.Models
{
    public class XmlRpcBinary : XmlRpcValue
    {
        public XmlRpcBinary(MemoryStream value)
        {
            Value = value;
        }

        public override string ToXml()
        {
            return "<base64>" + Convert.ToBase64String((byte[]) Value) + "</base64>";
        }

        public override void BuildXml(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("base64");

            writer.WriteString(Convert.ToBase64String(((MemoryStream)Value).ToArray()));

            writer.WriteEndElement();
        }
    }
}
