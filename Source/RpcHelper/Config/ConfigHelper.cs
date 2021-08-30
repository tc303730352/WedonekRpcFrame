using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using Newtonsoft.Json.Linq;
namespace RpcHelper.Config
{
        internal class ConfigHelper
        {
                public static void InitToken(JToken token, IConfigItem parent, IConfig config)
                {
                        Dictionary<string, object> dic = token.ToObject<Dictionary<string, object>>();
                        if (dic.Count > 0)
                        {
                                foreach (KeyValuePair<string, object> i in dic)
                                {
                                        config.SetConfig(i.Key, i.Value, parent);
                                }
                        }
                }
                public static bool InitObject(Type type, object data, IConfigItem parent, IConfig config)
                {
                        if (type.IsArray)
                        {
                               return parent.SetValue(data.ToJson(), ItemType.Array, parent.Prower);
                        }
                        else if (type.IsGenericType && type.FullName.StartsWith("System.Collections."))
                        {
                              return  parent.SetValue(data.ToJson(), ItemType.Array, parent.Prower);
                        }
                        else if (type == PublicDataDic.StrType)
                        {
                              return  parent.SetValue(data == null ? string.Empty : data.ToString(), ItemType.String, parent.Prower);
                        }
                        else if (type == PublicDataDic.UriType || type == PublicDataDic.GuidType)
                        {
                              return  parent.SetValue(data.ToString(), ItemType.String, parent.Prower);
                        }
                        else if (type.IsEnum)
                        {
                              return  parent.SetValue(((int)data).ToString(), ItemType.Num, parent.Prower);
                        }
                        else if (type == PublicDataDic.BoolType)
                        {
                              return  parent.SetValue(data.ToString().ToLower(), ItemType.Boolean, parent.Prower);
                        }
                        else if (!type.IsClass)
                        {
                              return  parent.SetValue(data.ToString(), ItemType.Num, parent.Prower);
                        }
                        else
                        {
                                bool isSet = false;
                                foreach (PropertyInfo i in type.GetProperties())
                                {
                                        object obj = i.GetValue(data);
                                        if(config.SetConfig(i.Name, i.PropertyType, obj, parent))
                                        {
                                                isSet = true;
                                        }
                                }
                                return isSet;
                        }
                }
                public static bool InitObject<T>(T data, IConfigItem parent, IConfig config) where T : class
                {
                        Type type = typeof(T);
                        return InitObject(type, data, parent, config);
                }
                public static JObject CreateObject(string[] name, int index, object value)
                {
                        string str;
                        bool isClass = false;
                        if (value is JObject)
                        {
                                isClass = true;
                                str = value.ToString();
                        }
                        else
                        {
                                str = (string)value;
                        }
                        StringBuilder json = new StringBuilder("{");
                        int end = name.Length - 1;
                        for (int i = index; i <= end; i++)
                        {
                                if (i == end)
                                {
                                        if (isClass)
                                        {
                                                json.AppendFormat("\"{0}\":{1}", name[i], str);
                                        }
                                        else
                                        {
                                                json.AppendFormat("\"{0}\":\"{1}\"", name[i], str);
                                        }
                                        break;
                                }
                                json.AppendFormat("\"{0}\":{", name[i]);
                        }
                        end.For(index, a =>
                         {
                                 json.Append("}");
                         });
                        return (JObject)json.ToString().Json<object>();
                }
        }
}
