using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using XmlRpcPortable.Utilities;

namespace XmlRpcPortable.Models
{
    public class XmlRpcArray : XmlRpcValue
    {
        private IXmlNode _node;

        public XmlRpcArray()
        {
            Values = new List<XmlRpcValue>();
            Value = Values;
        }
        public XmlRpcArray(IXmlNode node)
        {
            _node = node;
            ProcessNode();
        }

        public List<XmlRpcValue> Values { private set; get; }

        private void ProcessNode()
        {
            var valueNodes = _node.SelectNodes("data/value");

            if (valueNodes != null && valueNodes.Count() > 0)
            {
                Values = new List<XmlRpcValue>();

                foreach(var val in valueNodes) {
                    if (val.ChildNodes != null && val.ChildNodes.Count() > 0)
                    {
                        var itm = XmlRpcParser.Parse(val.ChildNodes[0]);

                        if (itm != null)
                        {
                            Values.Add(itm);
                        }
                    }
                }

            }

            Value = Values;
        }

        public override string ToXml()
        {
            var ret = "<array><data>";


            if (Values != null)
            {
                foreach (var val in Values)
                {
                    ret += "<value>" + val.ToXml() + "</value>";
                }
            }
            ret += "</data></array>";
            return ret;
            //return base.ToXml();
        }

        public override void BuildXml(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("array");
            writer.WriteStartElement("data");

            if (Values != null)
            {
                foreach (var val in Values)
                {
                    writer.WriteStartElement("value");
                    val.BuildXml(writer);
                    writer.WriteEndElement();
                }

            }
            writer.WriteEndElement();
            writer.WriteEndElement();
        }

    }
}
