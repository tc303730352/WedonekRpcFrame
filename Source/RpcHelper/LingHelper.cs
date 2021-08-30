using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using RpcHelper.Validate;
namespace RpcHelper
{
        public delegate bool PredicateFun<T>(T data, out string error);
        public delegate bool ArrayTrue<T>(T data, int index, out string error);

        public static class LingHelper
        {
                private static readonly int _Size = 1024 * 1024;
                public static void AddOrSet<Key, Value>(this IDictionary<Key, Value> dic, Key key, Value val)
                {
                        if (dic.ContainsKey(key))
                        {
                                dic[key] = val;
                        }
                        else
                        {
                                dic.Add(key, val);
                        }
                }
                public static void TryAdd<Key, Value>(this Dictionary<Key, Value> dic, Key key, Value val)
                {
                        if (!dic.ContainsKey(key))
                        {
                                dic.Add(key, val);
                        }
                }
                public static T[] Random<T>(this List<T> list)
                {
                        var array = list.ConvertAll(a => new
                        {
                                a,
                                sort = Guid.NewGuid().ToString()
                        });
                        return array.OrderBy(a => a.sort).Select(a => a.a).ToArray();
                }
                public static bool IsExists<T>(this IEnumerable<T> list, Func<T, bool> func)
                {
                        return list.FirstOrDefault(func) != null;
                }
                public static bool IsEquals<T>(this T[] data, T[] other)
                {
                        return ArrayHelper.CompareSortArray(data, other);
                }
                public static bool IsEquals<T>(this T data, T other) where T : class
                {
                        return data.GetMd5() == other.GetMd5();
                }
                public static bool IsEquals<One, Two>(this One data, Two other) where One : class where Two : class
                {
                        return data.GetMd5() == other.GetMd5(typeof(One));
                }
                public static Out[] ConvertAll<T, Out>(this ICollection<T> data, Func<T, Out> func)
                {
                        Out[] list = new Out[data.Count];
                        int i = 0;
                        foreach (T k in data)
                        {
                                list[i++] = func(k);
                        }
                        return list;
                }
                public static Out[] ConvertAll<T, Out>(this List<T> data, Func<T, Out> func)
                {
                        Out[] list = new Out[data.Count];
                        int i = 0;
                        foreach (T k in data)
                        {
                                list[i++] = func(k);
                        }
                        return list;
                }
                public static long Arg(this long[] data)
                {
                        return data.Sum(a => a) / data.Length;
                }
                public static int Arg(this int[] data)
                {
                        return data.Sum(a => a) / data.Length;
                }
                public static int Arg<T>(this T[] data, Func<T, int> func)
                {
                        return data.Sum(func) / data.Length;
                }
                public static short Arg(this short[] data)
                {
                        return (short)(data.Sum(a => a) / data.Length);
                }
                /// <summary>
                /// 获取ZoneIndex
                /// </summary>
                /// <param name="str"></param>
                /// <returns></returns>
                public static short GetZIndex(this string str)
                {
                        return Tools.GetZoneIndex(str);
                }
                /// <summary>
                /// 向URI追加参数
                /// </summary>
                /// <param name="uri"></param>
                /// <param name="param"></param>
                /// <returns></returns>
                public static Uri AppendParam(this Uri uri, string param)
                {
                        return Tools.GetJumpUri(uri, param);
                }
                /// <summary>
                /// 获取ZoneIndex
                /// </summary>
                /// <param name="str"></param>
                /// <returns></returns>
                public static short GetZIndex(this Guid str)
                {
                        return Tools.GetZoneIndex(str.ToString());
                }

                /// <summary>
                /// 字符串转DateTime
                /// </summary>
                /// <param name="str"></param>
                /// <returns></returns>
                public static DateTime ToDateTime(this string str)
                {
                        return Tools.PaseDateTime(str);
                }
                /// <summary>
                /// 检查文件后缀
                /// </summary>
                /// <param name="file"></param>
                /// <param name="containStr"></param>
                /// <returns></returns>
                public static bool CheckFile(this FileInfo file, string[] containStr)
                {
                        string ext = file.Extension.ToLower();
                        return Array.FindIndex(containStr, a => ext == a) != -1;
                }
                /// <summary>
                /// 验证字符串格式
                /// </summary>
                /// <param name="str">字符串</param>
                /// <param name="format">格式</param>
                /// <param name="containStr">包含的字符</param>
                /// <returns></returns>
                public static bool Validate(this string str, ValidateFormat format, string containStr)
                {
                        return ValidateHelper.CheckData(str, format, containStr);
                }
                /// <summary>
                /// 验证字符串格式
                /// </summary>
                /// <param name="str">字符串</param>
                /// <param name="format">格式</param>
                /// <returns></returns>
                public static bool Validate(this string str, ValidateFormat format)
                {
                        return ValidateHelper.CheckData(str, format, null);
                }
                /// <summary>
                /// 不能为空
                /// </summary>
                /// <param name="str"></param>
                /// <returns></returns>
                public static bool NoNull(this string str)
                {
                        return str != null && str != string.Empty;
                }

                /// <summary>
                /// 排序
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="datas"></param>
                public static void Sort<T>(this T[] datas) where T : struct
                {
                        Array.Sort(datas);
                }
                public static string[] Sort(this string[] datas)
                {
                        Array.Sort(datas);
                        return datas;
                }
                /// <summary>
                /// 是否空
                /// </summary>
                /// <param name="str"></param>
                /// <returns></returns>
                public static bool IsNull(this string str)
                {
                        return str == null || str == string.Empty;
                }
                public static string ToString(this Dictionary<string, string> dic)
                {
                        StringBuilder str = new StringBuilder();
                        foreach (string i in dic.Keys)
                        {
                                str.AppendFormat("&{0}={1}", i, dic[i]);
                        }
                        return str.ToString();
                }
                public static string ToString(this Dictionary<string, string> dic, params string[] remove)
                {
                        if (remove != null)
                        {
                                remove.ForEach(a => dic.Remove(a));
                        }
                        if (dic.Count == 0)
                        {
                                return string.Empty;
                        }
                        StringBuilder str = new StringBuilder();
                        foreach (string i in dic.Keys)
                        {
                                str.AppendFormat("&{0}={1}", i, dic[i]);
                        }
                        return str.ToString();
                }
                /// <summary>
                /// 分页
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="array">分页数组</param>
                /// <param name="index">页码</param>
                /// <param name="size">每页大小</param>
                /// <returns></returns>
                public static T[] Paging<T>(this T[] array, int index, int size)
                {
                        long skip = (index - 1) * size;
                        if (skip >= array.LongLength)
                        {
                                return new T[0];
                        }
                        long len = array.LongLength - skip;
                        if (len > size)
                        {
                                len = size;
                        }
                        T[] datas = new T[len];
                        for (int i = 0; i < datas.Length; i++)
                        {
                                datas[i] = array[skip + i];
                        }
                        return datas;
                }
                /// <summary>
                /// 分页
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="array">分页数组</param>
                /// <param name="index">页码</param>
                /// <param name="size">每页大小</param>
                /// <returns></returns>
                public static T[] Paging<T>(this List<T> array, int index, int size)
                {
                        int skip = index * size;
                        if (skip >= array.Count)
                        {
                                return Array.Empty<T>();
                        }
                        int len = array.Count - skip;
                        if (len > size)
                        {
                                len = size;
                        }
                        T[] datas = new T[len];
                        for (int i = 0; i < datas.Length; i++)
                        {
                                datas[i] = array[skip + i];
                        }
                        return datas;
                }
                /// <summary>
                /// 分页
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="list">List</param>
                /// <param name="index">页码</param>
                /// <param name="size">每页大小</param>
                /// <returns></returns>
                public static T[] ToArray<T>(this List<T> list, int skip, int size)
                {
                        int len = list.Count - skip;
                        if (len > size)
                        {
                                len = size;
                        }
                        T[] datas = new T[len];
                        for (int i = 0; i < datas.Length; i++)
                        {
                                datas[i] = list[skip + i];
                        }
                        return datas;
                }
                /// <summary>
                /// 将数组用指定分隔符链接成字符串
                /// </summary>
                /// <typeparam name="T">数据</typeparam>
                /// <param name="array">分隔的数组</param>
                /// <param name="separator">分隔符</param>
                /// <param name="func">将数据装换格式的方法（数据，数组索引）</param>
                /// <returns></returns>
                public static string Join<T>(this T[] array, string separator, Func<T, int, string> func)
                {
                        if (array == null || array.Length == 0)
                        {
                                return string.Empty;
                        }
                        StringBuilder str = new StringBuilder();
                        for (int i = 0; i < array.Length; i++)
                        {
                                str.Append(separator);
                                str.Append(func(array[i], i));
                        }
                        str.Remove(0, 1);
                        return str.ToString();
                }
                /// <summary>
                /// 将数组用指定分隔符链接成字符串
                /// </summary>
                /// <typeparam name="T">数据</typeparam>
                /// <param name="array">分隔的数组</param>
                /// <param name="separator">分隔符</param>
                /// <returns></returns>
                public static string Join<T>(this T[] array, string separator)
                {
                        if (array == null || array.Length == 0)
                        {
                                return string.Empty;
                        }
                        StringBuilder str = new StringBuilder();
                        foreach (T i in array)
                        {
                                str.Append(separator);
                                str.Append(i);
                        }
                        str.Remove(0, 1);
                        return str.ToString();
                }
                /// <summary>
                /// 用指定分隔符链接成字符串
                /// </summary>
                /// <typeparam name="T">数据</typeparam>
                /// <param name="array">分隔的数组</param>
                /// <param name="separator">分隔符</param>
                /// <returns></returns>
                public static string Join<T>(this List<T> list, string separator)
                {
                        if (list == null || list.Count == 0)
                        {
                                return string.Empty;
                        }
                        StringBuilder str = new StringBuilder();
                        foreach (T i in list)
                        {
                                str.Append(separator);
                                str.Append(i);
                        }
                        str.Remove(0, 1);
                        return str.ToString();
                }
                /// <summary>
                /// 将数组用指定分隔符链接成字符串
                /// </summary>
                /// <typeparam name="T">数据</typeparam>
                /// <param name="array">分隔的数组</param>
                /// <param name="separator">分隔符</param>
                /// <param name="func">将数据装换格式的方法（数据）</param>
                /// <returns></returns>
                public static string Join<T>(this T[] array, string separator, Func<T, string> func)
                {
                        if (array == null || array.Length == 0)
                        {
                                return string.Empty;
                        }
                        StringBuilder str = new StringBuilder(64);
                        foreach (T i in array)
                        {
                                str.Append(separator);
                                str.Append(func(i));
                        }
                        str.Remove(0, separator.Length);
                        return str.ToString();
                }
                public static void Join<T>(this T[] array, string separator, Func<T, string> func, StringBuilder str)
                {
                        foreach (T i in array)
                        {
                                str.Append(func(i));
                                str.Append(separator);
                        }
                        int len = separator.Length;
                        str.Remove(str.Length - len, len);
                }
                /// <summary>
                /// 添加成员
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="array">原数组</param>
                /// <param name="add">添加的成员</param>
                /// <returns></returns>
                public static T[] Add<T>(this T[] array, T add)
                {
                        if (array == null || array.Length == 0)
                        {
                                return new T[] { add };
                        }
                        T[] data = new T[array.Length + 1];
                        array.CopyTo(data, 0);
                        data[array.Length] = add;
                        return data;
                }
                /// <summary>
                /// 添加成员
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="array">原数组</param>
                /// <param name="add">添加的成员</param>
                /// <returns></returns>
                public static T[] Add<T>(this T[] array, IList<T> add)
                {
                        if (array == null || array.Length == 0)
                        {
                                return add.ToArray();
                        }
                        T[] data = new T[array.Length + add.Count];
                        array.CopyTo(data, 0);
                        add.CopyTo(data, array.Length);
                        return data;
                }
                /// <summary>
                /// 添加成员
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="array">原数组</param>
                /// <param name="add">添加的成员</param>
                /// <returns></returns>
                public static T[] Add<T>(this T[] array, T add, T two)
                {
                        if (array == null || array.Length == 0)
                        {
                                return new T[] { add, two };
                        }
                        T[] data = new T[array.Length + 2];
                        array.CopyTo(data, 0);
                        data[array.Length] = add;
                        data[array.Length + 1] = two;
                        return data;
                }
                /// <summary>
                /// 添加成员
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="array">原数组</param>
                /// <param name="add">添加的成员</param>
                /// <returns></returns>
                public static T[] Add<T>(this T[] array, T add, T two, T third)
                {
                        if (array == null || array.Length == 0)
                        {
                                return new T[] { add, two, third };
                        }
                        T[] data = new T[array.Length + 3];
                        array.CopyTo(data, 0);
                        data[array.Length] = add;
                        data[array.Length + 1] = two;
                        data[array.Length + 2] = third;
                        return data;
                }
                /// <summary>
                /// 添加成员
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="array">原数组</param>
                /// <param name="add">添加的成员</param>
                /// <param name="adds">新增的成员数组</param>
                /// <returns></returns>
                public static T[] Add<T>(this T[] array, T add, T[] adds)
                {
                        if (adds == null || adds.Length == 0)
                        {
                                return array.Add(add);
                        }
                        T[] data = new T[array.Length + 1 + adds.Length];
                        array.CopyTo(data, 0);
                        data[array.Length] = add;
                        adds.CopyTo(data, array.Length + 1);
                        return data;
                }
                public static T[] Add<T>(this T[] array, T[] adds, int len)
                {
                        if (array == null)
                        {
                                return adds;
                        }
                        T[] data = new T[array.Length + len];
                        array.CopyTo(data, 0);
                        adds.CopyTo(data, array.Length);
                        return data;
                }
                public static T[] Add<T>(this T[] array, T[] adds)
                {
                        if (adds.Length == 0)
                        {
                                return array;
                        }
                        return array.Add(adds, adds.Length);
                }
                public static T[] Add<T>(this T[] array, T[] one, T[] two)
                {
                        if (one == null || one.Length == 0)
                        {
                                return array.Join(two);
                        }
                        T[] data = new T[array.Length + one.Length + two.Length];
                        array.CopyTo(data, 0);
                        int begin = array.Length;
                        one.CopyTo(data, begin);
                        begin += one.Length;
                        two.CopyTo(data, begin);
                        return data;
                }
                public static Out[] Add<T, Out>(this T[] array, Out add, Converter<T, Out> converter)
                {
                        if (array == null || array.Length == 0)
                        {
                                return new Out[] { add };
                        }
                        Out[] data = new Out[array.Length + 1];
                        for (int i = 0; i < array.Length; i++)
                        {
                                data[i] = converter(array[i]);
                        }
                        data[array.Length] = add;
                        return data;
                }
                /// <summary>
                /// 向数组顶部插入成员
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="array">原数组</param>
                /// <param name="add">添加的成员</param>
                /// <returns></returns>
                public static T[] TopInsert<T>(this T[] array, T add)
                {
                        if (array == null || array.Length == 0)
                        {
                                return new T[] { add };
                        }
                        T[] data = new T[array.Length + 1];
                        array.CopyTo(data, 1);
                        data[0] = add;
                        return data;
                }
                /// <summary>
                /// 向数组顶部插入成员
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="array">原数组</param>
                /// <param name="add">添加的成员</param>
                /// <returns></returns>
                public static T[] TopInsert<T>(this T[] array, T[] add)
                {
                        if (array == null)
                        {
                                return add;
                        }
                        T[] data = new T[array.Length + add.Length];
                        array.CopyTo(data, add.Length);
                        add.CopyTo(data, 0);
                        return data;
                }
                /// <summary>
                /// 向数组的指定索引位添加成员
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="array">原数组</param>
                /// <param name="add">添加的成员</param>
                /// <param name="index">插入的位置</param>
                /// <returns></returns>
                public static T[] Insert<T>(this T[] array, T add, int index)
                {
                        if (index == array.Length)
                        {
                                return Add(array, add);
                        }
                        else if (index == 0)
                        {
                                return TopInsert(array, add);
                        }
                        else
                        {
                                T[] data = new T[array.Length + 1];
                                data[index] = add;
                                Array.Copy(array, 0, data, 0, index);
                                Array.Copy(array, index, data, index + 1, array.Length - index);
                                return data;
                        }
                }
                /// <summary>
                /// 不对两个数组成员并设置成员属性值
                /// </summary>
                /// <typeparam name="T">设置的成员</typeparam>
                /// <typeparam name="T1">比对的成员</typeparam>
                /// <param name="array">成员数组</param>
                /// <param name="datas">比对成员数组</param>
                /// <param name="match">匹配成员</param>
                /// <param name="action">比对成员赋值处理</param>
                public static void SetAttrVal<T, T1>(this T[] array, T1[] datas, Func<T, T1, bool> match, Action<T, T1> action) where T : class
                {
                        Array.ForEach(array, a =>
                        {
                                foreach (T1 i in datas)
                                {
                                        if (match(a, i))
                                        {
                                                action(a, i);
                                                break;
                                        }
                                }
                        });
                }

