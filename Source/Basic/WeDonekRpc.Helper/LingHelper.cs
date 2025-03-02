using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Collections.Pooled;
using WeDonekRpc.Helper.Json;
using WeDonekRpc.Helper.Reflection;
using WeDonekRpc.Helper.Validate;

namespace WeDonekRpc.Helper
{
    internal static class _ObjectClone<T> where T : class
    {
        private static readonly Func<T, T> cache = GetFunc();
        private static Func<T, T> GetFunc ()
        {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "p");
            List<MemberBinding> memberBindingList = [];
            Type type = typeof(T);
            PropertyInfo[] pros = type.GetProperties();
            pros.ForEach(c =>
            {
                if ( !c.CanWrite )
                {
                    return;
                }
                MemberExpression property = Expression.Property(parameterExpression, c);
                MemberBinding memberBinding = Expression.Bind(c, property);
                memberBindingList.Add(memberBinding);
            });
            MemberInitExpression memberInitExpression = Expression.MemberInit(Expression.New(type), memberBindingList.ToArray());
            Expression<Func<T, T>> lambda = Expression.Lambda<Func<T, T>>(memberInitExpression, new ParameterExpression[] { parameterExpression });
            return lambda.Compile();
        }

        public static T Trans ( T tIn )
        {
            return cache(tIn);
        }

    }
    public delegate bool PredicateFun<T> ( T data, out string error );
    public delegate bool ArrayTrue<T> ( T data, int index, out string error );
    public delegate bool FindFunc<T, Result> ( T data, out Result result );
    public static class LingHelper
    {
        public static long[] SplitToLong ( this string sour, char value )
        {
            int index = sour.IndexOf(value);
            if ( index == -1 )
            {
                return new long[] { long.Parse(sour) };
            }
            int begin = 0;
            int end = index;
            List<long> result = [];
            do
            {
                result.Add(long.Parse(sour.Substring(begin, end - begin)));
                begin = end + 1;
                end = sour.IndexOf(value, begin);
            } while ( end != -1 );
            if ( begin != sour.Length )
            {
                result.Add(long.Parse(sour.Substring(begin)));
            }
            return result.ToArray();
        }
        public static void SplitWriteLong ( this string sour, char value, List<long> result )
        {
            int index = sour.IndexOf(value);
            if ( index == -1 )
            {
                result.Add(long.Parse(sour));
                return;
            }
            int begin = 0;
            int end = index;
            do
            {
                result.Add(long.Parse(sour.Substring(begin, end - begin)));
                begin = end + 1;
                end = sour.IndexOf(value, begin);
            } while ( end != -1 );
            if ( begin != sour.Length )
            {
                result.Add(long.Parse(sour.Substring(begin)));
            }
        }
        public static void SplitWriteInt ( this string sour, char value, List<int> result )
        {
            int index = sour.IndexOf(value);
            if ( index == -1 )
            {
                result.Add(int.Parse(sour));
                return;
            }
            int begin = 0;
            int end = index;
            do
            {
                result.Add(int.Parse(sour.Substring(begin, end - begin)));
                begin = end + 1;
                end = sour.IndexOf(value, begin);
            } while ( end != -1 );
            if ( begin != sour.Length )
            {
                result.Add(int.Parse(sour.Substring(begin)));
            }
        }
        public static int[] SplitToInt ( this string sour, char value )
        {
            int index = sour.IndexOf(value);
            if ( index == -1 )
            {
                return new int[] { int.Parse(sour) };
            }
            int begin = 0;
            int end = index;
            List<int> result = [];
            do
            {
                result.Add(int.Parse(sour.Substring(begin, end - begin)));
                begin = end + 1;
                end = sour.IndexOf(value, begin);
            } while ( end != -1 );
            if ( begin != sour.Length )
            {
                result.Add(int.Parse(sour.Substring(begin)));
            }
            return result.ToArray();
        }
        public static TValue GetOrAdd<TKey, TValue> ( this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> valueFactory ) where TKey : notnull
        {
            if ( dictionary.TryGetValue(key, out TValue value) )
            {
                return value;
            }
            value = valueFactory(key);
            dictionary.Add(key, value);
            return value;
        }
        public static bool IsPublic ( this PropertyInfo property )
        {
            MethodInfo getMethod = property.GetGetMethod(true);
            if ( getMethod != null )
            {
                return getMethod.IsPublic;
            }
            MethodInfo setMethod = property.GetSetMethod(true);
            if ( setMethod != null )
            {
                return setMethod.IsPublic;
            }
            return false;
        }

        public static Guid ToGuid ( this string str )
        {
            return Guid.Parse(str);
        }
        public static FileInfo SaveFile ( this byte[] stream, string path )
        {
            FileInfo file = new FileInfo(path);
            if ( file.Exists )
            {
                return file;
            }
            else if ( !file.Directory.Exists )
            {
                file.Directory.Create();
            }
            using ( FileStream fileStream = file.Create() )
            {
                fileStream.Write(stream);
                fileStream.Flush();
            }
            return file;
        }
        public static MemoryStream ReadStream ( this FileInfo file )
        {
            MemoryStream stream = new MemoryStream((int)file.Length);
            using ( FileStream fileStream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Delete) )
            {
                fileStream.CopyTo(stream);
                fileStream.Flush();
            }
            return stream;
        }
        public static byte[] ReadBytes ( this FileInfo file )
        {
            using ( MemoryStream stream = new MemoryStream((int)file.Length) )
            {
                using ( FileStream fileStream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Delete) )
                {
                    fileStream.CopyTo(stream);
                    fileStream.Flush();
                }
                return stream.ToArray();
            }
        }
        public static void Validate<T> ( this T data ) where T : new()
        {
            if ( !DataValidateHepler.ValidateData(data, out string error) )
            {
                throw new ErrorException(error);
            }
        }

        public static T Clone<T> ( this T data ) where T : class
        {
            return _ObjectClone<T>.Trans(data);
        }
        public static bool IsMatch ( this string value, string regex )
        {
            return Regex.IsMatch(value, regex);
        }
        private static readonly int _Size = 1024 * 1024;
        public static void AddOrSet<Key, Value> ( this IDictionary<Key, Value> dic, Key key, Value val )
        {
            if ( dic.ContainsKey(key) )
            {
                dic[key] = val;
            }
            else
            {
                dic.Add(key, val);
            }
        }
        public static byte[] ToBytes ( this decimal val )
        {
            return Tools.ToByte(val);
        }
        public static int Write ( this decimal val, byte[] bytes, int index )
        {
            return Tools.ToByte(val, bytes, index);
        }
        public static decimal ToDecimal ( this byte[] bytes )
        {
            return Tools.ToDecimal(bytes);
        }
        public static decimal ToDecimal ( this Span<byte> bytes, int index )
        {
            return Tools.ToDecimal(bytes, index);
        }
        public static decimal ToDecimal ( this byte[] bytes, int index )
        {
            return Tools.ToDecimal(bytes, index);
        }
        public static string ToFirstPinYin ( this string text )
        {
            return Tools.ToFirstPinYin(text);
        }
        public static void TryAdd<Key, Value> ( this Dictionary<Key, Value> dic, Key key, Value val )
        {
            if ( !dic.ContainsKey(key) )
            {
                dic.Add(key, val);
            }
        }
        public static T[] Random<T> ( this List<T> list )
        {
            var array = list.ConvertAll(a => new
            {
                a,
                sort = Guid.NewGuid().ToString("N")
            });
            return array.OrderBy(a => a.sort).Select(a => a.a).ToArray();
        }
        public static bool IsExists<T> ( this IEnumerable<T> list, Func<T, bool> func )
        {
            return list.FirstOrDefault(func) != null;
        }
        public static bool IsExists<T> ( this T[] list, params T[] array ) where T : struct
        {
            if ( array.IsNull() )
            {
                return false;
            }
            return list.IsExists(c => array.IsExists(c));
        }
        public static bool IsExists<T> ( this IEnumerable<T> list, T value )
        {
            return list.Contains(value);
        }
        public static bool IsEquals<T> ( this T[] data, T[] other )
        {
            if ( data.Length != other.Length )
            {
                return false;
            }
            foreach ( T i in data )
            {
                bool isEqual = false;
                foreach ( T k in other )
                {
                    if ( i.Equals(k) )
                    {
                        isEqual = true;
                        break;
                    }
                }
                if ( !isEqual )
                {
                    return false;
                }
            }
            return true;
        }
        public static bool IsEquals<T> ( this T data, T other, bool? isExclude = null, params string[] pros ) where T : class
        {
            return ReflectionHepler.IsEquals<T>(data, other, isExclude, pros);
        }
        public static bool IsEquals<One, Two> ( this One data, Two other, bool? isExclude = null, params string[] pros ) where One : class where Two : class
        {
            return ReflectionHepler.IsEquals<One, Two>(data, other, isExclude, pros);
        }
        public static Out[] ConvertAll<T, Out> ( this ICollection<T> data, Func<T, Out> func )
        {
            Out[] list = new Out[data.Count];
            int i = 0;
            foreach ( T k in data )
            {
                list[i++] = func(k);
            }
            return list;
        }
        public static Out[] ConvertAll<T, Out> ( this List<T> data, Func<T, Out> func )
        {
            Out[] list = new Out[data.Count];
            Span<T> vals = CollectionsMarshal.AsSpan(data);
            for ( int i = 0 ; i < vals.Length ; i++ )
            {
                list[i] = func(vals[i]);
            }
            return list;
        }
        public static long Arg ( this long[] data )
        {
            return data.Sum(a => a) / data.Length;
        }
        public static int Arg ( this int[] data )
        {
            return data.Sum(a => a) / data.Length;
        }
        public static int Arg<T> ( this T[] data, Func<T, int> func )
        {
            return data.Sum(func) / data.Length;
        }
        public static short Arg ( this short[] data )
        {
            return (short)( data.Sum(a => a) / data.Length );
        }
        /// <summary>
        /// 获取ZoneIndex
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte GetZIndex ( this string str )
        {
            return Tools.GetZoneIndex(str);
        }
        /// <summary>
        /// 向URI追加参数
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static Uri AppendParam ( this Uri uri, string param )
        {
            return Tools.GetJumpUri(uri, param);
        }
        /// <summary>
        /// 获取ZoneIndex
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static short GetZIndex ( this Guid str )
        {
            return Tools.GetZoneIndex(str.ToString());
        }

        /// <summary>
        /// 字符串转DateTime
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTime ToDateTime ( this string str )
        {
            return Tools.PaseDateTime(str);
        }
        public static bool TryToDateTime ( this string str, out DateTime time )
        {
            if ( long.TryParse(str, out long res) )
            {
                time = res.ToDateTime();
                return true;
            }
            time = DateTime.MinValue;
            return false;
        }
        public static bool GetBitValue ( this byte val, int index )
        {
            return Tools.GetBitValue(val, index);
        }
        public static byte SetBitValue ( this byte source, int index, bool flag )
        {
            return Tools.SetBitValue(source, index, flag);
        }
        /// <summary>
        /// 检查文件后缀
        /// </summary>
        /// <param name="file"></param>
        /// <param name="containStr"></param>
        /// <returns></returns>
        public static bool CheckFile ( this FileInfo file, string[] containStr )
        {
            string ext = file.Extension.ToLower();
            return containStr.IsExists(a => ext == a);
        }
        /// <summary>
        /// 验证字符串格式
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="format">格式</param>
        /// <param name="containStr">包含的字符</param>
        /// <returns></returns>
        public static bool Validate ( this string str, ValidateFormat format, string containStr )
        {
            return ValidateHelper.CheckData(str, format, containStr);
        }
        /// <summary>
        /// 验证字符串格式
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="format">格式</param>
        /// <returns></returns>
        public static bool Validate ( this string str, ValidateFormat format )
        {
            return ValidateHelper.CheckData(str, format, null);
        }
        public static bool Validate ( this string str, string regexStr, RegexOptions options = RegexOptions.None )
        {
            return Regex.IsMatch(str, regexStr, options);
        }
        /// <summary>
        /// 不能为空
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool NoNull ( this string str )
        {
            return str != null && str != string.Empty;
        }
        public static int[] ForToArray ( this int end, int begin )
        {
            int[] vals = new int[end - begin];
            int index = 0;
            for ( int i = begin ; i <= end ; i++ )
            {
                vals[index++] = i;
            }
            return vals;
        }
        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="datas"></param>
        public static void Sort<T> ( this T[] datas ) where T : struct
        {
            Span<T> span = new Span<T>(datas);
            span.Sort();
        }
        public static void Sort<T, Res> ( this T[] datas, Func<T, Res> func ) where Res : struct, IComparable<Res>
        {
            Array.Sort(datas, new _DelegateIComparer<T, Res>(func));
        }
        private class _DelegateIComparer<T, Res> : IComparer<T> where Res : IComparable<Res>
        {
            private readonly Func<T, Res> _Func;
            public _DelegateIComparer ( Func<T, Res> func )
            {
                this._Func = func;
            }
            public int Compare ( T? x, T? y )
            {
                Res one = this._Func(x);
                Res two = this._Func(y);
                return one.CompareTo(two);
            }
        }
        public static string[] Sort ( this string[] datas )
        {
            Array.Sort(datas);
            return datas;
        }
        /// <summary>
        /// 是否空
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNull ( this string str )
        {
            return str == null || str == string.Empty;
        }
        public static string GetValueOrDefault ( this string str, string def )
        {
            return str == null || str == string.Empty ? def : str;
        }
        public static bool IsNotNull ( this string str )
        {
            return str != null && str != string.Empty;
        }
        public static string ToString ( this Dictionary<string, string> dic )
        {
            StringBuilder str = new StringBuilder();
            foreach ( string i in dic.Keys )
            {
                _ = str.AppendFormat("&{0}={1}", i, dic[i]);
            }
            return str.ToString();
        }
        public static string ToString ( this Dictionary<string, string> dic, params string[] remove )
        {
            remove?.ForEach(a => dic.Remove(a));
            if ( dic.Count == 0 )
            {
                return string.Empty;
            }
            StringBuilder str = new StringBuilder();
            foreach ( string i in dic.Keys )
            {
                _ = str.AppendFormat("&{0}={1}", i, dic[i]);
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
        public static T[] Paging<T> ( this T[] array, int index, int size )
        {
            long skip = ( index - 1 ) * size;
            if ( skip >= array.LongLength )
            {
                return Array.Empty<T>();
            }
            long len = array.LongLength - skip;
            if ( len > size )
            {
                len = size;
            }
            T[] datas = new T[len];
            for ( int i = 0 ; i < datas.Length ; i++ )
            {
                datas[i] = array[skip + i];
            }
            return datas;
        }
        public static void RemoveOne<T> ( this List<T> list, Predicate<T> func )
        {
            int index = list.FindIndex(func);
            if ( index != -1 )
            {
                list.RemoveAt(index);
            }
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">分页数组</param>
        /// <param name="index">页码</param>
        /// <param name="size">每页大小</param>
        /// <returns></returns>
        public static T[] Paging<T> ( this List<T> array, int index, int size )
        {
            int skip = index * size;
            if ( skip >= array.Count )
            {
                return Array.Empty<T>();
            }
            int len = array.Count - skip;
            if ( len > size )
            {
                len = size;
            }
            T[] datas = new T[len];
            for ( int i = 0 ; i < datas.Length ; i++ )
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
        public static T[] ToArray<T> ( this List<T> list, int skip, int size )
        {
            int len = list.Count - skip;
            if ( len > size )
            {
                len = size;
            }
            T[] t = new T[len];
            for ( int i = 0 ; i < t.Length ; i++ )
            {
                t[i] = list[skip + i];
            }
            return t;
        }
        /// <summary>
        /// 将数组用指定分隔符链接成字符串
        /// </summary>
        /// <typeparam name="T">数据</typeparam>
        /// <param name="array">分隔的数组</param>
        /// <param name="separator">分隔符</param>
        /// <param name="func">将数据装换格式的方法（数据，数组索引）</param>
        /// <returns></returns>
        public static string Join<T> ( this T[] array, string separator, Func<T, int, string> func )
        {
            if ( array == null || array.Length == 0 )
            {
                return string.Empty;
            }
            StringBuilder str = new StringBuilder();
            for ( int i = 0 ; i < array.Length ; i++ )
            {
                _ = str.Append(separator);
                _ = str.Append(func(array[i], i));
            }
            _ = str.Remove(0, 1);
            return str.ToString();
        }
        /// <summary>
        /// 将数组用指定分隔符链接成字符串
        /// </summary>
        /// <typeparam name="T">数据</typeparam>
        /// <param name="array">分隔的数组</param>
        /// <param name="separator">分隔符</param>
        /// <returns></returns>
        public static string Join<T> ( this T[] array, string separator )
        {
            if ( array == null || array.Length == 0 )
            {
                return string.Empty;
            }
            else if ( array.Length == 1 )
            {
                return array[0].ToString();
            }
            StringBuilder str = new StringBuilder();
            foreach ( T i in array )
            {
                _ = str.Append(separator);
                _ = str.Append(i);
            }
            _ = str.Remove(0, 1);
            return str.ToString();
        }
        public static string Join<T> ( this T[] array, char separator )
        {
            if ( array == null || array.Length == 0 )
            {
                return string.Empty;
            }
            else if ( array.Length == 1 )
            {
                return array[0].ToString();
            }
            StringBuilder str = new StringBuilder();
            foreach ( T i in array )
            {
                _ = str.Append(separator);
                _ = str.Append(i);
            }
            _ = str.Remove(0, 1);
            return str.ToString();
        }
        public static string Join<T> ( this T[] array, char separator, char around )
        {
            if ( array == null || array.Length == 0 )
            {
                return string.Empty;
            }
            else if ( array.Length == 1 )
            {
                return around + array[0].ToString() + around;
            }
            StringBuilder str = new StringBuilder(around);
            foreach ( T i in array )
            {
                _ = str.Append(i);
                _ = str.Append(separator);
            }
            _ = str.Remove(str.Length - 1, 1);
            _ = str.Append(around);
            return str.ToString();
        }
        /// <summary>
        /// 用指定分隔符链接成字符串
        /// </summary>
        /// <typeparam name="T">数据</typeparam>
        /// <param name="array">分隔的数组</param>
        /// <param name="separator">分隔符</param>
        /// <returns></returns>
        public static string Join<T> ( this List<T> list, string separator )
        {
            if ( list == null || list.Count == 0 )
            {
                return string.Empty;
            }
            StringBuilder str = new StringBuilder();
            foreach ( T i in CollectionsMarshal.AsSpan<T>(list) )
            {
                _ = str.Append(separator);
                _ = str.Append(i);
            }
            _ = str.Remove(0, 1);
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
        public static string Join<T> ( this T[] array, string separator, Func<T, string> func )
        {
            if ( array == null || array.Length == 0 )
            {
                return string.Empty;
            }
            StringBuilder str = new StringBuilder(64);
            foreach ( T i in array )
            {
                string val = func(i);
                if ( !string.IsNullOrEmpty(val) )
                {
                    _ = str.Append(separator);
                    _ = str.Append(val);
                }
            }
            if ( str.Length > 0 )
            {
                _ = str.Remove(0, separator.Length);
                return str.ToString();
            }
            return string.Empty;
        }
        public static string Join<T> ( this T[] array, char separator, Func<T, string> func )
        {
            if ( array == null || array.Length == 0 )
            {
                return string.Empty;
            }
            StringBuilder str = new StringBuilder(64);
            foreach ( T i in array )
            {
                string val = func(i);
                if ( !string.IsNullOrEmpty(val) )
                {
                    _ = str.Append(separator);
                    _ = str.Append(val);
                }
            }
            if ( str.Length > 0 )
            {
                _ = str.Remove(0, 1);
                return str.ToString();
            }
            return string.Empty;
        }
        public static void Join<T> ( this T[] array, string separator, Func<T, string> func, StringBuilder str )
        {
            foreach ( T i in array )
            {
                _ = str.Append(func(i));
                _ = str.Append(separator);
            }
            int len = separator.Length;
            _ = str.Remove(str.Length - len, len);
        }
        /// <summary>
        /// 添加成员
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">原数组</param>
        /// <param name="add">添加的成员</param>
        /// <returns></returns>
        public static T[] Add<T> ( this T[] array, T add )
        {
            if ( array == null || array.Length == 0 )
            {
                return new T[] { add };
            }
            T[] data = new T[array.Length + 1];
            array.CopyTo(data, 0);
            data[array.Length] = add;
            return data;
        }
        public static T[] Add<T> ( this T[] array, T add, Func<T, T, bool> sort )
        {
            if ( array == null || array.Length == 0 )
            {
                return new T[] { add };
            }
            int index = array.FindIndex(a => sort(a, add));
            if ( index == -1 )
            {
                index = 0;
            }
            return array.Insert(add, index);
        }
        /// <summary>
        /// 添加成员
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">原数组</param>
        /// <param name="add">添加的成员</param>
        /// <returns></returns>
        public static T[] Add<T> ( this T[] array, IList<T> add )
        {
            if ( array == null || array.Length == 0 )
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
        public static T[] Add<T> ( this T[] array, T add, T two )
        {
            if ( array == null || array.Length == 0 )
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
        public static T[] Add<T> ( this T[] array, T add, T two, T third )
        {
            if ( array == null || array.Length == 0 )
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
        public static T[] Add<T> ( this T[] array, T add, T[] adds )
        {
            if ( adds == null || adds.Length == 0 )
            {
                return array.Add(add);
            }
            T[] data = new T[array.Length + 1 + adds.Length];
            array.CopyTo(data, 0);
            data[array.Length] = add;
            adds.CopyTo(data, array.Length + 1);
            return data;
        }
        public static T[] Add<T> ( this T[] array, T[] adds, int len )
        {
            if ( array == null )
            {
                return adds;
            }
            T[] data = new T[array.Length + len];
            array.CopyTo(data, 0);
            adds.CopyTo(data, array.Length);
            return data;
        }
        public static T[] Add<T> ( this T[] array, T[] adds )
        {
            if ( adds.Length == 0 )
            {
                return array;
            }
            return array.Add(adds, adds.Length);
        }
        public static T[] Add<T> ( this T[] array, T[] one, T[] two )
        {
            if ( one == null || one.Length == 0 )
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
        public static Out[] Add<T, Out> ( this T[] array, Out add, Converter<T, Out> converter )
        {
            if ( array == null || array.Length == 0 )
            {
                return new Out[] { add };
            }
            Out[] data = new Out[array.Length + 1];
            for ( int i = 0 ; i < array.Length ; i++ )
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
        public static T[] TopInsert<T> ( this T[] array, T add )
        {
            if ( array == null || array.Length == 0 )
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
        public static T[] TopInsert<T> ( this T[] array, T[] add )
        {
            if ( array == null )
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
        public static T[] Insert<T> ( this T[] array, T add, int index )
        {
            if ( index >= array.Length )
            {
                return Add(array, add);
            }
            else if ( index == 0 )
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
        public static void SetAttrVal<T, T1> ( this T[] array, T1[] datas, Func<T, T1, bool> match, Action<T, T1> action ) where T : class
        {
            foreach ( T a in array )
            {
                foreach ( T1 i in datas )
                {
                    if ( match(a, i) )
                    {
                        action(a, i);
                        break;
                    }
                }
            }
        }


        public static T[] EextendArray<T> ( this T[] data, int size, Func<T, int> find, Func<int, T> def ) where T : class
        {
            if ( data.Length == size )
            {
                return data;
            }
            T[] list = new T[size];
            foreach ( T i in data )
            {
                int k = find(i);
                list[k] = i;
            }
            for ( int i = 0 ; i < list.Length ; i++ )
            {
                if ( list[i] == null )
                {
                    list[i] = def(i);
                }
            }
            return list;
        }
        public static void SetEmptyDef<T> ( this T[] data, Func<int, T> def ) where T : class
        {
            for ( int i = 0 ; i < data.Length ; i++ )
            {
                if ( data[i] == null )
                {
                    data[i] = def(i);
                }
            }
        }
        public static void Init<T> ( this T[] array, Func<int, T> func )
        {
            for ( int i = 0 ; i < array.Length ; i++ )
            {
                array[i] = func(i);
            }
        }
        public static void Init<T> ( this T[] array, int skip, int size, Func<int, T> func )
        {
            int end = skip + size;
            for ( int i = skip ; i < end ; i++ )
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
        public static void Init<T, T1> ( this T data, T1[] array, Func<string, T1, object> func ) where T : class
        {
            Type type = data.GetType();
            IReflectionBody body = ReflectionHepler.GetReflection(type);
            body.Properties.ForEach(a =>
            {
                foreach ( T1 i in array )
                {
                    object val = func(a.Name, i);
                    if ( val != null )
                    {
                        a.SetValue(data, val);
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
        public static void Init<T> ( this T[] data, Func<int, T, T> func ) where T : class
        {
            for ( int i = 0 ; i < data.Length ; i++ )
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
        public static string Join<T> ( this T[] array, Func<T, string> func )
        {
            if ( array.Length == 1 )
            {
                return func(array[0]);
            }
            StringBuilder str = new StringBuilder();
            foreach ( T i in array )
            {
                _ = str.Append(func(i));
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
        public static string Join<T> ( this T[] array, Func<T, int, string> func )
        {
            StringBuilder str = new StringBuilder();
            for ( int i = 0 ; i < array.Length ; i++ )
            {
                _ = str.Append(func(array[i], i));
            }
            return str.ToString();
        }

        /// <summary>
        /// 将字符串数组用指定分隔符链接成字符串
        /// </summary>
        /// <param name="array"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string Join ( this string[] array, string separator )
        {
            if ( array == null || array.Length == 0 )
            {
                return string.Empty;
            }
            StringBuilder str = new StringBuilder();
            foreach ( string i in array )
            {
                _ = str.Append(separator);
                _ = str.Append(i);
            }
            _ = str.Remove(0, 1);
            return str.ToString();
        }

        public static string Join<T> ( this T[] array, string separator, Func<T, bool> match, Func<T, string> func )
        {
            if ( array == null || array.Length == 0 )
            {
                return string.Empty;
            }
            StringBuilder str = new StringBuilder();
            foreach ( T i in array )
            {
                if ( match(i) )
                {
                    _ = str.Append(separator);
                    _ = str.Append(func(i));
                }
            }
            if ( str.Length > 0 )
            {
                _ = str.Remove(0, 1);
            }
            return str.ToString();
        }
        /// <summary>
        /// 将字符串数组连接为完整字符串
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string Join ( this string[] array )
        {
            StringBuilder str = new StringBuilder();
            foreach ( string i in array )
            {
                _ = str.Append(i);
            }
            return str.ToString();
        }
        /// <summary>
        /// 时间戳转DateTime
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTime ToDateTime ( this long timestamp )
        {
            return Tools.GetTimeStamp(timestamp);
        }
        public static DateTime ToDateTime ( this long timestamp, DateTime def )
        {
            if ( timestamp <= 0 )
            {
                return def;
            }
            return Tools.GetTimeStamp(timestamp);
        }
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long ToLong ( this DateTime time )
        {
            return Tools.GetTimeSpan(time);
        }
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long ToMilliseconds ( this DateTime time )
        {
            return Tools.GetTotalMilliseconds(time);
        }
        /// <summary>
        /// 获取时间的时辰范围
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static TimeIntervalType GetTimeIntervalType ( this DateTime time )
        {
            return Tools.GetTimeIntervalType(time);
        }
        /// <summary>
        /// 获取MD5
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetMd5 ( this string str )
        {
            return Tools.GetMD5(str);
        }
        /// <summary>
        /// 获取MD5
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetMd5 ( this Uri uri )
        {
            return Tools.GetMD5(uri.AbsoluteUri);
        }
        ///// <summary>
        ///// 获取MD5
        ///// </summary>
        ///// <param name="str"></param>
        ///// <returns></returns>
        public static string GetMd5<T> ( this T data, params string[] remove ) where T : class
        {
            return ReflectionHepler.ToMd5(data, remove);
        }

        ///// 获取MD5
        ///// </summary>
        ///// <param name="str"></param>
        ///// <returns></returns>
        //public static string GetMd5<T, Source>(this T data, params string[] remove) where T : class
        //{
        //    return Tools.GetClassMd5<Source>(data, remove);
        //}
        public static string GetMd5 ( this string[] data )
        {
            return Tools.GetMD5(string.Join(",", data));
        }
        public static string GetMd5 ( this Guid[] data )
        {
            return Tools.GetMD5(string.Join(",", data));
        }
        public static string GetMd5 ( this byte[] bytes )
        {
            return Tools.GetMD5(bytes);
        }
        public static string GetMd5 ( this byte[] bytes, int offset, int count )
        {
            return Tools.GetMD5(bytes, offset, count);
        }

        /// <summary>
        /// 搜索数组成员
        /// </summary>
        /// <typeparam name="T">成员</typeparam>
        /// <param name="array">成员数组</param>
        /// <param name="match">条件（成员）</param>
        /// <returns>成员</returns>
        public static T Find<T> ( this T[] array, Predicate<T> match )
        {
            foreach ( T i in array )
            {
                if ( match(i) )
                {
                    return i;
                }
            }
            return default;
        }
        public static Result Find<T, Result> ( this T[] array, FindFunc<T, Result> func )
        {
            foreach ( T i in array )
            {
                if ( func(i, out Result result) )
                {
                    return result;
                }
            }
            return default;
        }
        public static bool TryGet<T> ( this T[] array, Predicate<T> match, out T data )
        {
            if ( array.Length != 0 )
            {
                foreach ( T i in array )
                {
                    if ( match(i) )
                    {
                        data = i;
                        return true;
                    }
                }
            }
            data = default;
            return false;
        }
        /// <summary>
        /// 搜索数组成员
        /// </summary>
        /// <typeparam name="T">成员</typeparam>
        /// <param name="array">成员数组</param>
        /// <param name="match">条件（成员）</param>
        /// <returns>成员</returns>
        public static T Find<T, T1> ( this T[] array, T1[] datas, Func<T, T1, bool> match )
        {
            foreach ( T i in array )
            {
                if ( datas.FindIndex(a => match(i, a)) != -1 )
                {
                    return i;
                }
            }
            return default;
        }
        public static T Find<T> ( this T[] array, int begin, int end, Predicate<T> match )
        {
            for ( int i = begin ; i <= end ; i++ )
            {
                T data = array[i];
                if ( match(data) )
                {
                    return data;
                }
            }
            return default;
        }
        public static T Find<T> ( this T[] array, int begin, Predicate<T> match )
        {
            for ( int i = begin ; i < array.Length ; i++ )
            {
                T data = array[i];
                if ( match(data) )
                {
                    return data;
                }
            }
            return default;
        }
        public static T Find<T> ( this T[] array, Predicate<T> match, T def )
        {
            foreach ( T i in array )
            {
                if ( match(i) )
                {
                    return i;
                }
            }
            return def;
        }
        public static short Max<T> ( this T[] array, Predicate<T> match, Converter<T, short> convert, short defVal )
        {
            if ( array.IsNull() )
            {
                return defVal;
            }
            short max = short.MinValue;
            bool isNull = true;
            foreach ( T i in array )
            {
                if ( match(i) )
                {
                    isNull = false;
                    short a = convert(i);
                    if ( a > max )
                    {
                        max = a;
                    }
                }
            }
            return isNull ? defVal : max;
        }
        public static short Min<T> ( this T[] array, Predicate<T> match, Converter<T, short> convert, short defVal )
        {
            if ( array.IsNull() )
            {
                return defVal;
            }
            short min = short.MaxValue;
            bool isNull = true;
            foreach ( T i in array )
            {
                if ( match(i) )
                {
                    isNull = false;
                    short a = convert(i);
                    if ( a < min )
                    {
                        min = a;
                    }
                }
            }
            return isNull ? defVal : min;
        }
        public static int Max<T> ( this T[] array, Predicate<T> match, Converter<T, int> convert, int defVal )
        {
            if ( array.IsNull() )
            {
                return defVal;
            }
            int max = int.MinValue;
            bool isNull = true;
            foreach ( T i in array )
            {
                if ( match(i) )
                {
                    isNull = false;
                    int a = convert(i);
                    if ( a > max )
                    {
                        max = a;
                    }
                }
            }
            return isNull ? defVal : max;
        }
        public static int Min<T> ( this T[] array, Predicate<T> match, Converter<T, int> convert, int defVal )
        {
            if ( array.IsNull() )
            {
                return defVal;
            }
            int min = int.MaxValue;
            bool isNull = true;
            foreach ( T i in array )
            {
                if ( match(i) )
                {
                    isNull = false;
                    int a = convert(i);
                    if ( a < min )
                    {
                        min = a;
                    }
                }
            }
            return isNull ? defVal : min;
        }
        public static long Max<T> ( this T[] array, Predicate<T> match, Converter<T, long> convert, long defVal )
        {
            if ( array.IsNull() )
            {
                return defVal;
            }
            long max = long.MinValue;
            bool isNull = true;
            foreach ( T i in array )
            {
                if ( match(i) )
                {
                    isNull = false;
                    long a = convert(i);
                    if ( a > max )
                    {
                        max = a;
                    }
                }
            }
            return isNull ? defVal : max;
        }
        public static long Min<T> ( this T[] array, Predicate<T> match, Converter<T, long> convert, long defVal )
        {
            if ( array.IsNull() )
            {
                return defVal;
            }
            long min = long.MaxValue;
            bool isNull = true;
            foreach ( T i in array )
            {
                if ( match(i) )
                {
                    isNull = false;
                    long a = convert(i);
                    if ( a < min )
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
        public static Result Find<T, Result> ( this T[] array, Predicate<T> match, Func<T, Result> func )
        {
            foreach ( T i in array )
            {
                if ( match(i) )
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
        public static Result Find<T, Result> ( this T[] array, Predicate<T> match, Func<T, Result> func, Result def )
        {
            foreach ( T i in array )
            {
                if ( match(i) )
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
        public static T[] Join<T> ( this T[] array, T[] two )
        {
            if ( array == null || array.Length == 0 )
            {
                return two;
            }
            else if ( two == null || two.Length == 0 )
            {
                return array;
            }
            T[] list = new T[array.Length + two.Length];
            array.CopyTo(list, 0);
            two.CopyTo(list, array.Length);
            return list;
        }
        public static Result[] Join<T, Result> ( this T[] array, T[] two, Func<T, Result> func )
        {
            if ( array == null || array.Length == 0 )
            {
                return two.ConvertAll(func);
            }
            else if ( two == null || two.Length == 0 )
            {
                return array.ConvertAll(func);
            }
            Result[] list = new Result[array.Length + two.Length];
            int k = 0;
            foreach ( T i in array )
            {
                list[k++] = func(i);
            }
            foreach ( T i in two )
            {
                list[k++] = func(i);
            }
            return list;
        }
        /// <summary>
        /// 合并数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="Result"></typeparam>
        /// <param name="array"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static Result[] Join<T, Result> ( this T[] array, Func<T, Result[]> func )
        {
            if ( array == null || array.Length == 0 )
            {
                return Array.Empty<Result>();
            }
            using ( PooledList<Result> results = new PooledList<Result>(array.Length * 3) )
            {
                array.ForEach(a =>
               {
                   Result[] list = func(a);
                   if ( list != null && list.Length > 0 )
                   {
                       results.AddRange(list);
                   }
               });
                return results.ToArray();
            }
        }
        public static T[] Join<T> ( this T[] array, List<T> two )
        {
            if ( array == null || array.Length == 0 )
            {
                return two.ToArray();
            }
            else if ( two == null || two.Count == 0 )
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
        public static T[] Join<T, T1> ( this T[] array, T1[] two, Func<T, T1, bool> func, Converter<T1, T> convert )
        {
            if ( array == null || array.Length == 0 )
            {
                return two.ConvertAll(convert);
            }
            using ( PooledList<T> list = new PooledList<T>(array) )
            {
                list.Capacity = array.Length + two.Length;
                foreach ( T1 k in two )
                {
                    if ( array.FindIndex(a => func(a, k)) == -1 )
                    {
                        list.Add(convert(k));
                    }
                }
                return list.ToArray();
            }
        }

        public static string[] Join<T, T1> ( this T[] array, string separator, T1[] two, Func<T, T1, bool> func, Func<T1, string> convert )
        {
            return array.ConvertAll(k =>
             {
                 T1[] t = two.FindAll(a => func(k, a));
                 return t.Length > 0 ? t.Join(separator, convert) : string.Empty;
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
        public static T[] Join<T> ( this T[] array, T[] two, Func<T, T, bool> func )
        {
            if ( array == null || array.Length == 0 )
            {
                return two;
            }
            using ( PooledList<T> list = new PooledList<T>(array) )
            {
                foreach ( T k in two )
                {
                    if ( list.FindIndex(a => func(a, k)) == -1 )
                    {
                        list.Add(k);
                    }
                }
                return list.ToArray();
            }
        }
        /// <summary>
        /// 使用指定值初始化数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="def"></param>
        public static void Initialize<T> ( this T[] array, T def )
        {
            for ( int i = 0 ; i < array.Length ; i++ )
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
        public static int FindIndex<T> ( this T[] array, Func<T, bool> match )
        {
            if ( array == null )
            {
                return -1;
            }
            for ( int i = 0 ; i < array.Length ; i++ )
            {
                if ( match(array[i]) )
                {
                    return i;
                }
            }
            return -1;
        }
        public static int[] FindAllIndex<T> ( this T[] array, Func<T, bool> match )
        {
            if ( array == null )
            {
                return null;
            }
            Span<int> list = new Span<int>(new int[array.Length]);
            int k = 0;
            for ( int i = 0 ; i < array.Length ; i++ )
            {
                if ( match(array[i]) )
                {
                    list[k++] = i;
                }
            }
            return list.Slice(0, k).ToArray();
        }
        public static int[] FindAllIndex<T> ( this T[] array, int size, Func<T, bool> match )
        {
            if ( array == null )
            {
                return null;
            }
            Span<int> list = new Span<int>(new int[size]);
            int k = 0;
            for ( int i = 0 ; i < array.Length ; i++ )
            {
                if ( match(array[i]) )
                {
                    list[k++] = i;
                    if ( k == size )
                    {
                        break;
                    }
                }
            }
            return list.Slice(0, k).ToArray();
        }
        /// <summary>
        /// 从开始索引处搜索数组成员返回数组索引
        /// </summary>
        /// <typeparam name="T">成员</typeparam>
        /// <param name="array">数组</param>
        /// <param name="begin">起始索引</param>
        /// <param name="match">搜索的方法(成员)</param>
        /// <returns>数组索引(-1 无)</returns>
        public static int FindIndex<T> ( this T[] array, int begin, Predicate<T> match )
        {
            if ( array.Length <= begin )
            {
                return -1;
            }
            for ( int i = begin ; i < array.Length ; i++ )
            {
                if ( match(array[i]) )
                {
                    return i;
                }
            }
            return -1;
        }
        /// <summary>
        /// 倒序搜索数组成员返回数组索引
        /// </summary>
        /// <typeparam name="T">成员</typeparam>
        /// <param name="array">数组</param>
        /// <param name="match">搜索的方法(成员)</param>
        /// <returns>数组索引(-1 无)</returns>
        public static int FindLastIndex<T> ( this T[] array, Predicate<T> match )
        {
            if ( array == null )
            {
                return -1;
            }
            for ( int i = array.Length - 1 ; i >= 0 ; i-- )
            {
                if ( match(array[i]) )
                {
                    return i;
                }
            }
            return -1;
        }
        public static int FindLastIndex ( this string[] array, string val )
        {
            for ( int i = array.Length - 1 ; i >= 0 ; i-- )
            {
                if ( array[i] == val )
                {
                    return i;
                }
            }
            return -1;
        }
        public static int FindLastIndex ( this string[] array, string val, int end )
        {
            for ( int i = array.Length - 1 ; i >= end ; i-- )
            {
                if ( array[i] == val )
                {
                    return i;
                }
            }
            return -1;
        }
        /// <summary>
        /// 倒序搜索数组成员返回数组索引
        /// </summary>
        /// <typeparam name="T">成员</typeparam>
        /// <param name="array">数组</param>
        /// <param name="match">搜索的方法(成员)</param>
        /// <returns>数组索引(-1 无)</returns>
        public static int FindLastIndex<T> ( this T[] array, int begin, int end, Predicate<T> match )
        {
            for ( int i = begin ; i >= end ; i-- )
            {
                if ( match(array[i]) )
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
        public static Out[] Distinct<T, Out> ( this T[] array, Func<T, Out> fun )
        {
            return array.Select(fun).Distinct().ToArray();
        }
        /// <summary>
        /// 数组去重
        /// </summary>
        /// <typeparam name="T">成员</typeparam>
        /// <param name="array">成员数组</param>
        /// <returns>输出成员数组</returns>
        public static string[] Distinct ( this string[] array )
        {
            return array.Distinct<string>().ToArray();
        }
        /// <summary>
        /// 数组去重
        /// </summary>
        /// <param name="array">成员数组</param>
        /// <returns>输出成员数组</returns>
        public static int[] Distinct ( this int[] array )
        {
            return array.Distinct<int>().ToArray();
        }
        /// <summary>
        /// 数组去重
        /// </summary>
        /// <param name="array">成员数组</param>
        /// <returns>输出成员数组</returns>
        public static long[] Distinct ( this long[] array )
        {
            return array.Distinct<long>().ToArray();
        }
        private class _Equality<T> : IEqualityComparer<T>
        {
            private readonly Func<T, T, bool> _Func = null;
            public _Equality ( Func<T, T, bool> func )
            {
                this._Func = func;
            }
            public bool Equals ( T x, T y )
            {
                return this._Func(x, y);
            }

            public int GetHashCode ( T obj )
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
        public static T[] Distinct<T> ( this T[] array, Func<T, T, bool> match ) where T : class
        {
            return array.Distinct(new _Equality<T>(match)).ToArray();
        }
        public static Out[] Distinct<T, Out> ( this T[] array, Func<T, Out> convert, Func<Out, Out, bool> match ) where T : class
        {
            return array.Select(convert).Distinct(new _Equality<Out>(match)).ToArray();
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
        public static Out[] Distinct<T, Out> ( this T[] array, Func<T, T, bool> match, Func<T, Out> converter )
        {
            return array.Distinct(new _Equality<T>(match)).Select(converter).ToArray();
        }

        public static Out[] Distinct<T, Out> ( this T[] array, Func<T, bool> func, Func<T, Out> converter, Func<T, T, bool> match ) where T : class
        {
            return array.Where(func).Distinct(new _Equality<T>(match)).Select(converter).ToArray();
        }
        public static Out[] Distinct<T, T1, Out> ( this T[] array, Converter<T, T1> fun, Func<T, T1, Out> converter )
        {
            using ( PooledList<T1> list = new PooledList<T1>(array.Length) )
            {
                using ( PooledList<Out> outs = new PooledList<Out>(array.Length) )
                {
                    foreach ( T i in array )
                    {
                        T1 t = fun(i);
                        if ( t != null && !list.Contains(t) )
                        {
                            list.Add(t);
                            outs.Add(converter(i, t));
                        }
                    }
                    return outs.ToArray();
                }
            }
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
        public static Out[] Distinct<T, Out> ( this T[] array, Func<T, bool> match, Func<T, Out> fun ) where T : class
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
        public static string[] Distinct<T> ( this T[] array, Func<T, bool> match, Func<T, string> fun ) where T : class
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
        public static T[] Copy<T> ( this T[] array, int index, int len )
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
        public static string Join<T> ( this T[] array, int begin, int end )
        {
            StringBuilder str = new StringBuilder();
            for ( int i = begin ; i < end ; i++ )
            {
                _ = str.Append(array[i]);
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
        public static string Join<T> ( this T[] array, int begin, int end, Func<T, string> func )
        {
            StringBuilder str = new StringBuilder();
            for ( int i = begin ; i < end ; i++ )
            {
                _ = str.Append(func(array[i]));
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
        public static Result[] ToArray<T, Result> ( this List<T> array, Func<T, Result> func )
        {
            Result[] t = new Result[array.Count];
            Span<T> vals = CollectionsMarshal.AsSpan<T>(array);
            for ( int i = 0 ; i < array.Count ; i++ )
            {
                t[i] = func(vals[i]);
            }
            return t;
        }
        public static T[] ToArray<T> ( this List<T> array, Func<T, bool> func )
        {
            if ( array == null || array.Count == 0 )
            {
                return new T[0];
            }
            T[] t = new T[array.Count];
            int i = 0;
            array.ForEach(c =>
            {
                if ( func(c) )
                {
                    t[i++] = c;
                }
            });
            if ( i == 0 )
            {
                return new T[0];
            }
            return t.AsSpan().Slice(0, i).ToArray();
        }

        /// <summary>
        /// List转数组并转换类型
        /// </summary>
        /// <typeparam name="T">成员</typeparam>
        /// <typeparam name="Result">新成员</typeparam>
        /// <param name="array">泛型集合</param>
        /// <param name="func">数据转换方法(成员,新成员)</param>
        /// <returns>新成员数组</returns>
        public static T[] ToArray<T> ( this List<T> array, int index )
        {
            Span<T> vals = CollectionsMarshal.AsSpan<T>(array);
            return vals.Slice(index).ToArray();
        }
        /// <summary>
        /// List转数组并转换类型
        /// </summary>
        /// <typeparam name="T">成员</typeparam>
        /// <typeparam name="Result">新成员</typeparam>
        /// <param name="array">泛型集合</param>
        /// <param name="func">数据转换方法(成员,泛型索引位,新成员)</param>
        /// <returns>新成员数组</returns>
        public static Result[] ToArray<T, Result> ( this List<T> array, Func<T, int, Result> func )
        {
            Result[] t = new Result[array.Count];
            Span<T> vals = CollectionsMarshal.AsSpan<T>(array);
            for ( int i = 0 ; i < array.Count ; i++ )
            {
                t[i] = func(vals[i], i);
            }
            return t;
        }
        /// <summary>
        /// 数组移除成员
        /// </summary>
        /// <typeparam name="T">成员</typeparam>
        /// <param name="array">数组</param>
        /// <param name="func">检查是否移除(成员，是否移除)</param>
        /// <returns></returns>
        public static T[] Remove<T> ( this T[] array, Func<T, bool> func )
        {
            using ( PooledList<T> list = new PooledList<T>(array.Length) )
            {
                foreach ( T a in array )
                {
                    if ( !func(a) )
                    {
                        list.Add(a);
                    }
                }
                return list.ToArray();
            }
        }

        public static T[] RemoveOne<T> ( this T[] array, Func<T, bool> func )
        {
            int index = array.FindIndex(func);
            return index == -1 ? array : array.Remove(index);
        }
        public static T[] RemoveOne<T> ( this T[] array, Func<T, bool> func, out T source )
        {
            int index = array.FindIndex(func);
            if ( index == -1 )
            {
                source = default;
                return array;
            }
            source = array[index];
            return array.Remove(index);
        }
        /// <summary>
        /// 数组移除成员
        /// </summary>
        /// <typeparam name="T">成员</typeparam>
        /// <param name="array">数组</param>
        /// <param name="func">检查是否移除(成员，是否移除)</param>
        /// <returns></returns>
        public static T[] Remove<T> ( this T[] array, int index )
        {
            if ( array.Length == 1 )
            {
                return Array.Empty<T>();
            }
            else if ( index == 0 )
            {
                return new Memory<T>(array).Slice(1).ToArray();
            }
            else if ( index == array.Length - 1 )
            {
                return new Memory<T>(array).Slice(0, index).ToArray();
            }
            else
            {
                T[] data = new T[array.Length - 1];
                Array.Copy(array, 0, data, 0, index);
                int len = data.Length - index;
                Array.Copy(array, index + 1, data, index, len);
                return data;
            }
        }
        /// <summary>
        /// 数组移除成员并添加成员
        /// </summary>
        /// <typeparam name="T">成员</typeparam>
        /// <param name="array">数组</param>
        /// <param name="func">检查是否移除(成员，是否移除)</param>
        /// <param name="adds">添加的成员</param>
        /// <returns></returns>
        public static T[] Remove<T> ( this T[] array, Func<T, bool> func, T[] adds )
        {
            using ( PooledList<T> list = new PooledList<T>(adds.Length + array.Length) )
            {
                list.AddRange(adds);
                foreach ( T a in array )
                {
                    if ( !func(a) )
                    {
                        list.Add(a);
                    }
                }
                return list.ToArray();
            }
        }
        /// <summary>
        /// 数组移除成员
        /// </summary>
        /// <typeparam name="T">成员</typeparam>
        /// <param name="data">移除的成员</param>
        /// <param name="array">数组</param>
        /// <returns></returns>
        public static T[] Remove<T> ( this T[] array, T data )
        {
            using ( PooledList<T> list = new PooledList<T>(array.Length) )
            {
                foreach ( T a in array )
                {
                    if ( !a.Equals(data) )
                    {
                        list.Add(a);
                    }
                }
                return list.ToArray();
            }
        }
        private static bool _CheckIsRemove<T, T1> ( T i, T1[] remove, Func<T, T1, bool> func )
        {
            foreach ( T1 k in remove )
            {
                if ( func(i, k) )
                {
                    return true;
                }
            }
            return false;
        }
        private static bool _CheckIsRemove<T, T1> ( T i, List<T1> remove, Func<T, T1, bool> func )
        {
            foreach ( T1 k in remove )
            {
                if ( func(i, k) )
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
        public static T[] Remove<T, T1> ( this T[] array, List<T1> remove, Func<T, T1, bool> func )
        {
            using ( PooledList<T> list = new PooledList<T>(array.Length) )
            {
                foreach ( T i in array )
                {
                    if ( !_CheckIsRemove(i, remove, func) )
                    {
                        list.Add(i);
                    }
                }
                return list.ToArray();
            }
        }
        public static T[] Remove<T, T1> ( this T[] array, T1[] remove, Func<T, T1, bool> func )
        {
            using ( PooledList<T> list = new PooledList<T>(array.Length) )
            {
                foreach ( T i in array )
                {
                    if ( !_CheckIsRemove(i, remove, func) )
                    {
                        list.Add(i);
                    }
                }
                return list.ToArray();
            }
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <typeparam name="T">成员</typeparam>
        /// <param name="array">成员数组</param>
        /// <param name="func">比对成员大小(当前成员，下一个成员，)</param>
        public static void Sort<T> ( this T[] array, Func<T, T, bool> func )
        {
            for ( int i = 1 ; i < array.Length ; i++ )
            {
                int j = i - 1;
                if ( func(array[j], array[i]) )
                {
                    (array[j], array[i]) = (array[i], array[j]);
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
        public static T[] Remove<T> ( this T[] array, T[] remove )
        {
            using ( PooledList<T> list = new PooledList<T>(array.Length) )
            {
                foreach ( T a in array )
                {
                    if ( !remove.IsExists(a) )
                    {
                        list.Add(a);
                    }
                }
                return list.ToArray();
            }
        }
        /// <summary>
        /// 移除筛选成员不存在的数组成员并追加新成员
        /// </summary>
        /// <typeparam name="T">成员</typeparam>
        /// <param name="array">成员数组</param>
        /// <param name="filter">筛选数组</param>
        /// <param name="adds">追加的成员</param>
        /// <returns>新成员数组</returns>
        public static T[] Remove<T> ( this T[] array, T[] filter, T[] adds )
        {
            using ( PooledList<T> list = new PooledList<T>(array.Length + adds.Length) )
            {
                array.ForEach(a =>
                {
                    if ( !filter.IsExists(b => b.Equals(a)) )
                    {
                        list.Add(a);
                    }
                });
                list.AddRange(adds);
                return list.ToArray();
            }
        }
        /// <summary>
        /// 将对象循环转换为详
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="Out"></typeparam>
        /// <param name="source"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static Out[] ConvertAll<T, Out> ( this T[] source, Action<T, IList<Out>> action ) where T : class
        {
            using ( PooledList<Out> list = new PooledList<Out>(source.Length * 2) )
            {
                source.ForEach(c =>
                {
                    action(c, list);
                });
                return list.ToArray();
            }
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
        public static T[] Remove<T> ( this T[] array, T[] filter, Func<T, T, bool> match, T[] adds )
        {
            using ( PooledList<T> list = new PooledList<T>(array.Length + adds.Length) )
            {
                foreach ( T a in array )
                {
                    if ( !filter.IsExists(b => match(a, b)) )
                    {
                        list.Add(a);
                    }
                }
                list.AddRange(adds);
                return list.ToArray();
            }
        }
        /// <summary>
        /// 移除成员
        /// </summary>
        /// <typeparam name="T">成员</typeparam>
        /// <param name="array">成员数组</param>
        /// <param name="filter">筛选的数组</param>
        /// <param name="match">比对成员和筛选成员是否相同(原成员，筛选的成员，是否相同)</param>
        /// <returns>新成员数组</returns>
        public static T[] Remove<T> ( this T[] array, T[] filter, Func<T, T, bool> match )
        {
            using ( PooledList<T> list = new PooledList<T>(array.Length) )
            {
                foreach ( T a in array )
                {
                    if ( filter.FindIndex(b => match(a, b)) == -1 )
                    {
                        list.Add(a);
                    }
                }
                return list.ToArray();
            }
        }

        /// <summary>
        /// 替换数组中的成员
        /// </summary>
        /// <typeparam name="T">成员</typeparam>
        /// <param name="array">成员数组</param>
        /// <param name="filter">替换的数组</param>
        /// <param name="match">比对成员和替换成员是否相同(原成员，替换的成员，是否相同)</param>
        /// <returns>新成员数组</returns>
        public static T[] Replace<T> ( this T[] array, T[] filter, Func<T, T, bool> match )
        {
            for ( int i = 0 ; i < array.Length ; i++ )
            {
                T source = array[i];
                T replace = filter.Find(b => match(source, b));
                if ( replace != null )
                {
                    array[i] = replace;
                }
            }
            return array;
        }
        public static T[] Replace<T> ( this T[] array, T[] filter, Func<T, T, T> match )
        {
            for ( int i = 0 ; i < array.Length ; i++ )
            {
                T source = array[i];
                foreach ( T k in filter )
                {
                    T replace = match(source, k);
                    if ( replace != null )
                    {
                        array[i] = replace;
                        break;
                    }
                }
            }
            return array;
        }
        public static string[] Replace ( this string[] array, string[] filter, Func<string, string, string> match )
        {
            string[] reps = new string[filter.Length];
            int j = 0;
            for ( int i = 0 ; i < array.Length ; i++ )
            {
                string source = array[i];
                foreach ( string k in filter )
                {
                    string replace = match(source, k);
                    if ( replace != null )
                    {
                        array[i] = replace;
                        reps[j++] = replace;
                        break;
                    }
                }
            }
            return reps;
        }

        /// <summary>
        /// 将Array类型转换为数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public static T[] ToArray<T> ( this Array array )
        {
            T[] list = new T[array.Length];
            for ( int i = 0 ; i < array.Length ; i++ )
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
        public static Out[] ConvertAll<T, Out> ( this Array array, Converter<T, Out> converter )
        {
            Out[] list = new Out[array.Length];
            for ( int i = 0 ; i < array.Length ; i++ )
            {
                list[i] = converter((T)array.GetValue(i));
            }
            return list;
        }
        public static Out[] ConvertAll<Out> ( this Array array, Converter<object, Out> converter )
        {
            Out[] list = new Out[array.Length];
            for ( int i = 0 ; i < array.Length ; i++ )
            {
                list[i] = converter(array.GetValue(i));
            }
            return list;
        }
        public static Out[] ParallelToArray<T, Out> ( this List<T> array, Converter<T, Out> converter )
        {
            if ( array.Count == 0 )
            {
                return Array.Empty<Out>();
            }
            Out[] outs = new Out[array.Count];
            if ( outs.Length == 1 )
            {
                outs[0] = converter(array[0]);
                return outs;
            }
            ParallelLoopResult result = Parallel.For(0, array.Count, a =>
            {
                outs[a] = converter(array[a]);
            });
            if ( result.IsCompleted )
            {
                return outs;
            }
            throw new ErrorException("public.parallel.convert.error");
        }
        public static Out[] ParallelConvertAll<T, Out> ( this T[] array, Converter<T, Out> converter )
        {
            if ( array.Length == 0 )
            {
                return Array.Empty<Out>();
            }
            Out[] outs = new Out[array.Length];
            if ( outs.Length == 1 )
            {
                outs[0] = converter(array[0]);
                return outs;
            }
            ParallelLoopResult result = Parallel.For(0, array.Length, a =>
            {
                outs[a] = converter(array[a]);
            });
            if ( result.IsCompleted )
            {
                return outs;
            }
            throw new ErrorException("public.parallel.convert.error");
        }
        /// <summary>
        /// 转换数组格式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="Out"></typeparam>
        /// <param name="array"></param>
        /// <param name="converter"></param>
        /// <returns></returns>
        public static Out[] ConvertAll<T, Out> ( this T[] array, Converter<T, Out> converter )
        {
            if ( array == null || array.Length == 0 )
            {
                return Array.Empty<Out>();
            }
            Out[] t = new Out[array.Length];
            for ( int i = 0 ; i < array.Length ; i++ )
            {
                t[i] = converter(array[i]);
            }
            return t;
        }
        public static List<Out> ConvertAllToList<T, Out> ( this T[] array, Converter<T, Out> converter )
        {
            if ( array == null )
            {
                return null;
            }
            else if ( array.Length == 0 )
            {
                return [];
            }
            List<Out> t = new List<Out>(array.Length);
            for ( int i = 0 ; i < array.Length ; i++ )
            {
                t.Add(converter(array[i]));
            }
            return t;
        }
        public static Out[] ConvertAllToArray<T, Out> ( this IList<T> array, Converter<T, Out> converter )
        {
            if ( array == null )
            {
                return null;
            }
            Out[] t = new Out[array.Count];
            for ( int i = 0 ; i < array.Count ; i++ )
            {
                t[i] = converter(array[i]);
            }
            return t;
        }
        public static Out[] Convert<T, Out> ( this IList<T> array, Func<T, bool> match, Converter<T, Out> converter )
        {
            if ( array == null )
            {
                return null;
            }
            Out[] t = new Out[array.Count];
            int index = 0;
            foreach ( T i in array )
            {
                if ( match(i) )
                {
                    t[index++] = converter(i);
                }
            }
            if ( index == 0 )
            {
                return Array.Empty<Out>();
            }
            return t.AsSpan().Slice(0, index).ToArray();
        }
        /// <summary>
        /// 计数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static int Count<T> ( this T[] array, Func<T, bool> func )
        {
            int num = 0;
            foreach ( T i in array )
            {
                if ( func(i) )
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
        public static Out[] Convert<T, T1, Out> ( this T[] array, T1[] datas, Func<T, T1, bool> match, Func<T, T1[], Out> converter )
        {
            if ( array == null )
            {
                return null;
            }
            Out[] list = new Out[array.Length];
            for ( int i = 0 ; i < array.Length ; i++ )
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
        public static T[] Merge<T> ( this T[] array, T[] datas, Func<T, T, bool> match, Func<T, T, T> converter )
        {
            if ( array == null )
            {
                return null;
            }
            T[] list = new T[array.Length];
            for ( int i = 0 ; i < array.Length ; i++ )
            {
                T t = array[i];
                T k = datas.Find(b => match(t, b));
                list[i] = converter(t, k);
            }
            return list;
        }
        public static Out[] Merge<T, Out> ( this T[] array, T[] datas, Func<T, T, bool> match, Func<T, T, Out> converter )
        {
            if ( array == null )
            {
                return null;
            }
            Out[] list = new Out[array.Length];
            for ( int i = 0 ; i < array.Length ; i++ )
            {
                T t = array[i];
                T k = datas.Find(b => match(t, b));
                list[i] = converter(t, k);
            }
            return list;
        }
        public static Out[] Merge<T, T1, Out> ( this T[] array, T1[] datas, Func<T, T1, bool> match, Func<T, T1, Out> converter )
        {
            if ( array == null )
            {
                return null;
            }
            Out[] list = new Out[array.Length];
            for ( int i = 0 ; i < array.Length ; i++ )
            {
                T t = array[i];
                T1 k = datas.Find(b => match(t, b));
                list[i] = converter(t, k);
            }
            return list;
        }
        public static Out[] GroupBy<T, Out> ( this T[] array, Func<T, T[], Out> func )
        {
            if ( array == null || array.Length == 0 )
            {
                return Array.Empty<Out>();
            }
            else if ( array.Length == 1 )
            {
                return new Out[] { func(array[0], array) };
            }
            else
            {
                using ( PooledList<T> keys = new PooledList<T>(array.Length) )
                {
                    using ( PooledList<Out> outs = new PooledList<Out>(array.Length) )
                    {
                        foreach ( T i in array )
                        {
                            if ( !array.IsExists(a => a.Equals(i)) )
                            {
                                keys.Add(i);
                                T[] ts = array.FindAll(a => i.Equals(a));
                                outs.Add(func(i, ts));
                            }
                        }
                        return outs.ToArray();
                    }
                }
            }
        }
        public static void GroupBy<T, Result> ( this IEnumerable<T> datas, Func<T, Result> func, Func<T, Result, bool> match, Action<Result, T[]> action ) where T : class
        {
            if ( datas == null )
            {
                return;
            }
            else
            {
                T[] array = datas.ToArray();
                Result[] res = datas.Select(func).Distinct().ToArray();
                if ( res.Length == 1 )
                {
                    action(res[0], array);
                    return;
                }
                foreach ( Result i in res )
                {
                    action(i, array.FindAll(a => match(a, i)));
                }
            }
        }
        public static Result[] GroupBy<T, T1, Result> ( this T[] array, Func<T, T1> func, Func<T, T1, bool> match, Func<T1, T[], Result> action ) where T : class
        {
            if ( array == null || array.Length == 0 )
            {
                return null;
            }
            else if ( array.Length == 1 )
            {
                return new Result[] { action(func(array[0]), array) };
            }
            else
            {
                T1[] res = array.Select(func).Distinct().ToArray();
                return res.Length == 1
                        ? ( new Result[] { action(func(array[0]), array) } )
                        : res.ConvertAll(i =>
                {
                    return action(i, array.FindAll(a => match(a, i)));
                });
            }
        }
        public static void GroupBy<T, Result> ( this T[] array, Func<T, Result> func, Func<T, Result, bool> match, Action<Result, T[]> action ) where T : class
        {
            if ( array == null || array.Length == 0 )
            {
                return;
            }
            else if ( array.Length == 1 )
            {
                action(func(array[0]), array);
            }
            else
            {
                Result[] res = array.Select(func).Distinct().ToArray();
                if ( res.Length == 1 )
                {
                    action(res[0], array);
                    return;
                }
                foreach ( Result i in res )
                {
                    T[] datas = array.FindAll(a => match(a, i));
                    action(i, datas);
                }
            }
        }
        public static Out[] GroupBy<T, T1, Out> ( this T[] array, Converter<T, T1> converter, Func<T1, T[], Out> func )
        {
            if ( array == null || array.Length == 0 )
            {
                return Array.Empty<Out>();
            }
            else if ( array.Length == 1 )
            {
                return new Out[] { func(converter(array[0]), array) };
            }
            else
            {
                using ( PooledList<T1> keys = new PooledList<T1>(array.Length) )
                {
                    using ( PooledList<Out> outs = new PooledList<Out>(array.Length) )
                    {
                        foreach ( T i in array )
                        {
                            T1 k = converter(i);
                            if ( !keys.Contains(k) )
                            {
                                keys.Add(k);
                                T[] ts = array.FindAll(a => converter(a).Equals(k));
                                outs.Add(func(k, ts));
                            }
                        }
                        return outs.ToArray();
                    }
                }
            }
        }
        public static Out[] GroupBy<T, Out> ( this T[] array, Func<T, T, bool> match, Func<T, T[], Out> func )
        {
            if ( array == null || array.Length == 0 )
            {
                return Array.Empty<Out>();
            }
            else if ( array.Length == 1 )
            {
                return new Out[] { func(array[0], array) };
            }
            else
            {
                using ( PooledList<T> keys = new PooledList<T>(array.Length) )
                {
                    using ( PooledList<Out> outs = new PooledList<Out>(array.Length) )
                    {
                        foreach ( T i in array )
                        {
                            if ( keys.FindIndex(a => match(i, a)) == -1 )
                            {
                                keys.Add(i);
                                T[] ts = array.FindAll(a => match(i, a));
                                outs.Add(func(i, ts));
                            }
                        }
                        return outs.ToArray();
                    }
                }
            }
        }
        public static Out[] GroupBy<T, Out> ( this List<T> array, Func<T, T, bool> match, Func<T, IList<T>, Out> func )
        {
            if ( array == null || array.Count == 0 )
            {
                return Array.Empty<Out>();
            }
            else if ( array.Count == 1 )
            {
                return new Out[] { func(array[0], array) };
            }
            else
            {
                using ( PooledList<T> keys = new PooledList<T>(array.Count) )
                {
                    using ( PooledList<Out> outs = new PooledList<Out>(array.Count) )
                    {
                        foreach ( T i in array )
                        {
                            if ( keys.FindIndex(a => match(i, a)) == -1 )
                            {
                                keys.Add(i);
                                using ( PooledList<T> ts = _FindAll(array, i, match) )
                                {
                                    outs.Add(func(i, ts));
                                }
                            }
                        }
                        return outs.ToArray();
                    }
                }
            }
        }
        private static PooledList<T> _FindAll<T> ( List<T> list, T data, Func<T, T, bool> match )
        {
            PooledList<T> keys = new PooledList<T>(list.Count);
            foreach ( T i in list )
            {
                if ( match(data, i) )
                {
                    keys.Add(i);
                }
            }
            return keys;
        }
        public static int Count ( this string[] array, string[] datas )
        {
            int num = 0;
            for ( int i = 0 ; i < array.Length ; i++ )
            {
                if ( array[i] == datas[i] )
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
        public static Out[] Convert<T, Out> ( this T[] array, Converter<T, Out> converter )
        {
            Out[] list = new Out[array.Length];
            int i = 0;
            Out def = default;
            foreach ( T a in array )
            {
                Out obj = converter(a);
                if ( obj != null && !obj.Equals(def) )
                {
                    list[i++] = obj;
                }
            }
            if ( list.Length == i )
            {
                return list;
            }
            return new Span<Out>(list).Slice(0, i).ToArray();
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
        public static Out[] Convert<T, Out> ( this T[] array, Converter<T, Out> converter, Out def )
        {
            Out[] list = new Out[array.Length];
            int i = 0;
            foreach ( T a in array )
            {
                Out obj = converter(a);
                if ( obj != null && !obj.Equals(def) )
                {
                    list[i++] = obj;
                }
            }
            if ( list.Length == i )
            {
                return list;
            }
            return new Span<Out>(list).Slice(0, i).ToArray();
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
        public static Out[] Convert<T, Out> ( this T[] array, Predicate<T> find, Converter<T, Out> converter )
        {
            Out[] t = new Out[array.Length];
            int i = 0;
            foreach ( T a in array )
            {
                if ( find(a) )
                {
                    t[i++] = converter(a);
                }
            }
            if ( t.Length == i )
            {
                return t;
            }
            return new Span<Out>(t).Slice(0, i).ToArray();
        }
        public static Out[] Convert<T, Out> ( this T[] array, Predicate<T> find, Func<T, int, Out> converter )
        {
            Out[] t = new Out[array.Length];
            int i = 0;
            int k = 0;
            foreach ( T a in array )
            {
                if ( find(a) )
                {
                    t[i++] = converter(a, k);
                }
                k++;
            }
            if ( t.Length == i )
            {
                return t;
            }
            return new Span<Out>(t).Slice(0, i).ToArray();
        }

        public static int[] ConvertIndex<T> ( this T[] array, Predicate<T> find )
        {
            int[] t = new int[array.Length];
            int i = 0;
            for ( int k = 0 ; k < array.Length ; k++ )
            {
                if ( find(array[k]) )
                {
                    t[i++] = k;
                }
            }
            if ( t.Length == i )
            {
                return t;
            }
            return new Span<int>(t).Slice(0, i).ToArray();
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
        public static Out[] Recursion<T, Out> ( this T[] array, Predicate<T> match, Func<T, T[], Out> converter ) where T : class
        {
            using ( PooledList<Out> list = new PooledList<Out>(array.Length) )
            {
                foreach ( T i in array )
                {
                    if ( match(i) )
                    {
                        list.Add(converter(i, array));
                    }
                }
                return list.ToArray();
            }
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
        public static Out[] Recursion<T, Out> ( this T[] array, Predicate<T> match, Func<T, T, bool> func, Func<T, Out> converter ) where T : class
        {
            using ( PooledList<Out> list = new PooledList<Out>(array.Length) )
            {
                foreach ( T i in array )
                {
                    if ( match(i) )
                    {
                        list.Add(converter(i));
                        _Recursion(array, i, func, converter, list);
                    }
                }
                return list.ToArray();
            }
        }
        private static void _Recursion<T, Out> ( T[] array, T source, Func<T, T, bool> match, Func<T, Out> converter, PooledList<Out> outs ) where T : class
        {
            foreach ( T i in array )
            {
                if ( match(source, i) )
                {
                    outs.Add(converter(i));
                    _Recursion(array, i, match, converter, outs);
                }
            }
        }

        /// <summary>
        /// 移除数组中的空成员
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public static T[] NoEmpty<T> ( this T[] array )
        {
            return array.FindAll(a => a != null);
        }
        /// <summary>
        /// 转换数组类型
        /// </summary>
        /// <typeparam name="T">成员</typeparam>
        /// <typeparam name="Out">输出成员</typeparam>
        /// <param name="array">成员数组</param>
        /// <param name="action">成员类型转换(成员，数组索引，输出成员)</param>
        /// <returns>输出成员数组</returns>
        public static Out[] ConvertAll<T, Out> ( this T[] array, Func<T, int, Out> action )
        {
            Out[] list = new Out[array.Length];
            for ( int i = 0 ; i < array.Length ; i++ )
            {
                list[i] = action(array[i], i);
            }
            return list;
        }
        public static Out[] ConvertAll<T, T1, Out> ( this T[] array, T1[] datas, Func<T, T1, Out> action )
        {
            Out[] list = new Out[array.Length * datas.Length];
            int n = 0;
            foreach ( T i in array )
            {
                foreach ( T1 k in datas )
                {
                    list[n++] = action(i, k);
                }
            }
            return list;
        }
        /// <summary>
        /// 循环数组
        /// </summary>
        /// <typeparam name="T">成员</typeparam>
        /// <param name="array">原成员数组</param>
        /// <param name="action">循环方法(成员)</param>
        public static void ForEach<T> ( this T[] array, Action<T> action )
        {
            if ( array.Length == 1 )
            {
                action(array[0]);
                return;
            }
            foreach ( T i in array )
            {
                action(i);
            }
        }
        public static bool True<TKey, TValue> ( this Dictionary<TKey, TValue> dic, Func<TKey, TValue, bool> action )
        {
            if ( dic.Count == 1 )
            {
                KeyValuePair<TKey, TValue> d = dic.First();
                return action(d.Key, d.Value);
            }
            foreach ( KeyValuePair<TKey, TValue> i in dic )
            {
                if ( !action(i.Key, i.Value) )
                {
                    return false;
                }
            }
            return true;
        }
        public static void ForEach<TKey, TValue> ( this Dictionary<TKey, TValue> dic, Action<TKey, TValue> action )
        {
            if ( dic == null )
            {
                return;
            }
            if ( dic.Count == 1 )
            {
                KeyValuePair<TKey, TValue> d = dic.First();
                action(d.Key, d.Value);
                return;
            }
            foreach ( KeyValuePair<TKey, TValue> i in dic )
            {
                action(i.Key, i.Value);
            }
        }
        public static void ForEach<TKey, TValue> ( this Dictionary<TKey, TValue> dic, Action<TKey, TValue, int> action )
        {
            if ( dic.Count == 1 )
            {
                KeyValuePair<TKey, TValue> d = dic.First();
                action(d.Key, d.Value, 0);
                return;
            }
            int index = 0;
            foreach ( KeyValuePair<TKey, TValue> i in dic )
            {
                action(i.Key, i.Value, index);
                index++;
            }
        }
        public static void ForEach<T> ( this HashSet<T> array, Action<T> action )
        {
            if ( array.Count == 1 )
            {
                action(array.First());
                return;
            }
            foreach ( T i in array )
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
        public static void ForEach<T, T1> ( this T[] array, T1[] two, Action<T, T1> action )
        {
            if ( array.Length == 1 && two.Length == 1 )
            {
                action(array[0], two[0]);
                return;
            }
            else if ( array.Length == 1 )
            {
                T i = array[0];
                foreach ( T1 k in two )
                {
                    action(i, k);
                }
            }
            else if ( two.Length == 1 )
            {
                T1 k = two[0];
                foreach ( T i in array )
                {
                    action(i, k);
                }
            }
            else
            {
                foreach ( T i in array )
                {
                    foreach ( T1 k in two )
                    {
                        action(i, k);
                    }
                }
            }
        }
        public static void ForEachByParallel<T> ( this T[] array, Action<T> action )
        {
            _ = Parallel.ForEach(array, action);
        }
        /// <summary>
        /// 循环数组
        /// </summary>
        /// <typeparam name="T">成员</typeparam>
        /// <param name="match">检查成员</param>
        /// <param name="array">原成员数组</param>
        /// <param name="action">循环方法(成员)</param>
        public static void ForEach<T> ( this T[] array, Predicate<T> match, Action<T> action )
        {
            foreach ( T i in array )
            {
                if ( match(i) )
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
        public static void ForEach<T> ( this T[] array, Action<T, int> action )
        {
            for ( int i = 0 ; i < array.Length ; i++ )
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
        public static bool TrueForAll<T> ( this T[] array, Predicate<T> match )
        {
            foreach ( T i in array )
            {
                if ( !match(i) )
                {
                    return false;
                }
            }
            return true;
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
        public static void ForEach<T, T1> ( this T[] array, T1[] datas, Func<T, T1, bool> match, Action<T, T1> result )
        {
            foreach ( T i in array )
            {
                foreach ( T1 k in datas )
                {
                    if ( match(i, k) )
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
        public static bool TrueForAll<T> ( this T[] array, Func<T, int, bool> match )
        {
            for ( int i = 0 ; i < array.Length ; i++ )
            {
                if ( !match(array[i], i) )
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
        public static T[] For<T> ( this int num, Func<int, T> fun )
        {
            T[] list = new T[num];
            for ( int i = 0 ; i < num ; i++ )
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
        public static T[] For<T> ( this int begin, int end, Func<int, T> fun )
        {
            T[] t = new T[end - begin];
            int k = 0;
            for ( int i = begin ; i < end ; i++ )
            {
                t[k++] = fun(i);
            }
            return t;
        }
        /// <summary>
        /// 循环指定次数
        /// </summary>
        /// <param name="num"></param>
        /// <param name="action"></param>
        public static void For ( this int num, Action<int> action )
        {
            for ( int i = 0 ; i < num ; i++ )
            {
                action(i);
            }
        }
        public static string ToMd5 ( this Stream stream )
        {
            if ( stream.Length <= ( _Size * 10 ) )
            {
                return Tools.GetMD5(stream.ToBytes());
            }
            return Tools.GetMD5(stream, _Size);
        }
        public static byte[] ToBytes ( this Stream stream )
        {
            stream.Position = 0;
            byte[] bytes = new byte[stream.Length];
            _ = stream.Read(bytes, 0, bytes.Length);
            return bytes;
        }
        public static byte[] ToBytes ( this Stream stream, int index, int take )
        {
            stream.Position = index * take;
            if ( ( stream.Position + take ) > stream.Length )
            {
                take = (int)( stream.Length - stream.Position );
            }
            byte[] bytes = new byte[take];
            _ = stream.Read(bytes, 0, bytes.Length);
            return bytes;
        }
        /// <summary>
        /// 循环指定次数
        /// </summary>
        /// <param name="num"></param>
        /// <param name="action"></param>
        public static void For ( this short num, Action<short> action )
        {
            if ( num == 1 )
            {
                action(0);
                return;
            }
            for ( short i = 0 ; i < num ; i++ )
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
        public static void For ( this int end, int begin, Action<int> action )
        {
            for ( int i = begin ; i <= end ; i++ )
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
        public static T[] For<T> ( this short num, Func<short, T> fun )
        {
            T[] list = new T[num];
            for ( short i = 0 ; i < num ; i++ )
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
        public static bool TrueForAll<T> ( this T[] array, PredicateFun<T> match, out string error )
        {
            string msg = null;
            if ( !array.TrueForAll(a =>
            {
                return match(a, out msg);
            }) )
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
        public static bool TrueForAll<T> ( this T[] array, ArrayTrue<T> match, out string error )
        {
            for ( int i = 0 ; i < array.Length ; i++ )
            {
                if ( !match(array[i], i, out error) )
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
        public static T[] FindAll<T> ( this T[] array, Predicate<T> match )
        {
            if ( array == null || array.Length == 0 )
            {
                return Array.Empty<T>();
            }
            using ( PooledList<T> list = [] )
            {
                foreach ( T i in array )
                {
                    if ( match(i) )
                    {
                        list.Add(i);
                    }
                }
                return list.ToArray();
            }
        }
        /// <summary>
        /// 将数组乱序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public static T[] Disorder<T> ( this T[] array )
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
        public static T[] FindAll<T, T1> ( this T[] array, T1[] datas, Func<T, T1, bool> match ) where T : class
        {
            using ( PooledList<T> list = [] )
            {
                foreach ( T i in array )
                {
                    foreach ( T1 k in datas )
                    {
                        if ( match(i, k) )
                        {
                            list.Add(i);
                            break;
                        }
                    }
                }
                return list.ToArray();
            }
        }
        public static int Sum<T, T1> ( this T[] array, T1[] datas, Func<T, T1, bool> match, Func<T, T1, int> fun ) where T : class
        {
            int num = 0;
            foreach ( T i in array )
            {
                foreach ( T1 k in datas )
                {
                    if ( match(i, k) )
                    {
                        num += fun(i, k);
                        break;
                    }
                }
            }
            return num;
        }

        public static int Sum<T> ( this T[] array, Func<T, int> fun, int def ) where T : class
        {
            if ( array == null || array.Length == 0 )
            {
                return def;
            }
            int num = 0;
            foreach ( T i in array )
            {
                num += fun(i);
            }
            return num;
        }
        public static int Sum<T> ( this T[] array, Predicate<T> match, Func<T, int> fun, int def ) where T : class
        {
            if ( array == null || array.Length == 0 )
            {
                return def;
            }
            int num = 0;
            foreach ( T i in array )
            {
                if ( match(i) )
                {
                    num += fun(i);
                }
            }
            return num;
        }
        public static long Sum<T> ( this T[] array, Func<T, long> fun, int def ) where T : class
        {
            if ( array == null || array.Length == 0 )
            {
                return def;
            }
            long num = 0;
            foreach ( T i in array )
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
        public static Result[] ConvertAll<T, T1, Result> ( this T[] array, T1[] datas, Func<T, T1, bool> match, Func<T, T1, Result> converter )
        {
            using ( PooledList<Result> list = [] )
            {
                foreach ( T i in array )
                {
                    foreach ( T1 k in datas )
                    {
                        if ( match(i, k) )
                        {
                            list.Add(converter(i, k));
                            break;
                        }
                    }
                }
                return list.ToArray();
            }
        }
        /// <summary>
        /// 查询数组中满足条件的所有成员
        /// </summary>
        /// <typeparam name="T">成员</typeparam>
        /// <param name="array">成员数组</param>
        /// <param name="datas">搜索数组</param>
        /// <param name="match">比对搜索成员和成员是否相同（成员，搜索成员，是否相同）</param>
        /// <returns></returns>
        public static T[] FindAll<T> ( this T[] array, T[] datas, Func<T, T, bool> match ) where T : class
        {
            using ( PooledList<T> list = new PooledList<T>(datas.Length) )
            {
                foreach ( T i in array )
                {
                    foreach ( T k in datas )
                    {
                        if ( match(i, k) )
                        {
                            list.Add(i);
                            break;
                        }
                    }
                }
                return list.ToArray();
            }
        }
        /// <summary>
        /// 查询数组中满足条件的所有成员
        /// </summary>
        /// <typeparam name="T">成员</typeparam>
        /// <typeparam name="T1">需要搜索的成员</typeparam>
        /// <param name="array">成员数组</param>
        /// <param name="datas">搜索数组</param>
        /// <returns></returns>
        public static T[] FindAll<T, T1> ( this T[] array, T1[] datas )
        {
            using ( PooledList<T> list = new PooledList<T>(array.Length) )
            {
                foreach ( T i in array )
                {
                    foreach ( T1 k in datas )
                    {
                        if ( i.Equals(k) )
                        {
                            list.Add(i);
                            break;
                        }
                    }
                }
                return list.ToArray();
            }
        }
        /// <summary>
        /// 查询数组中满足条件的所有成员
        /// </summary>
        /// <typeparam name="T">成员</typeparam>
        /// <param name="array">成员数组</param>
        /// <param name="datas">搜索数组</param>
        /// <returns></returns>
        public static T[] FindAll<T> ( this T[] array, T[] datas )
        {
            using ( PooledList<T> list = [] )
            {
                foreach ( T i in array )
                {
                    foreach ( T k in datas )
                    {
                        if ( i.Equals(k) )
                        {
                            list.Add(i);
                            break;
                        }
                    }
                }
                return list.ToArray();
            }
        }
        /// <summary>
        /// 检查数组中是否存在满足条件成员
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public static bool IsExists<T> ( this T[] array, Predicate<T> match )
        {
            if ( array == null )
            {
                return false;
            }
            foreach ( T i in array )
            {
                if ( match(i) )
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
        public static bool IsExists<T, T1> ( this T[] array, T1[] source, Func<T, T1, bool> match )
        {
            foreach ( T i in array )
            {
                if ( !source.IsExists(b => match(i, b)) )
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
        public static bool IsExists<T> ( this T[] array, T[] source, Func<T, T, bool> match )
        {
            foreach ( T i in array )
            {
                if ( !source.IsExists(i, match) )
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
        public static bool IsExists<T> ( this T[] array, T data )
        {
            foreach ( T a in array )
            {
                if ( a.Equals(data) )
                {
                    return true;
                }
            }
            return false;
        }
        public static bool StartsWith ( this string[] array, string str )
        {
            foreach ( string a in array )
            {
                if ( str.StartsWith(a) )
                {
                    return true;
                }
            }
            return false;
        }
        public static bool EndsWith ( this string[] array, string str )
        {
            foreach ( string a in array )
            {
                if ( str.EndsWith(a) )
                {
                    return true;
                }
            }
            return false;
        }
        public static bool IsExists ( this string[] array, string str )
        {
            foreach ( string a in array )
            {
                if ( a == str )
                {
                    return true;
                }
            }
            return false;
        }
        public static bool IsExistByEnd ( this string[] array, string str )
        {
            foreach ( string a in array )
            {
                if ( str.EndsWith(a) )
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
        public static bool IsExists<T> ( this T[] array, T data, Func<T, T, bool> match )
        {
            foreach ( T a in array )
            {
                if ( match(a, data) )
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
        public static bool IsNull<T> ( this T[] array )
        {
            return array == null || array.Length == 0;
        }
        public static bool IsNull<T> ( this T[] array, int minLen )
        {
            return array == null || array.Length < minLen;
        }
        /// <summary>
        /// 字符串强转
        /// </summary>
        /// <param name="str"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static dynamic Parse ( this string str, Type type )
        {
            return StringParseTools.Parse(str, type);
        }
        public static bool TryParse ( this string str, Type type, out dynamic res )
        {
            return StringParseTools.TryParse(str, type, out res);
        }
        public static bool TryParse<T> ( this string str, out T res )
        {
            if ( StringParseTools.TryParse(str, typeof(T), out dynamic result) )
            {
                res = (T)result;
                return true;
            }
            res = default;
            return false;
        }
        public static T Parse<T> ( this string str )
        {
            return (T)StringParseTools.Parse(str, typeof(T));
        }
        /// <summary>
        /// 检查2个相同类型数组是否相等
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool IsEqual<T> ( this T[] array, T[] data )
        {
            if ( ( array == null && data != null ) || ( array != null && data == null ) || array.Length != data.Length )
            {
                return false;
            }
            else if ( array == data || data.Length == 0 )
            {
                return true;
            }
            foreach ( T i in array )
            {
                if ( !_IsEqual(i, data) )
                {
                    return false;
                }
            }
            return true;
        }
        private static bool _IsEqual<T> ( T data, T[] array )
        {
            foreach ( T k in array )
            {
                if ( k.Equals(data) )
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 检查2个相同类型数组是否相等
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool IsEqual<T> ( this T[] array, T[] data, int begin )
        {
            if ( array == null || data == null )
            {
                return false;
            }
            foreach ( T i in array )
            {
                if ( !i.Equals(data[begin++]) )
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
        public static int EqualNum<T> ( this T[] array, T[] data, int begin )
        {
            int num = 0;
            foreach ( T i in array )
            {
                if ( i.Equals(data[begin++]) )
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
        public static bool IsEqual<T, T1> ( this T[] array, T1[] data, Func<T, T1, bool> match )
        {
            if ( array.Length != data.Length )
            {
                return false;
            }
            int num = 0;
            foreach ( T i in array )
            {
                foreach ( T1 k in data )
                {
                    if ( match(i, k) )
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
        public static bool CheckLength<T> ( this T[] array, int len )
        {
            return array != null && array.Length >= len;
        }
        /// <summary>
        /// 对象转JSON
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson<T> ( this T obj ) where T : class
        {
            return obj == null ? string.Empty : JsonTools.Json(obj);
        }
        /// <summary>
        /// JSON字符串反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T Json<T> ( this string str )
        {
            return JsonTools.Json<T>(str);
        }
        public static JsonBodyValue JsonObject ( this string str )
        {
            JsonElement ele = JsonTools.Json<JsonElement>(str);
            return new JsonBodyValue(ele);
        }
        public static ref T ReturnRef<T> ( this ref T data ) where T : struct
        {
            return ref data;
        }
        /// <summary>
        /// JSON字符串反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static object Json ( this string str, Type type )
        {
            return JsonTools.Json(str, type);
        }
    }
}
