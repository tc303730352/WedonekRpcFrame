using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using ToolGood.Words.FirstPinyin;
using WeDonekRpc.Helper.Json;
namespace WeDonekRpc.Helper
{
    public enum OSType : byte
    {
        Windows = 0,
        Linux = 1,
        OSX = 2,
        FreeBSD = 3,
        Other = 4
    }
    public partial class Tools
    {
        private static readonly DateTime _DtStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);


        public static OSType GetOSType ()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return OSType.Windows;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return OSType.Linux;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return OSType.OSX;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
            {
                return OSType.FreeBSD;
            }
            else
            {
                return OSType.Other;
            }
        }

        public static BigInteger ToBigInteger (IPAddress address)
        {
            if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
            {
                return new BigInteger(address.GetAddressBytes());
            }
            return address.ScopeId;
        }
        public static bool IsAdminRun ()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
            }
            return Mono.Unix.Native.Syscall.geteuid() == 0;
        }
        public static byte[] ToByte (decimal num)
        {
            Span<int> bits = new int[4];
            int len = decimal.GetBits(num, bits);
            byte[] bytes = new byte[len * 4];
            for (int i = 0; i < len; i++)
            {
                int index = i * 4;
                for (int j = 0; j < 4; j++)
                {
                    bytes[index + j] = (byte)( bits[i] >> ( j * 8 ) );
                }
            }
            return bytes;
        }
        public static int ToByte (decimal num, byte[] bytes, int index)
        {
            Span<int> bits = new int[4];
            int len = decimal.GetBits(num, bits);
            for (int i = 0; i < len; i++)
            {
                int k = index + ( i * 4 );
                for (int j = 0; j < 4; j++)
                {
                    bytes[k + j] = (byte)( bits[i] >> ( j * 8 ) );
                }
            }
            return len * 4;
        }
        public static string GetRunUserIdentity ()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return WindowsIdentity.GetCurrent().Name;
            }
            return Mono.Unix.Native.Syscall.geteuid().ToString();
        }
        public static string[] GetRunUserGroups ()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                int[] roles = EnumHelper.GetValues(typeof(WindowsBuiltInRole));
                WindowsPrincipal principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
                roles = roles.FindAll(principal.IsInRole);
                return roles.ConvertAll(c => ( (WindowsBuiltInRole)c ).ToString());
            }
            return new string[]
            {
                "uid_"+Mono.Unix.Native.Syscall.geteuid(),
                "euid_"+Mono.Unix.Native.Syscall.geteuid()
            };
        }
        /// <summary>
        /// 压缩JSON
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static string CompressJson (string json)
        {
            StringBuilder sb = new StringBuilder();
            using (StringReader reader = new StringReader(json))
            {
                int ch = -1;
                int lastch = '\0';
                bool isQuoteStart = false;
                while (( ch = reader.Read() ) > -1)
                {
                    if (lastch != '\\' && (char)ch == '\"')
                    {
                        isQuoteStart = !isQuoteStart;
                    }
                    if (!char.IsWhiteSpace((char)ch) || isQuoteStart)
                    {
                        _ = sb.Append((char)ch);
                    }
                    lastch = ch;
                }
            }
            return sb.ToString().Trim();
        }

        public static void WriteText (string path, string text, Encoding encoding, bool iscover = true)
        {
            FileInfo file = new FileInfo(path);
            if (!file.Directory.Exists)
            {
                file.Directory.Create();
            }
            else if (iscover && file.Exists)
            {
                file.Delete();
            }
            using (StreamWriter write = new StreamWriter(file.Open(FileMode.CreateNew, FileAccess.Write, FileShare.Delete), encoding))
            {
                write.Write(text);
                write.Flush();
            }
        }
        public static void WriteText (FileInfo file, string text, Encoding encoding, bool iscover = true)
        {
            if (!file.Directory.Exists)
            {
                file.Directory.Create();
            }
            else if (iscover && file.Exists)
            {
                file.Delete();
            }
            using (StreamWriter write = new StreamWriter(file.Open(FileMode.CreateNew, FileAccess.Write, FileShare.Delete), encoding))
            {
                write.Write(text);
                write.Flush();
            }
        }
        public static byte[] ReadStream (FileInfo file)
        {
            byte[] bytes = null;
            using (FileStream fileStream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                bytes = new byte[fileStream.Length];
                _ = fileStream.Read(bytes, 0, bytes.Length);
            }
            return bytes;
        }
        public static byte[] ReadStream (string path)
        {
            byte[] bytes = null;
            using (FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                bytes = new byte[fileStream.Length];
                _ = fileStream.Read(bytes, 0, bytes.Length);
            }
            return bytes;
        }
        public static void ReadChar (byte[] stream, Encoding encoding, int size, Action<char[], int> action)
        {
            char[] chars = new char[size];
            int begin = 0;
            int sum = stream.Length;
            do
            {
                int len = Encoding.UTF8.GetChars(stream, begin, size, chars, 0);
                if (len != 0)
                {
                    action(chars, len);
                }
                sum -= len;
                if (sum <= 0)
                {
                    break;
                }
                else if (sum < size)
                {
                    size = sum;
                }
                begin += len;
            } while (true);
        }
        public static void ReadTextLine (FileInfo file, Encoding encoding, Action<string> action)
        {
            using (StreamReader stream = new StreamReader(file.Open(FileMode.Open, FileAccess.Read, FileShare.Read), encoding))
            {
                do
                {
                    string line = stream.ReadLine();
                    if (line == null)
                    {
                        break;
                    }
                    action(line);
                } while (true);
            }
        }

        public static string ReadText (string path, Encoding encoding, bool dropRn = false)
        {
            return ReadText(new FileInfo(path), encoding, dropRn);
        }
        public static string ReadText (FileInfo file, Encoding encoding, bool dropRn = false)
        {
            if (!file.Exists)
            {
                return null;
            }
            string str = null;
            using (StreamReader read = new StreamReader(file.Open(FileMode.Open, FileAccess.Read, FileShare.Delete), encoding))
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
                return DropEscapeChar(str);
            }
        }

        private const double _EARTH_RADIUS = 6378.137;//地球半径

        private static readonly RNGCryptoServiceProvider _rng = new RNGCryptoServiceProvider();
        private static SequentialGuidType _GuidTye = SequentialGuidType.SequentialAtEnd;
        public static void SetDefaultGuidType (SequentialGuidType guidType)
        {
            _GuidTye = guidType;
        }
        /// <summary>
        /// 创建易于SQL Server快速存储的GUID
        /// </summary>
        /// <returns></returns>
        public static Guid NewGuid ()
        {
            return NewGuid(_GuidTye);
        }
        /// <summary>
        /// 按照指定规则生成GUID
        /// </summary>
        /// <param name="guidType"></param>
        /// <returns></returns>
        public static Guid NewGuid (SequentialGuidType guidType)
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
            if (guidType == SequentialGuidType.SequentialAtEnd)
            {
                Buffer.BlockCopy(randomBytes, 0, guidBytes, 0, 10);
                Buffer.BlockCopy(timestampBytes, 2, guidBytes, 10, 6);
            }
            else
            {
                Buffer.BlockCopy(timestampBytes, 2, guidBytes, 0, 6);
                Buffer.BlockCopy(randomBytes, 0, guidBytes, 6, 10);
                if (guidType == SequentialGuidType.SequentialAsString && BitConverter.IsLittleEndian)
                {
                    Array.Reverse(guidBytes, 0, 4);
                    Array.Reverse(guidBytes, 4, 2);
                }
            }
            return new Guid(guidBytes);
        }
        public static decimal ToDecimal (byte[] array)
        {
            int[] bits = new int[array.Length / 4];
            for (int i = 0; i < bits.Length; i++)
            {
                int index = i * 4;
                for (int j = 0; j < 4; j++)
                {
                    bits[i] |= array[index + j] << ( j * 8 );
                }
            }
            return new decimal(bits);
        }
        public static decimal ToDecimal (Span<byte> array, int index)
        {
            int[] bits = new int[array.Length / 4];
            for (int i = 0; i < bits.Length; i++)
            {
                int k = index + ( i * 4 );
                for (int j = 0; j < 4; j++)
                {
                    bits[i] |= array[k + j] << ( j * 8 );
                }
            }
            return new decimal(bits);
        }
        public static decimal ToDecimal (byte[] array, int index)
        {
            int[] bits = new int[array.Length / 4];
            for (int i = 0; i < bits.Length; i++)
            {
                int k = index + ( i * 4 );
                for (int j = 0; j < 4; j++)
                {
                    bits[i] |= array[k + j] << ( j * 8 );
                }
            }
            return new decimal(bits);
        }
        public static string DecodeURI (string value)
        {
            return HttpUtility.UrlDecode(value);
        }

        public static string HmacSha1Sign (string secret, string strOrgData)
        {
            HMACSHA1 hmacsha1 = new HMACSHA1(Encoding.UTF8.GetBytes(secret));
            byte[] dataBuffer = Encoding.UTF8.GetBytes(strOrgData);
            return Convert.ToBase64String(hmacsha1.ComputeHash(dataBuffer));
        }
        private static DateTime _FormatTime (object res)
        {
            Type type = res.GetType();
            if (type == PublicDataDic.DateTimeType)
            {
                DateTime time = (DateTime)res;
                if (time == SqlMinTime)
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
                return GetTimeStamp((long)res);
            }
            else if (type == PublicDataDic.StrType)
            {
                return DateTime.Parse((string)res);
            }
            return DateTime.MinValue;
        }
        public static bool IsBasicType (Type type)
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
        public static object ChangeType (Type type, object res)
        {
            if (type.IsClass && type != PublicDataDic.StrType)
            {
                string json = Convert.ToString(res);
                if (json != string.Empty)
                {
                    return JsonTools.Json(json, type);
                }
                return null;
            }
            else if (type.IsEnum)
            {
                return EnumHelper.ToObject(type, res);
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


        public static object GetTypeDefValue (Type type)
        {
            if (type.IsValueType)
            {
                if (type.IsEnum)
                {
                    return EnumHelper.GetFistValue(type);
                }
                else if (type == PublicDataDic.GuidType)
                {
                    return Guid.Empty;
                }
                return Activator.CreateInstance(type);
            }
            else if (type == PublicDataDic.StrType)
            {
                return string.Empty;
            }
            else
            {
                return null;
            }
        }
        public static int GetArabicNum (string chinaNum)
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
                if (i == chars.Length - 1)
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
        public static string GetChinaNum (long num)
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
                _ = str.Append(val);
                if (i != 0 && val != '零')
                {
                    _ = str.Append(PublicDataDic.ChinaNumUnit[i]);
                }
            }
            return str.ToString();
        }

        /// <summary>
        /// 获取秒的时间戳
        /// </summary>
        /// <param name="now"></param>
        /// <returns></returns>
        public static long GetTimeSpan (DateTime now)
        {
            return now == DateTime.MinValue ? 0 : (long)( now.ToUniversalTime() - _DtStart ).TotalSeconds;
        }
        public static long GetTotalMilliseconds (DateTime time)
        {
            return (long)( ( time.ToUniversalTime() - _DtStart ).TotalMilliseconds * 1000L );
        }
        public static long GetTimeSpan ()
        {
            return (long)( DateTime.Now.ToUniversalTime() - _DtStart ).TotalSeconds;
        }

        public static readonly DateTime SqlMinTime = new DateTime(1900, 1, 1);
        public static string GetClassMd5<T> (T data)
        {
            string str = JsonTools.Json(data);
            return GetMD5(str);
        }

        public static byte GetZoneIndex (string str)
        {
            char[] chars = str.ToArray();
            return (byte)( GetOneAscii(chars, 0) | GetOneAscii(chars, chars.Length - 1) );
        }
        public static short GetRightZoneIndex (string str)
        {
            char[] chars = str.Substring(str.Length - 2, 2).ToCharArray();
            return (short)( GetOneAscii(chars, 0) | GetOneAscii(chars, 1) );
        }
        public static byte GetZoneIndex (char[] chars, int one, int two)
        {
            return (byte)( GetOneAscii(chars, one) | GetOneAscii(chars, two) );
        }



        public static T CreateObject<T> (string path, string className)
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
        public static T CreateObject<T> (string path)
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
        public static T CreateObject<T> (Assembly assembly)
        {
            Type iType = typeof(T);
            Type type = assembly.GetTypes().Where(a => a.GetInterface(iType.FullName) != null).FirstOrDefault();
            if (type != null)
            {
                return (T)assembly.CreateInstance(type.FullName, false);
            }
            return default;
        }
        public static bool CheckAssembly (string path)
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
        public static Assembly LoadAssembly (FileInfo file)
        {
            if (!file.Exists)
            {
                return null;
            }
            byte[] dllByte = null;
            using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Delete))
            {
                dllByte = new byte[stream.Length];
                _ = stream.Read(dllByte, 0, dllByte.Length);
                stream.Close();
                stream.Dispose();
            }
            return AppDomain.CurrentDomain.Load(dllByte);
        }
        private static Assembly LoadAssembly (string name)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, name);
            return _LoadAssembly(path);
        }
        private static Assembly _LoadAssembly (string path)
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
                _ = stream.Read(dllByte, 0, dllByte.Length);
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
        public static T GetObjectAttrVal<T> (object data, string attrName)
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
        public static object GetObjectAttrVal (object data, string attrName)
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
        public static object GetObjectAttrVal (object data, string attrName, out Type proType)
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
        public static int GetUserSexByCardID (string cardId)
        {
            int sex = cardId.Length == 15 ? int.Parse(cardId.Substring(14, 1)) : int.Parse(cardId.Substring(16, 1));
            return sex % 2 == 0 ? 1 : 0;
        }
        /// <summary>
        /// 获取身份证中的生日
        /// </summary>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public static DateTime GetBirthdayByCardID (string cardId)
        {
            string val = cardId.Substring(6, 8).Insert(4, "-").Insert(7, "-");
            if (DateTime.TryParse(val, out DateTime date))
            {
                return date;
            }
            return DateTime.MinValue;
        }
        public static bool RunCmd (string cmd, out string res)
        {
            return RunCmd(new string[] { cmd }, out res);
        }
        /// <summary>
        /// 运行Cmd命令
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public static bool RunCmd (string[] cmd, out string res)
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
        public static bool RunExeFile (string execPath, string param, out string res)
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
        public static Guid GetProcedureGuid ()
        {
            Attribute guid_attr = Attribute.GetCustomAttribute(Assembly.GetEntryAssembly(), typeof(GuidAttribute));
            return guid_attr == null ? Guid.Empty : new Guid(( (GuidAttribute)guid_attr ).Value);
        }
        /// <summary>
        /// 获取字符串首位的ASCII值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static short GetOneAscii (string str)
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
        public static string HidePhone (string phone)
        {
            if (phone.IsNull())
            {
                return phone;
            }
            phone = phone.Remove(phone.Length - 8, 5);
            return phone.Insert(phone.Length - 3, "*****");
        }
        public static string HideIDCard (string cardId)
        {
            if (cardId.IsNull())
            {
                return cardId;
            }
            cardId = cardId.Remove(6, 8);
            return cardId.Insert(6, "********");
        }
        public static string HideRealName (string name)
        {
            if (name.IsNull())
            {
                return name;
            }
            else if (name.Length == 1)
            {
                return name + "*";
            }
            int len = name.Length;
            return name.Substring(0, 1).PadRight(len, '*');
        }
        /// <summary>
        /// 获取IP的数字表示形式
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static long GetIpNum (string ip)
        {
            string[] items = ip.Split('.');
            return ( long.Parse(items[0]) << 24 ) | ( long.Parse(items[1]) << 16 ) | ( long.Parse(items[2]) << 8 ) | long.Parse(items[3]);
        }
        /// <summary>
        /// 获取字符串中某位的ASCII码
        /// </summary>
        /// <param name="str"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static short GetOneAscii (string str, int index)
        {
            byte[] myByte = Encoding.ASCII.GetBytes(str);
            return myByte.Length > index ? myByte[index] : (short)0;
        }
        /// <summary>
        /// 生成随机的MAC
        /// </summary>
        /// <returns></returns>
        public static string GetRandomMac ()
        {
            string mac = Guid.NewGuid().ToString("N");
            int index = GetRandom(0, 17);
            mac = mac.Substring(index, 12);
            return FormatMac(mac);
        }

        /// <summary>
        /// 获取首位的ASCII码
        /// </summary>
        /// <param name="chars"></param>
        /// <returns></returns>
        public static short GetOneAscii (char[] chars)
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
        internal static double RssitoDistance (int rssi, int a, double n)
        {
            return Math.Pow(10, ( Math.Abs(rssi) - a ) / ( 10.0 * n ));
        }
        private static double _rad (double d)
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
        public static int GetDistance (double lat1, double lng1, double lat2, double lng2)
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
        public static DateTime PaseDateTime (string time)
        {
            StringBuilder str = new StringBuilder(time);
            if (time.Length == 8)
            {
                _ = str.Insert(4, "-").Insert(7, "-");
            }
            else if (time.Length == 14)
            {
                _ = str.Insert(4, "-").Insert(7, "-").Insert(10, " ").Insert(13, ":").Insert(16, ":");
            }
            else if (time.Length > 14)
            {
                _ = str.Insert(4, "-").Insert(7, "-").Insert(10, " ").Insert(13, ":").Insert(16, ":");
            }
            else
            {
                return DateTime.MinValue;
            }
            return DateTime.Parse(str.ToString());
        }
        private static double[] _FormatGps (double lat, double lng, GpsType type, GpsType change)
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
        public static int GetDistance (double lat1, double lng1, GpsType type, double lat2, double lng2, GpsType twoType)
        {
            double[] one = new double[]
            {
                                lat1,
                                lng1
            };
            double[] two = _FormatGps(lat2, lng2, type, twoType);
            return _GetDistance(one, two);
        }
        public static int GetDistance (decimal lat1, decimal lng1, GpsType type, decimal lat2, decimal lng2, GpsType twoType)
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
        public static int GetDistance (decimal lat1, decimal lng1, decimal lat2, decimal lng2)
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
        private static int _GetDistance (double[] one, double[] two)
        {
            double[] point = new double[2];
            for (int i = 0; i < one.Length; i++)
            {
                one[i] = _rad(one[i]);
                two[i] = _rad(two[i]);
                point[i] = Math.Abs(one[i] - two[i]);
            }
            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(point[0] / 2), 2) +
              ( Math.Cos(one[0]) * Math.Cos(two[0]) * Math.Pow(Math.Sin(point[1] / 2), 2) )));
            s *= _EARTH_RADIUS;
            return (int)Math.Round(s * 1000);
        }

        public static TimeIntervalType GetTimeIntervalType ()
        {
            return GetTimeIntervalType(DateTime.Now);
        }
        /// <summary>
        /// 获取时段类型
        /// </summary>
        /// <returns></returns>
        public static TimeIntervalType GetTimeIntervalType (DateTime time)
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

        public static string DropEscapeChar (string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }
            StringBuilder t = new StringBuilder(str);
            Array.ForEach(_EscapeChar, a =>
            {
                _ = t.Replace(a, string.Empty);
            });
            return t.ToString().Trim();
        }
        /// <summary>
        /// 获取时间间隔的天数
        /// </summary>
        /// <param name="begin"></param>
        /// <returns></returns>
        public static int GetTimeByDay (DateTime begin)
        {
            return (int)( DateTime.Now - begin ).TotalDays;
        }


        public static string ToFirstPinYin (string china)
        {
            return WordsHelper.GetFirstPinyin(china);
        }



        /// <summary>
        /// Basic64解码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string DecodeBase64 (string code)
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
        public static string GetMobileModel (string uAgent)
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
        public static string Sha1 (string content)
        {
            return Sha1(content, Encoding.UTF8);
        }
        public static string Sha1ToBase64 (string content, Encoding encode)
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
        public static byte[] Sha1 (byte[] mybytes)
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
        public static string Sha1 (string content, Encoding encode)
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
        public static int GetRandom ()
        {
            long ticks = DateTime.Now.Ticks;
            ticks = ( ticks & 0xffffffffL ) | ( ticks >> 32 );
            return new Random((int)( ticks % int.MaxValue )).Next();
        }
        public static int GetRandom (int min, int max)
        {
            if (min == max)
            {
                return min;
            }
            long ticks = DateTime.Now.Ticks;
            return new Random((int)( ( ( ticks & 0xffffffffL ) | ( ticks >> 32 ) ) % int.MaxValue )).Next(min, max);
        }
        public static long GetRandom (long min, long max)
        {
            if (min == max)
            {
                return min;
            }
            return new Random((int)( DateTime.Now.Ticks & 0xffffffffL ) | (int)( DateTime.Now.Ticks >> 32 )).NextInt64(min, max);
        }

        /// <summary>
        /// 将内容MD5后basic64加密
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string GetMD5ToBase64 (string content)
        {
            byte[] myByte = null;
            using (MD5 md5 = MD5.Create())
            {
                md5.Initialize();
                myByte = md5.ComputeHash(Encoding.ASCII.GetBytes(content));
            }
            return Convert.ToBase64String(myByte);
        }
        /// <summary>
        /// 获取真实金额
        /// </summary>
        /// <param name="money"></param>
        /// <returns></returns>
        public static decimal GetRealDataMoney (decimal money)
        {
            return money / 100;
        }
        /// <summary>
        /// 获取真实金额
        /// </summary>
        /// <param name="money"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static decimal GetRealDataMoney (decimal money, MoneyUnit unit)
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
        public static string GetMD5 (byte[] content)
        {
            string resule = null;
            using (MD5 md5 = MD5.Create())
            {
                md5.Initialize();
                byte[] hash_byte = md5.ComputeHash(content);
                resule = BitConverter.ToString(hash_byte).Replace("-", string.Empty);
            }
            return resule;
        }
        public static string GetFileMd5 (FileInfo file, int size = 5 * 1024 * 1024)
        {
            using (Stream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return GetMD5 (stream, size);
            }
        }
        public static string GetMD5 (Stream stream, int size = 5 * 1024 * 1024)
        {
            stream.Position = 0;
            byte[] buffer = new byte[size];
            int page = (int)Math.Ceiling((decimal)stream.Length / size);
            int end = page - 1;
            using (MD5 md5 = MD5.Create())
            {
                md5.Initialize();
                page.For(i =>
                {
                    int len = stream.Read(buffer, 0, buffer.Length);
                    if (i == end)
                    {
                        _ = md5.TransformFinalBlock(buffer, 0, len);
                    }
                    else
                    {
                        _ = md5.TransformBlock(buffer, 0, len, buffer, 0);
                    }
                });
                return BitConverter.ToString(md5.Hash).Replace("-", string.Empty);
            }
        }
        public static string GetMD5 (byte[] content, int offset, int count)
        {
            string resule = null;
            using (MD5 md5 = MD5.Create())
            {
                md5.Initialize();
                byte[] hash_byte = md5.ComputeHash(content, offset, count);
                resule = BitConverter.ToString(hash_byte).Replace("-", string.Empty);
            }
            return resule;
        }
        public static string FormatStreamMB (long size)
        {
            StringBuilder str = new StringBuilder();
            long num = size / 1073741824;
            if (num != 0)
            {
                _ = str.Append(num.ToString());
                _ = str.Append("GB ");
                size = size % 1073741824;
            }
            num = size / 1048576;
            if (num != 0)
            {
                _ = str.Append(num.ToString());
                _ = str.Append("MB");
            }
            return str.ToString().TrimEnd();
        }
        /// <summary>
        /// 将MAC地址格式成统一格式:00:00:00:00:00:00
        /// </summary>
        /// <param name="mac"></param>
        /// <returns></returns>
        public static string FormatMac (string mac)
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
                    _ = macInfo.Append(":");
                }
                if (i.Length == 1)
                {
                    _ = macInfo.Append(i.PadLeft(2, '0'));
                }
                else if (i.Length == 4)
                {
                    _ = macInfo.Append(i.Insert(2, ":"));
                }
                else
                {
                    _ = macInfo.Append(i);
                }
            }
            return macInfo.ToString();
        }
        /// <summary>
        /// 判断请求的类型
        /// </summary>
        /// <param name="uagent"></param>
        /// <returns></returns>
        public static RequestType GetRequestType (string uagent)
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
        public static DevType GetDevType (string uagent)
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
            if (Apple().IsMatch(str))
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
        public static DevType GetDevInfo (string uagent, out string devName, ref RequestType requestType)
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
        public static DevType GetDevInfo (string uagent, out string devName)
        {
            DevType devType = GetDevType(uagent);
            if (devType == DevType.苹果)
            {
                Match match = AppleOS().Match(uagent);
                devName = match.Success ? string.Format("iPhone {0}", match.Value) : "iPhone";
            }
            else if (devType == DevType.安卓)
            {
                Match match = _Android().Match(uagent);
                devName = match.Success ? match.Value.Replace("Build", string.Empty).Trim() : "未知";
            }
            else if (devType == DevType.PC)
            {
                Match match = _Windows().Match(uagent);
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
                    match = _Macintosh().Match(uagent);
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
                Match match = WindowsPhone().Match(uagent);
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
        /// IP转数字
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static long IpToLong (string ip)
        {
            string[] ips = ip.Split('.');
            return ( long.Parse(ips[0]) << 0x18 ) | ( long.Parse(ips[1]) << 0x10 ) | ( long.Parse(ips[2]) << 0x8 ) | long.Parse(ips[3]);
        }


        /// <summary>
        /// 拼接URI和GET参数
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string GetJumpUri (string uri, string param)
        {
            if (!uri.Contains('?'))
            {
                return string.Concat(uri, "?", param);
            }
            return string.Concat(uri, "&", param);
        }
        public static Uri GetJumpUri (Uri uri, string param)
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
        public static string UrlEncode (string uri)
        {
            return HttpUtility.UrlEncode(uri);
        }
        /// <summary>
        /// URL解码
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static string UrlDecode (string uri)
        {
            return HttpUtility.UrlDecode(uri);
        }
        /// <summary>
        /// 获取字符数组中某位ASCII
        /// </summary>
        /// <param name="i"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static byte GetOneAscii (char[] i, int index)
        {
            return Encoding.ASCII.GetBytes(i, index, 1)[0];
        }
        public static string GetFileExtension (string contentType)
        {
            int i = contentType.IndexOf('/');
            if (i == -1)
            {
                return null;
            }
            return string.Concat(".", contentType.Remove(0, i + 1));
        }
        public static string GetImgExtension (string contentType)
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
        private static string _GetImgExtension (string contentType)
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
        public static bool CheckIsWxBrowser (string uagent)
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
        /// 将时间戳转时间
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime GetTimeStamp (string timeStamp)
        {
            TimeSpan toNow = new TimeSpan(long.Parse(timeStamp));
            return _DtStart.Add(toNow).ToLocalTime();
        }
        /// <summary>
        /// 获取当前时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetTimeStamp ()
        {
            TimeSpan ts = DateTime.UtcNow - _DtStart;
            return (long)ts.TotalSeconds;
        }
        /// <summary>
        /// 获取指定时间的时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long GetTimeStamp (DateTime time)
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
        public static byte[] GetMD5ByByte (byte[] mybyte)
        {
            using (MD5 md5 = MD5.Create())
            {
                md5.Initialize();
                return md5.ComputeHash(mybyte);
            }
        }

        public static byte[] FromBase64String (string str)
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
        public static string HtmlDecode (string content)
        {
            return HttpUtility.HtmlDecode(content);
        }
        /// <summary>
        /// HTML编码
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string HtmlEncode (string content)
        {
            return HttpUtility.HtmlEncode(content);
        }
        /// <summary>
        /// 将连续的IP数字转成正常的IP
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static string FormatIp (long ip)
        {
            string str = ip.ToString();
            StringBuilder ipStr = new StringBuilder();
            int num = 0;
            for (short i = 0; i < 4; i++)
            {
                if (i > 0)
                {
                    _ = ipStr.Append(".");
                }
                if (i == 0 && str.Length < 12)
                {
                    num = 12 - str.Length;
                    _ = ipStr.Append(short.Parse(str.Substring(0, num)));
                }
                else
                {
                    _ = ipStr.Append(short.Parse(str.Substring(num, 3)));
                    num += 3;
                }
            }
            return ipStr.ToString();
        }
        public static string GetSerialNo (string head)
        {
            return GetSerialNo(DateTime.Now, head);
        }
        public static string GetRandomStr (int len)
        {
            int end = (int)Math.Ceiling(len / 32.0) * 2;
            StringBuilder str = new StringBuilder(end * 32);
            for (int i = 0; i < end; i++)
            {
                _ = str.Append(Guid.NewGuid().ToString("N"));
            }
            int start = GetRandom(0, str.Length - len);
            return str.ToString().Substring(start, len);
        }
        public static string GetSerialNo (DateTime now, string head)
        {
            StringBuilder serialNo = new StringBuilder(32);
            _ = serialNo.Append(head.PadRight(4, '0'));
            _ = serialNo.Append(now.ToString("yyyyMMdd"));
            _ = serialNo.Append(now.Millisecond.ToString().PadLeft(3, '0'));
            char[] chars = Array.FindAll(Guid.NewGuid().ToString("N").ToCharArray(), a => a >= 48 && a <= 57);
            string str = string.Concat(chars);
            if (str.Length > 17)
            {
                _ = serialNo.Append(str.Substring(0, 17));
            }
            else if (str.Length < 17)
            {
                _ = serialNo.Append(str.PadLeft(17, '0'));
            }
            else
            {
                _ = serialNo.Append(str);
            }
            return serialNo.ToString();
        }
        /// <summary>
        /// 将金额格式成固定格式的字符串
        /// </summary>
        /// <param name="balance"></param>
        /// <returns></returns>
        public static string FormatMoney (decimal balance)
        {
            if (balance == 0)
            {
                return "0";
            }
            else
            {
                return ( balance / 100 ).ToString();
            }
        }

        /// <summary>
        /// CRC校验，参数data为byte数组
        /// </summary>
        /// <param name="data">校验数据，字节数组</param>
        /// <returns>字节0是高8位，字节1是低8位</returns>
        public static ushort CRC16 (byte[] data, int len)
        {
            //crc计算赋初始值
            int crc = 0xffff;
            for (int i = 0; i < len; i++)
            {
                crc = crc ^ data[i];
                for (int j = 0; j < 8; j++)
                {
                    int temp = crc & 1;
                    crc = crc >> 1;
                    crc = crc & 0x7fff;
                    if (temp == 1)
                    {
                        crc = crc ^ 0xa001;
                    }
                    crc = crc & 0xffff;
                }
            }
            return (ushort)crc;
        }
        public static int CS (byte[] data, int begin, int end)
        {
            int cs = 0;
            for (int i = begin; i < end; i++)
            {
                cs = ( cs + data[i] ) % 256;
            }
            return cs;
        }
        public static int CS (byte[] data, int end)
        {
            int cs = 0;
            for (int i = 0; i < end; i++)
            {
                cs = ( cs + data[i] ) % 256;
            }
            return cs;
        }
        public static byte CSByByte (byte[] data)
        {
            int cs = 0;
            foreach (byte i in data)
            {
                cs = ( cs + i ) % 256;
            }
            return (byte)cs;
        }
        public static byte CSByByte (byte[] data, int end)
        {
            int cs = 0;
            for (int i = 0; i < end; i++)
            {
                cs = ( cs + data[i] ) % 256;
            }
            return (byte)cs;
        }
        private static readonly int _maxCsSize = 5 * 1024 * 1024;
        public static int CS (FileStream stream, long skip, int size)
        {
            int surps = (int)( stream.Length - skip );
            if (size > surps)
            {
                size = surps;
            }
            stream.Position = skip;
            if (size <= _maxCsSize)
            {
                byte[] bytes = new byte[size];
                int len = stream.Read(bytes, 0, bytes.Length);
                return CS(bytes, len);
            }
            int num = size;
            int i = _maxCsSize;
            int cs = 0;
            do
            {
                if (i > num)
                {
                    i = num;
                }
                byte[] bytes = new byte[i];
                int len = stream.Read(bytes, 0, i);
                cs = ( cs + CS(bytes, len) ) % _v;
                num -= i;
            } while (num != 0);
            return cs;
        }

        private static readonly int _v = 256;
        public static int CS (byte[] one, byte[] two, int end)
        {
            int cs = CS(one, one.Length);
            return ( cs + CS(two, end) ) % _v;
        }

        public static void CSMerge (byte[] one, ref int cs)
        {
            cs = ( cs + CS(one, one.Length) ) % _v;
        }
        public static void CSMerge (byte[] one, int len, ref int cs)
        {
            cs = ( cs + CS(one, len) ) % _v;
        }
        public static int CS (byte[] one, byte[] two)
        {
            int cs = CS(one, one.Length);
            return ( cs + CS(two, two.Length) ) % _v;
        }
        public static bool GetBitValue (byte val, int index)
        {
            return ( val & ( 1 << index ) ) == 1;
        }
        public static byte SetBitValue (byte source, int index, bool flag)
        {
            byte v = (byte)( 1 << index );
            return flag ? (byte)( source | v ) : (byte)( source & ~v );
        }
        /// <summary>
        /// 时间戳转时间
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime GetTimeStamp (long timeStamp)
        {
            return _DtStart.AddSeconds(timeStamp).ToLocalTime();
        }
        /// <summary>
        /// 格式化时间长度描述(秒开始)
        /// </summary>
        /// <param name="second"></param>
        /// <returns></returns>
        public static string FormatSecond (int second)
        {
            if (second == 0)
            {
                return "00:00:00";
            }
            else if (second < 60)
            {
                return string.Concat("00:00:", second);
            }
            int hour = second / 3600;
            if (hour != 0)
            {
                second %= 3600;
            }
            int min = second / 60;
            if (min != 0)
            {
                second %= 60;
            }
            return string.Format("{0}:{1}:{2}",
                    hour.ToString().PadLeft(2, '0'),
                    min.ToString().PadLeft(2, '0'),
                    second.ToString().PadLeft(2, '0'));
        }
        /// <summary>
        /// 格式化时间长度描述(秒开始)
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string FormatTime (long time)
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
            int num = (int)( time / 86400 );
            if (num != 0)
            {
                _ = str.AppendFormat("{0}天", num);
                time %= 86400;
            }
            num = (int)( time / 3600 );
            if (num != 0)
            {
                _ = str.AppendFormat("{0}小时", num);
                time %= 3600;
            }
            num = (int)( time / 60 );
            if (num != 0)
            {
                _ = str.AppendFormat("{0}分", num);
                time %= 60;
            }
            if (time != 0)
            {
                _ = str.AppendFormat("{0}秒", num);
            }
            return str.ToString();
        }
        public static string FormatTimeMilli (long time)
        {
            if (time == 0)
            {
                return "0毫秒";
            }
            else if (time < 1000)
            {
                return string.Concat(time, "毫秒");
            }

            int milli = (int)( time % 1000 );
            time = time / 1000;
            StringBuilder str = new StringBuilder();
            int num = (int)( time / 86400 );
            if (num != 0)
            {
                _ = str.AppendFormat("{0}天", num);
                time %= 86400;
            }
            num = (int)( time / 3600 );
            if (num != 0)
            {
                _ = str.AppendFormat("{0}小时", num);
                time %= 3600;
            }
            num = (int)( time / 60 );
            if (num != 0)
            {
                _ = str.AppendFormat("{0}分", num);
                time %= 60;
            }
            if (time != 0)
            {
                _ = str.AppendFormat("{0}秒", time);
            }
            if (milli != 0)
            {
                _ = str.AppendFormat("{0}毫秒", milli);
            }
            return str.ToString();
        }
        /// <summary>
        /// 返回一字符串，十进制 number 以 radix 进制的表示
        /// </summary>
        /// <param name="dec">数字</param>
        /// <param name="toRadix">进制</param>
        /// <returns></returns>
        public static string Dec2Any (long dec, int toRadix)
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
                buf[charPos--] = num62.Substring(-(int)( dec % toRadix ), 1);
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
        public static short GetSerialNoMonth (string serialNo)
        {
            return short.Parse(serialNo.Substring(8, 2));
        }

        /// <summary>
        /// 返回一字符串，包含 number 以 10 进制的表示。
        /// </summary>
        /// <param name="number"></param>
        /// <param name="fromRadix"></param>
        /// <returns></returns>
        public static long Any2Dec (string number, int fromRadix)
        {
            string num62 = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            long dec = 0;
            int len = number.Length - 1;
            for (int t = 0; t <= len; t++)
            {
                int digitValue = num62.IndexOf(number[t]);
                dec = ( dec * fromRadix ) + digitValue;
            }
            return dec;
        }
        /// <summary>
        /// 将字符串Basic64
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string ToBasic64 (string code)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(code);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>   
        /// 得到字符串的长度，一个汉字算2个字符   
        /// </summary>   
        /// <param name="str">字符串</param>   
        /// <returns>返回字符串长度</returns>   
        public static int GetLength (string str)
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

        public static string Substring (string str, ref int index, int len)
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
                    end = tempLen == len ? i : i - 1;
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
        public static decimal GetLength (string str, decimal num, out int len)
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
        public static string Escape (string s)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byteArr = Encoding.Unicode.GetBytes(s);
            for (int i = 0; i < byteArr.Length; i += 2)
            {
                _ = sb.Append("%u");
                _ = sb.Append(byteArr[i + 1].ToString("X2"));
                _ = sb.Append(byteArr[i].ToString("X2"));
            }
            return sb.ToString();

        }

        public static string UnEscape (string s)
        {
            string str = s.Remove(0, 2);
            string[] strArr = str.Split(new string[] { "%u" }, StringSplitOptions.None);
            byte[] byteArr = new byte[strArr.Length * 2];
            for (int i = 0, j = 0; i < strArr.Length; i++, j += 2)
            {
                byteArr[j + 1] = Convert.ToByte(strArr[i].Substring(0, 2), 16);
                byteArr[j] = Convert.ToByte(strArr[i].Substring(2, 2), 16);
            }
            str = Encoding.Unicode.GetString(byteArr);
            return str;

        }
        public static string EncryptSHA256 (string txt, string secret)
        {
            secret ??= "";
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(txt);
            using (HMACSHA256 hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashmessage.Length; i++)
                {
                    _ = builder.Append(hashmessage[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        public static int GetLength (char[] chars, int index)
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
        public static string RSAEncrypt (string publickey, string content)
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
        public static string DSAEncrypt (string publickey, string content)
        {
            DSACryptoServiceProvider dsa = new DSACryptoServiceProvider();
            byte[] cipherbytes;
            dsa.FromXmlString(publickey);
            cipherbytes = dsa.SignData(Encoding.UTF8.GetBytes(content));
            return Convert.ToBase64String(cipherbytes);
        }

        /// <summary>
        /// 获取毫秒为单位的时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetTimeStampByMs ()
        {
            TimeSpan ts = DateTime.UtcNow - _DtStart;
            return (long)ts.TotalMilliseconds;
        }
        /// <summary>
        /// 获取毫秒为单位的时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetTimeStampByMs (DateTime time)
        {
            TimeSpan ts = time.ToUniversalTime() - _DtStart;
            return (long)ts.TotalMilliseconds;
        }

        /// <summary>
        /// 获取字符串MD5值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetMD5 (string str)
        {
            using (MD5 md5 = MD5.Create())
            {
                md5.Initialize();
                byte[] result = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
                return BitConverter.ToString(result).Replace("-", string.Empty);
            }
        }
        /// <summary>
        /// 获取本地的IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIp ()
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
        public static string DeleteHTML (string content)
        {
            if (!string.IsNullOrEmpty(content))
            {
                content = content.Replace("\r", string.Empty);
                content = content.Replace("\n", string.Empty);
                return FilterHtml(content);
            }
            return content;
        }
        private static readonly IReadOnlyList<FilterHtmlType> _FilterEnum = EnumHelper.GetValues<FilterHtmlType>();
        private static readonly FilterHtmlType _AllFilter = FilterHtmlType.注释 | FilterHtmlType.Style | FilterHtmlType.Script | FilterHtmlType.Link | FilterHtmlType.If | FilterHtmlType.Event | FilterHtmlType.A标签;
        public static string FilterHtml (string content, FilterHtmlType filter)
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
                if (( i & filter ) == i)
                {
                    content = _FilterHtml(content, i);
                }
            }
            return content;
        }
        private static string _FilterHtml (string content, FilterHtmlType type)
        {
            switch (type)
            {
                case FilterHtmlType.If:
                    return _ReplaceIf().Replace(content, string.Empty);
                case FilterHtmlType.Link:
                    return _ReplaceLink().Replace(content, string.Empty);
                case FilterHtmlType.Script:
                    content = _ReplaceScript().Replace(content, string.Empty);
                    content = _ReplaceJavascript().Replace(content, string.Empty);
                    return content;
                case FilterHtmlType.Style:
                    return _ReplaceStyle().Replace(content, string.Empty);
                case FilterHtmlType.注释:
                    content = _ReplaceAnnotation().Replace(content, string.Empty);
                    content = _ReplaceAnnotationTwo().Replace(content, string.Empty);
                    return content;
                case FilterHtmlType.A标签:
                    return _ReplaceAlink().Replace(content, string.Empty);
                case FilterHtmlType.Event:
                    return _ReplaceJsEvent().Replace(content, string.Empty);
            }
            return content;
        }
        /// <summary>
        /// 过滤掉内容中的HTML
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string FilterHtml (string content)
        {
            content = _ReplaceIf().Replace(content, string.Empty);
            content = _ReplaceScript().Replace(content, string.Empty);
            content = _ReplaceLink().Replace(content, string.Empty);
            content = _ReplaceStyle().Replace(content, string.Empty);
            content = _ReplaceSymbol().Replace(content, string.Empty);
            content = _ReplaceAnnotation().Replace(content, string.Empty);
            content = _ReplaceArrow().Replace(content, string.Empty);
            content = _ReplaceSpecialSymbol().Replace(content, string.Empty);
            return content;
        }

        public static string HideEmail (string email)
        {
            if (email.IsNull())
            {
                return email;
            }
            int index = email.LastIndexOf("@");
            int len = (int)Math.Ceiling(index / 2.5);
            int begin = GetRandom(0, len);
            StringBuilder str = new StringBuilder(email);
            _ = str.Remove(begin, len);
            _ = str.Insert(begin, "*".PadLeft(len, '*'));
            return str.ToString();
        }

        [GeneratedRegex("(<link.*?>)|(<link.*?/>)", RegexOptions.Multiline)]
        private static partial Regex _ReplaceLink ();
        [GeneratedRegex("([<][!][-][-][[]if [!]IE[]][>].+[<][!][[]endif[]][-][-][>])+", RegexOptions.Multiline)]
        private static partial Regex _ReplaceIf ();
        [GeneratedRegex("(<script.*?>.*?</script>)", RegexOptions.Multiline)]
        private static partial Regex _ReplaceScript ();
        [GeneratedRegex("(<style.*?>.*?</style>)", RegexOptions.Multiline)]
        private static partial Regex _ReplaceStyle ();
        [GeneratedRegex("<!.*?>")]
        private static partial Regex _ReplaceSymbol ();
        [GeneratedRegex("<!--.*?-->")]
        private static partial Regex _ReplaceAnnotation ();
        [GeneratedRegex("<.*?>")]
        private static partial Regex _ReplaceArrow ();
        [GeneratedRegex("&.*?;")]
        private static partial Regex _ReplaceSpecialSymbol ();
        [GeneratedRegex("javascript[:].+[^\",'>]", RegexOptions.Multiline)]
        private static partial Regex _ReplaceJavascript ();
        [GeneratedRegex("[/][*].*?[*][/]", RegexOptions.Multiline)]
        private static partial Regex _ReplaceAnnotationTwo ();
        [GeneratedRegex("(<a.*?>.*?</a>)", RegexOptions.Multiline)]
        private static partial Regex _ReplaceAlink ();
        [GeneratedRegex("([ ]on\\w+[=].+?['\"'])", RegexOptions.Multiline)]
        private static partial Regex _ReplaceJsEvent ();
        [GeneratedRegex("([ ]([a-zA-Z0-9-.+]+[ ])+Build){1}", RegexOptions.IgnoreCase, "zh-CN")]
        private static partial Regex _Android ();
        [GeneratedRegex("(Windows[ ]NT[ ]\\d+[.]\\d+){1}", RegexOptions.IgnoreCase, "zh-CN")]
        private static partial Regex _Windows ();
        [GeneratedRegex("(Intel[ ]Mac[ ]OS[ ]X[ ](\\d+[._])+\\d*){1}", RegexOptions.IgnoreCase, "zh-CN")]
        private static partial Regex _Macintosh ();
        [GeneratedRegex("(IEMobile[/]\\d+[.]\\d+[;](([ ]|\\w)+[;]{0,1})+[^)]){1}", RegexOptions.IgnoreCase, "zh-CN")]
        private static partial Regex WindowsPhone ();
        [GeneratedRegex("^wifi.+[ ]cfnetwork[/]\\d+([.]\\d+)*[ ]darwin[/]\\d+([.]\\d+)*$")]
        private static partial Regex Apple ();
        [GeneratedRegex("(OS[ ]((\\d+[_.])+\\d{0,}){1}){1}", RegexOptions.IgnoreCase, "zh-CN")]
        private static partial Regex AppleOS ();
    }
}
