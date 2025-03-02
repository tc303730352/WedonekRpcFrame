using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using WeDonekRpc.ApiGateway.Model;
using WeDonekRpc.HttpApiDoc.Collect;
using WeDonekRpc.HttpApiDoc.Interface;
using WeDonekRpc.HttpApiDoc.Model;
using WeDonekRpc.Helper;

namespace WeDonekRpc.HttpApiDoc.Helper
{
    internal class ApiHelper
    {
        private static readonly string _NullableType = "System.Nullable`1";

        public static Type GetSourceType (Type type)
        {
            if (type.IsArray)
            {
                return type.GetElementType();
            }
            Type[] types = type.GetGenericArguments();
            return types.Length == 1 ? types[0] : type;
        }
        public static ApiRequestType GetRequestType (Type type)
        {
            if (type.IsArray)
            {
                return ApiRequestType.数组;
            }
            else if (type.IsGenericType && type.FullName.StartsWith("System.Collections."))
            {
                return ApiRequestType.数组;
            }
            else if (type.IsClass && type != PublicDataDic.StrType)
            {
                return ApiRequestType.对象;
            }
            return ApiRequestType.字典;
        }
        private static ElementClass _GetElementClass (Type type, string name = null)
        {
            if (!type.IsPrimitive && type.Name != PublicDataDic.StringTypeName)
            {
                if (type.IsArray)
                {
                    type = type.GetElementType();
                }
                return new ElementClass
                {
                    Id = ClassCollect.LoadClass(type, name),
                    ElementName = name ?? type.Name,
                    ElementType = ElementType.对象
                };
            }
            else
            {
                return new ElementClass
                {
                    ElementName = name ?? type.Name,
                    ElementType = ElementType.基本
                };
            }
        }

        internal static ApiClassBody GetClassBody (Type type)
        {
            ApiDataType dataType = FormatType(type, out string paramType, out string defVal, out ElementClass[] element);
            return new ApiClassBody
            {
                ParamType = paramType,
                DataType = dataType,
                DefValue = defVal,
                ElementType = element
            };
        }
        public static IApiDataFormat GetApiDataFormat (ApiAttrShow attr)
        {
            ApiDataType dataType = FormatType(attr.AttrType, out string paramType, out string defVal, out ElementClass[] element);
            return new ApiDataFormat
            {
                ParamName = attr.AttrName,
                ParamType = paramType,
                DataType = dataType,
                DefValue = attr.DefValue ?? defVal,
                ElementType = element,
                ParamShow = attr.AttrShow
            };
        }



