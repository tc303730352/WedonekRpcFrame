using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WeDonekRpc.Helper;

namespace WeDonekRpc.Helper.Validate
{
    public class DataValidateHepler
    {
        private static string[] _ProhibitValid = new string[]
        {
            "System",
            "Microsoft"
        };
        public static void AddProhibit ( params string[] adds )
        {
            _ProhibitValid = _ProhibitValid.Add(adds);
        }
        private static readonly ConcurrentDictionary<string, IValidateCache> _ValidateCache = new ConcurrentDictionary<string, IValidateCache>();
        public static bool ValidateData ( dynamic data, out string error )
        {
            Type type = data.GetType();
            if ( type.IsArray )
            {
                type = type.GetElementType();
                foreach ( object i in data )
                {
                    if ( !_ValidateData(type, i, data, out error) )
                    {
                        return false;
                    }
                }
                error = null;
                return true;
            }
            else if ( type.IsGenericType && type.GetInterface("System.Collections.IEnumerable") != null )
            {
                if ( type.GenericTypeArguments.Length > 1 )
                {
                    throw new Exception("no.support.many.generic");
                }
                type = type.GenericTypeArguments[0];
                foreach ( object i in data )
                {
                    if ( !_ValidateData(type, i, data, out error) )
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
        public static bool ValidateData ( Type type, dynamic data, out string error )
        {
            if ( type.IsArray )
            {
                type = type.GetElementType();
                foreach ( object i in data )
                {
                    if ( !_ValidateData(type, i, data, out error) )
                    {
                        return false;
                    }
                }
                error = null;
                return true;
            }
            else if ( type.IsGenericType && type.GetInterface("System.Collections.IEnumerable") != null )
            {
                if ( type.GenericTypeArguments.Length > 1 )
                {
                    throw new Exception("no.support.many.generic");
                }
                type = type.GenericTypeArguments[0];
                foreach ( object i in data )
                {
                    if ( !_ValidateData(type, i, data, out error) )
                    {
                        return false;
                    }
                }
                error = null;
                return true;
            }
            else
            {
                if ( type.IsGenericType && type.FullName.StartsWith("System.Nullable`1") )
                {
                    type = type.GenericTypeArguments[0];
                }
                return _ValidateData(type, data, data, out error);
            }
        }
        private static bool _ValidateData ( Type type, object data, object root, out string error )
        {
            if ( !_ValidateCache.TryGetValue(type.FullName, out IValidateCache cache) )
            {
                if ( !_CheckIsValidate(type, out cache) )
                {
                    error = null;
                    return true;
                }
            }
            return cache.ValidateData(data, root, out error);
        }

        internal static bool ValidateData ( Type type, object data, object parent, out string error )
        {
            if ( !_ValidateCache.TryGetValue(type.FullName, out IValidateCache cache) )
            {
                if ( !_CheckIsValidate(type, out cache) )
                {
                    error = null;
                    return true;
                }
            }
            return cache.ValidateData(data, parent, out error);
        }
        public static bool CheckAttrIsValidate ( Type attrType, out IValidateAttr[] validateAttrs )
        {
            Type type = attrType.DeclaringType;
            if ( !_ValidateCache.TryGetValue(type.FullName, out IValidateCache cache) )
            {
                cache = _ValidateCache.GetOrAdd(type.FullName, new ValidateCache(type));
                cache.InitValidate();
            }

            return cache.CheckAttrIsValidate(attrType, out validateAttrs);
        }
        private static bool _CheckIsValidate ( Type type, out IValidateCache cache )
        {
            if ( type.IsPrimitive )
            {
                cache = null;
                return false;
            }
            else if ( _ProhibitValid.IsExists(c => c == type.Namespace || type.Namespace.StartsWith(c + ".")) )
            {
                cache = new NoValidateCache(type);
                _ = _ValidateCache.TryAdd(type.FullName, cache);
                return false;
            }
            if ( !_ValidateCache.TryGetValue(type.FullName, out cache) )
            {
                cache = _ValidateCache.GetOrAdd(type.FullName, new ValidateCache(type));
                cache.InitValidate();
            }
            return cache.AttrNum != 0;
        }
        public static bool CheckIsValidate ( Type type )
        {
            if ( type.IsGenericType && type.FullName.StartsWith("System.Nullable`1") )
            {
                type = type.GenericTypeArguments[0];
            }
            if ( !_CheckIsValidate(type, out IValidateCache cache) )
            {
                return false;
            }
            return cache.AttrNum != 0;
        }
        internal static bool CheckGenericIsValidate ( Type type, Type source )
        {
            if ( type.GenericTypeArguments.Length == 1 && type.GetMethod("GetEnumerator") != null )
            {
                Type gType = type.GenericTypeArguments[0];
                return gType == source || CheckIsValidate(type.GenericTypeArguments[0]);
            }
            return false;
        }
        internal static bool CheckDictionaryIsValidate ( Type type, Type source, out int range )
        {
            if ( type.GetInterface("IDictionary") != null )
            {
                Type key = type.GenericTypeArguments[0];
                Type value = type.GenericTypeArguments[1];
                int num = 0;
                if ( Type.GetTypeCode(key) == TypeCode.Object && ( source == key || CheckIsValidate(key) ) )
                {
                    num += 2;
                }
                if ( Type.GetTypeCode(value) == TypeCode.Object && ( source == value || CheckIsValidate(value) ) )
                {
                    num += 4;
                }
                if ( num == 0 )
                {
                    range = 0;
                    return false;
                }
                range = num;
                return true;
            }
            range = 0;
            return false;
        }
        private static bool _CheckIsValueGeneric ( Type type )
        {
            if ( type.GenericTypeArguments.Length == 1 )
            {
                Type gType = type.GenericTypeArguments[0];
                return gType.IsValueType || gType.Name == DataValidate.StrTypeName;
            }
            return false;
        }
        private static bool _CheckIsValue ( Type type )
        {
            Type element = type.GetElementType();
            return element.IsValueType || element.Name == DataValidate.StrTypeName;
        }
        internal static bool CheckArrayIsValidate ( Type type )
        {
            Type element = type.GetElementType();
            if ( element == null || element.IsValueType || element.Name == DataValidate.StrTypeName )
            {
                return false;
            }
            return CheckIsValidate(element);
        }

        internal static IValidateAttr GetValidateAttrs ( Type attrType, Type source, ref IValidateAttr[] attr )
        {
            if ( attrType.IsGenericType && attrType.FullName.StartsWith("System.Nullable`1") )
            {
                attrType = attrType.GenericTypeArguments[0];
            }
            if ( ( attrType.IsClass && attrType.Name != DataValidate.StrTypeName ) || attrType.IsInterface )
            {
                if ( attrType.IsArray )
                {
                    if ( CheckArrayIsValidate(attrType) )
                    {
                        return new ValidateArray(attrType);
                    }
                    else if ( _CheckIsValue(attrType) )
                    {
                        IValidateAttr[] array = attr.FindAll(a => a.IsArray);
                        if ( array.Length > 0 )
                        {
                            attr = attr.Remove(a => a.IsArray);
                            return new ValidateAttrArray(attrType, array);
                        }
                    }
                }
                else if ( attrType.IsGenericType )
                {
                    if ( attrType.GetInterface("System.Collections.IEnumerable") == null )
                    {
                        foreach ( Type i in attrType.GenericTypeArguments )
                        {
                            if ( i == source || CheckIsValidate(i) )
                            {
                                return new ValidateObject();
                            }
                        }
                    }
                    else if ( CheckGenericIsValidate(attrType, source) )
                    {
                        return new ValidateGeneric(attrType);
                    }
                    else if ( CheckDictionaryIsValidate(attrType, source, out int range) )
                    {
                        return new ValidateDictionary(attrType, range);
                    }
                    else if ( _CheckIsValueGeneric(attrType) )
                    {
                        IValidateAttr[] array = attr.FindAll(a => a.IsArray);
                        if ( array.Length > 0 )
                        {
                            attr = attr.Remove(a => a.IsArray);
                            return new ValidateAttrGeneric(attrType, array);
                        }
                    }
                }
                else if ( CheckIsValidate(attrType) )
                {
                    return new ValidateObject();
                }
            }
            return null;
        }
        internal static IValidateData[] GetValidateList ( Type type )
        {
            Dictionary<string, IValidateData> attrs = [];
            Type[] inter = type.GetInterfaces();
            if ( inter.Length > 0 )
            {
                inter.ForEach(a =>
                {
                    IValidateData[] vAttrs = GetValidateList(a);
                    if ( vAttrs.Length > 0 )
                    {
                        vAttrs.ForEach(b =>
                                        {
                                            if ( !attrs.ContainsKey(b.AttrName) )
                                            {
                                                attrs.Add(b.AttrName, b);
                                            }
                                        });
                    }
                });
            }
            PropertyInfo[] pro = type.GetProperties();
            if ( pro.Length > 0 )
            {
                pro.ForEach(a =>
                {
                    IValidateAttr[] attr = a.GetCustomAttributes<ValidateAttr>(false).ToArray();
                    IValidateAttr vattr = GetValidateAttrs(a.PropertyType, type, ref attr);
                    if ( vattr != null )
                    {
                        attr = attr.Length == 0 ? ( new IValidateAttr[] { vattr } ) : attr.Add(vattr);
                    }
                    if ( attr.Length != 0 )
                    {
                        if ( attrs.TryGetValue(a.Name, out IValidateData data) )
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
            if ( fields.Length > 0 )
            {
                fields.ForEach(a =>
                {
                    IValidateAttr[] attr = a.GetCustomAttributes<ValidateAttr>(true).ToArray();
                    if ( attr.Length != 0 )
                    {
                        IValidateAttr vattr = GetValidateAttrs(a.FieldType, type, ref attr);
                        if ( vattr != null )
                        {
                            attr = attr.Length == 0 ? ( new IValidateAttr[] { vattr } ) : attr.Add(vattr);
                        }
                        if ( attrs.TryGetValue(a.Name, out IValidateData data) )
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
