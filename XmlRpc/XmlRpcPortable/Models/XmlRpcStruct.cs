using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using XmlRpcPortable.Utilities;

namespace XmlRpcPortable.Models
{
    public class XmlRpcStruct : XmlRpcValue
    {
        private IXmlNode _node;

        public XmlRpcStruct(IXmlNode node)
        {
            _node = node;
            ProcessNode();
        }

        public XmlRpcStruct()
        {
            Value = new Dictionary<string, XmlRpcValue>();
        }
        public Dictionary<string, XmlRpcValue> StructValue {

            get
            {
                if (Value is Dictionary<string, XmlRpcValue>)
                {
                    return (Dictionary<string, XmlRpcValue>)Value;
                }
                else
                {
                    return null;
                }
            }
        }

        private void ProcessNode()
        {
            var memberNodes = _node.SelectNodes("member");

            if (memberNodes != null && memberNodes.Count() > 0)
            {
                var newValue = new Dictionary<string, XmlRpcValue>();

                foreach (var mem in memberNodes)
                {
                    if (mem.ChildNodes != null && mem.ChildNodes.Count() > 1)
                    {
                        var nameNode = mem.SelectSingleNode("name");
                        if (nameNode != null)
                        {
                            var valNode = mem.SelectSingleNode("value");

                            if (valNode != null && valNode.ChildNodes != null && valNode.ChildNodes.Count() > 0)
                            {
                                var val = XmlRpcParser.Parse(valNode.ChildNodes[0]);

                                if (val != null && !String.IsNullOrEmpty(nameNode.InnerText))
                                {
                                    if (!newValue.ContainsKey(nameNode.InnerText))
                                        newValue.Add(nameNode.InnerText, val);
                                }
                            }
                        }
                    }
                }

                Value = newValue;

            }
        }

        public override string ToXml()
        {
            var ret = "<struct>";

            var structVal = StructValue;

            if (structVal != null)
            {
                foreach (var key in structVal.Keys)
                {
                    ret += "<member><name>" + key + "</name><value>" + structVal[key].ToXml() + "</value></member>";
                }
            }
            ret += "</struct>";
            return ret;
            //return base.ToXml();
        }

        public override void BuildXml(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("struct");

            var structVal = StructValue;

            if (structVal != null)
            {
                foreach (var key in structVal.Keys)
                {
                    writer.WriteStartElement("member");
                    writer.WriteElementString("name", key);
                    writer.WriteStartElement("value");
                    structVal[key].BuildXml(writer);
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }
            }

            writer.WriteEndElement();
        }

    }
}
