using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XmlRpcPortable.Models
{
    public abstract class XmlRpcValue 
    {
        public object Value { get; set; }

        public virtual string ToXml()
        {
            return Value.ToString();
        }

        public virtual void BuildXml(XmlWriter writer)
        {
            writer.WriteString(Value.ToString());

        }
    }
}
