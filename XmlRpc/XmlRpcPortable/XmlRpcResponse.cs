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
    public class XmlRpcResponse
    {
        private IXmlNode _node;

        public XmlRpcResponse(IXmlNode node)
        {
            _node = node;
            ProcessNode();
        }

        public XmlRpcValue Value { private set; get; }

        private void ProcessNode()
        {
            var valueNode = _node.SelectSingleNode("params/param/value");

            if (valueNode != null && valueNode.ChildNodes != null && valueNode.ChildNodes.Count() > 0)
            {
                Value = XmlRpcParser.Parse(valueNode.ChildNodes[0]);

            }
            else
            {
                var faultNode = _node.SelectSingleNode("fault/value/struct");

                if (faultNode != null) {
                    throw new XmlRpcException(faultNode);
                }
                else
                {
                    throw new XmlRpcException();
                }
            }
        }
    }
}
