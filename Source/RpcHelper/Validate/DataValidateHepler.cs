using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using RpcHelper.Validate;

namespace RpcHelper
{
        public class DataValidateHepler
        {
                private static readonly ConcurrentDictionary<string, ValidateCache> _ValidateCache = new ConcurrentDictionary<string, ValidateCache>();
                public static bool ValidateData(dynamic data, out string error)
                {
                        Type type = data.GetType();
                        if (type.IsArray)
                        {
                                type = type.GetElementType();
                                foreach (object i in data)
                                {
                                        if (!_ValidateData(type, i, data, out error))
                                        {
                                                return false;
                                        }
                                }
                                error = null;
                                return true;
                        }
                        else if (type.IsGenericType && type.GetInterface("System.Collections.IEnumerable") != null)
                        {
                                if (type.GenericTypeArguments.Length > 1)
                                {
                                        throw new Exception("no.support.many.generic");
                                }
                                type = type.GenericTypeArguments[0];
                                foreach (object i in data)
                                {
                                        if (!_ValidateData(type, i, data, out error))
                                        {
                                                return false;
                                        }
                                }
                                error = null;
                                return true;
                        }
                        else
                        {
                                return _ValidateData(type, data, data, out error);
                        }
                }
                public static bool ValidateData(Type type, dynamic data, out string error)
                {
                        if (type.IsArray)
                        {
                                type = type.GetElementType();
                                foreach (object i in data)
                                {
                                        if (!_ValidateData(type, i, data, out error))
                                        {
                                                return false;
                                        }
                                }
                                error = null;
                                return true;
                        }
                        else if (type.IsGenericType && type.GetInterface("System.Collections.IEnumerable") != null)
                        {
                                if (type.GenericTypeArguments.Length > 1)
                                {
                                        throw new Exception("no.support.many.generic");
                                }
                                type = type.GenericTypeArguments[0];
                                foreach (object i in data)
                                {
                                        if (!_ValidateData(type, i, data, out error))
                                        {
                                                return false;
                                        }
                                }
                                error = null;
                                return true;
                        }
                        else
                        {
                                return _ValidateData(type, data, data, out error);
                        }
                }
                private static bool _ValidateData(Type type, object data, object root, out string error)
                {
                        if (!_ValidateCache.TryGetValue(type.FullName, out ValidateCache cache))
                        {
                                cache = _ValidateCache.GetOrAdd(type.FullName, new ValidateCache(type));
                                cache.InitValidate();
                        }
                        return cache.ValidateData(data, root, out error);
                }

                internal static bool ValidateData(Type type, object data, object parent, out string error)
                {
                        if (!_ValidateCache.TryGetValue(type.FullName, out ValidateCache cache))
                        {
                                cache = _ValidateCache.GetOrAdd(type.FullName, new ValidateCache(type));
                                cache.InitValidate();
                        }
                        return cache.ValidateData(data, parent, out error);
                }
                public static bool CheckAttrIsValidate(Type attrType, out IValidateAttr[] validateAttrs)
                {
                        Type type = attrType.DeclaringType;
                        if (!_ValidateCache.TryGetValue(type.FullName, out ValidateCache cache))
                        {
                                cache = _ValidateCache.GetOrAdd(type.FullName, new ValidateCache(type));
                                cache.InitValidate();
                        }

                        return cache.CheckAttrIsValidate(attrType, out validateAttrs);
                }
                public static bool CheckIsValidate(Type type)
                {
                        if (type.IsGenericType && type.FullName.StartsWith("System.Nullable`1"))
                        {
                                type = type.GenericTypeArguments[0];
                        }
                        if (!_ValidateCache.TryGetValue(type.FullName, out ValidateCache cache))
                        {
                                cache = _ValidateCache.GetOrAdd(type.FullName, new ValidateCache(type));
                                cache.InitValidate();
                        }
                        return cache.AttrNum != 0;
                }
                internal static bool CheckGenericIsValidate(Type type, out Type gType, out bool isDic)
                {
                        if (type.GenericTypeArguments.Length == 1)
                        {
                                isDic = false;
                                gType = type.GenericTypeArguments[0];
                                return CheckIsValidate(type.GenericTypeArguments[0]);
                        }
                        else
                        {
                                isDic = true;
                                gType = Array.Find(type.GenericTypeArguments, a =>
                                {
                                        if (a.IsClass && a.Name != DataValidate.StrTypeName)
                                        {
                                                return CheckIsValidate(a);
                                        }
                                        else
                                        {
                                                return false;
                                        }
                                });
                                return gType != null;
                        }
                }
                private static bool _CheckIsValueGeneric(Type type, out Type gType)
                {
                        if (type.GenericTypeArguments.Length == 1)
                        {
                                gType = type.GenericTypeArguments[0];
                                return gType.IsValueType || gType.Name == DataValidate.StrTypeName;
                        }
                        gType = null;
                        return false;
                }
                private static bool _CheckIsValue(Type type)
                {
                        Type element = type.GetElementType();
                        return element.IsValueType || element.Name == DataValidate.StrTypeName;
                }
                internal static bool CheckArrayIsValidate(Type type)
                {
                        Type element = type.GetElementType();
                        if (element == null || element.IsValueType || element.Name == DataValidate.StrTypeName)
                        {
                                return false;
                        }
                        return CheckIsValidate(element);
                }

