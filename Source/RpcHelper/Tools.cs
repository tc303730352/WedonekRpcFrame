using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

using Newtonsoft.Json;

namespace RpcHelper
{
        public class Tools
        {
                private static readonly JsonSerializerSettings _Settling = new JsonSerializerSettings
                {
                        DateParseHandling = DateParseHandling.DateTime,
                        DateTimeZoneHandling = DateTimeZoneHandling.Local,
                        NullValueHandling = NullValueHandling.Ignore
                };
                static Tools()
                {
                        _Settling.Converters.Add(new Json.IJsonConverter());
                }
                /// <summary>
                /// 压缩JSON
                /// </summary>
                /// <param name="json"></param>
                /// <returns></returns>
                public static string CompressJson(string json)
                {
                        StringBuilder sb = new StringBuilder();
                        using (StringReader reader = new StringReader(json))
                        {
                                int ch = -1;
                                int lastch = -1;
                                bool isQuoteStart = false;
                                while ((ch = reader.Read()) > -1)
                                {
                                        if ((char)lastch != '\\' && (char)ch == '\"')
                                        {
                                                isQuoteStart = !isQuoteStart;
                                        }
                                        if (!char.IsWhiteSpace((char)ch) || isQuoteStart)
                                        {
                                                sb.Append((char)ch);
                                        }
                                        lastch = ch;
                                }
                        }
                        return sb.ToString().Trim();
                }

                public static void WriteText(string path, string text, Encoding encoding)
                {
                        FileInfo file = new FileInfo(path);
                        if (!file.Directory.Exists)
                        {
                                file.Directory.Create();
                        }
                        using (StreamWriter write = new StreamWriter(file.Open(FileMode.CreateNew, FileAccess.Write, FileShare.Delete), encoding))
                        {
                                write.Write(text);
                                write.Flush();
                        }
                }
                public static byte[] ReadStream(FileInfo file)
                {
                        byte[] bytes = null;
                        using (FileStream fileStream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                                bytes = new byte[fileStream.Length];
                                fileStream.Read(bytes, 0, bytes.Length);
                        }
                        return bytes;
                }
                public static byte[] ReadStream(string path)
                {
                        byte[] bytes = null;
                        using (FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                                bytes = new byte[fileStream.Length];
                                fileStream.Read(bytes, 0, bytes.Length);
                        }
                        return bytes;
                }
                public static string ReadText(string path, Encoding encoding, bool dropRn = false)
                {
                        if (!File.Exists(path))
                        {
                                return null;
                        }
                        string str = null;
                        using (StreamReader read = new StreamReader(File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Delete), encoding))
                        {
                                str = read.ReadToEnd().Trim();
                        }
                        if (!dropRn)
                        {
                                return str;
                        }
                        else if (str == string.Empty)
                        {
                                return str;
                        }
                        else
                        {
                                return new StringBuilder(str).Replace("\r\n", string.Empty).ToString();
                        }
                }

                private const double _EARTH_RADIUS = 6378.137;//地球半径

                private static readonly RNGCryptoServiceProvider _rng = new RNGCryptoServiceProvider();
                /// <summary>
                /// 创建易于SQL Server快速存储的GUID
                /// </summary>
                /// <returns></returns>
                public static Guid NewGuid()
                {
                        byte[] randomBytes = new byte[10];
                        _rng.GetBytes(randomBytes);
                        long timestamp = DateTime.UtcNow.Ticks / 10000L;
                        byte[] timestampBytes = BitConverter.GetBytes(timestamp);
                        if (BitConverter.IsLittleEndian)
                        {
                                Array.Reverse(timestampBytes);
                        }
                        byte[] guidBytes = new byte[16];
                        Buffer.BlockCopy(randomBytes, 0, guidBytes, 0, 10);
                        Buffer.BlockCopy(timestampBytes, 2, guidBytes, 10, 6);
                        return new Guid(guidBytes);
                }
                /// <summary>
                /// 按照指定规则生成GUID
                /// </summary>
                /// <param name="guidType"></param>
                /// <returns></returns>
                public static Guid NewGuid(SequentialGuidType guidType)
                {
                        byte[] randomBytes = new byte[10];
                        _rng.GetBytes(randomBytes);
                        long timestamp = DateTime.UtcNow.Ticks / 10000L;
                        byte[] timestampBytes = BitConverter.GetBytes(timestamp);
                        if (BitConverter.IsLittleEndian)
                        {
                                Array.Reverse(timestampBytes);
                        }
                        byte[] guidBytes = new byte[16];

                        switch (guidType)
                        {
                                case SequentialGuidType.SequentialAsString:
                                case SequentialGuidType.SequentialAsBinary:
                                        Buffer.BlockCopy(timestampBytes, 2, guidBytes, 0, 6);
                                        Buffer.BlockCopy(randomBytes, 0, guidBytes, 6, 10);
                                        if (guidType == SequentialGuidType.SequentialAsString && BitConverter.IsLittleEndian)
                                        {
                                                Array.Reverse(guidBytes, 0, 4);
                                                Array.Reverse(guidBytes, 4, 2);
                                        }
                                        break;

                                case SequentialGuidType.SequentialAtEnd:
                                        Buffer.BlockCopy(randomBytes, 0, guidBytes, 0, 10);
                                        Buffer.BlockCopy(timestampBytes, 2, guidBytes, 10, 6);
                                        break;
                        }
                        return new Guid(guidBytes);
                }

                public static string DecodeURI(string value)
                {
                        return HttpUtility.UrlDecode(value);
                }

                public static string HmacSha1Sign(string secret, string strOrgData)
                {
                        HMACSHA1 hmacsha1 = new HMACSHA1(Encoding.UTF8.GetBytes(secret));
                        byte[] dataBuffer = Encoding.UTF8.GetBytes(strOrgData);
                        return Convert.ToBase64String(hmacsha1.ComputeHash(dataBuffer));
                }
                private static DateTime _FormatTime(object res)
                {
                        Type type = res.GetType();
                        if (type == PublicDataDic.DateTimeType)
                        {
                                DateTime time = (DateTime)res;
                                if (time == Tools.SqlMinTime)
                                {
                                        return DateTime.MinValue;
                                }
                                else
                                {
                                        return time;
                                }
                        }
                        else if (type == PublicDataDic.DateTimeType)
                        {
                                return Tools.GetTimeStamp((long)res);
                        }
                        else if (type == PublicDataDic.StrType)
                        {
                                return DateTime.Parse((string)res);
                        }
                        return DateTime.MinValue;
                }
                public static bool IsBasicType(Type type)
                {
                        if (type.IsPrimitive)
                        {
                                return type.IsPrimitive;
                        }
                        else if (type == PublicDataDic.GuidType
                                || type == PublicDataDic.StrType
                                || type == PublicDataDic.UriType
                                || type == PublicDataDic.DateTimeType || type.IsEnum)
                        {
                                return true;
                        }
                        return false;
                }
                public static object ChangeType(Type type, object res)
                {
                        if (type.IsClass && type != PublicDataDic.StrType)
                        {
                                string json = Convert.ToString(res);
                                if (json != string.Empty)
                                {
                                        return Json(json, type);
                                }
                                return null;
                        }
                        else if (type.IsEnum)
                        {
                                return Enum.ToObject(type, res);
                        }
                        else if (type == PublicDataDic.StrType)
                        {
                                return Convert.ChangeType(res, type);
                        }
                        else if (type == PublicDataDic.DateTimeType)
                        {
                                return _FormatTime(res);
                        }
                        else if (PublicDataDic.UriType == type)
                        {
                                string uri = Convert.ToString(res);
                                if (uri != string.Empty)
                                {
                                        return new Uri(uri);
                                }
                                return null;
                        }
                        else
                        {
                                return Convert.ChangeType(res, type);
                        }
                }
                public static object StringParse(Type type, string str)
                {
                        if (type == PublicDataDic.StrType)
                        {
                                return str;
                        }
                        else if (type.IsEnum)
                        {
                                return Enum.ToObject(type, int.Parse(str));
                        }
                        else if (type == PublicDataDic.DateTimeType)
                        {
                                return DateTime.Parse(str);
                        }
                        else if (PublicDataDic.UriType == type)
                        {
                                return new Uri(str);
                        }
                        else if (PublicDataDic.GuidType == type)
                        {
                                return new Guid(str);
                        }
                        else if (type.IsGenericType || type.IsArray || type.IsClass)
                        {
                                return str.Json(type);
                        }
                        else
                        {
                                return Convert.ChangeType(str, type);
                        }
                }
                public static object GetTypeDefValue(Type type)
                {
                        if (type.IsValueType)
                        {
                                if (type.IsEnum)
                                {
                                        return Enum.GetValues(type).GetValue(0);
                                }
                                TypeCode code = Type.GetTypeCode(type);
                                return code == TypeCode.Boolean
                                        ? false
                                        : code == TypeCode.DateTime ? DateTime.MinValue : code == TypeCode.Char ? null : Convert.ChangeType(0, code);
                        }
                        else
                        {
                                return null;
                        }
                }
                public static int GetArabicNum(string chinaNum)
                {
                        char[] chars = chinaNum.ToCharArray();
                        if (chars.Length == 1)
                        {
                                char one = chars[0];
                                int val = PublicDataDic.ChinaNum.FindIndex(a => a == one);
                                if (val != -1)
                                {
                                        return val;
                                }
                                int n = PublicDataDic.ChinaNumUnit.FindIndex(a => a == one);
                                if (n == -1)
                                {
                                        return 0;
                                }
                                return (int)Math.Pow(10, n);
                        }
                        int num = 0;
                        int i = 0;
                        do
                        {
                                char one = chars[i];
                                int val = PublicDataDic.ChinaNum.FindIndex(a => a == one);
                                if (i == (chars.Length - 1))
                                {
                                        if (val != -1)
                                        {
                                                num += val;
                                        }
                                        break;
                                }
                                else if (val == -1)
                                {
                                        val = PublicDataDic.ChinaNumUnit.FindIndex(a => a == one);
                                        if (val == -1)
                                        {
                                                val = 1;
                                        }
                                        else
                                        {
                                                num += (int)Math.Pow(10, val);
                                                i += 1;
                                                continue;
                                        }
                                }
                                char unit = chars[i + 1];
                                int n = PublicDataDic.ChinaNumUnit.FindIndex(a => a == unit);
                                if (n != -1)
                                {
                                        num += val * (int)Math.Pow(10, n);
                                }
                                i += 2;
                        } while (i < chars.Length);
                        return num;
                }
                public static string GetChinaNum(long num)
                {
                        char[] chars = num.ToString().ToCharArray();
                        StringBuilder str = new StringBuilder(chars.Length * 2);
                        Array.Reverse(chars);
                        for (int i = chars.Length - 1; i >= 0; i--)
                        {
                                char val = PublicDataDic.ChinaNum[chars[i] - PublicDataDic.ZeroAscii];
                                if (i == 0 && val == '零')
                                {
                                        break;
                                }
                                str.Append(val);
                                if (i != 0 && val != '零')
                                {
                                        str.Append(PublicDataDic.ChinaNumUnit[i]);
                                }
                        }
                        return str.ToString();
                }