        public static IApiDataFormat GetApiDataFormat (ApiAttrShow attr, Type type, string show)
        {
            ApiDataType dataType = FormatType(type, out string paramType, out string defVal, out ElementClass[] element);
            return new ApiDataFormat
            {
                ParamName = attr.AttrName,
                ParamType = paramType,
                DataType = dataType,
                DefValue = attr.DefValue ?? defVal,
                ElementType = element,
                ParamShow = show.IsNull() ? attr.AttrShow : show
            };
        }
        public static ApiPostFormat GetApiPostFormat (Type type, string name, string show)
        {
            ApiDataType dataType = FormatType(type, out string paramType, out string defVal, out ElementClass[] element);
            return new ApiPostFormat
            {
                ParamName = name,
                ParamType = paramType,
                DataType = dataType,
                DefValue = defVal,
                ElementType = element,
                ParamShow = show
            };
        }
        public static ApiDataType FormatType (Type proType, out string paramType, out string defVal, out ElementClass[] element)
        {
            element = Array.Empty<ElementClass>();
            defVal = null;
            ApiDataType dataType = ApiDataType.数字;
            if (proType.IsGenericType && proType.FullName.StartsWith(_NullableType))
            {
                proType = proType.GetGenericArguments()[0];
                defVal = "Null";
            }
            if (proType.IsArray)
            {
                paramType = "Array";
                dataType = ApiDataType.数组;
                defVal = "Null";
                Type elemType = proType.GetElementType();
                element = new ElementClass[]
                {
                                      _GetElementClass(elemType,null)
                };
            }
            else if (proType.IsGenericType)
            {
                defVal = "Null";
                Type[] types = proType.GetGenericArguments();
                if (proType.FullName.StartsWith("System.Collections.Generic.Dictionary"))
                {
                    paramType = "object";
                    dataType = ApiDataType.泛型字典;
                    element = Array.ConvertAll(types, a =>
                    {
                        return _GetElementClass(a);
                    });
                }
                else if (proType.FullName.StartsWith("System.Collections"))
                {
                    paramType = "Array";
                    dataType = ApiDataType.数组;
                    element = Array.ConvertAll(types, a =>
                    {
                        return _GetElementClass(a);
                    });
                }
                else
                {
                    dataType = ApiDataType.泛型;
                    paramType = proType.Name;
                    int index = paramType.IndexOf('`');
                    paramType = paramType.Remove(index, paramType.Length - index);
                    StringBuilder pName = new StringBuilder(paramType);
                    _ = pName.Append("<");
                    Array.ForEach(types, a =>
                    {
                        _ = pName.Append(a.Name);
                        _ = pName.Append(",");
                    });
                    _ = pName.Remove(pName.Length - 1, 1);
                    _ = pName.Append(">");
                    element = new ElementClass[]
                    {
                                                 _GetElementClass(proType,pName.ToString())
                    };
                    defVal = "Null";
                }
            }
            else if (proType.Name == PublicDataDic.StringTypeName)
            {
                paramType = "string";
                dataType = ApiDataType.字符串;
                defVal = "Null";
            }
            else if (proType.Name == PublicDataDic.DateTimeType.Name)
            {
                paramType = "DateTime";
                dataType = ApiDataType.字符串;
                defVal = "Null";
            }
            else if (proType.IsEnum)
            {
                string id = EnumCollect.LoadEnum(proType, out int val);
                element = new ElementClass[]
                {
                                     new ElementClass
                                     {
                                             Id=id,
                                             ElementName=proType.Name,
                                             ElementType= ElementType.枚举
                                     }
                };
                defVal ??= val.ToString();
                dataType = ApiDataType.数字;
                paramType = "int";
            }
            else if (Type.GetTypeCode(proType) == TypeCode.Boolean)
            {
                paramType = "bool";
                dataType = ApiDataType.布尔;
                defVal = defVal ?? "false";
            }
            else if (proType == PublicDataDic.GuidType)
            {
                paramType = "Guid";
                dataType = ApiDataType.字符串;
                defVal = defVal ?? Guid.Empty.ToString();
            }
            else if (proType == PublicDataDic.UriType)
            {
                paramType = "Uri";
                dataType = ApiDataType.字符串;
                defVal = "Null";
            }
            else if (proType.IsClass)
            {
                paramType = proType.Name;
                dataType = ApiDataType.类;
                element = new ElementClass[]
                {
                                      _GetElementClass(proType)
                };
                defVal = "Null";
            }
            else
            {
                dataType = ApiDataType.数字;
                paramType = proType.Name;
                defVal = defVal ?? "0";
            }
            return dataType;
        }
        public static IApiDataFormat GetApiDataFormat (ResultBody result, string show)
        {
            ApiDataType dataType = FormatType(result.ResultType, out string paramType, out string defVal, out ElementClass[] element);
            return new ApiDataFormat
            {
                ParamName = result.AttrName,
                ParamType = paramType,
                DataType = dataType,
                DefValue = defVal,
                ElementType = element,
                ParamShow = show
            };
        }
        public static ApiPostFormat GetApiPostFormat (Type proType, string name, Type type)
        {
            ApiDataType dataType = FormatType(proType, out string paramType, out string defVal, out ElementClass[] element);
            return new ApiPostFormat
            {
                ParamName = name,
                ParamType = paramType,
                DataType = dataType,
                DefValue = defVal,
                ElementType = element,
                ParamShow = XmlShowHelper.FindParamShow(type, name)
            };
        }

