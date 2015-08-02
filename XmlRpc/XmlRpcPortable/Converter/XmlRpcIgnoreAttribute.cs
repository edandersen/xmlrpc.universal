using System;

namespace XmlRpcPortable.Converter
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class XmlRpcIgnoreAttribute : Attribute
    {
    }
}