                /// <summary>
                /// 获取秒的时间戳
                /// </summary>
                /// <param name="now"></param>
                /// <returns></returns>
                public static long GetTimeSpan(DateTime now)
                {
                        return now == DateTime.MinValue ? 0 : (long)(now.ToUniversalTime() - _DtStart).TotalSeconds;
                }
                public static long GetTotalMilliseconds(DateTime time)
                {
                        return (long)((time.ToUniversalTime() - _DtStart).TotalMilliseconds * 1000L);
                }
                public static long GetTimeSpan()
                {
                        return (long)(DateTime.Now.ToUniversalTime() - _DtStart).TotalSeconds;
                }

                public static readonly DateTime SqlMinTime = new DateTime(1900, 1, 1);
                public static string GetClassMd5<T>(T data, params string[] removePro)
                {
                        string str = GetClassStr(typeof(T), data, removePro);
                        return Tools.GetMD5(str);
                }
                public static string GetClassMd5<T>(object data, params string[] removePro)
                {
                        string str = GetClassStr(typeof(T), data, removePro);
                        return Tools.GetMD5(str);
                }
                public static string GetClassMd5<T>(Type type, T data, params string[] removePro)
                {
                        string str = GetClassStr(typeof(T), type, data, removePro);
                        return Tools.GetMD5(str);
                }
                public static string GetClassStr<T>(T data, params string[] removePro)
                {
                        return GetClassStr(typeof(T), data, removePro);
                }
                public static string GetClassStr(Type type, Type template, object data, params string[] removePro)
                {
                        PropertyInfo[] pros = template.GetProperties().OrderBy(a => a.Name).ToArray();
                        StringBuilder str = new StringBuilder();
                        Array.ForEach(pros, a =>
                        {
                                if (removePro != null && Array.FindIndex(removePro, b => b == a.Name) != -1)
                                {
                                        return;
                                }
                                PropertyInfo pro = type.GetProperty(a.Name);
                                if (pro == null)
                                {
                                        return;
                                }
                                object val = pro.GetValue(data);
                                if (val != null)
                                {
                                        if (str.Length != 0)
                                        {
                                                str.Append("_");
                                        }
                                        if (a.PropertyType.IsArray || a.PropertyType.IsGenericType)
                                        {
                                                str.Append(Tools.Json(val));
                                        }
                                        else
                                        {
                                                str.Append(val.ToString());
                                        }
                                }
                        });
                        return str.ToString();
                }
                private static string _GetArrayStr(Type type, object data, params string[] removePro)
                {
                        if (!type.IsClass)
                        {
                                return string.Join(",", (object[])data);
                        }
                        else if (type.Name == PublicDataDic.StringTypeName)
                        {
                                return string.Join(",", (string[])data);
                        }
                        else if (type.Name == PublicDataDic.UriTypeName)
                        {
                                Uri[] uris = (Uri[])data;
                                StringBuilder t = new StringBuilder();
                                Array.ForEach(uris, a =>
                                {
                                        t.Append(",");
                                        t.Append(a.AbsoluteUri);
                                });
                                return t.Remove(0, 1).ToString();
                        }
                        else
                        {
                                StringBuilder t = new StringBuilder();
                                object[] datas = (object[])data;
                                Array.ForEach(datas, a =>
                                {
                                        t.Append(",");
                                        t.Append(GetClassStr(type, a, removePro));
                                });
                                return t.Remove(0, 1).ToString();
                        }
                }
                private static string _GetGenericStr(Type type, object data, params string[] removePro)
                {
                        if (!type.IsClass)
                        {
                                return string.Join(",", (object[])data);
                        }
                        else if (type.Name == PublicDataDic.StringTypeName)
                        {
                                return string.Join(",", (string[])data);
                        }
                        else if (type.Name == PublicDataDic.UriTypeName)
                        {
                                Uri[] uris = (Uri[])data;
                                StringBuilder t = new StringBuilder();
                                Array.ForEach(uris, a =>
                                {
                                        t.Append(",");
                                        t.Append(a.AbsoluteUri);
                                });
                                return t.Remove(0, 1).ToString();
                        }
                        else
                        {
                                StringBuilder t = new StringBuilder();
                                IEnumerable ts = (IEnumerable)data;
                                object[] datas = (object[])data;
                                foreach (object a in ts)
                                {
                                        t.Append(",");
                                        t.Append(GetClassStr(type, a, removePro));
                                }
                                return t.Remove(0, 1).ToString();
                        }
                }
                public static string GetClassStr(Type type, object data, params string[] removePro)
                {
                        if (type.IsArray)
                        {
                                return _GetArrayStr(type.GetElementType(), data, removePro);
                        }
                        else if (type.IsGenericType && type.GetInterface("System.Collections.IEnumerable") != null)
                        {
                                Type[] types = type.GetGenericArguments();
                                if (types.Length == 1)
                                {
                                        return _GetGenericStr(type, data, removePro);
                                }
                                else
                                {
                                        throw new Exception("不支持多类型泛型!");
                                }
                        }
                        else if (type.Name == PublicDataDic.UriTypeName)
                        {
                                Uri[] uris = (Uri[])data;
                                StringBuilder t = new StringBuilder();
                                Array.ForEach(uris, a =>
                                {
                                        t.Append(",");
                                        t.Append(a.AbsoluteUri);
                                });
                                return t.Remove(0, 1).ToString();
                        }
                        PropertyInfo[] pros = type.GetProperties().OrderBy(a => a.Name).ToArray();
                        StringBuilder str = new StringBuilder();
                        Array.ForEach(pros, a =>
                        {
                                if (removePro != null && Array.FindIndex(removePro, b => b == a.Name) != -1)
                                {
                                        return;
                                }
                                object val = a.GetValue(data);
                                if (val != null)
                                {
                                        if (str.Length != 0)
                                        {
                                                str.Append("_");
                                        }
                                        if (a.PropertyType.IsArray || a.PropertyType.IsGenericType)
                                        {
                                                str.Append(Tools.Json(val));
                                        }
                                        else
                                        {
                                                str.Append(val.ToString());
                                        }
                                }
                        });
                        return str.ToString();
                }
                public static string GetClassMd5<T>(T data)
                {
                        string str = GetClassStr(typeof(T), data);
                        return Tools.GetMD5(str);
                }
                public static short GetZoneIndex(string str)
                {
                        char[] chars = str.ToArray();
                        return (short)(Tools.GetOneAscii(chars, 0) | Tools.GetOneAscii(chars, chars.Length - 1));
                }
                public static short GetRightZoneIndex(string str)
                {
                        char[] chars = str.Substring(str.Length - 2, 2).ToCharArray();
                        return (short)(Tools.GetOneAscii(chars, 0) | Tools.GetOneAscii(chars, 1));
                }
                public static short GetZoneIndex(char[] chars, int one, int two)
                {
                        return (short)(Tools.GetOneAscii(chars, one) | Tools.GetOneAscii(chars, two));
                }
                private static readonly DateTime _DtStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);


                public static T CreateObject<T>(string path, string className)
                {
                        if (!Path.IsPathRooted(path) && path.StartsWith(@"\"))
                        {
                                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
                        }
                        Assembly assembly = _LoadAssembly(path);
                        if (assembly == null)
                        {
                                return default;
                        }
                        return (T)assembly.CreateInstance(className, false);
                }
                public static T CreateObject<T>(string path)
                {
                        if (!path.EndsWith(".dll"))
                        {
                                path = path + ".dll";
                        }
                        if (!Path.IsPathRooted(path) && path.StartsWith(@"\"))
                        {
                                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
                        }
                        Assembly assembly = _LoadAssembly(path);
                        if (assembly == null)
                        {
                                return default;
                        }
                        Type iType = typeof(T);
                        Type type = assembly.GetTypes().Where(a => a.IsPublic && a.GetInterface(iType.FullName) != null).FirstOrDefault();
                        if (type != null)
                        {
                                return (T)assembly.CreateInstance(type.FullName, false);
                        }
                        return default;
                }
                public static T CreateObject<T>(Assembly assembly)
                {
                        Type iType = typeof(T);
                        Type type = assembly.GetTypes().Where(a => a.GetInterface(iType.FullName) != null).FirstOrDefault();
                        if (type != null)
                        {
                                return (T)assembly.CreateInstance(type.FullName, false);
                        }
                        return default;
                }
                public static bool CheckAssembly(string path)
                {
                        if (!File.Exists(path))
                        {
                                return false;
                        }
                        AssemblyName name = AssemblyName.GetAssemblyName(path);
                        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
                        return Array.FindIndex(assemblies, a =>
                        {
                                AssemblyName t = a.GetName();
                                return t.Name == name.Name && t.Version.ToString() == name.Version.ToString();
                        }) == -1;
                }
                public static Assembly LoadAssembly(FileInfo file)
                {
                        if (!file.Exists)
                        {
                                return null;
                        }
                        byte[] dllByte = null;
                        using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Delete))
                        {
                                dllByte = new byte[stream.Length];
                                stream.Read(dllByte, 0, dllByte.Length);
                                stream.Close();
                                stream.Dispose();
                        }
                        return AppDomain.CurrentDomain.Load(dllByte);
                }
                private static Assembly LoadAssembly(string name)
                {
                        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, name);
                        return _LoadAssembly(path);
                }
                private static Assembly _LoadAssembly(string path)
                {
                        if (!File.Exists(path))
                        {
                                return null;
                        }
                        AssemblyName name = AssemblyName.GetAssemblyName(path);
                        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
                        Assembly assembly = Array.Find(assemblies, a =>
                        {
                                AssemblyName t = a.GetName();
                                return t.Name == name.Name && t.Version.ToString() == name.Version.ToString();
                        });
                        if (assembly != null)
                        {
                                return assembly;
                        }
                        byte[] dllByte = null;
                        using (FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Delete))
                        {
                                dllByte = new byte[stream.Length];
                                stream.Read(dllByte, 0, dllByte.Length);
                                stream.Close();
                                stream.Dispose();
                        }
                        return AppDomain.CurrentDomain.Load(dllByte);
                }

                /// <summary>
                /// 获取对象属性值(反射获取)
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="data"></param>
                /// <param name="attrName"></param>
                /// <returns></returns>
                public static T GetObjectAttrVal<T>(object data, string attrName)
                {
                        Type type = data.GetType();
                        object val = null;
                        PropertyInfo pro = type.GetProperty(attrName);
                        if (pro != null)
                        {
                                val = pro.GetValue(data);
                        }
                        else
                        {
                                FieldInfo field = type.GetField(attrName);
                                if (field != null)
                                {
                                        val = field.GetValue(data);
                                }
                        }
                        return (T)Convert.ChangeType(val, typeof(T));
                }
                /// <summary>
                /// 获取对象属性值(反射获取)
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="data"></param>
                /// <param name="attrName"></param>
                /// <returns></returns>
                public static object GetObjectAttrVal(object data, string attrName)
                {
                        Type type = data.GetType();
                        PropertyInfo pro = type.GetProperty(attrName);
                        if (pro != null)
                        {
                                return pro.GetValue(data);
                        }
                        else
                        {
                                FieldInfo field = type.GetField(attrName);
                                if (field != null)
                                {
                                        return field.GetValue(data);
                                }
                        }
                        return null;
                }
                public static object GetObjectAttrVal(object data, string attrName, out Type proType)
                {
                        Type type = data.GetType();
                        PropertyInfo pro = type.GetProperty(attrName);
                        if (pro != null)
                        {
                                proType = pro.PropertyType;
                                return pro.GetValue(data);
                        }
                        else
                        {
                                FieldInfo field = type.GetField(attrName);
                                if (field != null)
                                {
                                        proType = field.FieldType;
                                        return field.GetValue(data);
                                }
                        }
                        proType = null;
                        return null;
                }

                /// <summary>
                /// 获取用户性别 0 男 1 女
                /// </summary>
                /// <param name="cardId"></param>
                /// <returns></returns>
                public static int GetUserSexByCardID(string cardId)
                {
                        int sex = cardId.Length == 15 ? int.Parse(cardId.Substring(14, 1)) : int.Parse(cardId.Substring(16, 1));
                        return sex % 2 == 0 ? 1 : 0;
                }
                /// <summary>
                /// 获取身份证中的生日
                /// </summary>
                /// <param name="cardId"></param>
                /// <returns></returns>
                public static DateTime GetBirthdayByCardID(string cardId)
                {
                        string val = cardId.Substring(6, 8).Insert(4, "-").Insert(7, "-");
                        if (DateTime.TryParse(val, out DateTime date))
                        {
                                return date;
                        }
                        return DateTime.MinValue;
                }
                public static bool RunCmd(string cmd, out string res)
                {
                        return RunCmd(new string[] { cmd }, out res);
                }
                /// <summary>
                /// 运行Cmd命令
                /// </summary>
                /// <param name="cmd"></param>
                /// <param name="res"></param>
                /// <returns></returns>
                public static bool RunCmd(string[] cmd, out string res)
                {
                        res = null;
                        using (Process process = new Process
                        {
                                EnableRaisingEvents = false,
                        })
                        {
                                process.StartInfo.CreateNoWindow = true;
                                process.StartInfo.FileName = "cmd.exe";
                                process.StartInfo.UseShellExecute = false;
                                process.StartInfo.RedirectStandardError = true;
                                process.StartInfo.RedirectStandardInput = true;
                                process.StartInfo.RedirectStandardOutput = true;
                                process.StartInfo.ErrorDialog = false;
                                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                                process.StartInfo.LoadUserProfile = true;
                                if (process.Start())
                                {
                                        cmd.ForEach(a =>
                                        {
                                                process.StandardInput.WriteLine(a);
                                        });
                                        //System.Threading.Thread.Sleep(3000);
                                        process.StandardInput.WriteLine("exit");
                                        res = process.StandardOutput.ReadToEnd();
                                        process.Close();
                                        return true;
                                }
                        }
                        return false;
                }
                public static bool RunExeFile(string execPath, string param, out string res)
                {
                        res = null;
                        using (Process process = new Process
                        {
                                EnableRaisingEvents = false,
                        })
                        {
                                process.StartInfo.Arguments = param;
                                process.StartInfo.CreateNoWindow = true;
                                process.StartInfo.FileName = execPath;
                                process.StartInfo.UseShellExecute = false;
                                process.StartInfo.RedirectStandardError = true;
                                process.StartInfo.RedirectStandardInput = true;
                                process.StartInfo.RedirectStandardOutput = true;
                                process.StartInfo.ErrorDialog = false;
                                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                                if (process.Start())
                                {
                                        res = process.StandardOutput.ReadToEnd();
                                        process.Close();
                                        return true;
                                }
                        }
                        return false;
                }
                /// <summary>
                /// 获取程序集的GUID值
                /// </summary>
                /// <returns></returns>
                public static Guid GetProcedureGuid()
                {
                        Attribute guid_attr = Attribute.GetCustomAttribute(System.Reflection.Assembly.GetEntryAssembly(), typeof(System.Runtime.InteropServices.GuidAttribute));
                        return guid_attr == null ? Guid.Empty : new Guid(((System.Runtime.InteropServices.GuidAttribute)guid_attr).Value);
                }
                /// <summary>
                /// 获取字符串首位的ASCII值
                /// </summary>
                /// <param name="str"></param>
                /// <returns></returns>
                public static short GetOneAscii(string str)
                {
                        byte[] myByte = Encoding.ASCII.GetBytes(str);
                        if (myByte.Length >= 1)
                        {
                                return myByte[0];
                        }
                        return 0;
                }
                /// <summary>
                /// 将手机号码中间五位做隐藏
                /// </summary>
                /// <param name="phone"></param>
                /// <returns></returns>
                public static string HidePhone(string phone)
                {
                        phone = phone.Remove(phone.Length - 8, 5);
                        return phone.Insert(phone.Length - 3, "*****");
                }
                public static string HideIDCard(string cardId)
                {
                        cardId = cardId.Remove(6, 8);
                        return cardId.Insert(6, "********");
                }
                /// <summary>
                /// 获取IP的数字表示形式
                /// </summary>
                /// <param name="ip"></param>
                /// <returns></returns>
                public static long GetIpNum(string ip)
                {
                        string[] items = ip.Split('.');
                        return (long.Parse(items[0]) << 24) | (long.Parse(items[1]) << 16) | (long.Parse(items[2]) << 8) | long.Parse(items[3]);
                }
                /// <summary>
                /// 获取字符串中某位的ASCII码
                /// </summary>
                /// <param name="str"></param>
                /// <param name="index"></param>
                /// <returns></returns>
                public static short GetOneAscii(string str, int index)
                {
                        byte[] myByte = Encoding.ASCII.GetBytes(str);
                        return myByte.Length > index ? myByte[index] : (short)0;
                }
                /// <summary>
                /// 生成随机的MAC
                /// </summary>
                /// <returns></returns>
                public static string GetRandomMac()
                {
                        string mac = Guid.NewGuid().ToString("N");
                        int index = GetRandom(0, 17);
                        mac = mac.Substring(index, 12);
                        return Tools.FormatMac(mac);
                }

                public static string RandomLowerStr(int len)
                {
                        StringBuilder str = new StringBuilder(64);
                        str.Append(Guid.NewGuid().ToString("N").ToLower());
                        str.Append(Guid.NewGuid().ToString("N").ToLower());
                        int index = GetRandom(0, str.Length - len);
                        return str.ToString().Substring(index, len);
                }
                public static string GetRandomStr(int len, params string[] removeStr)
                {
                        StringBuilder str = null;
                        if (len <= 128)
                        {
                                str = _GetRandomStr();
                        }
                        else
                        {
                                str = _GetRandomLenStr(len, removeStr != null);
                        }
                        if (removeStr != null)
                        {
                                Array.ForEach(removeStr, a =>
                                {
                                        str = str.Replace(a, string.Empty);
                                });
                        }
                        int index = GetRandom(0, str.Length - len);
                        return str.ToString().Substring(index, len);
                }
                private static StringBuilder _GetRandomLenStr(int len, bool isAdd)
                {
                        if (isAdd)
                        {
                                len = (int)Math.Ceiling((len * 1.4) / 32) * 32;
                        }
                        else
                        {
                                len = (int)Math.Ceiling(len / 32.0) * 32;
                        }
                        StringBuilder str = new StringBuilder(len);
                        len.For(a =>
                        {
                                if (a % 2 == 0)
                                {
                                        str.Append(Guid.NewGuid().ToString("N").ToLower());
                                        return;
                                }
                                str.Append(Guid.NewGuid().ToString("N"));
                        });
                        return str;
                }
                private static StringBuilder _GetRandomStr()
                {
                        StringBuilder str = new StringBuilder(128);
                        str.Append(Guid.NewGuid().ToString("N"));
                        str.Append(Guid.NewGuid().ToString("N").ToLower());
                        str.Append(Guid.NewGuid().ToString("N"));
                        str.Append(Guid.NewGuid().ToString("N").ToLower());
                        return str;
                }
                /// <summary>
                /// 获取首位的ASCII码
                /// </summary>
                /// <param name="chars"></param>
                /// <returns></returns>
                public static short GetOneAscii(char[] chars)
                {
                        return Encoding.ASCII.GetBytes(chars, 0, 1)[0];
                }
                /// <summary>
                /// 计算信号距离
                /// </summary>
                /// <param name="rssi"></param>
                /// <param name="a"></param>
                /// <param name="n"></param>
                /// <returns></returns>
                internal static double RssitoDistance(int rssi, int a, double n)
                {
                        return Math.Pow(10, (Math.Abs(rssi) - a) / (10.0 * n));
                }
                private static double _rad(double d)
                {
                        return d * Math.PI / 180.0;
                }
                /// <summary>
                /// 计算距离
                /// </summary>
                /// <param name="lat1">纬度(30.***.)</param>
                /// <param name="lng1">经度(100.***)</param>
                /// <param name="lat2">纬度</param>
                /// <param name="lng2">经度</param>
                /// <returns></returns>
                public static int GetDistance(double lat1, double lng1, double lat2, double lng2)
                {
                        return _GetDistance(new double[]
                        {
                               lat1,
                               lng1
                        }, new double[]
                        {
                               lat2,
                               lng2
                        });
                }
                public static DateTime PaseDateTime(string time)
                {
                        StringBuilder str = new StringBuilder(time);
                        if (time.Length == 8)
                        {
                                str.Insert(4, "-").Insert(7, "-");
                        }
                        else if (time.Length == 14)
                        {
                                str.Insert(4, "-").Insert(7, "-").Insert(10, " ").Insert(13, ":").Insert(16, ":");
                        }
                        else if (time.Length > 14)
                        {
                                str.Insert(4, "-").Insert(7, "-").Insert(10, " ").Insert(13, ":").Insert(16, ":");
                        }
                        else
                        {
                                return DateTime.MinValue;
                        }
                        return DateTime.Parse(str.ToString());
                }
                private static double[] _FormatGps(double lat, double lng, GpsType type, GpsType change)
                {
                        if (type == change)
                        {
                                return new double[]
                                {
                                        lat,
                                        lng,
                                };
                        }
                        else if (type == GpsType.百度坐标系BD09 && change == GpsType.GPS坐标)
                        {
                                return GpsUtil.Bd09ToGps84(lat, lng);
                        }
                        else if (type == GpsType.GPS坐标 && change == GpsType.百度坐标系BD09)
                        {
                                return GpsUtil.Gps84ToBd09(lat, lng);
                        }
                        else if (type == GpsType.GPS坐标 && change == GpsType.高德谷歌GCJ02)
                        {
                                return GpsUtil.Gps84ToGcj02(lat, lng);
                        }
                        else
                        {
                                return type == GpsType.百度坐标系BD09 && change == GpsType.高德谷歌GCJ02
                                        ? GpsUtil.Bd09ToGcj02(lat, lng)
                                        : type == GpsType.高德谷歌GCJ02 && change == GpsType.GPS坐标 ? GpsUtil.Gcj02ToGps84(lat, lng) : GpsUtil.Gcj02ToBd09(lat, lng);
                        }
                }
                public static int GetDistance(double lat1, double lng1, GpsType type, double lat2, double lng2, GpsType twoType)
                {
                        double[] one = new double[]
                        {
                                lat1,
                                lng1
                        };
                        double[] two = _FormatGps(lat2, lng2, type, twoType);
                        return _GetDistance(one, two);
                }
                public static int GetDistance(decimal lat1, decimal lng1, GpsType type, decimal lat2, decimal lng2, GpsType twoType)
                {
                        double[] two = new double[]
                        {
                                 decimal.ToDouble(lat2),
                                decimal.ToDouble(lng2),
                        };
                        double[] one = _FormatGps(decimal.ToDouble(lat1), decimal.ToDouble(lng1), type, twoType);
                        return _GetDistance(one, two);
                }
                /// <summary>
                /// 计算距离
                /// </summary>
                /// <param name="lat1">纬度(30.***.)</param>
                /// <param name="lng1">经度(100.***)</param>
                /// <param name="lat2">纬度</param>
                /// <param name="lng2">经度</param>
                /// <returns></returns>
                public static int GetDistance(decimal lat1, decimal lng1, decimal lat2, decimal lng2)
                {
                        double[] one = new double[]
                        {
                                decimal.ToDouble(lat1),
                                decimal.ToDouble(lng1),
                        };
                        double[] two = new double[]
                         {
                                decimal.ToDouble(lat2),
                                decimal.ToDouble(lng2),
                        };
                        return _GetDistance(one, two);
                }
                private static int _GetDistance(double[] one, double[] two)
                {
                        double[] point = new double[2];
                        for (int i = 0; i < one.Length; i++)
                        {
                                one[i] = _rad(one[i]);
                                two[i] = _rad(two[i]);
                                point[i] = Math.Abs(one[i] - two[i]);
                        }
                        double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(point[0] / 2), 2) +
                         (Math.Cos(one[0]) * Math.Cos(two[0]) * Math.Pow(Math.Sin(point[1] / 2), 2))));
                        s *= _EARTH_RADIUS;
                        return (int)Math.Round(s * 1000);
                }

                public static TimeIntervalType GetTimeIntervalType()
                {
                        return GetTimeIntervalType(DateTime.Now);
                }
                /// <summary>
                /// 获取时段类型
                /// </summary>
                /// <returns></returns>
                public static TimeIntervalType GetTimeIntervalType(DateTime time)
                {
                        int hour = time.Hour;
                        if (hour >= 5 && hour < 8)
                        {
                                return TimeIntervalType.早晨;
                        }
                        else if (hour >= 8 && hour < 12)
                        {
                                return TimeIntervalType.上午;
                        }
                        else if (hour >= 12 && hour < 14)
                        {
                                return TimeIntervalType.中午;
                        }
                        else if (hour >= 14 && hour < 17)
                        {
                                return TimeIntervalType.下午;
                        }
                        else if (hour >= 17 && hour < 19)
                        {
                                return TimeIntervalType.傍晚;
                        }
                        else if (hour >= 19 && hour < 22)
                        {
                                return TimeIntervalType.晚上;
                        }
                        else if (hour >= 22 || hour < 3)
                        {
                                return TimeIntervalType.深夜;
                        }
                        return TimeIntervalType.凌晨;
                }
                private static readonly string[] _EscapeChar = new string[] { "\r", "\n", "\t", "\0", "\f", "\a", "\b", "\v" };

                public static string DropEscapeChar(string str)
                {
                        if (string.IsNullOrEmpty(str))
                        {
                                return string.Empty;
                        }
                        StringBuilder t = new StringBuilder(str);
                        Array.ForEach(_EscapeChar, a =>
                        {
                                t.Replace(a, string.Empty);
                        });
                        return t.ToString().Trim();
                }
                /// <summary>
                /// 获取时间间隔的天数
                /// </summary>
                /// <param name="begin"></param>
                /// <returns></returns>
                public static int GetTimeByDay(DateTime begin)
                {
                        return (int)(DateTime.Now - begin).TotalDays;
                }
                /// <summary>
                /// 压缩数据
                /// </summary>
                /// <param name="data">数据流</param>
                /// <returns>压缩的数据</returns>
                public static byte[] CompressionData(byte[] data)
                {
                        using (MemoryStream fs = new MemoryStream())//创建文件
                        {
                                using (GZipStream gzipStream = new GZipStream(fs, CompressionMode.Compress, true))//创建压缩对象
                                {
                                        gzipStream.Write(data, 0, data.Length);
                                        gzipStream.Flush();
                                        gzipStream.Close();
                                }
                                return fs.ToArray();
                        }
                }
                public static void AutoCompression<T>(T data, FileInfo file)
                {
                        string json = Tools.Json(data);
                        byte[] bytes = AutoCompressionData(Encoding.UTF8.GetBytes(json));
                        if (!file.Directory.Exists)
                        {
                                file.Directory.Create();
                        }
                        using (FileStream stream = file.Open(FileMode.Create, FileAccess.ReadWrite, FileShare.Delete))
                        {
                                stream.Write(bytes, 0, bytes.Length);
                                stream.Flush();
                        }
                }
                public static T AutoDecompression<T>(FileInfo file)
                {
                        byte[] bytes = null;
                        using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Delete))
                        {
                                stream.Position = 0;
                                bytes = new byte[stream.Length];
                                stream.Write(bytes, 0, bytes.Length);
                        }
                        bytes = AutoDecompression(bytes);
                        return Json<T>(Encoding.UTF8.GetString(bytes));
                }
                public static byte[] AutoCompression<T>(T data)
                {
                        string json = Tools.Json(data);
                        return AutoCompressionData(Encoding.UTF8.GetBytes(json));
                }
                public static T AutoDecompression<T>(byte[] data)
                {
                        byte[] bytes = AutoDecompression(data);
                        if (bytes == null)
                        {

                        }
                        return Tools.Json<T>(Encoding.UTF8.GetString(bytes));
                }
                /// <summary>
                /// 压缩数据
                /// </summary>
                /// <param name="data">数据流</param>
                /// <returns>压缩的数据</returns>
                public static byte[] AutoCompressionData(byte[] data)
                {
                        using (MemoryStream fs = new MemoryStream())//创建文件
                        {
                                fs.Write(BitConverter.GetBytes(data.Length), 0, 4);
                                using (GZipStream gzipStream = new GZipStream(fs, CompressionMode.Compress, true))//创建压缩对象
                                {
                                        gzipStream.Write(data, 0, data.Length);
                                        gzipStream.Flush();
                                        gzipStream.Close();
                                        return fs.ToArray();
                                }
                        }
                }
                public static byte[] AutoCompressionData(byte[] data, byte[] stream)
                {
                        using (MemoryStream fs = new MemoryStream(stream))//创建文件
                        {
                                fs.Write(BitConverter.GetBytes(data.Length), 0, 4);
                                using (GZipStream gzipStream = new GZipStream(fs, CompressionMode.Compress, true))//创建压缩对象
                                {
                                        gzipStream.Write(data, 0, data.Length);
                                        gzipStream.Flush();
                                        gzipStream.Close();
                                        return stream.ToArray();
                                }
                        }
                }
                /// <summary>
                /// 解压数据
                /// </summary>
                /// <param name="data">压缩的数据</param>
                /// <param name="originalLen">原始字节长度</param>
                /// <returns></returns>
                public static byte[] AutoDecompression(byte[] data)
                {
                        int len = BitConverter.ToInt32(data, 0);
                        int size = data.Length - 4;
                        if (len >= (size * 35))
                        {
                                return null;
                        }
                        return Decompression(data, 4, size, len);
                }
                public static byte[] AutoDecompression(byte[] data, int index)
                {
                        int len = BitConverter.ToInt32(data, index);
                        index += 4;
                        int size = data.Length - index;
                        if (len >= (size * 35))
                        {
                                return null;
                        }
                        return Decompression(data, index, size, len);
                }
                /// <summary>
                /// 压缩字符串
                /// </summary>
                /// <param name="str">压缩的字符串</param>
                /// <param name="len">返回原始字节长度</param>
                /// <returns></returns>
                public static byte[] CompressionData(string str, out long len)
                {
                        byte[] data = Encoding.UTF8.GetBytes(str);
                        len = data.LongLength;
                        return CompressionData(data);
                }
                /// <summary>
                /// 解压数据
                /// </summary>
                /// <param name="data">压缩的数据</param>
                /// <param name="originalLen">原始字节长度</param>
                /// <returns></returns>
                public static string Decompression(byte[] data, long originalLen)
                {
                        byte[] myByte = Decompression(data, 0, data.Length, originalLen);
                        return myByte != null ? Encoding.UTF8.GetString(myByte) : null;
                }
                public static byte[] DecompressionData(byte[] data, long originalLen)
                {
                        return Decompression(data, 0, data.Length, originalLen);
                }
                /// <summary>
                /// 解压缩数据
                /// </summary>
                /// <param name="data">压缩的数据流</param>
                /// <param name="index">数据开始的索引</param>
                /// <param name="size">数据大小</param>
                /// <param name="originalLen">解压后的原始大小</param>
                /// <returns>解压后的数据流</returns>
                public static byte[] Decompression(byte[] data, int index, int size, long originalLen)
                {
                        try
                        {
                                byte[] tempByte = new byte[originalLen];
                                using (MemoryStream fs = new MemoryStream(size))
                                {
                                        fs.Write(data, index, size);
                                        fs.Seek(0, SeekOrigin.Begin);
                                        using (GZipStream gzipStream = new GZipStream(fs, CompressionMode.Decompress, true))//创建压缩对象
                                        {
                                                tempByte = new byte[originalLen];
                                                gzipStream.Read(tempByte, 0, tempByte.Length);
                                        }
                                        fs.Close();
                                }
                                return tempByte;
                        }
                        catch (Exception e)
                        {
                                new ErrorLog(e) { LogTitle = "解压缩失败!" }.Save();
                                return null;
                        }
                }
                public static byte[] Decompression(byte[] data, int index, int size)
                {
                        byte[] tempByte = new byte[4096];
                        using (MemoryStream fs = new MemoryStream(size))
                        {
                                fs.Write(data, index, size);
                                fs.Seek(0, SeekOrigin.Begin);
                                using (MemoryStream output = new MemoryStream())
                                {
                                        using (GZipStream gzipStream = new GZipStream(fs, CompressionMode.Decompress, true))//创建压缩对象
                                        {
                                                do
                                                {
                                                        int len = gzipStream.Read(tempByte, 0, tempByte.Length);
                                                        if (len > 0)
                                                        {
                                                                output.Write(tempByte, 0, len);
                                                        }
                                                        else
                                                        {
                                                                break;
                                                        }
                                                } while (true);
                                        }
                                        fs.Close();
                                        return output.ToArray();
                                }
                        }
                }
                /// <summary>
                /// Basic64解码
                /// </summary>
                /// <param name="code"></param>
                /// <returns></returns>
                public static string DecodeBase64(string code)
                {
                        if (code.IndexOf("%3D") != -1)
                        {
                                code = code.Replace("%3D", "=");
                        }
                        if (code.Length % 4 == 3)
                        {
                                code += "=";
                        }
                        byte[] bytes = Convert.FromBase64String(code);
                        return Encoding.UTF8.GetString(bytes);
                }
                /// <summary>
                /// 获取手机型号
                /// </summary>
                /// <param name="uAgent"></param>
                /// <returns></returns>
                public static string GetMobileModel(string uAgent)
                {
                        if (uAgent == null)
                        {
                                return "Android";
                        }
                        uAgent = uAgent.ToLower();
                        if (uAgent.IndexOf("itunes-ipad") != -1 || uAgent.IndexOf("(ipad;") != -1 || uAgent.IndexOf(";ipad;") != -1)
                        {
                                return "ipad";
                        }
                        if (uAgent.IndexOf("iphone5s") != -1 || uAgent.IndexOf("(iphone;") != -1 || uAgent.IndexOf("iph os ") != -1)
                        {
                                return "iphone";
                        }
                        if (uAgent.IndexOf(" android ") != -1 || uAgent.IndexOf(" android/") != -1)
                        {
                                return "Android";
                        }
                        return uAgent.IndexOf(" windows phone ") != -1 ? "Windows Phone" : string.Empty;
                }
                /// <summary>
                /// SHA加密
                /// </summary>
                /// <param name="content"></param>
                /// <returns></returns>
                public static string Sha1(string content)
                {
                        return Sha1(content, Encoding.UTF8);
                }
                public static string Sha1ToBase64(string content, Encoding encode)
                {
                        byte[] myByte = Sha1(encode.GetBytes(content));
                        return Convert.ToBase64String(myByte);
                }
                /// <summary>
                /// SHA加密
                /// </summary>
                /// <param name="content"></param>
                /// <param name="encode"></param>
                /// <returns></returns>
                public static byte[] Sha1(byte[] mybytes)
                {
                        try
                        {
                                using (SHA1 sha1 = SHA1.Create())
                                {
                                        return sha1.ComputeHash(mybytes);
                                }
                        }
                        catch (Exception)
                        {
                                return null;
                        }
                }
                /// <summary>
                /// SHA加密
                /// </summary>
                /// <param name="content"></param>
                /// <param name="encode"></param>
                /// <returns></returns>
                public static string Sha1(string content, Encoding encode)
                {
                        byte[] bytes_out = Sha1(encode.GetBytes(content));
                        string result = BitConverter.ToString(bytes_out);
                        result = result.Replace("-", string.Empty);
                        return result;
                }
                /// <summary>
                /// 获取随机数
                /// </summary>
                /// <returns></returns>
                public static int GetRandom()
                {
                        return new Random((int)(DateTime.Now.Ticks & 0xffffffffL) | (int)(DateTime.Now.Ticks >> 32)).Next();
                }
                public static int GetRandom(int min, int max)
                {
                        if (min == max)
                        {
                                return min;
                        }
                        return new Random((int)(DateTime.Now.Ticks & 0xffffffffL) | (int)(DateTime.Now.Ticks >> 32)).Next(min, max);
                }

                /// <summary>
                /// 将内容MD5后basic64加密
                /// </summary>
                /// <param name="content"></param>
                /// <returns></returns>
                public static string GetMD5ToBase64(string content)
                {
                        byte[] myByte = null;
                        using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
                        {
                                myByte = md5.ComputeHash(Encoding.ASCII.GetBytes(content));
                        }
                        return Convert.ToBase64String(myByte);
                }
                /// <summary>
                /// 获取真实金额
                /// </summary>
                /// <param name="money"></param>
                /// <returns></returns>
                public static decimal GetRealDataMoney(decimal money)
                {
                        return money / 100;
                }
                /// <summary>
                /// 获取真实金额
                /// </summary>
                /// <param name="money"></param>
                /// <param name="unit"></param>
                /// <returns></returns>
                public static decimal GetRealDataMoney(decimal money, MoneyUnit unit)
                {
                        if (unit == MoneyUnit.分)
                        {
                                return money / 100;
                        }
                        else if (unit == MoneyUnit.角)
                        {
                                return money / 10;
                        }
                        else if (unit == MoneyUnit.里)
                        {
                                return money / 1000;
                        }
                        else if (unit == MoneyUnit.毫)
                        {
                                return money / 10000;
                        }
                        return money;
                }
                /// <summary>
                /// 获取MD5
                /// </summary>
                /// <param name="content"></param>
                /// <returns></returns>
                public static string GetMD5(byte[] content)
                {
                        string resule = null;
                        using (System.Security.Cryptography.MD5CryptoServiceProvider get_md5 = new System.Security.Cryptography.MD5CryptoServiceProvider())
                        {
                                byte[] hash_byte = get_md5.ComputeHash(content);
                                resule = System.BitConverter.ToString(hash_byte).Replace("-", string.Empty);
                        }
                        return resule;
                }
                /// <summary>
                /// 将MAC地址格式成统一格式:00:00:00:00:00:00
                /// </summary>
                /// <param name="mac"></param>
                /// <returns></returns>
                public static string FormatMac(string mac)
                {
                        if (mac == null)
                        {
                                return string.Empty;
                        }
                        if (mac.Length == 12)
                        {
                                return mac.ToLower().Insert(2, ":").Insert(5, ":").Insert(8, ":").Insert(11, ":").Insert(14, ":");
                        }
                        mac = mac.Replace("-", ":").ToLower().Trim();
                        string[] temp = mac.Split(':');
                        StringBuilder macInfo = new StringBuilder(17);
                        foreach (string i in temp)
                        {
                                if (macInfo.Length != 0)
                                {
                                        macInfo.Append(":");
                                }
                                if (i.Length == 1)
                                {
                                        macInfo.Append(i.PadLeft(2, '0'));
                                }
                                else if (i.Length == 4)
                                {
                                        macInfo.Append(i.Insert(2, ":"));
                                }
                                else
                                {
                                        macInfo.Append(i);
                                }
                        }
                        return macInfo.ToString();
                }
                /// <summary>
                /// 判断请求的类型
                /// </summary>
                /// <param name="uagent"></param>
                /// <returns></returns>
                public static RequestType GetRequestType(string uagent)
                {
                        if (ValidateHelper.CheckIsPc(uagent))
                        {
                                return RequestType.PC;
                        }
                        if (uagent == null)
                        {
                                return RequestType.未知;
                        }
                        string str = uagent.ToLower();
                        return CheckIsWxBrowser(str) ? RequestType.微信 : RequestType.移动设备;
                }
                /// <summary>
                /// 判断请求的设备类型
                /// </summary>
                /// <param name="uagent"></param>
                /// <returns></returns>
                public static DevType GetDevType(string uagent)
                {
                        if (ValidateHelper.CheckIsPc(uagent))
                        {
                                return DevType.PC;
                        }
                        string str = uagent.ToLower();
                        if (str.IndexOf("android") != -1)
                        {
                                return DevType.安卓;
                        }
                        if (str.IndexOf("ipad") != -1)
                        {
                                return DevType.IPad;
                        }
                        if (str.IndexOf("windows phone") != -1)
                        {
                                return DevType.WindowsPhone;
                        }
                        if (str.IndexOf("iphone") != -1)
                        {
                                return DevType.苹果;
                        }
                        if (str.StartsWith("captivenetworksupport-"))
                        {
                                return DevType.苹果;
                        }
                        if (Regex.IsMatch(str, @"^wifi.+[ ]cfnetwork[/]\d+([.]\d+)*[ ]darwin[/]\d+([.]\d+)*$"))
                        {
                                return DevType.苹果;
                        }
                        return DevType.未知;
                }
                /// <summary>
                /// 获取请求的设备信息
                /// </summary>
                /// <param name="uagent"></param>
                /// <param name="devName"></param>
                /// <param name="requestType"></param>
                /// <returns></returns>
                public static DevType GetDevInfo(string uagent, out string devName, ref RequestType requestType)
                {
                        DevType devType = GetDevInfo(uagent, out devName);
                        requestType = devType == DevType.PC ? RequestType.PC : CheckIsWxBrowser(uagent) ? RequestType.微信 : RequestType.移动设备;
                        return devType;
                }
                /// <summary>
                /// 获取请求的设备信息
                /// </summary>
                /// <param name="uagent"></param>
                /// <param name="devName"></param>
                /// <returns></returns>
                public static DevType GetDevInfo(string uagent, out string devName)
                {
                        DevType devType = GetDevType(uagent);
                        if (devType == DevType.苹果)
                        {
                                Regex regex = new Regex(@"(OS[ ]((\d+[_.])+\d{0,}){1}){1}", RegexOptions.IgnoreCase);
                                Match match = regex.Match(uagent);
                                devName = match.Success ? string.Format("iPhone {0}", match.Value) : "iPhone";
                        }
                        else if (devType == DevType.安卓)
                        {
                                Regex regex = new Regex(@"([ ]([a-zA-Z0-9-.+]+[ ])+Build){1}", RegexOptions.IgnoreCase);
                                Match match = regex.Match(uagent);
                                devName = match.Success ? match.Value.Replace("Build", string.Empty).Trim() : "未知";
                        }
                        else if (devType == DevType.PC)
                        {
                                Regex regex = new Regex(@"(Windows[ ]NT[ ]\d+[.]\d+){1}", RegexOptions.IgnoreCase);
                                Match match = regex.Match(uagent);
                                if (match.Success)
                                {
                                        devName = match.Value.ToLower();
                                        if (devName == "windows nt 5.0")
                                        {
                                                devName = "Windows 2000";
                                        }
                                        else if (devName == "windows nt 5.1")
                                        {
                                                devName = "Windows XP";
                                        }
                                        else if (devName == "windows nt 5.2")
                                        {
                                                devName = "Windows XP/2003";
                                        }
                                        else if (devName == "windows nt 10.0" || devName == "windows nt 6.4")
                                        {
                                                devName = "Windows 10";
                                        }
                                        else if (devName == "windows nt 6.4")
                                        {
                                                devName = "Windows 10";
                                        }
                                        else if (devName == "windows nt 6.1")
                                        {
                                                devName = "Windows7/2008 r2";
                                        }
                                        else if (devName == "windows nt 6.0")
                                        {
                                                devName = "Windows Vista/2008";
                                        }
                                        else if (devName == "windows nt 6.2")
                                        {
                                                devName = "Windows 8/2012";
                                        }
                                        else if (devName == "windows nt 6.3")
                                        {
                                                devName = "Windows 8.1/2012 r2";
                                        }
                                }
                                else if (uagent.IndexOf("Macintosh;") != -1)
                                {
                                        regex = new Regex(@"(Intel[ ]Mac[ ]OS[ ]X[ ](\d+[._])+\d*){1}", RegexOptions.IgnoreCase);
                                        match = regex.Match(uagent);
                                        devName = match.Success
                                                ? string.Format("苹果(Macintosh {0})", match.Value.Replace("Intel Mac OS X ", string.Empty).Trim())
                                                : "苹果(Macintosh)";

                                }
                                else
                                {
                                        devName = uagent.IndexOf("Linux x86_64") != -1 ? "Linux" : "未知";
                                }
                        }
                        else if (devType == DevType.WindowsPhone)
                        {
                                Regex regex = new Regex(@"(IEMobile[/]\d+[.]\d+[;](([ ]|\w)+[;]{0,1})+[^)]){1}", RegexOptions.IgnoreCase);
                                Match match = regex.Match(uagent);
                                if (match.Success)
                                {
                                        string[] temp = match.Value.Split(';').Select(a => a.ToLower()).Distinct().ToArray();
                                        string t = temp[^2].Trim();
                                        devName = temp[^1].Replace(t, string.Empty).Trim();
                                        devName = string.Format("{0} {1}", t, devName);
                                }
                                else
                                {
                                        devName = "未知";
                                }
                        }
                        else if (devType == DevType.未知 && uagent.IndexOf("ZTE U795") != -1)
                        {
                                devName = "ZTE U795";
                                devType = DevType.安卓;
                        }
                        else if (devType == DevType.未知 && uagent.IndexOf("HUAWEI Y511-T00") != -1)
                        {
                                devName = "HUAWEI Y511-T00";
                                devType = DevType.安卓;
                        }
                        else
                        {
                                devName = "未知";
                        }
                        return devType;
                }



                /// <summary>
                /// 拼接URI和GET参数
                /// </summary>
                /// <param name="uri"></param>
                /// <param name="param"></param>
                /// <returns></returns>
                public static string GetJumpUri(string uri, string param)
                {
                        if (uri.IndexOf('?') == -1)
                        {
                                return string.Concat(uri, "?", param);
                        }
                        return string.Concat(uri, "&", param);
                }
                public static Uri GetJumpUri(Uri uri, string param)
                {
                        if (uri.Query == string.Empty)
                        {
                                return new Uri(string.Concat(uri.AbsoluteUri, "?", param));
                        }
                        return new Uri(string.Concat(uri.AbsoluteUri, "&", param));
                }
                /// <summary>
                /// URL编码
                /// </summary>
                /// <param name="uri"></param>
                /// <returns></returns>
                public static string UrlEncode(string uri)
                {
                        return System.Web.HttpUtility.UrlEncode(uri);
                }
                /// <summary>
                /// URL解码
                /// </summary>
                /// <param name="uri"></param>
                /// <returns></returns>
                public static string UrlDecode(string uri)
                {
                        return System.Web.HttpUtility.UrlDecode(uri);
                }
                /// <summary>
                /// 获取字符数组中某位ASCII
                /// </summary>
                /// <param name="i"></param>
                /// <param name="index"></param>
                /// <returns></returns>
                public static byte GetOneAscii(char[] i, int index)
                {
                        return ASCIIEncoding.ASCII.GetBytes(i, index, 1)[0];
                }
                /// <summary>
                /// JSON转对象
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="str"></param>
                /// <returns></returns>
                public static T Json<T>(string str)
                {
                        try
                        {
                                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str, _Settling);
                        }
                        catch (Exception e)
                        {
                                new ErrorLog(e, "JSON反序列化失败!", "Json") { LogContent = str }.Save();
                                return default;
                        }
                }
                public static object Json(string str, Type type)
                {
                        return JsonConvert.DeserializeObject(str, type, _Settling);
                }
                public static string GetFileExtension(string contentType)
                {
                        if (string.IsNullOrEmpty(contentType))
                        {
                                return null;
                        }
                        string[] t = contentType.Split('/');
                        if (t.Length == 2)
                        {
                                return string.Concat(".", t[1].ToLower());
                        }
                        return null;
                }
                public static string GetImgExtension(string contentType)
                {
                        if (string.IsNullOrEmpty(contentType))
                        {
                                return null;
                        }
                        contentType = contentType.ToLower();
                        return _GetImgExtension(contentType);
                }
                /// <summary>
                /// 重请求头的ContentType判断图片格式
                /// </summary>
                /// <param name="contentType"></param>
                /// <returns></returns>
                private static string _GetImgExtension(string contentType)
                {
                        return contentType switch
                        {
                                "image/jpeg" => ".jpg",
                                "image/jpg" => ".jpg",
                                "image/png" => ".png",
                                "image/gif" => ".gif",
                                _ => null,
                        };
                }

                /// <summary>
                /// 检查是否是微信内部浏览器
                /// </summary>
                /// <param name="uagent"></param>
                /// <returns></returns>
                public static bool CheckIsWxBrowser(string uagent)
                {
                        if (uagent == null)
                        {
                                return false;
                        }
                        string str = uagent.ToLower();
                        if (str.LastIndexOf("micromessenger") != -1 || str.StartsWith("wechat/"))
                        {
                                return true;
                        }
                        return false;
                }

                /// <summary>
                /// 将对象JSON
                /// </summary>
                /// <param name="obj"></param>
                /// <returns></returns>
                public static string Json(object obj)
                {
                        return Newtonsoft.Json.JsonConvert.SerializeObject(obj, _Settling);
                }
                public static string Json(object obj, Type type)
                {
                        return Newtonsoft.Json.JsonConvert.SerializeObject(obj, type, _Settling);
                }
                /// <summary>
                /// 将时间戳转时间
                /// </summary>
                /// <param name="timeStamp"></param>
                /// <returns></returns>
                public static DateTime GetTimeStamp(string timeStamp)
                {
                        TimeSpan toNow = new TimeSpan(long.Parse(timeStamp));
                        return _DtStart.Add(toNow).ToLocalTime();
                }
                /// <summary>
                /// 获取当前时间戳
                /// </summary>
                /// <returns></returns>
                public static long GetTimeStamp()
                {
                        TimeSpan ts = DateTime.UtcNow - _DtStart;
                        return (long)ts.TotalSeconds;
                }
                /// <summary>
                /// 获取指定时间的时间戳
                /// </summary>
                /// <param name="time"></param>
                /// <returns></returns>
                public static long GetTimeStamp(DateTime time)
                {
                        if (time == DateTime.MinValue)
                        {
                                return 0;
                        }
                        TimeSpan ts = time.ToUniversalTime() - _DtStart;
                        return (long)ts.TotalSeconds;
                }
                /// <summary>
                /// 获取MD5
                /// </summary>
                /// <param name="mybyte"></param>
                /// <returns></returns>
                public static byte[] GetMD5ByByte(byte[] mybyte)
                {
                        using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
                        {
                                return md5.ComputeHash(mybyte);
                        }
                }

                public static byte[] FromBase64String(string str)
                {
                        if (str.Length % 4 != 0)
                        {
                                str = string.Concat(str, "==");
                        }
                        return Convert.FromBase64String(str);
                }
                /// <summary>
                /// HTML解码
                /// </summary>
                /// <param name="content"></param>
                /// <returns></returns>
                public static string HtmlDecode(string content)
                {
                        return System.Web.HttpUtility.HtmlDecode(content);
                }
                /// <summary>
                /// HTML编码
                /// </summary>
                /// <param name="content"></param>
                /// <returns></returns>
                public static string HtmlEncode(string content)
                {
                        return System.Web.HttpUtility.HtmlEncode(content);
                }
                /// <summary>
                /// 将连续的IP数字转成正常的IP
                /// </summary>
                /// <param name="ip"></param>
                /// <returns></returns>
                public static string FormatIp(long ip)
                {
                        string str = ip.ToString();
                        StringBuilder ipStr = new StringBuilder();
                        int num = 0;
                        for (short i = 0; i < 4; i++)
                        {
                                if (i > 0)
                                {
                                        ipStr.Append(".");
                                }
                                if (i == 0 && str.Length < 12)
                                {
                                        num = 12 - str.Length;
                                        ipStr.Append(short.Parse(str.Substring(0, num)));
                                }
                                else
                                {
                                        ipStr.Append(short.Parse(str.Substring(num, 3)));
                                        num += 3;
                                }
                        }
                        return ipStr.ToString();
                }
                public static string GetSerialNo(string head)
                {
                        return GetSerialNo(DateTime.Now, head);
                }
                public static string GetRandomNum(int len)
                {
                        char[] chars = Array.FindAll(Guid.NewGuid().ToString("N").ToCharArray(), a => a >= 48 && a <= 57);
                        string str = string.Concat<char>(chars);
                        return str.Length > len ? str.Substring(0, len) : str.PadLeft(len, '0');
                }
                public static string GetSerialNo(DateTime now, string head)
                {
                        StringBuilder serialNo = new StringBuilder(32);
                        serialNo.Append(head.PadRight(4, '0'));
                        serialNo.Append(now.ToString("yyyyMMdd"));
                        serialNo.Append(now.Millisecond.ToString().PadLeft(3, '0'));
                        char[] chars = Array.FindAll(Guid.NewGuid().ToString("N").ToCharArray(), a => a >= 48 && a <= 57);
                        string str = string.Concat<char>(chars);
                        if (str.Length > 17)
                        {
                                serialNo.Append(str.Substring(0, 17));
                        }
                        else if (str.Length < 17)
                        {
                                serialNo.Append(str.PadLeft(17, '0'));
                        }
                        else
                        {
                                serialNo.Append(str);
                        }
                        return serialNo.ToString();
                }
                /// <summary>
                /// 将金额格式成固定格式的字符串
                /// </summary>
                /// <param name="balance"></param>
                /// <returns></returns>
                public static string FormatMoney(decimal balance)
                {
                        if (balance == 0)
                        {
                                return "0.00";
                        }
                        else
                        {
                                string str = (balance / 100).ToString();
                                int index = str.IndexOf(".");
                                if (index == -1)
                                {
                                        return str += ".00";
                                }
                                else
                                {
                                        string[] temp = str.Split('.');
                                        return string.Format("{0}.{1}", temp[0], temp[1].PadRight(2, '0'));
                                }
                        }
                }
                /// <summary>
                /// 校验和
                /// </summary>
                /// <param name="data"></param>
                /// <returns></returns>
                public static byte CS(byte[] data)
                {
                        byte cs = 0;
                        for (int i = 0; i < data.Length; i++)
                        {
                                cs = (byte)((cs + data[i]) % 256);
                        }
                        return cs;
                }
                public static byte CS(byte[] one, byte[] two, int end)
                {
                        byte cs = CS(one);
                        return (byte)((cs + CS(two, end)) % 256);
                }
                public static byte CS(byte[] data, int end)
                {
                        byte cs = 0;
                        for (int i = 0; i < end; i++)
                        {
                                cs = (byte)((cs + data[i]) % 256);
                        }
                        return cs;
                }
                /// <summary>
                /// 时间戳转时间
                /// </summary>
                /// <param name="timeStamp"></param>
                /// <returns></returns>
                public static DateTime GetTimeStamp(long timeStamp)
                {
                        return _DtStart.AddSeconds(timeStamp).ToLocalTime();
                }
                /// <summary>
                ///  生成随机的0-9a-zA-Z字符串
                /// </summary>
                /// <returns></returns>
                public static string GenerateKeys()
                {
                        string[] Chars = "0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z".Split(',');
                        int SeekSeek = unchecked((int)DateTime.Now.Ticks);
                        Random SeekRand = new Random(SeekSeek);
                        for (int i = 0; i < 100000; i++)
                        {
                                int r = SeekRand.Next(1, Chars.Length);
                                string f = Chars[0];
                                Chars[0] = Chars[r - 1];
                                Chars[r - 1] = f;
                        }
                        return string.Join(string.Empty, Chars);
                }
                /// <summary>
                /// 格式化时间长度描述(秒开始)
                /// </summary>
                /// <param name="time"></param>
                /// <returns></returns>
                public static string FormatTime(long time)
                {
                        if (time == 0)
                        {
                                return "0秒";
                        }
                        else if (time < 60)
                        {
                                return string.Concat(time, "秒");
                        }
                        StringBuilder str = new StringBuilder();
                        int num = (int)(time / 86400);
                        if (num != 0)
                        {
                                str.AppendFormat("{0}天", num);
                                time %= 86400;
                        }
                        num = (int)(time / 3600);
                        if (num != 0)
                        {
                                str.AppendFormat("{0}小时", num);
                                time %= 3600;
                        }
                        num = (int)(time / 60);
                        if (num != 0)
                        {
                                str.AppendFormat("{0}分", num);
                                time %= 60;
                        }
                        if (time != 0)
                        {
                                str.AppendFormat("{0}秒", num);
                        }
                        return str.ToString();
                }
                /// <summary>
                /// 返回一字符串，十进制 number 以 radix 进制的表示
                /// </summary>
                /// <param name="dec">数字</param>
                /// <param name="toRadix">进制</param>
                /// <returns></returns>
                public static string Dec2Any(long dec, int toRadix)
                {
                        int MIN_RADIX = 2;
                        int MAX_RADIX = 62;
                        string num62 = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

                        if (toRadix < MIN_RADIX || toRadix > MAX_RADIX)
                        {
                                toRadix = 2;
                        }
                        if (toRadix == 10)
                        {
                                return dec.ToString();
                        }

                        // -Long.MIN_VALUE 转换为 2 进制时长度为65   
                        string[] buf = new string[65];
                        int charPos = 64;
                        bool isNegative = dec < 0;
                        if (!isNegative)
                        {
                                dec = -dec;
                        }

                        while (dec <= -toRadix)
                        {
                                buf[charPos--] = num62.Substring(-((int)(dec % toRadix)), 1);
                                dec /= toRadix;
                        }
                        buf[charPos] = num62.Substring((int)-dec, 1);
                        if (isNegative)
                        {
                                buf[--charPos] = "-";
                        }
                        string _any = string.Empty;
                        for (int i = charPos; i < 65; i++)
                        {
                                _any += buf[i];
                        }
                        return _any;
                }
                public static short GetSerialNoMonth(string serialNo)
                {
                        return short.Parse(serialNo.Substring(8, 2));
                }

                /// <summary>
                /// 返回一字符串，包含 number 以 10 进制的表示。
                /// </summary>
                /// <param name="number"></param>
                /// <param name="fromRadix"></param>
                /// <returns></returns>
                public static long Any2Dec(string number, int fromRadix)
                {
                        string num62 = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
                        long dec = 0;
                        int len = number.Length - 1;
                        for (int t = 0; t <= len; t++)
                        {
                                int digitValue = num62.IndexOf(number[t]);
                                dec = (dec * fromRadix) + digitValue;
                        }
                        return dec;
                }
                /// <summary>
                /// 将字符串Basic64
                /// </summary>
                /// <param name="code"></param>
                /// <returns></returns>
                public static string ToBasic64(string code)
                {
                        byte[] bytes = Encoding.UTF8.GetBytes(code);
                        return Convert.ToBase64String(bytes);
                }
                /// <summary>
                /// 反序列化
                /// </summary>
                /// <param name="path"></param>
                /// <returns></returns>
                public static object Deserialize(string path)
                {
                        if (File.Exists(path))
                        {
                                using (Stream stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                                {
                                        BinaryFormatter bf = new BinaryFormatter();
                                        return bf.Deserialize(stream);
                                }
                        }
                        return null;
                }
                /// <summary>   
                /// 得到字符串的长度，一个汉字算2个字符   
                /// </summary>   
                /// <param name="str">字符串</param>   
                /// <returns>返回字符串长度</returns>   
                public static int GetLength(string str)
                {
                        if (str.Length == 0)
                        {
                                return 0;
                        }
                        ASCIIEncoding ascii = new ASCIIEncoding();
                        int tempLen = 0;
                        byte[] s = ascii.GetBytes(str);
                        for (int i = 0; i < s.Length; i++)
                        {
                                if (s[i] == 63)
                                {
                                        tempLen += 2;
                                }
                                else
                                {
                                        tempLen += 1;
                                }
                        }
                        return tempLen;
                }

                public static string Substring(string str, ref int index, int len)
                {
                        if (str.Length == 0)
                        {
                                index = -1;
                                return str;
                        }
                        ASCIIEncoding ascii = new ASCIIEncoding();
                        int tempLen = 0;
                        int end = 0;
                        len *= 2;
                        byte[] s = ascii.GetBytes(str);
                        for (int i = index; i < s.Length; i++)
                        {
                                if (s[i] == 63)
                                {
                                        tempLen += 2;
                                }
                                else
                                {
                                        tempLen += 1;
                                }
                                if (tempLen >= len)
                                {
                                        end = tempLen == len ? i : (i - 1);
                                        break;
                                }
                        }
                        string res;
                        if (end == 0)
                        {
                                res = str[index..];
                                index = -1;
                        }
                        else
                        {
                                res = str[index..end];
                                index = end;
                        }
                        return res;
                }
                /// <summary>   
                /// 得到字符串的长度，一个汉字算2个字符   
                /// </summary>   
                /// <param name="str">字符串</param>   
                /// <returns>返回字符串长度</returns>   
                public static decimal GetLength(string str, decimal num, out int len)
                {
                        ASCIIEncoding ascii = new ASCIIEncoding();
                        decimal tempLen = 0;
                        len = 0;
                        decimal max = num * 2;
                        byte[] s = ascii.GetBytes(str);
                        for (int i = 0; i < s.Length; i++)
                        {
                                if (s[i] == 63)
                                {
                                        tempLen += max;
                                        len += 2;
                                }
                                else
                                {
                                        tempLen += num;
                                        len += 1;
                                }
                        }
                        return tempLen;
                }
                public static string EncryptSHA256(string txt, string secret)
                {
                        secret ??= "";
                        UTF8Encoding encoding = new System.Text.UTF8Encoding();
                        byte[] keyByte = encoding.GetBytes(secret);
                        byte[] messageBytes = encoding.GetBytes(txt);
                        using (HMACSHA256 hmacsha256 = new HMACSHA256(keyByte))
                        {
                                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                                StringBuilder builder = new StringBuilder();
                                for (int i = 0; i < hashmessage.Length; i++)
                                {
                                        builder.Append(hashmessage[i].ToString("x2"));
                                }
                                return builder.ToString();
                        }
                }
                public static int GetLength(char[] chars, int index)
                {
                        ASCIIEncoding ascii = new ASCIIEncoding();
                        byte[] s = ascii.GetBytes(chars, index, 1);
                        return s[0] == 63 ? 2 : 1;
                }
                /// <summary>
                /// RSA算法加密
                /// </summary>
                /// <param name="publickey">公钥</param>
                /// <param name="content">内容</param>
                /// <returns></returns>
                public static string RSAEncrypt(string publickey, string content)
                {
                        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                        rsa.FromXmlString(publickey);
                        byte[] cipherbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(content), false);
                        return Convert.ToBase64String(cipherbytes);
                }
                /// <summary>
                ///  DSA算法加密
                /// </summary>
                /// <param name="publickey"></param>
                /// <param name="content"></param>
                /// <returns></returns>
                public static string DSAEncrypt(string publickey, string content)
                {
                        DSACryptoServiceProvider dsa = new DSACryptoServiceProvider();
                        byte[] cipherbytes;
                        dsa.FromXmlString(publickey);
                        cipherbytes = dsa.SignData(Encoding.UTF8.GetBytes(content));
                        return Convert.ToBase64String(cipherbytes);
                }
                /// <summary>
                /// 序列化对象并保存成文件
                /// </summary>
                /// <param name="data"></param>
                /// <param name="savePath"></param>
                public static void Serializable(object data, string savePath)
                {
                        using (Stream stream = File.Open(savePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
                        {
                                BinaryFormatter bf = new BinaryFormatter();
                                bf.Serialize(stream, data);
                                stream.Flush();
                        }
                }
                /// <summary>
                /// 序列化对象并保存成文件
                /// </summary>
                /// <param name="data"></param>
                /// <param name="savePath"></param>
                public static byte[] Serializable(object data)
                {
                        using (MemoryStream stream = new MemoryStream())
                        {
                                BinaryFormatter bf = new BinaryFormatter();
                                bf.Serialize(stream, data);
                                return stream.ToArray();
                        }
                }
                /// <summary>
                /// 获取毫秒为单位的时间戳
                /// </summary>
                /// <returns></returns>
                public static long GetTimeStampByMs()
                {
                        TimeSpan ts = DateTime.UtcNow - _DtStart;
                        return (long)ts.TotalMilliseconds;
                }
                /// <summary>
                /// 获取毫秒为单位的时间戳
                /// </summary>
                /// <returns></returns>
                public static long GetTimeStampByMs(DateTime time)
                {
                        TimeSpan ts = time.ToUniversalTime() - _DtStart;
                        return (long)ts.TotalMilliseconds;
                }



                /// <summary>
                /// 获取字符串MD5值
                /// </summary>
                /// <param name="str"></param>
                /// <returns></returns>
                public static string GetMD5(string str)
                {
                        using (MD5 md5 = MD5.Create())
                        {
                                byte[] result = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
                                return BitConverter.ToString(result).Replace("-", string.Empty);
                        }
                }
                /// <summary>
                /// 获取本地的IP地址
                /// </summary>
                /// <returns></returns>
                public static string GetLocalIp()
                {
                        IPAddress[] p = Dns.GetHostAddresses(Dns.GetHostName());
                        if (p != null && p.Length > 0)
                        {
                                foreach (IPAddress i in p)
                                {
                                        if (i.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                                        {
                                                return i.ToString();
                                        }
                                }
                        }
                        return null;
                }
                /// <summary>
                /// 删除HTML
                /// </summary>
                /// <param name="content"></param>
                /// <returns></returns>
                public static string DeleteHTML(string content)
                {
                        if (!string.IsNullOrEmpty(content))
                        {
                                content = content.Replace("\r", string.Empty);
                                content = content.Replace("\n", string.Empty);
                                return FilterHtml(content);
                        }
                        return content;
                }
                private static readonly Array _FilterEnum = Enum.GetValues(typeof(FilterHtmlType));
                private static readonly FilterHtmlType _AllFilter = FilterHtmlType.注释 | FilterHtmlType.Style | FilterHtmlType.Script | FilterHtmlType.Link | FilterHtmlType.If | FilterHtmlType.Event | FilterHtmlType.A标签;
                public static string FilterHtml(string content, FilterHtmlType filter)
                {
                        if (filter == FilterHtmlType.无)
                        {
                                return content;
                        }
                        if (filter == FilterHtmlType.HTML)
                        {
                                return FilterHtml(content);
                        }
                        else if (filter == FilterHtmlType.除基础HTML外全部)
                        {
                                filter = _AllFilter;
                        }
                        foreach (FilterHtmlType i in _FilterEnum)
                        {
                                if ((i & filter) == i)
                                {
                                        content = _FilterHtml(content, i);
                                }
                        }
                        return content;
                }
                private static string _FilterHtml(string content, FilterHtmlType type)
                {
                        switch (type)
                        {
                                case FilterHtmlType.If:
                                        return Regex.Replace(content, "([<][!][-][-][[]if [!]IE[]][>].+[<][!][[]endif[]][-][-][>])+", string.Empty, RegexOptions.Multiline);
                                case FilterHtmlType.Link:
                                        return Regex.Replace(content, "(<link.*?>)|(<link.*?/>)", string.Empty, RegexOptions.Multiline);
                                case FilterHtmlType.Script:
                                        content = Regex.Replace(content, "(<script.*?>.*?</script>)", string.Empty, RegexOptions.Multiline);
                                        content = Regex.Replace(content, "javascript[:].+[^\",'>]", string.Empty, RegexOptions.Multiline);
                                        return content;
                                case FilterHtmlType.Style:
                                        return Regex.Replace(content, "(<style.*?>.*?</style>)", string.Empty, RegexOptions.Multiline);
                                case FilterHtmlType.注释:
                                        content = Regex.Replace(content, "< !--.*? --> ", string.Empty, RegexOptions.Multiline);
                                        content = Regex.Replace(content, "[/][*].*?[*][/]", string.Empty, RegexOptions.Multiline);
                                        return content;
                                case FilterHtmlType.A标签:
                                        return Regex.Replace(content, "(<a.*?>.*?</a>)", string.Empty, RegexOptions.Multiline);
                                case FilterHtmlType.Event:
                                        return Regex.Replace(content, "([ ]on\\w+[=].+?['\"'])", string.Empty, RegexOptions.Multiline);
                        }
                        return content;
                }
                /// <summary>
                /// 过滤掉内容中的HTML
                /// </summary>
                /// <param name="content"></param>
                /// <returns></returns>
                public static string FilterHtml(string content)
                {
                        content = Regex.Replace(content, "([<][!][-][-][[]if [!]IE[]][>].+[<][!][[]endif[]][-][-][>])+", string.Empty);
                        content = Regex.Replace(content, "(<script.*?>.*?</script>)", string.Empty, RegexOptions.Multiline);
                        content = Regex.Replace(content, "(<link.*?>)|(<link.*?/>)", string.Empty, RegexOptions.Multiline);
                        content = Regex.Replace(content, "(<style.*?>.*?</style>)", string.Empty, RegexOptions.Multiline);
                        content = Regex.Replace(content, "<!.*?>", string.Empty);
                        content = Regex.Replace(content, "<!--.*?-->", string.Empty);
                        content = Regex.Replace(content, "<.*?>", string.Empty);
                        content = Regex.Replace(content, "&.*?;", string.Empty);
                        return content;
                }
        }
}
