using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlExecHelper
{
        internal static class LingHelper
        {
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
                public static string Connect(this string str, string left, string right)
                {
                        return left + str + right;
                }
                public static string Left(this string str, string left)
                {
                        return left + str;
                }
                public static string Right(this string str, string right)
                {
                        return str + right;
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
                        StringBuilder str = new StringBuilder(64);
                        foreach (T i in array)
                        {
                                str.Append(separator);
                                str.Append(func(i));
                        }
                        str.Remove(0, separator.Length);
                        return str.ToString();
                }
                public static string Join<T>(this T[] array, string separator, Func<T, bool> match, Func<T, string> func)
                {
                        StringBuilder str = new StringBuilder(64);
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
                                str.Remove(0, separator.Length);
                                return str.ToString();
                        }
                        return string.Empty;
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
                public static T[] Add<T>(this T[] array, List<T> add)
                {
                        if (array == null || array.Length == 0)
                        {
                                return add.ToArray();
                        }
                        else if (add.Count == 0)
                        {
                                return array;
                        }
                        T[] data = new T[array.Length + add.Count];
                        array.CopyTo(data, 0);
                        add.CopyTo(data, array.Length);
                        return data;
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
                /// 时间戳转DateTime
                /// </summary>
                /// <param name="str"></param>
                /// <returns></returns>
                public static DateTime ToDateTime(this long timestamp)
                {
                        return SqlToolsHelper.GetTimeStamp(timestamp);
                }
                /// <summary>
                /// 获取时间戳
                /// </summary>
                /// <param name="time"></param>
                /// <returns></returns>
                public static long ToLong(this DateTime time)
                {
                        return SqlToolsHelper.GetTimeStamp(time);
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
                /// 转换数组格式
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <typeparam name="Out"></typeparam>
                /// <param name="array"></param>
                /// <param name="converter"></param>
                /// <returns></returns>
                public static Out[] ConvertAll<T, Out>(this T[] array, Converter<T, Out> converter)
                {
                        return array == null ? null : Array.ConvertAll(array, converter);
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
                public static Out[] Convert<T, Out>(this T[] array, Func<T, bool> match, Func<T, int, Out> action)
                {
                        List<Out> list = new List<Out>(array.Length);
                        for (int i = 0; i < array.Length; i++)
                        {
                                T k = array[i];
                                if (match(k))
                                {
                                        list.Add(action(k, i));
                                }
                        }
                        return list.ToArray();
                }
                /// <summary>
                /// 循环数组
                /// </summary>
                /// <typeparam name="T">成员</typeparam>
                /// <param name="array">原成员数组</param>
                /// <param name="action">循环方法(成员)</param>
                public static void ForEach<T>(this T[] array, Action<T> action)
                {
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
                /// <param name="action">循环方法(成员,数组索引)</param>
                public static void ForEach<T>(this T[] array, Action<T, int> action)
                {
                        for (int i = 0; i < array.Length; i++)
                        {
                                action(array[i], i);
                        }
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
                /// 对象转JSON
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="obj"></param>
                /// <returns></returns>
                public static string ToJson<T>(this T obj) where T : class
                {
                        return obj == null ? string.Empty : SqlToolsHelper.Json(obj);
                }

        }
}
