using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XmlRpcPortable.Models;
using System.Reflection;

namespace XmlRpcPortable.Converter
{
    public static class XmlRpcConverter
    {

        #region MapTo
        public static object MapTo(XmlRpcValue value, Type toType)
        {
            if (toType.IsArray)
            {
                var baseType = toType.GetElementType();

                if (value is XmlRpcArray)
                {
                    return MapArrayTo(value as XmlRpcArray, baseType);
                }
                else
                {
                    // throw error because passed in type isnt an array
                    throw new XmlRpcException();
                }
            }
            else
            {
                switch (value.GetType().ToString())
                {
                    case "XmlRpcPortable.Models.XmlRpcArray":
                        // Throw error because passed in type wasn't an array
                        //throw new XmlRpcException();
                        return null;
                        break;
                    case "XmlRpcPortable.Models.XmlRpcStruct":
                        if (value is XmlRpcStruct)
                        {
                            return MapStructTo(value as XmlRpcStruct, toType);
                        }
                        else
                        {
                            // throw error because value wasn't a struct and can't map
                            return null;
                            //throw new XmlRpcException();
                        }
                        break;

                    default:

                        if (toType.GetTypeInfo().IsEnum)
                        {
                            var replaceMappers = toType.GetTypeInfo().GetCustomAttributes<XmlRpcConverterReplaceAttribute>();

                            var enumVal = value.Value.ToString();

                            if (replaceMappers != null && replaceMappers.Count() > 0)
                            {
                                foreach (var replace in replaceMappers)
                                {
                                    enumVal = enumVal.Replace(replace.OldValue, replace.NewValue);
                                }
                            }

                            if (Enum.IsDefined(toType, enumVal))
                            {
                                return Enum.Parse(toType, enumVal);
                            }
                            else
                            {
                                return null;
                            }
                        } else {
                            if (value.Value.GetType() == toType)
                            {
                                return value.Value;
                            }
                            else
                            {
                                return Convert.ChangeType(value.Value, toType);
                            }
                        }

                        break;
                }
            }

        }

        public static object MapTo<T>(XmlRpcValue value)
        {
            return MapTo(value, typeof(T));

            throw new XmlRpcException();
        }

        public static object MapArrayTo(XmlRpcArray value, Type toType)
        {
            if (value != null)
            {
                if (value.Values != null && value.Values.Count() > 0)
                {
                    var genTypeList = typeof(List<>);
                    var specTypeList = genTypeList.MakeGenericType(toType);
                    var arr = Activator.CreateInstance(specTypeList);

                    var props = toType.GetRuntimeProperties();

                    foreach (var val in value.Values)
                    {
                        var newItem = MapTo(val, toType);

                        specTypeList.GetTypeInfo().GetDeclaredMethod("Add").Invoke(arr, new[] { newItem});
                    }

                    return specTypeList.GetTypeInfo().GetDeclaredMethod("ToArray").Invoke(arr, null);
                }
            }

            return null;
        }
        public static object MapStructTo(XmlRpcStruct value, Type toType)
        {

            if (value != null)
            {
                var info = toType.GetTypeInfo();


                if (value is XmlRpcStruct)
                {
                    var values = (value as XmlRpcStruct).StructValue;

                    if (values != null && values.Keys.Count() > 0)
                    {
                        var props = toType.GetRuntimeProperties();

                        var ret = Activator.CreateInstance(toType);

                        foreach (var prop in props)
                        {
                            var custAttrs = prop.GetCustomAttribute<XmlRpcNameAttribute>();

                            if (custAttrs != null)
                            {
                                if (values.ContainsKey(custAttrs.Name))
                                {

                                    var val = values[custAttrs.Name];

                                    prop.SetValue(ret, MapTo(val, prop.PropertyType));

                                }
                            }
                        }

                        return ret;

                    }
                }
            }

            throw new XmlRpcMapperException();
        }

        #endregion

