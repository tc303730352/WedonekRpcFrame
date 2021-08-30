using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace RpcHelper
{
        public class HttpTools
        {
                private const string _DefImgFormat = ".jpg";
                static HttpTools()
                {
                        System.Net.ServicePointManager.DefaultConnectionLimit = int.MaxValue;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                        ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(_CheckValidationResult);
                }
                public static void SetSecurityProtocolType(SecurityProtocolType type)
                {
                        ServicePointManager.SecurityProtocol = type;
                }
                private static bool _CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
                {
                        return true; //总是接受  
                }
                private static HttpWebRequest _GetRequest(Uri uri, string post, HttpRequestSet reqSet)
                {
                        if (post.IsNull())
                        {
                                return _CreateRequest(uri, reqSet, "GET");
                        }
                        return _GetRequest(uri, reqSet.RequestEncoding.GetBytes(post), reqSet);
                }
                private static HttpWebRequest _CreateRequest(Uri uri, HttpRequestSet reqSet, string method)
                {
                        HttpWebRequest _Request = (HttpWebRequest)HttpWebRequest.Create(uri);
                        _Request.Expect = null;
                        _Request.Accept = reqSet.Accept;
                        _Request.Timeout = reqSet.Timeout;
                        _Request.Host = uri.Host;
                        _Request.ReadWriteTimeout = reqSet.ReadWriteTimeout;
                        _Request.ContinueTimeout = reqSet.ContinueTimeout;
                        _Request.UserAgent = reqSet.UserAgent;
                        _Request.ContentType = reqSet.ContentType;
                        _Request.KeepAlive = false;
                        _Request.SendChunked = reqSet.SendChunked;
                        _Request.Referer = reqSet.Referer;
                        _Request.ProtocolVersion = reqSet.ProtocolVersion;
                        _Request.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);
                        _Request.Method = reqSet.SubmitMethod.IsNull() ? method : reqSet.SubmitMethod;
                        if (uri.Scheme == "https")
                        {
                                _Request.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(_CheckValidationResult);
                                if (reqSet.HttpsCert != null)
                                {
                                        CertInfo cert = reqSet.HttpsCert;
                                        _Request.ClientCertificates = new X509CertificateCollection();
                                        X509Certificate2 obj = new X509Certificate2(cert.CertPath, cert.CertPwd);
                                        _Request.ClientCertificates.Add(obj);
                                }
                                ServicePointManager.SecurityProtocol = reqSet.SecurityProtocolType;
                        }
                        if (reqSet.HeadList.Count > 0)
                        {
                                foreach (KeyValuePair<string, string> a in reqSet.HeadList)
                                {
                                        _Request.Headers.Add(string.Format("{0}:{1}", a.Key, a.Value));
                                }
                        }
                        if (reqSet.Cookies.Count > 0)
                        {
                                _Request.CookieContainer = new CookieContainer();
                                foreach (KeyValuePair<string, string> a in reqSet.Cookies)
                                {
                                        _Request.CookieContainer.Add(new Cookie(a.Key, a.Value, "/", uri.Authority));
                                }
                        }

                        return _Request;
                }
                private static HttpWebRequest _GetRequest(Uri uri, byte[] post, HttpRequestSet reqSet)
                {
                        HttpWebRequest _Request = _CreateRequest(uri, reqSet, "POST");
                        _Request.ContentLength = post.Length;
                        using (Stream stream = _Request.GetRequestStream())
                        {
                                stream.Write(post, 0, post.Length);
                        }
                        return _Request;
                }

                #region 读取流的方法
                public static byte[] ReadStream(Stream stream)
                {
                        using (MemoryStream ms = new MemoryStream())
                        {
                                int read = 0;
                                while (true)
                                {
                                        read = stream.ReadByte();
                                        if (read == -1)
                                        {
                                                return ms.GetBuffer();
                                        }
                                        ms.WriteByte((byte)read);
                                }
                        }
                }
                public static void SaveFileStream(Stream stream, string path, long len, int size = 1024)
                {
                        if (size < len)
                        {
                                size = (int)len;
                        }
                        long index = 0;
                        long max = len - size;
                        byte[] myByte = new byte[size];
                        FileInfo file = new FileInfo(path);
                        if (!file.Directory.Exists)
                        {
                                file.Directory.Create();
                        }
                        using (FileStream ms = file.Open(FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Delete))
                        {
                                ms.Position = 0;
                                do
                                {
                                        int num = stream.Read(myByte, 0, size);
                                        if (num > 0)
                                        {
                                                ms.Write(myByte, 0, num);
                                                index += num;
                                                if (index > max && index != len)
                                                {
                                                        size = (int)(len - index);
                                                        myByte = new byte[size];
                                                }
                                        }
                                } while (index != len);
                                ms.Flush();
                                ms.Close();
                        }
                }
                public static byte[] ReadStream(Stream SourceStream, long slen, int size = 1024)
                {
                        if (slen == -1)
                        {
                                return ReadStream(SourceStream);
                        }
                        int len = (int)slen;
                        if (size < len)
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
                                        int num = SourceStream.Read(myByte, 0, size);
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
                                return stream.ToArray();
                        }
                }
                #endregion
                public static bool SubmitData(Uri uri, string post, out HttpStatusCode status, out string str, HttpRequestSet reqSet)
                {
                        if (SubmitData(uri, post, out status, out byte[] bytes, reqSet))
                        {
                                str = bytes.Length != 0 ? reqSet.HtmlEncoding.GetString(bytes).Replace("\0", string.Empty) : string.Empty;
                                return true;
                        }
                        str = null;
                        return false;
                }
                public static bool SubmitData(Uri uri, string post, out string str, HttpRequestSet reqSet)
                {
                        if (SubmitData(uri, post, out byte[] bytes, reqSet))
                        {
                                str = bytes.Length != 0 ? reqSet.HtmlEncoding.GetString(bytes).Replace("\0", string.Empty) : string.Empty;
                                return true;
                        }
                        str = null;
                        return false;
                }
                public static bool GetStream(Uri uri, string post, out HttpStreamRes res, HttpRequestSet reqSet)
                {
                        HttpWebRequest _Request = null;
                        try
                        {
                                _Request = _GetRequest(uri, post, reqSet);
                                using (HttpWebResponse response = (HttpWebResponse)_Request.GetResponse())
                                {
                                        res = new HttpStreamRes
                                        {
                                                StatusCode = HttpStatusCode.OK
                                        };
                                        res.HeadList = new KeValue[response.Headers.Count];
                                        res.ContentType = response.ContentType;
                                        int i = 0;
                                        foreach (string name in response.Headers)
                                        {
                                                res.HeadList[i++] = new KeValue
                                                {
                                                        Name = name,
                                                        Value = response.Headers[name]
                                                };
                                        }
                                        if (response.Cookies.Count > 0)
                                        {
                                                res.Cookies = new KeValue[response.Cookies.Count];
                                                i = 0;
                                                foreach (Cookie cookie in response.Cookies)
                                                {
                                                        res.Cookies[i++] = new KeValue
                                                        {
                                                                Name = cookie.Name,
                                                                Value = cookie.Value
                                                        };
                                                }
                                        }
                                        using (Stream stream = response.GetResponseStream())
                                        {
                                                res.Stream = ReadStream(stream, response.ContentLength);
                                        }
                                        response.Close();
                                        response.Dispose();
                                }
                                return true;
                        }
                        catch (WebException e)
                        {
                                res = e.Response is HttpWebResponse response
                                      ? new HttpStreamRes
                                      {
                                              StatusCode = response.StatusCode,
                                              Error = e
                                      }
                                      : new HttpStreamRes
                                      {
                                              StatusCode = HttpStatusCode.InternalServerError,
                                              Error = e
                                      };
                                return false;
                        }
                        catch (Exception e)
                        {
                                res = new HttpStreamRes
                                {
                                        StatusCode = HttpStatusCode.InternalServerError,
                                        Error = e
                                };
                                return false;
                        }
                        finally
                        {
                                if (_Request != null)
                                {
                                        _Request.Abort();
                                }
                        }
                }
                public static bool SubmitData(Uri uri, string post, out byte[] bytes, HttpRequestSet reqSet)
                {
                        HttpWebRequest _Request = null;
                        try
                        {
                                _Request = _GetRequest(uri, post, reqSet);
                                using (HttpWebResponse response = (HttpWebResponse)_Request.GetResponse())
                                {
                                        using (Stream stream = response.GetResponseStream())
                                        {
                                                bytes = ReadStream(stream, response.ContentLength);
                                        }
                                        response.Close();
                                        response.Dispose();
                                }
                                return true;
                        }
                        catch (WebException e)
                        {
                                HttpStatusCode status = HttpStatusCode.Accepted;
                                status = e.Response is HttpWebResponse response ? response.StatusCode : HttpStatusCode.InternalServerError;
                                bytes = null;
                                return false;
                        }
                        catch (Exception e)
                        {
                                _AddErrorLog(uri, e);
                                bytes = null;
                                return false;
                        }
                        finally
                        {
                                if (_Request != null)
                                {
                                        _Request.Abort();
                                }
                        }
                }
                public static bool SubmitData(Uri uri, string post, out HttpStatusCode status, out byte[] bytes, HttpRequestSet reqSet)
                {
                        HttpWebRequest _Request = null;
                        try
                        {
                                _Request = _GetRequest(uri, post, reqSet);
                                using (HttpWebResponse response = (HttpWebResponse)_Request.GetResponse())
                                {
                                        status = response.StatusCode;
                                        using (Stream stream = response.GetResponseStream())
                                        {
                                                bytes = ReadStream(stream, response.ContentLength);
                                        }
                                        response.Close();
                                        response.Dispose();
                                }
                                return true;
                        }
                        catch (WebException e)
                        {
                                status = e.Response is HttpWebResponse response ? response.StatusCode : HttpStatusCode.InternalServerError;
                                bytes = null;
                                return false;
                        }
                        catch (Exception e)
                        {
                                _AddErrorLog(uri, e);
                                status = HttpStatusCode.BadRequest;
                                bytes = null;
                                return false;
                        }
                        finally
                        {
                                if (_Request != null)
                                {
                                        _Request.Abort();
                                }
                        }
                }
                private static void _AddErrorLog(Uri uri, Exception e)
                {
                        new ErrorLog(e, "HTTP请求失败!") { LogContent = uri.AbsoluteUri }.Save();
                }
                public static bool SubmitByte(Uri uri, byte[] post, HttpRequestSet reqSet)
                {
                        HttpWebRequest _Request = null;
                        try
                        {
                                _Request = _GetRequest(uri, post, reqSet);
                                using (HttpWebResponse response = (HttpWebResponse)_Request.GetResponse())
                                {
                                        response.Close();
                                        response.Dispose();
                                }
                                return true;
                        }
                        catch (Exception e)
                        {
                                _AddErrorLog(uri, e);
                                return false;
                        }
                        finally
                        {
                                if (_Request != null)
                                {
                                        _Request.Abort();
                                }
                        }
                }

                /// <summary>
                /// 获取请求头中的文件名
                /// </summary>
                /// <param name="head"></param>
                /// <param name="fileName"></param>
                /// <returns></returns>
                private static bool _GetHeaderFileName(string head, out string fileName)
                {
                        string[] temp = head.ToLower().Split(';');
                        head = Array.Find(temp, a => a.Trim().StartsWith("filename="));
                        if (head != null)
                        {
                                fileName = head.Replace("filename=", string.Empty).Replace("\"", string.Empty).Trim();
                                return true;
                        }
                        fileName = null;
                        return false;
                }
                private static bool _GetFileSavePath(Uri uri, HttpWebResponse response, ref string savePath, HttpRequestSet reqSet)
                {
                        string extension = null;
                        if (!string.IsNullOrEmpty(response.Headers.Get("Content-Disposition")))
                        {
                                if (_GetHeaderFileName(response.Headers.Get("Content-Disposition"), out string name))
                                {
                                        extension = Path.GetExtension(name);
                                }
                                else if (Path.HasExtension(savePath))
                                {
                                        extension = Path.GetExtension(name);
                                }
                        }
                        else if (Path.HasExtension(savePath))
                        {
                                extension = Path.GetExtension(savePath);
                        }
                        if (reqSet.FileSave != null)
                        {
                                savePath = reqSet.FileSave.Invoke(uri, savePath, extension);
                        }
                        else if (!string.IsNullOrEmpty(extension))
                        {
                                savePath = !reqSet.IsResetFileName
                                        ? Path.ChangeExtension(savePath, extension)
                                        : Path.Combine(Path.GetDirectoryName(savePath), string.Format("{0}{1}", Guid.NewGuid().ToString("n"), extension));
                        }
                        else
                        {
                                return false;
                        }
                        return true;
                }
                public static bool DownFile(Uri uri, string post, ref string savePath, out HttpStatusCode status, HttpRequestSet reqSet)
                {
                        HttpWebRequest _Request = null;
                        try
                        {
                                _Request = _GetRequest(uri, post, reqSet);
                                using (HttpWebResponse response = (HttpWebResponse)_Request.GetResponse())
                                {
                                        status = response.StatusCode;
                                        if (response.StatusCode != HttpStatusCode.OK)
                                        {
                                                return false;
                                        }
                                        else if (response.ContentLength < reqSet.MinFileSize)
                                        {
                                                return false;
                                        }
                                        else if (!_GetFileSavePath(uri, response, ref savePath, reqSet))
                                        {
                                                return false;
                                        }
                                        else
                                        {
                                                using (Stream stream = response.GetResponseStream())
                                                {
                                                        SaveFileStream(stream, savePath, response.ContentLength);
                                                        response.Close();
                                                        response.Dispose();
                                                        return true;
                                                }
                                        }
                                }
                        }
                        catch (WebException e)
                        {
                                status = e.Response is HttpWebResponse response ? response.StatusCode : HttpStatusCode.InternalServerError;
                                return false;
                        }
                        catch (Exception e)
                        {
                                status = HttpStatusCode.InternalServerError;
                                _AddErrorLog(uri, e);
                                return false;
                        }
                        finally
                        {
                                if (_Request != null)
                                {
                                        _Request.Abort();
                                }
                        }
                }

                public static bool UploadFile(Uri uri, string postData, string filePath, out HttpStatusCode status, out string res, HttpRequestSet reqSet)
                {
                        FileInfo file = new FileInfo(filePath);
                        if (!file.Exists)
                        {
                                status = HttpStatusCode.NotFound;
                                res = null;
                                return false;
                        }
                        HttpWebRequest _Request = null;
                        try
                        {
                                string boundary = "----" + DateTime.Now.Ticks.ToString("x");
                                reqSet.ContentType = string.Format("multipart/form-data; boundary={0}", boundary);
                                _Request = _CreateRequest(uri, reqSet, "POST");
                                using (Stream stream = _Request.GetRequestStream())
                                {
                                        string line = null;
                                        byte[] myByte = null;
                                        if (!string.IsNullOrEmpty(postData))
                                        {
                                                line = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"description\"\r\n\r\n{1}\r\n", boundary, postData);
                                                myByte = reqSet.RequestEncoding.GetBytes(line);
                                                stream.Write(myByte, 0, myByte.Length);
                                        }
                                        line = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"media\"; filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\n\r\n", boundary, file.Name);
                                        myByte = reqSet.RequestEncoding.GetBytes(line);
                                        stream.Write(myByte, 0, myByte.Length);
                                        using (FileStream fStream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
                                        {
                                                myByte = new byte[1024];
                                                int bytesRead = 0;
                                                while ((bytesRead = fStream.Read(myByte, 0, myByte.Length)) != 0)
                                                {
                                                        stream.Write(myByte, 0, bytesRead);
                                                }
                                        }
                                        myByte = reqSet.RequestEncoding.GetBytes("\r\n--" + boundary + "--\r\n");
                                        stream.Write(myByte, 0, myByte.Length);
                                        stream.Flush();
                                }
                                byte[] resByte = null;
                                using (HttpWebResponse response = (HttpWebResponse)_Request.GetResponse())
                                {
                                        status = response.StatusCode;
                                        using (Stream stream = response.GetResponseStream())
                                        {
                                                resByte = ReadStream(stream, response.ContentLength);
                                        }
                                        response.Close();
                                        response.Dispose();
                                }
                                if (resByte != null)
                                {
                                        res = reqSet.HtmlEncoding.GetString(resByte).Replace("\0", string.Empty);
                                        status = HttpStatusCode.OK;
                                        return true;
                                }
                        }
                        catch (WebException e)
                        {
                                res = null;
                                status = e.Response is HttpWebResponse response ? response.StatusCode : HttpStatusCode.InternalServerError;
                                return false;
                        }
                        catch (Exception e)
                        {
                                res = null;
                                status = HttpStatusCode.InternalServerError;
                                _AddErrorLog(uri, e);
                                return false;
                        }
                        finally
                        {
                                if (_Request != null)
                                {
                                        _Request.Abort();
                                }
                        }
                        res = null;
                        return false;
                }


                private static string _GetImgType(Uri uri, HttpWebResponse response, string savePath, HttpRequestSet reqSet)
                {
                        string imgType = null;
                        if (!string.IsNullOrEmpty(response.Headers.Get("Content-Disposition")))
                        {
                                if (_GetHeaderFileName(response.Headers.Get("Content-Disposition"), out string newName))
                                {
                                        imgType = Path.GetExtension(newName);
                                }
                        }
                        if (string.IsNullOrEmpty(imgType) && !string.IsNullOrEmpty(response.ContentType))
                        {
                                imgType = Tools.GetImgExtension(response.ContentType);
                        }
                        if (string.IsNullOrEmpty(imgType))
                        {
                                imgType = Path.GetExtension(uri.LocalPath);
                                if (string.IsNullOrEmpty(imgType) && Path.HasExtension(savePath))
                                {
                                        imgType = Path.GetExtension(savePath);
                                }
                        }
                        if (string.IsNullOrEmpty(imgType) && reqSet.ImageExten != null)
                        {
                                imgType = reqSet.ImageExten.Invoke(uri, response);
                        }
                        if (string.IsNullOrEmpty(imgType))
                        {
                                imgType = _DefImgFormat;
                        }
                        return imgType;
                }
                private static void _InitImgSavePath(Uri uri, HttpWebResponse response, ref string savePath, HttpRequestSet reqSet)
                {
                        string imgType = _GetImgType(uri, response, savePath, reqSet);
                        if (reqSet.FileSave != null)
                        {
                                string path = reqSet.FileSave.Invoke(uri, savePath, imgType);
                                savePath = string.IsNullOrEmpty(path)
                                        ? Path.Combine(Path.GetDirectoryName(savePath), string.Format("{0}{1}", Guid.NewGuid().ToString("N"), imgType))
                                        : path;
                        }
                        else
                        {
                                savePath = Path.HasExtension(savePath)
                                        ? !reqSet.IsResetFileName
                                                ? Path.ChangeExtension(savePath, imgType)
                                                : Path.Combine(Path.GetDirectoryName(savePath), string.Format("{0}{1}", Guid.NewGuid().ToString("N"), imgType))
                                        : Path.Combine(savePath, string.Format("{0}{1}", Guid.NewGuid().ToString("N"), imgType));
                        }
                }
                public static bool DownImageFile(Uri uri, ref string savePath, out HttpStatusCode status, HttpRequestSet reqSet)
                {
                        HttpWebRequest _Request = null;
                        try
                        {
                                _Request = _CreateRequest(uri, reqSet, "GET");
                                using (HttpWebResponse response = (HttpWebResponse)_Request.GetResponse())
                                {
                                        status = response.StatusCode;
                                        _InitImgSavePath(uri, response, ref savePath, reqSet);
                                        using (Stream stream = response.GetResponseStream())
                                        {
                                                SaveFileStream(stream, savePath, response.ContentLength, 200);
                                                response.Close();
                                                response.Dispose();
                                                return true;
                                        }
                                }
                        }
                        catch (WebException e)
                        {
                                status = e.Response is HttpWebResponse response ? response.StatusCode : HttpStatusCode.InternalServerError;
                                return false;
                        }
                        catch (Exception e)
                        {
                                _AddErrorLog(uri, e);
                                status = HttpStatusCode.InternalServerError;
                                return false;
                        }
                        finally
                        {
                                if (_Request != null)
                                {
                                        _Request.Abort();
                                }
                        }
                }


                public static bool SendRequest(Uri uri, string post, out HttpResponseRes res, HttpRequestSet reqSet)
                {
                        HttpWebRequest _Request = null;
                        try
                        {
                                _Request = _GetRequest(uri, post, reqSet);
                                using (HttpWebResponse response = (HttpWebResponse)_Request.GetResponse())
                                {
                                        byte[] bytes = null;
                                        HttpStatusCode status = HttpStatusCode.OK;
                                        KeValue[] cookies = null;
                                        using (Stream stream = response.GetResponseStream())
                                        {
                                                status = response.StatusCode;
                                                bytes = ReadStream(stream, response.ContentLength);

                                                if (response.Cookies.Count > 0)
                                                {
                                                        cookies = new KeValue[response.Cookies.Count];
                                                        int i = 0;
                                                        foreach (Cookie cookie in response.Cookies)
                                                        {
                                                                cookies[i++] = new KeValue
                                                                {
                                                                        Name = cookie.Name,
                                                                        Value = cookie.Value
                                                                };
                                                        }
                                                }
                                                response.Close();
                                                response.Dispose();
                                        }
                                        res = new HttpResponseRes
                                        {
                                                StatusCode = status,
                                                Content = reqSet.HtmlEncoding.GetString(bytes).Replace("\0", string.Empty),
                                                Cookies = cookies
                                        };
                                }
                                return true;
                        }
                        catch (WebException e)
                        {
                                res = e.Response is HttpWebResponse response
                                        ? new HttpResponseRes
                                        {
                                                StatusCode = response.StatusCode,
                                                Error = e
                                        }
                                        : new HttpResponseRes
                                        {
                                                StatusCode = HttpStatusCode.InternalServerError,
                                                Error = e
                                        };
                                return false;
                        }
                        catch (Exception e)
                        {
                                res = new HttpResponseRes
                                {
                                        StatusCode = HttpStatusCode.InternalServerError,
                                        Error = e
                                };
                                return false;
                        }
                        finally
                        {
                                if (_Request != null)
                                {
                                        _Request.Abort();
                                }
                        }
                }

        }
}
