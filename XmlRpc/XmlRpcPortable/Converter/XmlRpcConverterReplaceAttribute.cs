using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlRpcPortable.Converter
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Enum, AllowMultiple=true)]
    public class XmlRpcConverterReplaceAttribute : Attribute
    {
        public XmlRpcConverterReplaceAttribute()
        {
        }

        public XmlRpcConverterReplaceAttribute(string oldValue, string newValue)
        {
            _old = oldValue;
            _new = newValue;
        }

        public string OldValue
        {
            get { return _old; }
        }

        public string NewValue
        {
            get { return _new; }
        }

        public override string ToString()
        {
            string value = "Replace : '" + _old + "' with '" + _new + "'";
            return value;
        }

        private string _old = "";
        private string _new = "";
    }
}