        #region MapFrom
        public static XmlRpcValue MapFrom(object value, Type toType)
        {
            var valueType = value.GetType();
            var valueTypeInfo = valueType.GetTypeInfo();

            switch (toType.ToString())
            {
                case "XmlRpcPortable.Models.XmlRpcArray":
                    return MapArrayFrom(value);
                    break;
                case "XmlRpcPortable.Models.XmlRpcStruct":
                    return MapStructFrom(value);

                    break;
                case "XmlRpcPortable.Models.XmlRpcInt":
                    if (value is int)
                    {
                        return new XmlRpcInt((int)value);
                    }
                    else
                    {
                        return XmlRpcInt.Parse(value.ToString());
                    }

                    break;
                case "XmlRpcPortable.Models.XmlRpcDouble":
                    if (value is double)
                    {
                        return new XmlRpcDouble((double)value);
                    }
                    else
                    {
                        return XmlRpcDouble.Parse(value.ToString());
                    }

                    break;
                case "XmlRpcPortable.Models.XmlRpcBoolean":
                    if (value is bool)
                    {
                        return new XmlRpcBoolean((bool)value);
                    }
                    else
                    {
                        return XmlRpcBoolean.Parse(value.ToString());
                    }

                    break;
                case "XmlRpcPortable.Models.XmlRpcDateTime":
                    if (value is DateTime)
                    {
                        return new XmlRpcDateTime((DateTime)value);
                    }
                    else
                    {
                        return XmlRpcDateTime.Parse(value.ToString());
                    }

                    break;
                case "XmlRpcPortable.Models.XmlRpcString":
                    return new XmlRpcString(value.ToString());

                    break;

                default:
                    switch (valueType.Name)
                    {
                        case "Int32":
                            return new XmlRpcInt((int)value);
                            break;
                        case "Double":
                            return new XmlRpcDouble((double)value);
                            break;
                        case "DateTime":
                            return new XmlRpcDateTime((DateTime)value);
                            break;
                        case "Boolean":
                            return new XmlRpcBoolean((bool)value);
                            break;
                        default:
                            if (valueTypeInfo.IsEnum)
                            {
                                var replaceMappers = valueTypeInfo.GetCustomAttributes<XmlRpcConverterReplaceAttribute>();

                                var enumVal = value.ToString();

                                if (replaceMappers != null && replaceMappers.Count() > 0)
                                {
                                    foreach (var replace in replaceMappers)
                                    {
                                        enumVal = enumVal.Replace(replace.NewValue, replace.OldValue);
                                    }
                                }

                                return new XmlRpcString(enumVal);
                            }
                            else
                            {
                                return new XmlRpcString(value.ToString());
                            }
                            break;
                    }
                    return new XmlRpcString(value.ToString());
                    break;
            }

            return null;
        }

        public static XmlRpcValue MapFrom(object value)
        {
            var valueType = value.GetType();
            var valueTypeInfo = valueType.GetTypeInfo();
            var valueAttr = valueTypeInfo.GetCustomAttribute<XmlRpcTypeAttribute>();

            if (valueAttr != null)
            {
                if (valueAttr.Type == typeof(XmlRpcStruct))
                {
                    return MapFrom(value, typeof(XmlRpcStruct));
                }
            }

            if (valueType.IsArray)
            {
                return MapFrom(value, typeof(XmlRpcArray));
            }
            return MapFrom(value, typeof(XmlRpcValue));
        }

        public static XmlRpcArray MapArrayFrom(object value)
        {

            object[] array = (object[])value;

            var ret = new XmlRpcArray();

            foreach (var itm in array)
            {
                var rpcValue = MapFrom(itm);

                if (rpcValue != null)
                {
                    ret.Values.Add(rpcValue);
                }
            }

            return null;
        }
        public static XmlRpcStruct MapStructFrom(object value)
        {
            var valueType = value.GetType();
            var valueTypeInfo = valueType.GetTypeInfo();
            var valueAttr = valueTypeInfo.GetCustomAttribute<XmlRpcTypeAttribute>();

            if (valueAttr != null)
            {
                if (valueAttr.Type == typeof(XmlRpcStruct))
                {
                    var ret = new XmlRpcStruct();

                    var props = valueType.GetRuntimeProperties();

                    if (props != null && props.Count() > 0)
                    {
                        foreach (var prop in props)
                        {
                            var nameAttr = prop.GetCustomAttribute<XmlRpcNameAttribute>();

                            if (nameAttr != null)
                            {
                                var objVal = prop.GetValue(value);

                                if (objVal != null)
                                {
                                    var rpcVal = MapFrom(objVal);

                                    if (rpcVal != null)
                                    {
                                        ret.StructValue.Add(nameAttr.Name, rpcVal);
                                    }
                                }
                            }
                        }
                    }

                    return ret;
                }
            }
            return null;
        }
        #endregion
    }
}
