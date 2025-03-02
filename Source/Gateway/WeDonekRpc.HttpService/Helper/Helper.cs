using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using WeDonekRpc.Helper;
using WeDonekRpc.HttpService.FileUp;
using WeDonekRpc.HttpService.Handler;
using WeDonekRpc.HttpService.Model;

namespace WeDonekRpc.HttpService.Helper
{
    internal static class Helper
    {
        private static readonly Dictionary<string, string> _TypeRegex = [];
        private static readonly string _wildcard = "*";
        static Helper ()
        {
            _TypeRegex.Add("integer", @"[-]{0,1}\d+");
            _TypeRegex.Add("uinteger", @"\d+");
            _TypeRegex.Add("bool", @"((true)|(false)){1}");
            _TypeRegex.Add("guid", @"((\w{32}|)|(\w{8}[-]\w{4}[-]\w{4}[-]\w{4}[-]\w{12})){1}");
            _TypeRegex.Add("double", @"[-]{0,1}\d+([.]\d+){0,1}");
            _TypeRegex.Add("udouble", @"\d+([.]\d+){0,1}");
            _TypeRegex.Add("alpha", @"[a-z]+");
            _TypeRegex.Add("date", @"(\d{4}[-]((1[0-2])|([0]{0,1}[1-9])){1}[-](([0][1-9])|([1-2][0-9])|([3][0-1])){1}){0,1}");
        }
        public static RequestPathType GetPathType (string path, out KeyValuePair<int, string>[] keys, out string[] ruleParth)
        {
            if (!path.StartsWith('/'))
            {
                path = path.Insert(0, "/");
            }
            string[] paths = path.Split('/');
            int[] index = paths.FindAllIndex(a => a != string.Empty && a[0] == '{' && a[a.Length - 1] == '}');
            if (index.Length > 0)
            {
                keys = index.ConvertAll(a =>
                {
                    string val = paths[a];
                    val = val.Remove(0, 1);
                    val = val.Remove(val.Length - 1, 1);
                    paths[a] = _wildcard;
                    return new KeyValuePair<int, string>(a, val);
                });
                ruleParth = paths;
                return RequestPathType.RulePath;
            }
            else if (paths[paths.Length - 1] == string.Empty)
            {
                keys = null;
                ruleParth = null;
                return RequestPathType.Relative;
            }
            ruleParth = null;
            keys = null;
            return RequestPathType.Full;
        }
        public static bool CheckRulePath (string[] paths, string[] rulePath)
        {
            if (paths.Length != rulePath.Length)
            {
                return false;
            }
            for (int i = 0; i < paths.Length; i++)
            {
                if (!CheckRulePath(paths[i], rulePath[i]))
                {
                    return false;
                }
            }
            return true;
        }
        private static bool CheckRulePath (string path, string rulePath)
        {
            return path == rulePath || rulePath == _wildcard;
        }
        public static string GetRegexStr (string type)
        {
            if (_TypeRegex.TryGetValue(type.ToLower(), out string val))
            {
                return val;
            }
            throw new ErrorException("http.type.not.find", "Type:" + type);
        }
        public static Uri GetUrlReferrer (HttpListenerRequest request)
        {
            Uri uri = request.UrlReferrer;
            if (uri == null && request.Headers["origin"] != null)
            {
                string temp = request.Headers["origin"];
                if (!string.IsNullOrEmpty(temp))
                {
                    uri = new Uri(temp);
                }
            }
            return uri;
        }
        /// <summary>
        /// 获取上传临时文件保存路径
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string GetUpTempFileSavePath (UpFileParam param)
        {
            return FileHelper.GetTempSavePath(string.Concat(@"\upTempFile\", Guid.NewGuid().ToString("N"), Path.GetExtension(param.FileName)));
        }
        public static bool CheckIsUpFile (string contentType)
        {
            if (string.IsNullOrEmpty(contentType))
            {
                return false;
            }
            return contentType.StartsWith("multipart/form-data;");
        }
        /// <summary>
        /// 读取字节流
        /// </summary>
        /// <param name="source"></param>
        /// <param name="len"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static byte[] ReadLineAsBytes (Stream source, int len, int size = 2048)
        {
            if (len == -1)
            {
                return _ReadLineAsBytes(source);
            }
            if (size > len)
            {
                size = len;
            }
            int index = 0;
            int max = len - size;
            byte[] myByte = new byte[size];
            using (MemoryStream stream = new MemoryStream(len))
            {
                stream.Position = 0;
                do
                {
                    int num = source.Read(myByte, 0, size);
                    if (num > 0)
                    {
                        stream.Write(myByte, 0, num);
                        index += num;
                        if (index > max && index != len)
                        {
                            size = len - index;
                            myByte = new byte[size];
                        }
                    }
                } while (index != len);
                stream.Flush();
                return stream.ToArray();
            }
        }
        private static byte[] _ReadLineAsBytes (Stream stream)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                int read = 0;
                while (true)
                {
                    read = stream.ReadByte();
                    if (read == -1)
                    {
                        ms.Flush();
                        return ms.ToArray();
                    }
                    ms.WriteByte((byte)read);
                }
            }
        }
        public static NameValueCollection GetForm (string str)
        {
            NameValueCollection from = [];
            string[] temp = str.Split('&');
            foreach (string i in temp)
            {
                int index = i.IndexOf("=");
                if (index != -1)
                {
                    string key = i.Substring(0, index).Trim();
                    string value = i.Substring(index + 1).Trim();
                    from.Add(key, Tools.DecodeURI(value));
                }
            }
            return from;
        }

        internal static void SetRuleQuery (BasicHandler handler, KeyValuePair<int, string>[] keys)
        {
            string[] paths = handler.Request.Url.AbsolutePath.Split('/');
            handler.PathArgs = [];
            foreach (KeyValuePair<int, string> i in keys)
            {
                handler.PathArgs.Add(i.Value, paths[i.Key]);
            }
        }
        public static string ToClientIp (this HttpListenerRequest request)
        {
            string ip = request.Headers["X-Real-IP"];
            if (ip.IsNull())
            {
                return request.RemoteEndPoint.Address.ToString();
            }
            string port = request.Headers["X-Real-PORT"];
            if (port.IsNull())
            {
                port = request.RemoteEndPoint.Port.ToString();
            }
            return ip + ":" + port;//x-real-url
        }

    }
}