                public static T[] EextendArray<T>(this T[] data, int size, Func<T, int> find, Func<int, T> def) where T : class
                {
                        if (data.Length == size)
                        {
                                return data;
                        }
                        T[] list = new T[size];
                        foreach (T i in data)
                        {
                                int k = find(i);
                                list[k] = i;
                        }
                        for (int i = 0; i < list.Length; i++)
                        {
                                if (list[i] == null)
                                {
                                        list[i] = def(i);
                                }
                        }
                        return list;
                }
                public static void SetEmptyDef<T>(this T[] data, Func<int, T> def) where T : class
                {
                        for (int i = 0; i < data.Length; i++)
                        {
                                if (data[i] == null)
                                {
                                        data[i] = def(i);
                                }
                        }
                }
                public static void Init<T>(this T[] array, int skip, int size, Func<int, T> func)
                {
                        int end = skip + size;
                        for (int i = skip; i < end; i++)
                        {
                                array[i] = func(i);
                        }
                }
                /// <summary>
                /// 从指定数组中搜索对象字段值进行赋值
                /// </summary>
                /// <typeparam name="T">输出对象</typeparam>
                /// <typeparam name="T1">输入对象</typeparam>
                /// <param name="data">输出对象</param>
                /// <param name="array">输入的数组</param>
                /// <param name="func">搜索对象的方法(属性名,搜索的输入对象,返回搜索结果)</param>
                public static void Init<T, T1>(this T data, T1[] array, Func<string, T1, object> func) where T : class
                {
                        Type type = data.GetType();
                        PropertyInfo[] proList = type.GetProperties();
                        Array.ForEach(proList, a =>
                        {
                                foreach (T1 i in array)
                                {
                                        object val = func(a.Name, i);
                                        if (val != null)
                                        {
                                                a.SetValue(data, ModelHelper.ChangeType(a.PropertyType, val));
                                                break;
                                        }
                                }
                        });
                }
                /// <summary>
                /// 初始化数组中的成员
                /// </summary>
                /// <typeparam name="T">成员</typeparam>
                /// <param name="data">成员数组</param>
                /// <param name="func">成员方法</param>
                public static void Init<T>(this T[] data, Func<int, T, T> func) where T : class
                {
                        for (int i = 0; i < data.Length; i++)
                        {
                                data[i] = func(i, data[i]);
                        }
                }
                /// <summary>
                /// 将数组用链接成字符串
                /// </summary>
                /// <typeparam name="T">连接的数据</typeparam>
                /// <param name="array">输入的数组</param>
                /// <param name="func">数据转换的方法(数据)</param>
                /// <returns></returns>
                public static string Join<T>(this T[] array, Func<T, string> func)
                {
                        if (array.Length == 1)
                        {
                                return func(array[0]);
                        }
                        StringBuilder str = new StringBuilder();
                        foreach (T i in array)
                        {
                                str.Append(func(i));
                        }
                        return str.ToString();
                }
                /// <summary>
                /// 将数组用链接成字符串
                /// </summary>
                /// <typeparam name="T">连接的数据</typeparam>
                /// <param name="array">输入的数组</param>
                /// <param name="func">数据转换的方法(数据,数组索引)</param>
                /// <returns></returns>
                public static string Join<T>(this T[] array, Func<T, int, string> func)
                {
                        StringBuilder str = new StringBuilder();
                        for (int i = 0; i < array.Length; i++)
                        {
                                str.Append(func(array[i], i));
                        }
                        return str.ToString();
                }

                /// <summary>
                /// 将字符串数组用指定分隔符链接成字符串
                /// </summary>
                /// <param name="array"></param>
                /// <param name="separator"></param>
                /// <returns></returns>
                public static string Join(this string[] array, string separator)
                {
                        if (array == null || array.Length == 0)
                        {
                                return string.Empty;
                        }
                        StringBuilder str = new StringBuilder();
                        foreach (string i in array)
                        {
                                str.Append(separator);
                                str.Append(i);
                        }
                        str.Remove(0, 1);
                        return str.ToString();
                }