        internal static IApiDataFormat[] GetReturnClass (ApiDataFormat result, IApiTemplate template)
        {
            if (template == null)
            {
                return new IApiDataFormat[0];
            }
            else if (result == null)
            {
                return new IApiDataFormat[]
                {
                                        GetApiDataFormat(template.ErrorCode),
                                        GetApiDataFormat(template.ErrorMsg)
                };
            }
            else
            {
                return new IApiDataFormat[]
                {
                                        GetApiDataFormat(template.ErrorCode),
                                        GetApiDataFormat(template.ErrorMsg),
                                        result
                };
            }
        }
        internal static IApiDataFormat[] GetPagingClass (ApiFuncBody body, IApiTemplate template)
        {
            ResultBody list = body.Results.Find(a => a.ParamName == "returns");
            if (list == null)
            {
                list = body.Results.Find(a => a.ResultType.IsArray || a.ResultType.IsGenericType);
            }
            string show = XmlShowHelper.FindParamShow(body.Source, body.Method, list.ParamName);
            List<IApiDataFormat> pros = new List<IApiDataFormat>(body.Results.Length)
                        {
                                GetApiDataFormat(template.PagingData,list.ResultType,show),
                                GetApiDataFormat(template.Count)
                        };
            body.Results.ForEach(a =>
            {
                if (a.ParamName != list.ParamName && a.ParamName != "count")
                {
                    show = XmlShowHelper.FindParamShow(body.Source, body.Method, a.ParamName);
                    pros.Add(GetApiDataFormat(a, show));
                }
            });
            return pros.ToArray();
        }
        internal static IApiDataFormat[] GetPagingClass (Type type, IApiTemplate template, string show)
        {
            return new IApiDataFormat[]
            {
                                GetApiDataFormat(template.PagingData,type,show),
                                GetApiDataFormat(template.Count)
            };
        }
        public static ApiDataFormat GetDataFormat (ResultBody result, string show)
        {
            ApiDataType dataType = FormatType(result.ResultType, out string paramType, out string defVal, out ElementClass[] element);
            return new ApiDataFormat
            {
                ParamName = result.AttrName,
                ParamType = paramType,
                DataType = dataType,
                DefValue = defVal,
                ElementType = element,
                ParamShow = show
            };
        }
        public static IApiDataFormat[] GetApiPostFormat (Type type)
        {
            if (type.IsGenericType && type.FullName.StartsWith("System.Collections"))
            {
                Type[] types = type.GetGenericArguments();
                if (type.FullName.StartsWith("System.Collections.Generic.Dictionary"))
                {
                    return new IApiDataFormat[]
                    {
                        ApiHelper.GetApiPostFormat(types[0], "Key", types[0].DeclaringType),
                        ApiHelper.GetApiPostFormat(types[0], "Value", types[0].DeclaringType)
                    };
                }
                else if (types.Length == 1)
                {
                    return new IApiDataFormat[]
                    {
                        ApiHelper.GetApiPostFormat(types[0], "Value", types[0].DeclaringType)
                    };
                }
            }
            PropertyInfo[] pros = type.GetProperties();
            IApiDataFormat[] list = pros.ConvertAll(a =>
            {
                ApiPostFormat format = ApiHelper.GetApiPostFormat(a.PropertyType, a.Name, a.DeclaringType);
                format.InitPro(a);
                return format;
            });
            IApiDataFormat[] fileds = _GetFieldParam(type);
            return fileds.Length == 0 ? list : list.Concat(fileds).ToArray();
        }
        private static ApiPostFormat[] _GetFieldParam (Type type)
        {
            FieldInfo[] fields = type.GetFields();
            return fields.ConvertAll(a =>
            {
                ApiPostFormat format = ApiHelper.GetApiPostFormat(a.FieldType, a.Name, a.DeclaringType);
                format.InitField(a);
                return format;
            });
        }
    }
}
