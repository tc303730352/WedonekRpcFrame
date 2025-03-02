using System;
using System.Collections.Concurrent;
using WeDonekRpc.Helper.Reflection;

namespace WeDonekRpc.Helper
{

    public static class ReflectionLinq
    {
        private static readonly ConcurrentDictionary<Type, IGenericForeach> _Cache = new ConcurrentDictionary<Type, IGenericForeach>();
        /// <summary>
        /// 循环数组或泛型集合(需继承了IEnumerable)
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="action"></param>
        public static void ForEachObject (this object obj, Action<ObjectBody> action)
        {
            IGenericForeach each = _GetForeach(obj.GetType());
            each.Foreach(obj, action);
        }
        /// <summary>
        /// 获取数组或泛型指定索引的值
        /// </summary>
        /// <param name="source"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static T GetValue<T> (this object source, int index)
        {
            IReflectionBody body = ReflectionHepler.GetReflection(source.GetType());
            return (T)body.GetValue(source, index);
        }
        /// <summary>
        /// 获取数组或泛型指定索引的值
        /// </summary>
        /// <param name="source"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static object GetValue (this object source, int index)
        {
            IReflectionBody body = ReflectionHepler.GetReflection(source.GetType());
            return body.GetValue(source, index);
        }
        public static T GetObjectValue<T> (this object source, params string[] names)
        {
            IReflectionBody body = ReflectionHepler.GetReflection(source.GetType());
            return (T)body.GetValue(source, names);
        }
        /// <summary>
        /// 获取对象的反射代理对象
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static ObjectBody GetObject (this object source)
        {
            IReflectionBody body = ReflectionHepler.GetReflection(source.GetType());
            return new ObjectBody(source, body);
        }
        /// <summary>
        /// 获取对象属性或字段值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T GetObjectValue<T> (this object obj, string name)
        {
            IReflectionBody body = ReflectionHepler.GetReflection(obj.GetType());
            return (T)body.GetValue(obj, name);
        }
        /// <summary>
        /// 合并2个实体中的值返回已修改的属性或成员
        /// </summary>
        /// <typeparam name="T">修改源</typeparam>
        /// <typeparam name="Set">覆盖源</typeparam>
        /// <param name="source"></param>
        /// <param name="set"></param>
        /// <returns></returns>
        public static string[] Merge<T, Set> (this T source, Set set) where T : class
        {
            IReflectionBody body = ReflectionHepler.GetReflection(typeof(T));
            IReflectionBody setBody = ReflectionHepler.GetReflection(typeof(Set));
            return body.Cover(setBody, source, set);
        }
        public static New[] ToConvertAll<T, New> (this T[] source) where New : class, new()
        {
            IReflectionBody body = ReflectionHepler.GetReflection(typeof(T));
            IReflectionBody newBody = ReflectionHepler.GetReflection(typeof(New));
            return source.ConvertAll(c =>
            {
                New data = new New();
                newBody.Init(body, c, data);
                return data;
            });
        }
        public static New[] ToConvertAll<T, New> (this T[] source, out string[] pros) where New : class, new()
        {
            IReflectionBody body = ReflectionHepler.GetReflection(typeof(T));
            IReflectionBody newBody = ReflectionHepler.GetReflection(typeof(New));
            pros = body.Properties.ConvertAll(a => a.Name);
            return source.ConvertAll(c =>
            {
                New data = new New();
                newBody.Init(body, c, data);
                return data;
            });
        }
        /// <summary>
        /// 合并2个实体中的值返回已修改的属性或成员
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="set"></param>
        /// <returns></returns>
        public static string[] Merge<T> (this T source, T set) where T : class
        {
            IReflectionBody body = ReflectionHepler.GetReflection(typeof(T));
            return body.Cover(source, set);
        }
        /// <summary>
        /// 获取对象属性或字段值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static object GetObjectValue (this object obj, string name)
        {
            IReflectionBody body = ReflectionHepler.GetReflection(obj.GetType());
            return body.GetValue(obj, name);
        }
        /// <summary>
        /// 设置对象的属性或字段值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void SetObjectValue (this object obj, string name, object value)
        {
            IReflectionBody body = ReflectionHepler.GetReflection(obj.GetType());
            body.SetValue(obj, value, name);
        }
        /// <summary>
        /// 设置数组或集合中的值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public static void SetObjectValue (this object obj, int index, object value)
        {
            IReflectionBody body = ReflectionHepler.GetReflection(obj.GetType());
            body.SetValue(obj, index, value);
        }
        /// <summary>
        /// 设置对象中的成员
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int SetObjectValue<T> (this T obj, T value) where T : class
        {
            IReflectionBody body = ReflectionHepler.GetReflection(typeof(T));
            IReflectionProperty[] list = body.Properties.FindAll(c => c.IsWrite && c.IsRead);
            int row = 0;
            list.ForEach(c =>
            {
                object val = c.GetValue(obj);
                object newVal = c.GetValue(value);
                if (val.Equals(newVal))
                {
                    return;
                }
                c.SetValue(obj, newVal);
                row += 1;
            });
            return row;
        }
        /// <summary>
        /// 设置对象的属性或字段值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="action"></param>
        public static void SetObjectValue (this object obj, Action<ObjectBody> action)
        {
            IReflectionBody body = ReflectionHepler.GetReflection(obj.GetType());
            action(new ObjectBody(obj, body));
        }
        /// <summary>
        /// 获取对象静态属性或字段值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T GetObjectValue<T> (this Type type, string name)
        {
            IReflectionBody body = ReflectionHepler.GetReflection(type);
            return (T)body.GetValue(null, name);
        }
        /// <summary>
        /// 获取对象静态属性或字段值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static object GetObjectValue (this Type type, string name)
        {
            IReflectionBody body = ReflectionHepler.GetReflection(type);
            return body.GetValue(null, name);
        }
        /// <summary>
        /// 设置对象的属性或字段值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void SetObjectValue (this Type type, string name, object value)
        {
            IReflectionBody body = ReflectionHepler.GetReflection(type);
            body.SetValue(null, value, name);
        }

        /// <summary>
        /// 创建一个新的实例
        /// </summary>
        /// <param name="type"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static ObjectBody Create (this Type type)
        {
            IReflectionBody body = ReflectionHepler.GetReflection(type);
            object obj = Activator.CreateInstance(type);
            return new ObjectBody(obj, body);
        }

        private static IGenericForeach _GetForeach (Type type)
        {
            if (_Cache.TryGetValue(type, out IGenericForeach generic))
            {
                return generic;
            }
            generic = new GenericForeach(type);
            if (!generic.IsForeach)
            {
                throw new ErrorException("reflection.type.no.IEnumerable");
            }
            return _Cache.GetOrAdd(type, generic);
        }
    }
}
