using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlRpcPortable.Converter
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class XmlRpcNameAttribute : Attribute
    {
        public XmlRpcNameAttribute()
        {
        }

        public XmlRpcNameAttribute(string name)
        {
            this.name = name;
        }

        public string Name
        {
            get { return name; }
        }

        public override string ToString()
        {
            string value = "Name : " + name;
            return value;
        }

        private string name = "";
    }
}
