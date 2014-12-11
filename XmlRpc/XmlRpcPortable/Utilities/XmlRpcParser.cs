using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using XmlRpcPortable.Models;

namespace XmlRpcPortable.Utilities
{
    public static class XmlRpcParser
    {
        public static XmlRpcValue Parse(IXmlNode node)
        {
            switch (node.NodeName.ToLower())
            {
                case "array":
                    return new XmlRpcArray(node);
                    break;
                case "struct":
                    return new XmlRpcStruct(node);
                    break;
                case "boolean":
                    return new XmlRpcBoolean(node);
                    break;
                case "int":
                    return new XmlRpcInt(node);
                    break;
                case "i4":
                    return new XmlRpcInt(node);
                    break;
                case "double":
                    return new XmlRpcDouble(node);
                    break;
                case "datetime.iso8601":
                    return new XmlRpcDateTime(node);
                    break;
                default:
                    return new XmlRpcString(node);
            }

            return null;
        }
    }
}
