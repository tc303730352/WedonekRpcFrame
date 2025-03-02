using System;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

namespace WeDonekRpc.Helper.Reflection
{
    internal delegate object PropertyGet (object target);

    internal delegate bool HasValueAction (object target);
    internal delegate void PropertySet (object target, object value);
    internal delegate object GetArrayValue (object target, int index);
    internal delegate void SetArrayValue (object target, int index, object value);

    internal delegate object GetDicValue (object target, object key);
    internal delegate void SetDicValue (object target, object key, object value);

    internal delegate IDictionaryEnumerator GetDictionaryEnumerator (object target);

    internal delegate object GetEnumerator (object target);

    internal delegate bool MoveNext (object target);
    internal delegate void Reset (object target);
    internal class ReflectionTools
    {
        public static Reset GetEnumeratorReset (MethodInfo method, Type type)
        {
            ParameterExpression target = Expression.Parameter(PublicDataDic.ObjectType, "target");
            Expression exception = Expression.Convert(target, type);
            MethodCallExpression methodExp = Expression.Call(exception, method);
            return Expression.Lambda<Reset>(methodExp, target).Compile();
        }
        public static MoveNext GetEnumeratorMoveNext (MethodInfo method, Type type)
        {
            ParameterExpression target = Expression.Parameter(PublicDataDic.ObjectType, "target");
            Expression exception = Expression.Convert(target, type);
            MethodCallExpression methodExp = Expression.Call(exception, method);
            return Expression.Lambda<MoveNext>(methodExp, target).Compile();
        }
        public static GetDictionaryEnumerator GetDictionaryEnumerator (MethodInfo method, Type type)
        {
            ParameterExpression target = Expression.Parameter(PublicDataDic.ObjectType, "target");
            Expression exception = Expression.Convert(target, type);
            MethodCallExpression methodExp = Expression.Call(exception, method);
            return Expression.Lambda<GetDictionaryEnumerator>(Expression.Convert(methodExp, typeof(IDictionaryEnumerator)), target).Compile();
        }
        public static GetEnumerator GetEnumerator (MethodInfo method, Type type)
        {
            ParameterExpression target = Expression.Parameter(PublicDataDic.ObjectType, "target");
            Expression exception = Expression.Convert(target, type);
            MethodCallExpression methodExp = Expression.Call(exception, method);
            return Expression.Lambda<GetEnumerator>(Expression.Convert(methodExp, PublicDataDic.ObjectType), target).Compile();
        }
        private static readonly Type _DictionaryType = typeof(IDictionary);
        private static readonly Type _CollectionType = typeof(ICollection);
        public static ReflectionType GetClassType (Type type)
        {
            TypeCode code = Type.GetTypeCode(type);
            if (code == TypeCode.Object)
            {
                if (type.IsArray)
                {
                    return ReflectionType.数组;
                }
                Type[] types = type.GetInterfaces();
                if (types.IsExists(_DictionaryType))
                {
                    return ReflectionType.字典;
                }
                else if (types.IsExists(_CollectionType))
                {
                    return ReflectionType.集合;
                }
                return ReflectionType.对象;
            }
            return ReflectionType.基础类型;
        }
        public static GetDicValue GetDicValue (MethodInfo method, Type type, Type keyType)
        {
            ParameterExpression target = Expression.Parameter(PublicDataDic.ObjectType, "target");
            ParameterExpression key = Expression.Parameter(PublicDataDic.ObjectType, "key");
            Expression exception = Expression.Convert(target, type);
            MethodCallExpression methodExp = Expression.Call(exception, method, Expression.Convert(key, keyType));
            return Expression.Lambda<GetDicValue>(Expression.Convert(methodExp, PublicDataDic.ObjectType), target, key).Compile();
        }
        public static SetDicValue SetDicValue (MethodInfo method, Type type, Type keyType, Type valueType)
        {
            ParameterExpression target = Expression.Parameter(PublicDataDic.ObjectType, "target");
            ParameterExpression key = Expression.Parameter(PublicDataDic.ObjectType, "key");
            ParameterExpression value = Expression.Parameter(PublicDataDic.ObjectType, "value");
            Expression exception = Expression.Convert(target, type);
            MethodCallExpression methodExp = Expression.Call(exception, method, Expression.Convert(key, keyType), Expression.Convert(value, valueType));
            return Expression.Lambda<SetDicValue>(methodExp, target, key, value).Compile();
        }
        public static PropertyGet GetFieldGet (FieldInfo field)
        {
            ParameterExpression target = Expression.Parameter(PublicDataDic.ObjectType, "target");
            if (field.IsStatic)
            {
                MemberExpression member = Expression.Field(null, field);
                return Expression.Lambda<PropertyGet>(Expression.Convert(member, PublicDataDic.ObjectType), target).Compile();
            }
            else
            {
                MemberExpression member = Expression.Field(Expression.Convert(target, type: field.DeclaringType), field);
                return Expression.Lambda<PropertyGet>(Expression.Convert(member, PublicDataDic.ObjectType), target).Compile();
            }
        }
        public static HasValueAction GetNullHasValue (MethodInfo method, Type type)
        {
            ParameterExpression target = Expression.Parameter(PublicDataDic.ObjectType, "target");
            Expression exception = Expression.Convert(target, type);
            MethodCallExpression methodExp = Expression.Call(exception, method);
            return Expression.Lambda<HasValueAction>(Expression.Convert(methodExp, PublicDataDic.ObjectType), target).Compile();
        }
        public static GetArrayValue GetArrayValue (MethodInfo method, Type type)
        {
            ParameterExpression target = Expression.Parameter(PublicDataDic.ObjectType, "target");
            ParameterExpression index = Expression.Parameter(PublicDataDic.IntType, "index");
            Expression exception = Expression.Convert(target, type);
            MethodCallExpression methodExp = Expression.Call(exception, method, index);
            return Expression.Lambda<GetArrayValue>(Expression.Convert(methodExp, PublicDataDic.ObjectType), target, index).Compile();
        }
        public static SetArrayValue SetArrayValue (MethodInfo method, Type type)
        {
            ParameterExpression target = Expression.Parameter(PublicDataDic.ObjectType, "target");
            ParameterExpression index = Expression.Parameter(PublicDataDic.IntType, "index");
            ParameterExpression value = Expression.Parameter(PublicDataDic.ObjectType, "value");
            Expression exception = Expression.Convert(target, type);
            MethodCallExpression methodExp = Expression.Call(exception, method, value, index);
            return Expression.Lambda<SetArrayValue>(methodExp, target, index, value).Compile();
        }
        public static SetArrayValue SetCollectionValue (MethodInfo method, Type type, Type valueType)
        {
            ParameterExpression target = Expression.Parameter(PublicDataDic.ObjectType, "target");
            ParameterExpression index = Expression.Parameter(PublicDataDic.IntType, "index");
            ParameterExpression value = Expression.Parameter(PublicDataDic.ObjectType, "value");
            Expression exception = Expression.Convert(target, type);
            MethodCallExpression methodExp = Expression.Call(exception, method, Expression.Convert(value, valueType), index);
            return Expression.Lambda<SetArrayValue>(methodExp, target, index, value).Compile();
        }
        public static PropertyGet GetPropertyGet (MethodInfo method)
        {
            ParameterExpression target = Expression.Parameter(PublicDataDic.ObjectType, "target");
            if (method.IsStatic)
            {
                MethodCallExpression methodExp = Expression.Call(method);
                return Expression.Lambda<PropertyGet>(Expression.Convert(methodExp, PublicDataDic.ObjectType), target).Compile();
            }
            else
            {
                Expression exception = Expression.Convert(target, method.DeclaringType);
                MethodCallExpression methodExp = Expression.Call(exception, method);
                return Expression.Lambda<PropertyGet>(Expression.Convert(methodExp, PublicDataDic.ObjectType), target).Compile();
            }
        }
        public static PropertySet GetFieldSet (FieldInfo field)
        {
            ParameterExpression target = Expression.Parameter(PublicDataDic.ObjectType, "target");
            ParameterExpression value = Expression.Parameter(PublicDataDic.ObjectType, "value");
            if (field.IsStatic)
            {
                MemberExpression member = Expression.Field(null, field);
                BinaryExpression expression = Expression.Assign(member, Expression.Convert(value, field.FieldType));
                return Expression.Lambda<PropertySet>(expression, target, value).Compile();
            }
            else
            {
                try
                {
                    MemberExpression member = Expression.Field(Expression.Convert(target, type: field.DeclaringType), field);
                    BinaryExpression expression = Expression.Assign(member, Expression.Convert(value, field.FieldType));
                    return Expression.Lambda<PropertySet>(expression, target, value).Compile();
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        public static PropertySet GetPropertySet (MethodInfo method, Type type)
        {
            if (method == null)
            {
                return null;
            }
            ParameterExpression target = Expression.Parameter(PublicDataDic.ObjectType, "target");
            ParameterExpression value = Expression.Parameter(PublicDataDic.ObjectType, "value");
            if (method.IsStatic)
            {
                MethodCallExpression methodExp = Expression.Call(method, Expression.Convert(value, type));
                return Expression.Lambda<PropertySet>(methodExp, target, value).Compile();
            }
            else
            {
                try
                {
                    Expression exception = Expression.Convert(target, method.DeclaringType);
                    MethodCallExpression methodExp = Expression.Call(exception, method, Expression.Convert(value, type));
                    return Expression.Lambda<PropertySet>(methodExp, target, value).Compile();
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
    }
}