                public static string Join<T>(this T[] array, string separator, Func<T, bool> match, Func<T, string> func)
                {
                        if (array == null || array.Length == 0)
                        {
                                return string.Empty;
                        }
                        StringBuilder str = new StringBuilder();
                        foreach (T i in array)
                        {
                                if (match(i))
                                {
                                        str.Append(separator);
                                        str.Append(func(i));
                                }
                        }
                        if (str.Length > 0)
                        {
                                str.Remove(0, 1);
                        }
                        return str.ToString();
                }
                /// <summary>
                /// 将字符串数组连接为完整字符串
                /// </summary>
                /// <param name="array"></param>
                /// <returns></returns>
                public static string Join(this string[] array)
                {
                        StringBuilder str = new StringBuilder();
                        foreach (string i in array)
                        {
                                str.Append(i);
                        }
                        return str.ToString();
                }
                /// <summary>
                /// 时间戳转DateTime
                /// </summary>
                /// <param name="str"></param>
                /// <returns></returns>
                public static DateTime ToDateTime(this long timestamp)
                {
                        return Tools.GetTimeStamp(timestamp);
                }
                /// <summary>
                /// 获取时间戳
                /// </summary>
                /// <param name="time"></param>
                /// <returns></returns>
                public static long ToLong(this DateTime time)
                {
                        return Tools.GetTimeSpan(time);
                }
                /// <summary>
                /// 获取时间戳
                /// </summary>
                /// <param name="time"></param>
                /// <returns></returns>
                public static long ToMilliseconds(this DateTime time)
                {
                        return Tools.GetTotalMilliseconds(time);
                }
                /// <summary>
                /// 获取时间的时辰范围
                /// </summary>
                /// <param name="time"></param>
                /// <returns></returns>
                public static TimeIntervalType GetTimeIntervalType(this DateTime time)
                {
                        return Tools.GetTimeIntervalType(time);
                }
                /// <summary>
                /// 获取MD5
                /// </summary>
                /// <param name="str"></param>
                /// <returns></returns>
                public static string GetMd5(this string str)
                {
                        return Tools.GetMD5(str);
                }
                /// <summary>
                /// 获取MD5
                /// </summary>
                /// <param name="str"></param>
                /// <returns></returns>
                public static string GetMd5(this Uri uri)
                {
                        return Tools.GetMD5(uri.AbsoluteUri);
                }
                /// <summary>
                /// 获取MD5
                /// </summary>
                /// <param name="str"></param>
                /// <returns></returns>
                public static string GetMd5<T>(this T data, params string[] remove) where T : class
                {
                        return Tools.GetClassMd5(data, remove);
                }
                /// 获取MD5
                /// </summary>
                /// <param name="str"></param>
                /// <returns></returns>
                public static string GetMd5<T, Source>(this T data, params string[] remove) where T : class
                {
                        return Tools.GetClassMd5<Source>(data, remove);
                }
                public static string GetMd5(this string[] data)
                {
                        return Tools.GetMD5(string.Join(",", data));
                }
                public static string GetMd5(this Guid[] data)
                {
                        return Tools.GetMD5(string.Join(",", data));
                }
                public static string GetMd5<T>(this T[] data, params string[] remove) where T : class
                {
                        Array.Sort(data);
                        StringBuilder str = new StringBuilder();
                        Type type = typeof(T);
                        foreach (T i in data)
                        {
                                str.Append(",");
                                str.Append(Tools.GetClassStr(type, i, remove));
                        }
                        str.Remove(0, 1);
                        return str.ToString().GetMd5();
                }
                /// <summary>
                /// 获取MD5
                /// </summary>
                /// <param name="str"></param>
                /// <returns></returns>
                public static string GetMd5<T>(this T data, Type template, params string[] remove) where T : class
                {
                        return Tools.GetClassMd5(template, data, remove);
                }
                /// <summary>
                /// 获取MD5
                /// </summary>
                /// <param name="str"></param>
                /// <returns></returns>
                public static string GetMd5<T>(this T data, Type template) where T : class
                {
                        return Tools.GetClassMd5(template, data);

                }
                /// <summary>
                /// 搜索数组成员
                /// </summary>
                /// <typeparam name="T">成员</typeparam>
                /// <param name="array">成员数组</param>
                /// <param name="match">条件（成员）</param>
                /// <returns>成员</returns>
                public static T Find<T>(this T[] array, Predicate<T> match)
                {
                        foreach (T i in array)
                        {
                                if (match(i))
                                {
                                        return i;
                                }
                        }
                        return default;
                }
                /// <summary>
                /// 搜索数组成员
                /// </summary>
                /// <typeparam name="T">成员</typeparam>
                /// <param name="array">成员数组</param>
                /// <param name="match">条件（成员）</param>
                /// <returns>成员</returns>
                public static T Find<T, T1>(this T[] array, T1[] datas, Func<T, T1, bool> match)
                {
                        foreach (T i in array)
                        {
                                if (datas.FindIndex(a => match(i, a)) != -1)
                                {
                                        return i;
                                }
                        }
                        return default;
                }
                public static T Find<T>(this T[] array, int begin, int end, Predicate<T> match)
                {
                        for (int i = begin; i <= end; i++)
                        {
                                T data = array[i];
                                if (match(data))
                                {
                                        return data;
                                }
                        }
                        return default;
                }
                public static T Find<T>(this T[] array, int begin, Predicate<T> match)
                {
                        for (int i = begin; i < array.Length; i++)
                        {
                                T data = array[i];
                                if (match(data))
                                {
                                        return data;
                                }
                        }
                        return default;
                }
                public static T Find<T>(this T[] array, Predicate<T> match, T def)
                {
                        foreach (T i in array)
                        {
                                if (match(i))
                                {
                                        return i;
                                }
                        }
                        return def;
                }
                public static short Max<T>(this T[] array, Predicate<T> match, Converter<T, short> convert, short defVal)
                {
                        if (array.IsNull())
                        {
                                return defVal;
                        }
                        short max = short.MinValue;
                        bool isNull = true;
                        foreach (T i in array)
                        {
                                if (match(i))
                                {
                                        isNull = false;
                                        short a = convert(i);
                                        if (a > max)
                                        {
                                                max = a;
                                        }
                                }
                        }
                        return isNull ? defVal : max;
                }
                public static short Min<T>(this T[] array, Predicate<T> match, Converter<T, short> convert, short defVal)
                {
                        if (array.IsNull())
                        {
                                return defVal;
                        }
                        short min = short.MaxValue;
                        bool isNull = true;
                        foreach (T i in array)
                        {
                                if (match(i))
                                {
                                        isNull = false;
                                        short a = convert(i);
                                        if (a < min)
                                        {
                                                min = a;
                                        }
                                }
                        }
                        return isNull ? defVal : min;
                }
                public static int Max<T>(this T[] array, Predicate<T> match, Converter<T, int> convert, int defVal)
                {
                        if (array.IsNull())
                        {
                                return defVal;
                        }
                        int max = int.MinValue;
                        bool isNull = true;
                        foreach (T i in array)
                        {
                                if (match(i))
                                {
                                        isNull = false;
                                        int a = convert(i);
                                        if (a > max)
                                        {
                                                max = a;
                                        }
                                }
                        }
                        return isNull ? defVal : max;
                }
                public static int Min<T>(this T[] array, Predicate<T> match, Converter<T, int> convert, int defVal)
                {
                        if (array.IsNull())
                        {
                                return defVal;
                        }
                        int min = int.MaxValue;
                        bool isNull = true;
                        foreach (T i in array)
                        {
                                if (match(i))
                                {
                                        isNull = false;
                                        int a = convert(i);
                                        if (a < min)
                                        {
                                                min = a;
                                        }
                                }
                        }
                        return isNull ? defVal : min;
                }
                public static long Max<T>(this T[] array, Predicate<T> match, Converter<T, long> convert, long defVal)
                {
                        if (array.IsNull())
                        {
                                return defVal;
                        }
                        long max = long.MinValue;
                        bool isNull = true;
                        foreach (T i in array)
                        {
                                if (match(i))
                                {
                                        isNull = false;
                                        long a = convert(i);
                                        if (a > max)
                                        {
                                                max = a;
                                        }
                                }
                        }
                        return isNull ? defVal : max;
                }
                public static long Min<T>(this T[] array, Predicate<T> match, Converter<T, long> convert, long defVal)
                {
                        if (array.IsNull())
                        {
                                return defVal;
                        }
                        long min = long.MaxValue;
                        bool isNull = true;
                        foreach (T i in array)
                        {
                                if (match(i))
                                {
                                        isNull = false;
                                        long a = convert(i);
                                        if (a < min)
                                        {
                                                min = a;
                                        }
                                }
                        }
                        return isNull ? defVal : min;
                }
                /// <summary>
                /// 搜索数组成员并转换其类型
                /// </summary>
                /// <typeparam name="T">成员</typeparam>
                /// <typeparam name="Result">转换结果</typeparam>
                /// <param name="array">成员数组</param>
                /// <param name="match">搜索条件(成员)</param>
                /// <param name="func">成员数据转换(成员，转换结果)</param>
                /// <returns>转换结果</returns>
                public static Result Find<T, Result>(this T[] array, Predicate<T> match, Func<T, Result> func)
                {
                        foreach (T i in array)
                        {
                                if (match(i))
                                {
                                        return func(i);
                                }
                        }
                        return default;
                }
                /// <summary>
                /// 搜索数组成员并转换其类型
                /// </summary>
                /// <typeparam name="T">成员</typeparam>
                /// <typeparam name="Result">转换结果</typeparam>
                /// <param name="array">成员数组</param>
                /// <param name="match">搜索条件(成员)</param>
                /// <param name="func">成员数据转换(成员，转换结果)</param>
                /// <returns>转换结果</returns>
                public static Result Find<T, Result>(this T[] array, Predicate<T> match, Func<T, Result> func, Result def)
                {
                        foreach (T i in array)
                        {
                                if (match(i))
                                {
                                        return func(i);
                                }
                        }
                        return def;
                }
                /// <summary>
                /// 向数组追加成员
                /// </summary>
                /// <typeparam name="T">成员</typeparam>
                /// <param name="array">原数组</param>
                /// <param name="two">追加的数组</param>
                /// <returns>新数组</returns>
                public static T[] Join<T>(this T[] array, T[] two)
                {
                        if (array == null || array.Length == 0)
                        {
                                return two;
                        }
                        else if (two == null || two.Length == 0)
                        {
                                return array;
                        }
                        T[] list = new T[array.Length + two.Length];
                        array.CopyTo(list, 0);
                        two.CopyTo(list, array.Length);
                        return list;
                }
                public static T[] Join<T>(this T[] array, List<T> two)
                {
                        if (array == null || array.Length == 0)
                        {
                                return two.ToArray();
                        }
                        else if (two == null || two.Count == 0)
                        {
                                return array;
                        }
                        T[] list = new T[array.Length + two.Count];
                        array.CopyTo(list, 0);
                        two.CopyTo(list, array.Length);
                        return list;
                }
                /// <summary>
                /// 向数组追加成员
                /// </summary>
                /// <typeparam name="T">成员</typeparam>
                /// <typeparam name="T1">输入的成员</typeparam>
                /// <param name="array">原成员数组</param>
                /// <param name="two">追加的输入成员数组</param>
                /// <param name="func">比对输入成员和原成员是否相等（成员，输入成员，是否相等）</param>
                /// <param name="convert">将输入成员转换为成员类型(输入成员，成员)</param>
                /// <returns></returns>
                public static T[] Join<T, T1>(this T[] array, T1[] two, Func<T, T1, bool> func, Converter<T1, T> convert)
                {
                        if (array == null || array.Length == 0)
                        {
                                return two.ConvertAll(convert);
                        }
                        List<T> list = new List<T>(array);
                        foreach (T1 k in two)
                        {
                                if (array.FindIndex(a => func(a, k)) == -1)
                                {
                                        list.Add(convert(k));
                                }
                        }
                        return list.ToArray();
                }

