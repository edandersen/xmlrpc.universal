using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlRpcPortable.Converter
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class XmlRpcTypeAttribute : Attribute
    {
        public XmlRpcTypeAttribute()
        {
        }

        public XmlRpcTypeAttribute(Type type)
        {
            _type = type;
        }

        public Type Type
        {
            get { return _type; }
        }

        public override string ToString()
        {
            string value = "Type : " + _type.ToString();
            return value;
        }

        private Type _type;
    }
}
