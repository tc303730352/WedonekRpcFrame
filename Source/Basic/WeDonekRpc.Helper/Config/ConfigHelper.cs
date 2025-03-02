using System;
using System.Text.Json;
using WeDonekRpc.Helper.Json;
using WeDonekRpc.Helper.Reflection;

namespace WeDonekRpc.Helper.Config
{
    internal class ConfigHelper
    {
        public static bool InitToken (JsonBodyValue val, IConfigItem parent, IConfig config)
        {
            if (val.IsObject)
            {
                bool isSet = false;
                foreach (JsonProperty i in val)
                {
                    if (config.SetConfig(i.Name, new JsonBodyValue(i.Value), parent))
                    {
                        isSet = true;
                    }
                }
                return isSet;
            }
            return false;
        }
        public static bool InitObject (Type type, object data, IConfigItem parent, IConfig config)
        {
            if (type.IsArray)
            {
                return parent.SetValue(data.ToJson(), ItemType.Array, parent.Prower);
            }
            else if (type.IsGenericType && type.FullName.StartsWith("System.Collections."))
            {
                return parent.SetValue(data.ToJson(), ItemType.Array, parent.Prower);
            }
            else if (type == PublicDataDic.StrType)
            {
                return parent.SetValue(data == null ? string.Empty : data.ToString(), ItemType.String, parent.Prower);
            }
            else if (type == PublicDataDic.UriType || type == PublicDataDic.GuidType)
            {
                return parent.SetValue(data.ToString(), ItemType.String, parent.Prower);
            }
            else if (type.IsEnum)
            {
                return parent.SetValue(( (int)data ).ToString(), ItemType.Num, parent.Prower);
            }
            else if (type == PublicDataDic.BoolType)
            {
                return parent.SetValue(data.ToString().ToLower(), ItemType.Boolean, parent.Prower);
            }
            else if (type.IsValueType && type.IsPrimitive)
            {
                return parent.SetValue(data.ToString(), ItemType.Num, parent.Prower);
            }
            else if (!type.IsClass)
            {
                return parent.SetValue(data.ToString(), ItemType.String, parent.Prower);
            }
            else
            {
                bool isSet = false;
                IReflectionBody body = ReflectionHepler.GetReflection(type);
                foreach (IReflectionProperty i in body.Properties)
                {
                    object obj = i.GetValue(data);
                    if (obj != null && config.SetConfig(i.Name, i.Type, obj, parent))
                    {
                        isSet = true;
                    }
                }
                return isSet;
            }
        }
        public static bool InitObject<T> (T data, IConfigItem parent, IConfig config) where T : class
        {
            Type type = typeof(T);
            return InitObject(type, data, parent, config);
        }
    }
}