                public static string[] Join<T, T1>(this T[] array, string separator, T1[] two, Func<T, T1, bool> func, Func<T1, string> convert)
                {
                        return array.ConvertAll(k =>
                         {
                                 T1[] t = two.FindAll(a => func(k, a));
                                 if (t.Length > 0)
                                 {
                                         return t.Join(separator, convert);
                                 }
                                 return string.Empty;
                         });
                }
                /// <summary>
                /// 向数组追加成员并去重
                /// </summary>
                /// <typeparam name="T">成员</typeparam>
                /// <param name="array">成员数组</param>
                /// <param name="two">追加的输入成员</param>
                /// <param name="func">比对输入成员和成员是否相等（成员，输入成员，是否相等）</param>
                /// <param name="convert">将输入成员转换为成员类型(输入成员，成员)</param>
                /// <returns></returns>
                public static T[] Join<T>(this T[] array, T[] two, Func<T, T, bool> func)
                {
                        if (array == null || array.Length == 0)
                        {
                                return two;
                        }
                        List<T> list = new List<T>(array);
                        foreach (T k in two)
                        {
                                if (list.FindIndex(a => func(a, k)) == -1)
                                {
                                        list.Add(k);
                                }
                        }
                        return list.ToArray();
                }
                /// <summary>
                /// 使用指定值初始化数组
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="array"></param>
                /// <param name="def"></param>
                public static void Initialize<T>(this T[] array, T def)
                {
                        for (int i = 0; i < array.Length; i++)
                        {
                                array[i] = def;
                        }
                }
                /// <summary>
                /// 搜索数组成员返回数组索引
                /// </summary>
                /// <typeparam name="T">成员</typeparam>
                /// <param name="array">数组</param>
                /// <param name="match">搜索的方法(成员)</param>
                /// <returns>数组索引(-1 无)</returns>
                public static int FindIndex<T>(this T[] array, Func<T, bool> match)
                {
                        if (array == null)
                        {
                                return -1;
                        }
                        for (int i = 0; i < array.Length; i++)
                        {
                                if (match(array[i]))
                                {
                                        return i;
                                }
                        }
                        return -1;
                }
                /// <summary>
                /// 从开始索引处搜索数组成员返回数组索引
                /// </summary>
                /// <typeparam name="T">成员</typeparam>
                /// <param name="array">数组</param>
                /// <param name="begin">起始索引</param>
                /// <param name="match">搜索的方法(成员)</param>
                /// <returns>数组索引(-1 无)</returns>
                public static int FindIndex<T>(this T[] array, int begin, Predicate<T> match)
                {
                        return array == null ? -1 : Array.FindIndex(array, begin, match);
                }
                /// <summary>
                /// 倒序搜索数组成员返回数组索引
                /// </summary>
                /// <typeparam name="T">成员</typeparam>
                /// <param name="array">数组</param>
                /// <param name="match">搜索的方法(成员)</param>
                /// <returns>数组索引(-1 无)</returns>
                public static int FindLastIndex<T>(this T[] array, Predicate<T> match)
                {
                        return Array.FindLastIndex(array, match);
                }
                /// <summary>
                /// 倒序搜索数组成员返回数组索引
                /// </summary>
                /// <typeparam name="T">成员</typeparam>
                /// <param name="array">数组</param>
                /// <param name="match">搜索的方法(成员)</param>
                /// <returns>数组索引(-1 无)</returns>
                public static int FindLastIndex<T>(this T[] array, int begin, int end, Predicate<T> match)
                {
                        for (int i = begin; i >= end; i--)
                        {
                                if (match(array[i]))
                                {
                                        return i;
                                }
                        }
                        return -1;
                }
                /// <summary>
                /// 数组去重并转换类型
                /// </summary>
                /// <typeparam name="T">成员</typeparam>
                /// <typeparam name="Out">输出成员</typeparam>
                /// <param name="array">成员数组</param>
                /// <param name="fun">格式转换(成员，输出成员)</param>
                /// <returns>输出成员数组</returns>
                public static Out[] Distinct<T, Out>(this T[] array, Func<T, Out> fun)
                {
                        return array.Select(fun).Distinct().ToArray();
                }
                /// <summary>
                /// 数组去重
                /// </summary>
                /// <typeparam name="T">成员</typeparam>
                /// <param name="array">成员数组</param>
                /// <returns>输出成员数组</returns>
                public static string[] Distinct(this string[] array)
                {
                        return array.Distinct<string>().ToArray();
                }
                /// <summary>
                /// 数组去重
                /// </summary>
                /// <param name="array">成员数组</param>
                /// <returns>输出成员数组</returns>
                public static int[] Distinct(int[] array)
                {
                        return array.Distinct<int>().ToArray();
                }
                /// <summary>
                /// 数组去重
                /// </summary>
                /// <param name="array">成员数组</param>
                /// <returns>输出成员数组</returns>
                public static long[] Distinct(long[] array)
                {
                        return array.Distinct<long>().ToArray();
                }
                private class _Equality<T> : IEqualityComparer<T>
                {
                        private readonly Func<T, T, bool> _Func = null;
                        public _Equality(Func<T, T, bool> func)
                        {
                                this._Func = func;
                        }
                        public bool Equals(T x, T y)
                        {
                                return this._Func(x, y);
                        }

                        public int GetHashCode(T obj)
                        {
                                return obj.GetHashCode();
                        }
                }
                /// <summary>
                /// 对象数组去重
                /// </summary>
                /// <typeparam name="T">成员</typeparam>
                /// <param name="array">成员数组</param>
                /// <param name="match">比对成员是否相同</param>
                /// <returns></returns>
                public static T[] Distinct<T>(this T[] array, Func<T, T, bool> match) where T : class
                {
                        return array.Distinct(new _Equality<T>(match)).ToArray();
                }
                /// <summary>
                /// 数组去重并转换类型
                /// </summary>
                /// <typeparam name="T">成员</typeparam>
                /// <typeparam name="Out">输出成员</typeparam>
                /// <param name="array">成员数组</param>
                /// <param name="match">比对成员是否相同</param>
                /// <param name="converter">格式转换(成员，输出成员)</param>
                /// <returns></returns>
                public static Out[] Distinct<T, Out>(this T[] array, Func<T, T, bool> match, Func<T, Out> converter)
                {
                        return array.Distinct(new _Equality<T>(match)).Select(converter).ToArray();
                }

                public static Out[] Distinct<T, Out>(this T[] array, Func<T, bool> func, Func<T, Out> converter, Func<T, T, bool> match) where T : class
                {
                        return array.Where(func).Distinct(new _Equality<T>(match)).Select(converter).ToArray();
                }
                public static Out[] Distinct<T, T1, Out>(this T[] array, Converter<T, T1> fun, Func<T, T1, Out> converter)
                {
                        List<T1> list = new List<T1>(array.Length);
                        List<Out> outs = new List<Out>(array.Length);
                        foreach (T i in array)
                        {
                                T1 t = fun(i);
                                if (t != null && !list.Contains(t))
                                {
                                        list.Add(t);
                                        outs.Add(converter(i, t));
                                }
                        }
                        return outs.ToArray();
                }

                /// <summary>
                /// 数组检查去重并转换类型
                /// </summary>
                /// <typeparam name="T">成员</typeparam>
                /// <typeparam name="Out">输出成员</typeparam>
                /// <param name="array">成员数组</param>
                /// <param name="match">检查成员</param>
                /// <param name="fun">格式转换(成员，输出成员)</param>
                /// <returns>输出成员数组</returns>
                public static Out[] Distinct<T, Out>(this T[] array, Func<T, bool> match, Func<T, Out> fun) where T : class
                {
                        return array.Where(match).Select(fun).Distinct().ToArray();
                }
                /// <summary>
                /// 数组检查去重并转换类型
                /// </summary>
                /// <typeparam name="T">成员</typeparam>
                /// <typeparam name="Out">输出成员</typeparam>
                /// <param name="array">成员数组</param>
                /// <param name="match">检查成员</param>
                /// <param name="fun">格式转换(成员，输出成员)</param>
                /// <returns>输出成员数组</returns>
                public static string[] Distinct<T>(this T[] array, Func<T, bool> match, Func<T, string> fun) where T : class
                {
                        return array.Where(match).Select(fun).Distinct().ToArray();
                }
                /// <summary>
                /// 复制指定长度数组
                /// </summary>
                /// <typeparam name="T">成员</typeparam>
                /// <param name="array">成员数组</param>
                /// <param name="index">开始索引</param>
                /// <param name="len">新数组长度</param>
                /// <returns>新数组</returns>
                public static T[] Copy<T>(this T[] array, int index, int len)
                {
                        T[] list = new T[len];
                        Array.Copy(array, index, list, 0, len);
                        return list;
                }
                /// <summary>
                /// 将指定范围数组连接成字符串
                /// </summary>
                /// <typeparam name="T">成员</typeparam>
                /// <param name="array">成员数组</param>
                /// <param name="begin">开始索引</param>
                /// <param name="end">结束索引</param>
                /// <returns></returns>
                public static string Join<T>(this T[] array, int begin, int end)
                {
                        StringBuilder str = new StringBuilder();
                        for (int i = begin; i < end; i++)
                        {
                                str.Append(array[i]);
                        }
                        return str.ToString();
                }
                /// <summary>
                /// 将指定范围数组连接成字符串
                /// </summary>
                /// <typeparam name="T">成员</typeparam>
                /// <param name="array">成员数组</param>
                /// <param name="begin">开始索引</param>
                /// <param name="end">结束索引</param>
                /// <param name="func">格式转换（成员）</param>
                /// <returns></returns>
                public static string Join<T>(this T[] array, int begin, int end, Func<T, string> func)
                {
                        StringBuilder str = new StringBuilder();
                        for (int i = begin; i < end; i++)
                        {
                                str.Append(func(array[i]));
                        }
                        return str.ToString();
                }
                /// <summary>
                /// List转数组并转换类型
                /// </summary>
                /// <typeparam name="T">成员</typeparam>
                /// <typeparam name="Result">新成员</typeparam>
                /// <param name="array">泛型集合</param>
                /// <param name="func">数据转换方法(成员,新成员)</param>
                /// <returns>新成员数组</returns>
                public static Result[] ToArray<T, Result>(this List<T> array, Func<T, Result> func)
                {
                        Result[] list = new Result[array.Count];
                        for (int i = 0; i < array.Count; i++)
                        {
                                list[i] = func(array[i]);
                        }
                        return list;
                }


