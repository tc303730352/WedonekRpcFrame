using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;

namespace HttpService.Helper
{
        internal class Helper
        {
                public static Uri GetUrlReferrer(HttpListenerRequest request)
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
                public static string GetUpTempFileSavePath(UpFileParam param)
                {
                        return FileHelper.GetFilePath(string.Concat(@"\upTempFile\", Guid.NewGuid().ToString("N"), Path.GetExtension(param.FileName)));
                }
                public static bool CheckIsUpFile(string contentType)
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
                public static byte[] ReadLineAsBytes(Stream source, int len, int size = 2048)
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
                private static byte[] _ReadLineAsBytes(Stream stream)
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
                public static NameValueCollection GetForm(string str)
                {
                        NameValueCollection from = new NameValueCollection();
                        string[] temp = str.Split('&');
                        foreach (string i in temp)
                        {
                                int index = i.IndexOf("=");
                                if (index != -1)
                                {
                                        string key = i.Substring(0, index).Trim();
                                        string value = i.Substring(index + 1).Trim();
                                        from.Add(key, RpcHelper.Tools.DecodeURI(value));
                                }
                        }
                        return from;
                }
        }
}