                internal static IValidateAttr GetValidateAttrs(Type attrType, ref IValidateAttr[] attr)
                {
                        if (attrType.IsGenericType && attrType.FullName.StartsWith("System.Nullable`1"))
                        {
                                attrType = attrType.GenericTypeArguments[0];
                        }
                        if ((attrType.IsClass && attrType.Name != DataValidate.StrTypeName) || attrType.IsInterface)
                        {
                                if (attrType.IsArray)
                                {
                                        if (CheckArrayIsValidate(attrType))
                                        {
                                                return new ValidateArray(attrType);
                                        }
                                        else if (_CheckIsValue(attrType))
                                        {
                                                IValidateAttr[] array = attr.FindAll(a => a.IsArray);
                                                if (array.Length > 0)
                                                {
                                                        attr = attr.Remove(a => a.IsArray);
                                                        return new ValidateAttrArray(attrType, array);
                                                }
                                        }
                                }
                                else if (attrType.IsGenericType)
                                {
                                        if (attrType.GetInterface("System.Collections.IEnumerable") == null)
                                        {
                                                foreach (Type i in attrType.GenericTypeArguments)
                                                {
                                                        if (CheckIsValidate(i))
                                                        {
                                                                return new ValidateObject();
                                                        }
                                                }
                                        }
                                        else if (CheckGenericIsValidate(attrType, out Type gType, out bool isDic))
                                        {
                                                return new ValidateGeneric(gType, isDic);
                                        }
                                        else if (_CheckIsValueGeneric(attrType, out gType))
                                        {
                                                IValidateAttr[] array = attr.FindAll(a => a.IsArray);
                                                if (array.Length > 0)
                                                {
                                                        attr = attr.Remove(a => a.IsArray);
                                                        return new ValidateAttrGeneric(gType, attr);
                                                }
                                        }
                                }
                                else if (CheckIsValidate(attrType))
                                {
                                        return new ValidateObject();
                                }
                        }
                        return null;
                }
                internal static IValidateData[] GetValidateList(Type type)
                {
                        Dictionary<string, IValidateData> attrs = new Dictionary<string, IValidateData>();
                        Type[] inter = type.GetInterfaces();
                        if (inter.Length > 0)
                        {
                                inter.ForEach(a =>
                                {
                                        IValidateData[] vAttrs = GetValidateList(a);
                                        if (vAttrs.Length > 0)
                                        {
                                                vAttrs.ForEach(b =>
                                                {
                                                        if (!attrs.ContainsKey(b.AttrName))
                                                        {
                                                                attrs.Add(b.AttrName, b);
                                                        }
                                                });
                                        }
                                });
                        }
                        PropertyInfo[] pro = type.GetProperties();
                        if (pro.Length > 0)
                        {
                                pro.ForEach(a =>
                                {
                                        IValidateAttr[] attr = a.GetCustomAttributes<ValidateAttr>(false).ToArray();
                                        IValidateAttr vattr = GetValidateAttrs(a.PropertyType, ref attr);
                                        if (vattr != null)
                                        {
                                                if (attr.Length == 0)
                                                {
                                                        attr = new IValidateAttr[] { vattr };
                                                }
                                                else
                                                {
                                                        attr = attr.Add(vattr);
                                                }
                                        }
                                        if (attr.Length != 0)
                                        {
                                                if (attrs.TryGetValue(a.Name, out IValidateData data))
                                                {
                                                        data.SyncAttr(attr);
                                                }
                                                else
                                                {
                                                        attrs.Add(a.Name, new ValidateProperty(a, attr));
                                                }
                                        }
                                });
                        }
                        FieldInfo[] fields = type.GetFields();
                        if (fields.Length > 0)
                        {
                                fields.ForEach(a =>
                                {
                                        IValidateAttr[] attr = a.GetCustomAttributes<ValidateAttr>(true).ToArray();
                                        if (attr.Length != 0)
                                        {
                                                IValidateAttr vattr = GetValidateAttrs(a.FieldType, ref attr);
                                                if (vattr != null)
                                                {
                                                        if (attr.Length == 0)
                                                        {
                                                                attr = new IValidateAttr[] { vattr };
                                                        }
                                                        else
                                                        {
                                                                attr = attr.Add(vattr);
                                                        }
                                                }
                                                if (attrs.TryGetValue(a.Name, out IValidateData data))
                                                {
                                                        data.SyncAttr(attr);
                                                }
                                                else
                                                {
                                                        attrs.Add(a.Name, new ValidateField(a, attr));
                                                }
                                        }
                                });
                        }
                        return attrs.Values.ToArray();
                }
        }
}