                /// <summary>
                /// List转数组并转换类型
                /// </summary>
                /// <typeparam name="T">成员</typeparam>
                /// <typeparam name="Result">新成员</typeparam>
                /// <param name="array">泛型集合</param>
                /// <param name="func">数据转换方法(成员,新成员)</param>
                /// <returns>新成员数组</returns>
                public static T[] ToArray<T>(this List<T> array, int index)
                {
                        T[] list = new T[array.Count - index];
                        for (int i = index, k = 0; i < array.Count; i++, k++)
                        {
                                list[k] = array[i];
                        }
                        return list;
                }
                /// <summary>
                /// List转数组并转换类型
                /// </summary>
                /// <typeparam name="T">成员</typeparam>
                /// <typeparam name="Result">新成员</typeparam>
                /// <param name="array">泛型集合</param>
                /// <param name="func">数据转换方法(成员,泛型索引位,新成员)</param>
                /// <returns>新成员数组</returns>
                public static Result[] ToArray<T, Result>(this List<T> array, Func<T, int, Result> func)
                {
                        Result[] list = new Result[array.Count];
                        for (int i = 0; i < array.Count; i++)
                        {
                                list[i] = func(array[i], i);
                        }
                        return list;
                }
                /// <summary>
                /// 数组移除成员
                /// </summary>
                /// <typeparam name="T">成员</typeparam>
                /// <param name="array">数组</param>
                /// <param name="func">检查是否移除(成员，是否移除)</param>
                /// <returns></returns>
                public static T[] Remove<T>(this T[] array, Func<T, bool> func)
                {
                        List<T> list = new List<T>(array.Length);
                        foreach (T a in array)
                        {
                                if (!func(a))
                                {
                                        list.Add(a);
                                }
                        }
                        return list.ToArray();
                }
                public static T[] RemoveOne<T>(this T[] array, Func<T, bool> func)
                {
                        int index = array.FindIndex(func);
                        return index == -1 ? array : array.Remove(index);
                }
                /// <summary>
                /// 数组移除成员
                /// </summary>
                /// <typeparam name="T">成员</typeparam>
                /// <param name="array">数组</param>
                /// <param name="func">检查是否移除(成员，是否移除)</param>
                /// <returns></returns>
                public static T[] Remove<T>(this T[] array, int index)
                {
                        if (array.Length == 1)
                        {
                                return new T[0];
                        }
                        T[] data = new T[array.Length - 1];
                        if (index == 0)
                        {
                                Array.Copy(array, 1, data, 0, data.Length);
                        }
                        else if (index == data.Length)
                        {
                                Array.Copy(array, 0, data, 0, index);
                        }
                        else
                        {
                                Array.Copy(array, 0, data, 0, index);
                                int len = data.Length - index;
                                Array.Copy(array, index + 1, data, index, len);
                        }
                        return data;
                }
                /// <summary>
                /// 数组移除成员并添加成员
                /// </summary>
                /// <typeparam name="T">成员</typeparam>
                /// <param name="array">数组</param>
                /// <param name="func">检查是否移除(成员，是否移除)</param>
                /// <param name="adds">添加的成员</param>
                /// <returns></returns>
                public static T[] Remove<T>(this T[] array, Func<T, bool> func, T[] adds)
                {
                        List<T> list = new List<T>(adds);
                        foreach (T a in array)
                        {
                                if (!func(a))
                                {
                                        list.Add(a);
                                }
                        }
                        return list.ToArray();
                }
                /// <summary>
                /// 数组移除成员
                /// </summary>
                /// <typeparam name="T">成员</typeparam>
                /// <param name="data">移除的成员</param>
                /// <param name="array">数组</param>
                /// <returns></returns>
                public static T[] Remove<T>(this T[] array, T data)
                {
                        List<T> list = new List<T>(array.Length);
                        foreach (T a in array)
                        {
                                if (!a.Equals(data))
                                {
                                        list.Add(a);
                                }
                        }
                        return list.ToArray();
                }
                private static bool _CheckIsRemove<T, T1>(T i, T1[] remove, Func<T, T1, bool> func)
                {
                        foreach (T1 k in remove)
                        {
                                if (func(i, k))
                                {
                                        return true;
                                }
                        }
                        return false;
                }
                private static bool _CheckIsRemove<T, T1>(T i, List<T1> remove, Func<T, T1, bool> func)
                {
                        foreach (T1 k in remove)
                        {
                                if (func(i, k))
                                {
                                        return true;
                                }
                        }
                        return false;
                }
                /// <summary>
                /// 
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <typeparam name="T1"></typeparam>
                /// <param name="array"></param>
                /// <param name="remove"></param>
                /// <param name="func"></param>
                /// <returns></returns>
                public static T[] Remove<T, T1>(this T[] array, List<T1> remove, Func<T, T1, bool> func)
                {
                        List<T> list = new List<T>(array.Length);
                        foreach (T i in array)
                        {
                                if (!_CheckIsRemove(i, remove, func))
                                {
                                        list.Add(i);
                                }
                        }
                        return list.ToArray();
                }
                public static T[] Remove<T, T1>(this T[] array, T1[] remove, Func<T, T1, bool> func)
                {
                        List<T> list = new List<T>(array.Length);
                        foreach (T i in array)
                        {
                                if (!_CheckIsRemove(i, remove, func))
                                {
                                        list.Add(i);
                                }
                        }
                        return list.ToArray();
                }
                /// <summary>
                /// 数组排序
                /// </summary>
                /// <typeparam name="T">成员</typeparam>
                /// <param name="array">成员数组</param>
                /// <param name="func">比对成员大小(当前成员，下一个成员，)</param>
                public static void Sort<T>(this T[] array, Func<T, T, bool> func)
                {
                        for (int i = 1; i < array.Length; i++)
                        {
                                int j = i - 1;
                                if (func(array[j], array[i]))
                                {
                                        T def = array[i];
                                        array[i] = array[j];
                                        array[j] = def;
                                }
                        }
                }
                /// <summary>
                /// 移除成员
                /// </summary>
                /// <typeparam name="T">成员</typeparam>
                /// <param name="array">数组</param>
                /// <param name="remove">移除的成员</param>
                /// <returns>移除后的成员数组</returns>
                public static T[] Remove<T>(this T[] array, T[] remove)
                {
                        List<T> news = new List<T>(array.Length);
                        foreach (T a in array)
                        {
                                if (!remove.IsExists(a))
                                {
                                        news.Add(a);
                                }
                        }
                        return news.ToArray();
                }
                /// <summary>
                /// 移除筛选成员不存在的数组成员并追加新成员
                /// </summary>
                /// <typeparam name="T">成员</typeparam>
                /// <param name="array">成员数组</param>
                /// <param name="filter">筛选数组</param>
                /// <param name="adds">追加的成员</param>
                /// <returns>新成员数组</returns>
                public static T[] Remove<T>(this T[] array, T[] filter, T[] adds)
                {
                        List<T> list = new List<T>(array.Length + adds.Length);
                        Array.ForEach(array, a =>
                        {
                                if (Array.FindIndex(filter, b => b.Equals(a)) == -1)
                                {
                                        list.Add(a);
                                }
                        });
                        list.AddRange(adds);
                        return list.ToArray();
                }

                /// <summary>
                /// 移除筛选成员不存在的数组成员并追加新成员
                /// </summary>
                /// <typeparam name="T">成员</typeparam>
                /// <param name="array">成员数组</param>
                /// <param name="filter">筛选的数组</param>
                /// <param name="match">比对成员和筛选成员是否相同(原成员，筛选的成员，是否相同)</param>
                /// <param name="adds">追加的成员</param>
                /// <returns>新成员数组</returns>
                public static T[] Remove<T>(this T[] array, T[] filter, Func<T, T, bool> match, T[] adds)
                {
                        List<T> list = new List<T>(array.Length + adds.Length);
                        Array.ForEach(array, a =>
                        {
                                if (Array.FindIndex(filter, b => match(a, b)) == -1)
                                {
                                        list.Add(a);
                                }
                        });
                        list.AddRange(adds);
                        return list.ToArray();
                }
                /// <summary>
                /// 移除成员
                /// </summary>
                /// <typeparam name="T">成员</typeparam>
                /// <param name="array">成员数组</param>
                /// <param name="filter">筛选的数组</param>
                /// <param name="match">比对成员和筛选成员是否相同(原成员，筛选的成员，是否相同)</param>
                /// <returns>新成员数组</returns>
                public static T[] Remove<T>(this T[] array, T[] filter, Func<T, T, bool> match)
                {
                        List<T> list = new List<T>(array.Length);
                        foreach (T a in array)
                        {
                                if (filter.FindIndex(b => match(a, b)) == -1)
                                {
                                        list.Add(a);
                                }
                        }
                        return list.ToArray();
                }

                /// <summary>
                /// 替换数组中的成员
                /// </summary>
                /// <typeparam name="T">成员</typeparam>
                /// <param name="array">成员数组</param>
                /// <param name="filter">替换的数组</param>
                /// <param name="match">比对成员和替换成员是否相同(原成员，替换的成员，是否相同)</param>
                /// <returns>新成员数组</returns>
                public static T[] Replace<T>(this T[] array, T[] filter, Func<T, T, bool> match)
                {
                        List<T> list = new List<T>(array.Length);
                        for (int i = 0; i < array.Length; i++)
                        {
                                T source = array[i];
                                T replace = Array.Find(filter, b => match(source, b));
                                if (replace != null)
                                {
                                        array[i] = replace;
                                }
                        }
                        return array;
                }
                /// <summary>
                /// 将Array类型转换为数组
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="array"></param>
                /// <returns></returns>
                public static T[] ToArray<T>(this Array array)
                {
                        T[] list = new T[array.Length];
                        for (int i = 0; i < array.Length; i++)
                        {
                                list[i] = (T)array.GetValue(i);
                        }
                        return list;
                }
                /// <summary>
                /// 将Array类型转换为数组
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="array"></param>
                /// <returns></returns>
                public static Out[] ConvertAll<T, Out>(this Array array, Converter<T, Out> converter)
                {
                        Out[] list = new Out[array.Length];
                        for (int i = 0; i < array.Length; i++)
                        {
                                list[i] = converter((T)array.GetValue(i));
                        }
                        return list;
                }
                public static Out[] ConvertAll<Out>(this Array array, Converter<object, Out> converter)
                {
                        Out[] list = new Out[array.Length];
                        for (int i = 0; i < array.Length; i++)
                        {
                                list[i] = converter(array.GetValue(i));
                        }
                        return list;
                }
                /// <summary>
                /// 转换数组格式
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <typeparam name="Out"></typeparam>
                /// <param name="array"></param>
                /// <param name="converter"></param>
                /// <returns></returns>
                public static Out[] ConvertAll<T, Out>(this T[] array, Converter<T, Out> converter)
                {
                        if (array == null)
                        {
                                return null;
                        }
                        Out[] list = new Out[array.Length];
                        for (int i = 0; i < array.Length; i++)
                        {
                                list[i] = converter(array[i]);
                        }
                        return list;
                }

                /// <summary>
                /// 计数
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="array"></param>
                /// <param name="func"></param>
                /// <returns></returns>
                public static int Count<T>(this T[] array, Func<T, bool> func)
                {
                        int num = 0;
                        foreach (T i in array)
                        {
                                if (func(i))
                                {
                                        num++;
                                }
                        }
                        return num;
                }
                /// <summary>
                /// 转换数组格式
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <typeparam name="Out"></typeparam>
                /// <param name="array"></param>
                /// <param name="converter"></param>
                /// <returns></returns>
                public static Out[] Convert<T, T1, Out>(this T[] array, T1[] datas, Func<T, T1, bool> match, Func<T, T1[], Out> converter)
                {
                        if (array == null)
                        {
                                return null;
                        }
                        Out[] list = new Out[array.Length];
                        for (int i = 0; i < array.Length; i++)
                        {
                                T t = array[i];
                                T1[] k = datas.FindAll(b => match(t, b));
                                list[i] = converter(t, k);
                        }
                        return list;
                }

                /// <summary>
                /// 合并数组
                /// </summary>
                /// <typeparam name="T">数组成员</typeparam>
                /// <param name="array">原数组</param>
                /// <param name="datas">合并的数组</param>
                /// <param name="match">原匹配</param>
                /// <param name="converter"></param>
                /// <returns></returns>
                public static T[] Merge<T>(this T[] array, T[] datas, Func<T, T, bool> match, Func<T, T, T> converter)
                {
                        if (array == null)
                        {
                                return null;
                        }
                        T[] list = new T[array.Length];
                        for (int i = 0; i < array.Length; i++)
                        {
                                T t = array[i];
                                T k = datas.Find(b => match(t, b));
                                list[i] = converter(t, k);
                        }
                        return list;
                }
                public static Out[] Merge<T, Out>(this T[] array, T[] datas, Func<T, T, bool> match, Func<T, T, Out> converter)
                {
                        if (array == null)
                        {
                                return null;
                        }
                        Out[] list = new Out[array.Length];
                        for (int i = 0; i < array.Length; i++)
                        {
                                T t = array[i];
                                T k = datas.Find(b => match(t, b));
                                list[i] = converter(t, k);
                        }
                        return list;
                }
                public static Out[] Merge<T, T1, Out>(this T[] array, T1[] datas, Func<T, T1, bool> match, Func<T, T1, Out> converter)
                {
                        if (array == null)
                        {
                                return null;
                        }
                        Out[] list = new Out[array.Length];
                        for (int i = 0; i < array.Length; i++)
                        {
                                T t = array[i];
                                T1 k = datas.Find(b => match(t, b));
                                list[i] = converter(t, k);
                        }
                        return list;
                }
                public static Out[] GroupBy<T, Out>(this T[] array, Func<T, T[], Out> func)
                {
                        if (array == null || array.Length == 0)
                        {
                                return new Out[0];
                        }
                        else if (array.Length == 1)
                        {
                                return new Out[] { func(array[0], array) };
                        }
                        else
                        {
                                List<T> keys = new List<T>(array.Length);
                                List<Out> outs = new List<Out>(array.Length);
                                foreach (T i in array)
                                {
                                        if (Array.FindIndex(array, a => a.Equals(i)) == -1)
                                        {
                                                keys.Add(i);
                                                T[] ts = Array.FindAll(array, a => i.Equals(a));
                                                outs.Add(func(i, ts));
                                        }
                                }
                                return outs.ToArray();
                        }
                }
                public static void GroupBy<T, Result>(this IEnumerable<T> datas, Func<T, Result> func, Func<T, Result, bool> match, Action<Result, T[]> action) where T : class
                {
                        if (datas == null)
                        {
                                return;
                        }
                        else
                        {
                                T[] array = datas.ToArray();
                                Result[] res = datas.Select(func).Distinct().ToArray();
                                if (res.Length == 1)
                                {
                                        action(res[0], array);
                                        return;
                                }
                                foreach (Result i in res)
                                {
                                        action(i, array.FindAll(a => match(a, i)));
                                }
                        }
                }
                public static Result[] GroupBy<T, T1, Result>(this T[] array, Func<T, T1> func, Func<T, T1, bool> match, Func<T1, T[], Result> action) where T : class
                {
                        if (array == null || array.Length == 0)
                        {
                                return null;
                        }
                        else if (array.Length == 1)
                        {
                                return new Result[] { action(func(array[0]), array) };
                        }
                        else
                        {
                                T1[] res = array.Select(func).Distinct().ToArray();
                                return res.Length == 1
                                        ? (new Result[] { action(func(array[0]), array) })
                                        : res.ConvertAll(i =>
                                {
                                        return action(i, array.FindAll(a => match(a, i)));
                                });
                        }
                }
                public static void GroupBy<T, Result>(this T[] array, Func<T, Result> func, Func<T, Result, bool> match, Action<Result, T[]> action) where T : class
                {
                        if (array == null || array.Length == 0)
                        {
                                return;
                        }
                        else if (array.Length == 1)
                        {
                                action(func(array[0]), array);
                        }
                        else
                        {
                                Result[] res = array.Select(func).Distinct().ToArray();
                                if (res.Length == 1)
                                {
                                        action(res[0], array);
                                        return;
                                }
                                foreach (Result i in res)
                                {
                                        T[] datas = array.FindAll(a => match(a, i));
                                        action(i, datas);
                                }
                        }
                }
                public static Out[] GroupBy<T, T1, Out>(this T[] array, Converter<T, T1> converter, Func<T1, T[], Out> func)
                {
                        if (array == null || array.Length == 0)
                        {
                                return new Out[0];
                        }
                        else if (array.Length == 1)
                        {
                                return new Out[] { func(converter(array[0]), array) };
                        }
                        else
                        {
                                List<T1> keys = new List<T1>(array.Length);
                                List<Out> outs = new List<Out>(array.Length);
                                foreach (T i in array)
                                {
                                        T1 k = converter(i);
                                        if (!keys.Contains(k))
                                        {
                                                keys.Add(k);
                                                T[] ts = Array.FindAll(array, a => converter(a).Equals(k));
                                                outs.Add(func(k, ts));
                                        }
                                }
                                return outs.ToArray();
                        }
                }
                public static Out[] GroupBy<T, Out>(this T[] array, Func<T, T, bool> match, Func<T, T[], Out> func)
                {
                        if (array == null || array.Length == 0)
                        {
                                return new Out[0];
                        }
                        else if (array.Length == 1)
                        {
                                return new Out[] { func(array[0], array) };
                        }
                        else
                        {
                                List<T> keys = new List<T>(array.Length);
                                List<Out> outs = new List<Out>(array.Length);
                                foreach (T i in array)
                                {
                                        if (keys.FindIndex(a => match(i, a)) == -1)
                                        {
                                                keys.Add(i);
                                                T[] ts = Array.FindAll(array, a => match(i, a));
                                                outs.Add(func(i, ts));
                                        }
                                }
                                return outs.ToArray();
                        }
                }
                public static Out[] GroupBy<T, Out>(this List<T> array, Func<T, T, bool> match, Func<T, List<T>, Out> func)
                {
                        if (array == null || array.Count == 0)
                        {
                                return new Out[0];
                        }
                        else if (array.Count == 1)
                        {
                                return new Out[] { func(array[0], array) };
                        }
                        else
                        {
                                List<T> keys = new List<T>(array.Count);
                                List<Out> outs = new List<Out>(array.Count);
                                foreach (T i in array)
                                {
                                        if (keys.FindIndex(a => match(i, a)) == -1)
                                        {
                                                keys.Add(i);
                                                List<T> ts = array.FindAll(a => match(i, a));
                                                outs.Add(func(i, ts));
                                        }
                                }
                                return outs.ToArray();
                        }
                }
                public static int Count(this string[] array, string[] datas)
                {
                        int num = 0;
                        for (int i = 0; i < array.Length; i++)
                        {
                                if (array[i] == datas[i])
                                {
                                        ++num;
                                }
                        }
                        return num;
                }
                /// <summary>
                /// 转换数组格式并过滤掉默认值成员（去空）
                /// </summary>
                /// <typeparam name="T">原成员</typeparam>
                /// <typeparam name="Out">输出成员</typeparam>
                /// <param name="array">原成员数组</param>
                /// <param name="converter">原成员格式转换(原成员，输出成员)</param>
                /// <returns>无输出成员默认值的新数组（去空）</returns>
                public static Out[] Convert<T, Out>(this T[] array, Converter<T, Out> converter)
                {
                        Out[] list = new Out[array.Length];
                        int i = 0;
                        Out def = default;
                        foreach (T a in array)
                        {
                                Out obj = converter(a);
                                if (obj != null && !obj.Equals(def))
                                {
                                        list[i++] = obj;
                                }
                        }
                        if (list.Length == i)
                        {
                                return list;
                        }
                        Out[] t = new Out[i];
                        Array.Copy(list, t, i);
                        return t;
                }
                /// <summary>
                /// 转换数组格式并过滤掉指定成员
                /// </summary>
                /// <typeparam name="T">原成员</typeparam>
                /// <typeparam name="Out">输出成员</typeparam>
                /// <param name="array">原成员数组</param>
                /// <param name="find">检查成员是否满足条件(原成员)</param>
                /// <param name="converter">原成员格式转换(原成员，输出成员)</param>
                /// <returns></returns>
                public static Out[] Convert<T, Out>(this T[] array, Predicate<T> find, Converter<T, Out> converter)
                {
                        Out[] list = new Out[array.Length];
                        int i = 0;
                        foreach (T a in array)
                        {
                                if (find(a))
                                {
                                        list[i++] = converter(a);
                                }
                        }
                        if (list.Length == i)
                        {
                                return list;
                        }
                        Out[] t = new Out[i];
                        Array.Copy(list, t, i);
                        return t;
                }
                public static Out[] Convert<T, Out>(this T[] array, Predicate<T> find, Func<T, int, Out> converter)
                {
                        Out[] list = new Out[array.Length];
                        int i = 0;
                        int k = 0;
                        foreach (T a in array)
                        {
                                if (find(a))
                                {
                                        list[i++] = converter(a, k);
                                }
                                k++;
                        }
                        if (list.Length == i)
                        {
                                return list;
                        }
                        Out[] t = new Out[i];
                        Array.Copy(list, t, i);
                        return t;
                }

                public static int[] ConvertIndex<T>(this T[] array, Predicate<T> find)
                {
                        int[] list = new int[array.Length];
                        int i = 0;
                        for (int k = 0; k < array.Length; k++)
                        {
                                if (find(array[k]))
                                {
                                        list[i++] = k;
                                }
                        }
                        if (list.Length == i)
                        {
                                return list;
                        }
                        int[] t = new int[i];
                        Array.Copy(list, t, i);
                        return t;
                }

                /// <summary>
                /// 递归查询
                /// </summary>
                /// <typeparam name="T">原成员</typeparam>
                /// <typeparam name="Out">输出成员</typeparam>
                /// <param name="array">原成员数组</param>
                /// <param name="match">筛选数据(成员，是否相等）</param>
                /// <param name="converter">筛选结果数据装换(查询的成员，原成员数组，输出成员)</param>
                /// <returns></returns>
                public static Out[] Recursion<T, Out>(this T[] array, Predicate<T> match, Func<T, T[], Out> converter) where T : class
                {
                        List<Out> list = new List<Out>(array.Length);
                        foreach (T i in array)
                        {
                                if (match(i))
                                {
                                        list.Add(converter(i, array));
                                }
                        }
                        return list.ToArray();
                }
                /// <summary>
                /// 递归循环
                /// </summary>
                /// <typeparam name="T">成员</typeparam>
                /// <typeparam name="Out">输出成员</typeparam>
                /// <param name="array">成员数组</param>
                /// <param name="match">筛选成员</param>
                /// <param name="func">检查是否是子级成员(父级成员,当前成员,是否是子级)</param>
                /// <param name="converter">装换成员类型</param>
                /// <returns></returns>
                public static Out[] Recursion<T, Out>(this T[] array, Predicate<T> match, Func<T, T, bool> func, Func<T, Out> converter) where T : class
                {
                        List<Out> outs = new List<Out>(array.Length);
                        foreach (T i in array)
                        {
                                if (match(i))
                                {
                                        outs.Add(converter(i));
                                        _Recursion(array, i, func, converter, outs);
                                }
                        }
                        return outs.ToArray();
                }
                private static void _Recursion<T, Out>(T[] array, T source, Func<T, T, bool> match, Func<T, Out> converter, List<Out> outs) where T : class
                {
                        foreach (T i in array)
                        {
                                if (match(source, i))
                                {
                                        outs.Add(converter(i));
                                        _Recursion(array, i, match, converter, outs);
                                }
                        }
                }
                /// <summary>
                /// 转换数组格式并过滤掉指定值
                /// </summary>
                /// <typeparam name="T">原成员</typeparam>
                /// <typeparam name="Out">输出成员</typeparam>
                /// <param name="array">原成员数组</param>
                /// <param name="converter">原成员格式转换(原成员，输出成员)</param>
                /// <param name="def">过滤掉的成员值</param>
                /// <returns>无输出成员默认值的新数组（去空）</returns>
                public static Out[] Convert<T, Out>(this T[] array, Converter<T, Out> converter, Out def)
                {
                        Out[] list = new Out[array.Length];
                        int i = 0;
                        Array.ForEach(array, a =>
                        {
                                Out obj = converter(a);
                                if (!obj.Equals(def))
                                {
                                        list[i++] = obj;
                                }
                        });
                        if (list.Length == i)
                        {
                                return list;
                        }
                        Out[] t = new Out[i];
                        Array.Copy(list, t, i);
                        return t;
                }
                /// <summary>
                /// 移除数组中的空成员
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="array"></param>
                /// <returns></returns>
                public static T[] NoEmpty<T>(this T[] array)
                {
                        return Array.FindAll(array, a => a != null);
                }
                /// <summary>
                /// 转换数组类型
                /// </summary>
                /// <typeparam name="T">成员</typeparam>
                /// <typeparam name="Out">输出成员</typeparam>
                /// <param name="array">成员数组</param>
                /// <param name="action">成员类型转换(成员，数组索引，输出成员)</param>
                /// <returns>输出成员数组</returns>
                public static Out[] ConvertAll<T, Out>(this T[] array, Func<T, int, Out> action)
                {
                        Out[] my = new Out[array.Length];
                        for (int i = 0; i < array.Length; i++)
                        {
                                my[i] = action(array[i], i);
                        }
                        return my;
                }
                public static Out[] ConvertAll<T, T1, Out>(this T[] array, T1[] datas, Func<T, T1, Out> action)
                {
                        Out[] t = new Out[array.Length * datas.Length];
                        int n = 0;
                        foreach (T i in array)
                        {
                                foreach (T1 k in datas)
                                {
                                        t[n++] = action(i, k);
                                }
                        }
                        return t;
                }
                /// <summary>
                /// 循环数组
                /// </summary>
                /// <typeparam name="T">成员</typeparam>
                /// <param name="array">原成员数组</param>
                /// <param name="action">循环方法(成员)</param>
                public static void ForEach<T>(this T[] array, Action<T> action)
                {
                        if (array.Length == 1)
                        {
                                action(array[0]);
                                return;
                        }
                        foreach (T i in array)
                        {
                                action(i);
                        }
                }
                /// <summary>
                /// 循环数组
                /// </summary>
                /// <typeparam name="T">成员</typeparam>
                /// <param name="array">原成员数组</param>
                /// <param name="action">循环方法(成员)</param>
                public static void ForEach<T, T1>(this T[] array, T1[] two, Action<T, T1> action)
                {
                        if (array.Length == 1 && two.Length == 1)
                        {
                                action(array[0], two[0]);
                                return;
                        }
                        else if (array.Length == 1)
                        {
                                T i = array[0];
                                foreach (T1 k in two)
                                {
                                        action(i, k);
                                }
                        }
                        else if (two.Length == 1)
                        {
                                T1 k = two[0];
                                foreach (T i in array)
                                {
                                        action(i, k);
                                }
                        }
                        else
                        {
                                foreach (T i in array)
                                {
                                        foreach (T1 k in two)
                                        {
                                                action(i, k);
                                        }
                                }
                        }
                }
                public static void ForEachByParallel<T>(this T[] array, Action<T> action)
                {
                        Parallel.ForEach(array, action);
                }
                /// <summary>
                /// 循环数组
                /// </summary>
                /// <typeparam name="T">成员</typeparam>
                /// <param name="match">检查成员</param>
                /// <param name="array">原成员数组</param>
                /// <param name="action">循环方法(成员)</param>
                public static void ForEach<T>(this T[] array, Predicate<T> match, Action<T> action)
                {
                        foreach (T i in array)
                        {
                                if (match(i))
                                {
                                        action(i);
                                }
                        }
                }
                /// <summary>
                /// 循环数组
                /// </summary>
                /// <typeparam name="T">成员</typeparam>
                /// <param name="array">原成员数组</param>
                /// <param name="action">循环方法(成员,数组索引)</param>
                public static void ForEach<T>(this T[] array, Action<T, int> action)
                {
                        for (int i = 0; i < array.Length; i++)
                        {
                                action(array[i], i);
                        }
                }
                /// <summary>
                /// 确定数组中的每个元素是否都与指定谓词定义的条件匹配
                /// </summary>
                /// <typeparam name="T">成员</typeparam>
                /// <param name="array">原成员数组</param>
                /// <param name="match">用于定义检查元素时要对照的条件的谓词。</param>
                /// <returns>如果 array 中的每个元素都与指定谓词定义的条件匹配，则为 true；否则为 false。 如果数组中没有元素，则返回值为 true。</returns>
                public static bool TrueForAll<T>(this T[] array, Predicate<T> match)
                {
                        return Array.TrueForAll(array, match);
                }
                /// <summary>
                /// 循环匹配指定成员
                /// </summary>
                /// <typeparam name="T">原成员</typeparam>
                /// <typeparam name="T1">匹配的成员</typeparam>
                /// <param name="array">原成员数组</param>
                /// <param name="datas">匹配的成员数组</param>
                /// <param name="match">匹配项</param>
                /// <param name="result">匹配结果</param>
                public static void ForEach<T, T1>(this T[] array, T1[] datas, Func<T, T1, bool> match, Action<T, T1> result)
                {
                        foreach (T i in array)
                        {
                                foreach (T1 k in datas)
                                {
                                        if (match(i, k))
                                        {
                                                result(i, k);
                                        }
                                }
                        }
                }
                /// <summary>
                /// 确定数组中的每个元素是否都与指定谓词定义的条件匹配
                /// </summary>
                /// <typeparam name="T">成员</typeparam>
                /// <param name="array">原成员数组</param>
                /// <param name="match">用于定义检查元素时要对照的条件的谓词。(成员，数组索引，是否匹配)</param>
                /// <returns>如果 array 中的每个元素都与指定谓词定义的条件匹配，则为 true；否则为 false。 如果数组中没有元素，则返回值为 true。</returns>
                public static bool TrueForAll<T>(this T[] array, Func<T, int, bool> match)
                {
                        for (int i = 0; i < array.Length; i++)
                        {
                                if (!match(array[i], i))
                                {
                                        return false;
                                }
                        }
                        return true;
                }
                /// <summary>
                /// 指定循环多少次并返回新的成员数组
                /// </summary>
                /// <typeparam name="T">成员</typeparam>
                /// <param name="num">次数</param>
                /// <param name="fun">生成成员的方法(当前循环次数,成员)</param>
                /// <returns>成员数组</returns>
                public static T[] For<T>(this int num, Func<int, T> fun)
                {
                        T[] list = new T[num];
                        for (int i = 0; i < num; i++)
                        {
                                list[i] = fun(i);
                        }
                        return list;
                }
                /// <summary>
                /// 指定循环范围并返回新的成员数组
                /// </summary>
                /// <typeparam name="T">成员</typeparam>
                /// <param name="begin">开始数</param>
                /// <param name="end">结束数</param>
                /// <param name="fun">生成成员的方法(当前循环次数,成员)</param>
                /// <returns>成员数组</returns>
                public static T[] For<T>(this int begin, int end, Func<int, T> fun)
                {
                        T[] list = new T[end - begin];
                        int k = 0;
                        for (int i = begin; i < end; i++)
                        {
                                list[k] = fun(i);
                        }
                        return list;
                }
                /// <summary>
                /// 循环指定次数
                /// </summary>
                /// <param name="num"></param>
                /// <param name="action"></param>
                public static void For(this int num, Action<int> action)
                {
                        for (int i = 0; i < num; i++)
                        {
                                action(i);
                        }
                }
                public static string ToMd5(this Stream stream)
                {
                        if (stream.Length <= (_Size * 10))
                        {
                                return Tools.GetMD5(stream.ToBytes());
                        }
                        byte[] buffer = new byte[_Size];
                        int page = (int)Math.Ceiling((decimal)stream.Length / _Size);
                        int end = page - 1;
                        using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
                        {
                                page.For(i =>
                                {
                                        int len = stream.Read(buffer, 0, buffer.Length);
                                        if (i == end)
                                        {
                                                md5.TransformFinalBlock(buffer, 0, len);
                                        }
                                        else
                                        {
                                                md5.TransformBlock(buffer, 0, len, buffer, 0);
                                        }
                                });
                                stream.Position = 0;
                                return BitConverter.ToString(md5.Hash).Replace("-", string.Empty);
                        }
                }
                public static byte[] ToBytes(this Stream stream)
                {
                        stream.Position = 0;
                        byte[] bytes = new byte[stream.Length];
                        stream.Read(bytes, 0, bytes.Length);
                        return bytes;
                }
                public static byte[] ToBytes(this Stream stream, int index, int take)
                {
                        stream.Position = index * take;
                        if ((stream.Position + take) > stream.Length)
                        {
                                take = (int)(stream.Length - stream.Position);
                        }
                        byte[] bytes = new byte[take];
                        stream.Read(bytes, 0, bytes.Length);
                        return bytes;
                }
                /// <summary>
                /// 循环指定次数
                /// </summary>
                /// <param name="num"></param>
                /// <param name="action"></param>
                public static void For(this short num, Action<short> action)
                {
                        if (num == 1)
                        {
                                action(0);
                                return;
                        }
                        for (short i = 0; i < num; i++)
                        {
                                action(i);
                        }
                }
                /// <summary>
                /// 从指定数开始循环到指定数结束
                /// </summary>
                /// <param name="end">结束值</param>
                /// <param name="begin">开始数</param>
                /// <param name="action">循环的方法(当前值)</param>
                public static void For(this int end, int begin, Action<int> action)
                {
                        for (int i = begin; i <= end; i++)
                        {
                                action(i);
                        }
                }
                /// <summary>
                /// 指定循环多少次并返回新的成员数组
                /// </summary>
                /// <typeparam name="T">成员</typeparam>
                /// <param name="num">次数</param>
                /// <param name="fun">生成成员的方法(当前循环次数,成员)</param>
                /// <returns>成员数组</returns>
                public static T[] For<T>(this short num, Func<short, T> fun)
                {
                        T[] list = new T[num];
                        for (short i = 0; i < num; i++)
                        {
                                list[i] = fun(i);
                        }
                        return list;
                }
                /// <summary>
                /// 确定数组中的每个元素是否都与指定谓词定义的条件匹配
                /// </summary>
                /// <typeparam name="T">成员</typeparam>
                /// <param name="array">原成员数组</param>
                /// <param name="match">用于定义检查元素时要对照的条件的谓词。</param>
                /// <param name="error">返回错误信息</param>
                /// <returns>如果 array 中的每个元素都与指定谓词定义的条件匹配，则为 true；否则为 false。 如果数组中没有元素，则返回值为 true。</returns>
                public static bool TrueForAll<T>(this T[] array, PredicateFun<T> match, out string error)
                {
                        string msg = null;
                        if (!Array.TrueForAll(array, a =>
                        {
                                return match(a, out msg);
                        }))
                        {
                                error = msg;
                                return false;
                        }
                        error = null;
                        return true;
                }
                /// <summary>
                /// 确定数组中的每个元素是否都与指定谓词定义的条件匹配
                /// </summary>
                /// <typeparam name="T">成员</typeparam>
                /// <param name="array">原成员数组</param>
                /// <param name="match">用于定义检查元素时要对照的条件的谓词。</param>
                /// <param name="error">返回错误信息</param>
                /// <returns>如果 array 中的每个元素都与指定谓词定义的条件匹配，则为 true；否则为 false。 如果数组中没有元素，则返回值为 true。</returns>
                public static bool TrueForAll<T>(this T[] array, ArrayTrue<T> match, out string error)
                {
                        for (int i = 0; i < array.Length; i++)
                        {
                                if (!match(array[i], i, out error))
                                {
                                        return false;
                                }
                        }
                        error = null;
                        return true;
                }
                /// <summary>
                /// 查询数组中满足条件的所有成员
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="array"></param>
                /// <param name="match"></param>
                /// <returns></returns>
                public static T[] FindAll<T>(this T[] array, Predicate<T> match)
                {
                        List<T> list = new List<T>();
                        foreach (T i in array)
                        {
                                if (match(i))
                                {
                                        list.Add(i);
                                }
                        }
                        return list.ToArray();
                }
                /// <summary>
                /// 将数组乱序
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="array"></param>
                /// <returns></returns>
                public static T[] Disorder<T>(this T[] array)
                {
                        _ArrayDis<T>[] list = array.ConvertAll(a => new _ArrayDis<T>
                        {
                                data = a,
                                sort = Guid.NewGuid().ToString()
                        });
                        return list.OrderBy(a => a.sort).Select(a => a.data).ToArray();
                }
                private struct _ArrayDis<T>
                {
                        public T data;
                        public string sort;
                }
                /// <summary>
                /// 查询数组中满足条件的所有成员
                /// </summary>
                /// <typeparam name="T">成员</typeparam>
                /// <typeparam name="T1">需要搜索的成员</typeparam>
                /// <param name="array">成员数组</param>
                /// <param name="datas">搜索数组</param>
                /// <param name="match">比对搜索成员和成员是否相同（成员，搜索成员，是否相同）</param>
                /// <returns></returns>
                public static T[] FindAll<T, T1>(this T[] array, T1[] datas, Func<T, T1, bool> match) where T : class
                {
                        List<T> list = new List<T>();
                        foreach (T i in array)
                        {
                                foreach (T1 k in datas)
                                {
                                        if (match(i, k))
                                        {
                                                list.Add(i);
                                                break;
                                        }
                                }
                        }
                        return list.ToArray();
                }
                public static int Sum<T, T1>(this T[] array, T1[] datas, Func<T, T1, bool> match, Func<T, T1, int> fun) where T : class
                {
                        int num = 0;
                        foreach (T i in array)
                        {
                                foreach (T1 k in datas)
                                {
                                        if (match(i, k))
                                        {
                                                num += fun(i, k);
                                                break;
                                        }
                                }
                        }
                        return num;
                }
                public static long Sum<T, T1>(this T[] array, T1[] datas, Func<T, T1, bool> match, Func<T, T1, long> fun) where T : class
                {
                        long num = 0;
                        foreach (T i in array)
                        {
                                foreach (T1 k in datas)
                                {
                                        if (match(i, k))
                                        {
                                                num += fun(i, k);
                                                break;
                                        }
                                }
                        }
                        return num;
                }
                public static int Sum<T>(this T[] array, Func<T, int> fun, int def) where T : class
                {
                        if (array == null || array.Length == 0)
                        {
                                return def;
                        }
                        int num = 0;
                        foreach (T i in array)
                        {
                                num += fun(i);
                        }
                        return num;
                }
                public static long Sum<T>(this T[] array, Func<T, long> fun, int def) where T : class
                {
                        if (array == null || array.Length == 0)
                        {
                                return def;
                        }
                        long num = 0;
                        foreach (T i in array)
                        {
                                num += fun(i);
                        }
                        return num;
                }

                /// <summary>
                /// 比对2个数组成员并将匹配成员转换格式
                /// </summary>
                /// <typeparam name="T">原成员</typeparam>
                /// <typeparam name="T1">匹配的成员</typeparam>
                /// <typeparam name="Result">转换结果</typeparam>
                /// <param name="array">原成员数组</param>
                /// <param name="datas">匹配搜索的成员</param>
                /// <param name="match">匹配的方法</param>
                /// <param name="converter">格式转换</param>
                /// <returns>转换的结果</returns>
                public static Result[] ConvertAll<T, T1, Result>(this T[] array, T1[] datas, Func<T, T1, bool> match, Func<T, T1, Result> converter)
                {
                        List<Result> list = new List<Result>();
                        foreach (T i in array)
                        {
                                foreach (T1 k in datas)
                                {
                                        if (match(i, k))
                                        {
                                                list.Add(converter(i, k));
                                                break;
                                        }
                                }
                        }
                        return list.ToArray();
                }
                /// <summary>
                /// 查询数组中满足条件的所有成员
                /// </summary>
                /// <typeparam name="T">成员</typeparam>
                /// <param name="array">成员数组</param>
                /// <param name="datas">搜索数组</param>
                /// <param name="match">比对搜索成员和成员是否相同（成员，搜索成员，是否相同）</param>
                /// <returns></returns>
                public static T[] FindAll<T>(this T[] array, T[] datas, Func<T, T, bool> match) where T : class
                {
                        List<T> list = new List<T>();
                        foreach (T i in array)
                        {
                                foreach (T k in datas)
                                {
                                        if (match(i, k))
                                        {
                                                list.Add(i);
                                                break;
                                        }
                                }
                        }
                        return list.ToArray();
                }
                /// <summary>
                /// 查询数组中满足条件的所有成员
                /// </summary>
                /// <typeparam name="T">成员</typeparam>
                /// <typeparam name="T1">需要搜索的成员</typeparam>
                /// <param name="array">成员数组</param>
                /// <param name="datas">搜索数组</param>
                /// <returns></returns>
                public static T[] FindAll<T, T1>(this T[] array, T1[] datas)
                {
                        List<T> list = new List<T>();
                        foreach (T i in array)
                        {
                                foreach (T1 k in datas)
                                {
                                        if (i.Equals(k))
                                        {
                                                list.Add(i);
                                                break;
                                        }
                                }
                        }
                        return list.ToArray();
                }/// <summary>
                 /// 查询数组中满足条件的所有成员
                 /// </summary>
                 /// <typeparam name="T">成员</typeparam>
                 /// <param name="array">成员数组</param>
                 /// <param name="datas">搜索数组</param>
                 /// <returns></returns>
                public static T[] FindAll<T>(this T[] array, T[] datas)
                {
                        List<T> list = new List<T>();
                        foreach (T i in array)
                        {
                                foreach (T k in datas)
                                {
                                        if (i.Equals(k))
                                        {
                                                list.Add(i);
                                                break;
                                        }
                                }
                        }
                        return list.ToArray();
                }
                /// <summary>
                /// 检查数组中是否存在满足条件成员
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="array"></param>
                /// <param name="match"></param>
                /// <returns></returns>
                public static bool IsExists<T>(this T[] array, Predicate<T> match)
                {
                        foreach (T i in array)
                        {
                                if (match(i))
                                {
                                        return true;
                                }
                        }
                        return false;
                }
                /// <summary>
                /// 检查数组中是否存在满足条件成员
                /// </summary>
                /// <typeparam name="T">成员</typeparam>
                /// <typeparam name="T1">比对成员</typeparam>
                /// <param name="array">成员数组(成员少的一方)</param>
                /// <param name="source">比对成员数组(多的一方)</param>
                /// <param name="match">检查是否相同(成员，比对成员，是否相同)</param>
                /// <returns></returns>
                public static bool IsExists<T, T1>(this T[] array, T1[] source, Func<T, T1, bool> match)
                {
                        foreach (T i in array)
                        {
                                if (Array.FindIndex(source, b => match(i, b)) == -1)
                                {
                                        return false;
                                }
                        }
                        return true;
                }
                /// <summary>
                /// 检查数组中是否存在满足条件成员
                /// </summary>
                /// <typeparam name="T">成员</typeparam>
                /// <param name="array">成员数组(成员少的一方)</param>
                /// <param name="source">比对成员数组(多的一方)</param>
                /// <param name="match">检查是否相同(成员，比对成员，是否相同)</param>
                /// <returns></returns>
                public static bool IsExists<T>(this T[] array, T[] source, Func<T, T, bool> match)
                {
                        foreach (T i in array)
                        {
                                if (!source.IsExists(i, match))
                                {
                                        return false;
                                }
                        }
                        return true;
                }
                /// <summary>
                /// 检查数组中是否存在指定值
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="array"></param>
                /// <param name="data"></param>
                /// <returns></returns>
                public static bool IsExists<T>(this T[] array, T data)
                {
                        foreach (T a in array)
                        {
                                if (a.Equals(data))
                                {
                                        return true;
                                }
                        }
                        return false;
                }
                public static bool StartsWith(this string[] array, string str)
                {
                        foreach (string a in array)
                        {
                                if (str.StartsWith(a))
                                {
                                        return true;
                                }
                        }
                        return false;
                }
                public static bool EndsWith(this string[] array, string str)
                {
                        foreach (string a in array)
                        {
                                if (str.EndsWith(a))
                                {
                                        return true;
                                }
                        }
                        return false;
                }
                public static bool IsExists(this string[] array, string str)
                {
                        foreach (string a in array)
                        {
                                if (a == str)
                                {
                                        return true;
                                }
                        }
                        return false;
                }
                /// <summary>
                /// 检查数组中是否存在指定值
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="array"></param>
                /// <param name="data"></param>
                /// <returns></returns>
                public static bool IsExists<T>(this T[] array, T data, Func<T, T, bool> match)
                {
                        foreach (T a in array)
                        {
                                if (match(a, data))
                                {
                                        return true;
                                }
                        }
                        return false;
                }
                /// <summary>
                /// 检查数组是否为空
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="array"></param>
                /// <returns></returns>
                public static bool IsNull<T>(this T[] array)
                {
                        return array == null || array.Length == 0;
                }
                /// <summary>
                /// 字符串强转
                /// </summary>
                /// <param name="str"></param>
                /// <param name="type"></param>
                /// <returns></returns>
                public static object ToObject(this string str, Type type)
                {
                        return Tools.StringParse(type, str);
                }

                public static T ToObject<T>(this string str)
                {
                        return (T)Tools.StringParse(typeof(T), str);
                }
                /// <summary>
                /// 检查2个相同类型数组是否相等
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="array"></param>
                /// <param name="data"></param>
                /// <returns></returns>
                public static bool IsEqual<T>(this T[] array, T[] data)
                {
                        if (array == null || array.Length != data.Length)
                        {
                                return false;
                        }
                        int num = 0;
                        foreach (T i in array)
                        {
                                foreach (T k in data)
                                {
                                        if (k.Equals(i))
                                        {
                                                ++num;
                                                break;
                                        }
                                }
                        }
                        return num == array.Length;
                }
                /// <summary>
                /// 检查2个相同类型数组是否相等
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="array"></param>
                /// <param name="data"></param>
                /// <returns></returns>
                public static bool IsEqual<T>(this T[] array, T[] data, int begin)
                {
                        if (array == null)
                        {
                                return false;
                        }
                        foreach (T i in array)
                        {
                                if (!i.Equals(data[begin++]))
                                {
                                        return false;
                                }
                        }
                        return true;
                }
                /// <summary>
                /// 检查2个相同类型数组是否相等
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="array"></param>
                /// <param name="data"></param>
                /// <returns></returns>
                public static int EqualNum<T>(this T[] array, T[] data, int begin)
                {
                        int num = 0;
                        foreach (T i in array)
                        {
                                if (i.Equals(data[begin++]))
                                {
                                        ++num;
                                }
                        }
                        return num;
                }
                /// <summary>
                /// 检查2个不同类型数组是否相等
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <typeparam name="T1"></typeparam>
                /// <param name="array"></param>
                /// <param name="data"></param>
                /// <param name="match"></param>
                /// <returns></returns>
                public static bool IsEqual<T, T1>(this T[] array, T1[] data, Func<T, T1, bool> match)
                {
                        if (array.Length != data.Length)
                        {
                                return false;
                        }
                        int num = 0;
                        foreach (T i in array)
                        {
                                foreach (T1 k in data)
                                {
                                        if (match(i, k))
                                        {
                                                ++num;
                                                break;
                                        }
                                }
                        }
                        return num == array.Length;
                }
                /// <summary>
                /// 检查数组长度
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="array"></param>
                /// <param name="len"></param>
                /// <returns></returns>
                public static bool CheckLength<T>(this T[] array, int len)
                {
                        return array != null && array.Length >= len;
                }
                /// <summary>
                /// 对象转JSON
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="obj"></param>
                /// <returns></returns>
                public static string ToJson<T>(this T obj) where T : class
                {
                        return obj == null ? string.Empty : Tools.Json(obj);
                }
                public static string ToJson<T>(this T obj, Type type) where T : class
                {
                        return Tools.Json(obj, type);
                }
                /// <summary>
                /// JSON字符串反序列化
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="str"></param>
                /// <returns></returns>
                public static T Json<T>(this string str) where T : class
                {
                        return Tools.Json<T>(str);
                }
                /// <summary>
                /// JSON字符串反序列化
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="str"></param>
                /// <returns></returns>
                public static object Json(this string str, Type type)
                {
                        return Tools.Json(str, type);
                }
        }
}
