using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace RpcHelper
{
        public delegate void AsyncHttpComplate(HttpDataRes res);

        public class AsyncHttpHelper
        {
                private HttpWebRequest _Request = null;

                private readonly AsyncHttpComplate _AsyncFun = null;


                private readonly object _Arg = null;

                private readonly int _TimeOut = 5000;
                private readonly int _ReadWriteTime = 5000;
                public AsyncHttpHelper(AsyncHttpComplate async, object arg, int timeOut = 5000, int rwtimeOut = 5000)
                {
                        this._AsyncFun = async;
                        this._Arg = arg;
                        this._TimeOut = timeOut;
                        this._ReadWriteTime = rwtimeOut;
                }
                static AsyncHttpHelper()
                {
                        System.Net.ServicePointManager.DefaultConnectionLimit = 2048;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                        ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(_CheckValidationResult);
                }
                internal static byte[] ReadFully(Stream SourceStream, long slen, int size = 1024)
                {
                        int len = (int)slen;
                        if (len == -1)
                        {
                                return _ReadFully(SourceStream);
                        }
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
                private static byte[] _ReadFully(Stream stream)
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
                private static bool _CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
                {
                        return true; //总是接受  
                }
                public void SubmitData(string uri, string data)
                {
                        if (string.IsNullOrEmpty(data))
                        {
                                this.SubmitGetData(uri);
                        }
                        else
                        {
                                this.SubmitPost(uri, data);
                        }
                }
                private void _InitRequest(string uri, string post, bool isJson)
                {
                        this._Request = (HttpWebRequest)HttpWebRequest.Create(uri);
                        if (uri.StartsWith("https", StringComparison.CurrentCultureIgnoreCase))
                        {
                                this._Request.ProtocolVersion = HttpVersion.Version11;
                        }
                        this._Request.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);
                        this._Request.Method = post == null ? "GET" : "POST";
                        this._Request.Timeout = this._TimeOut;
                        this._Request.ReadWriteTimeout = this._ReadWriteTime;
                        this._Request.ContentType = isJson ? "application/json" : "application/octet-stream";
                        byte[] myByte = Encoding.UTF8.GetBytes(post);
                        this._Request.ContentLength = myByte.Length;
                        using (Stream stream = this._Request.GetRequestStream())
                        {
                                stream.Write(myByte, 0, myByte.Length);
                        }
                }
                public void SubmitPost(string uri, string data)
                {
                        this._InitRequest(uri, data, false);
                        HttpDataRes res = new HttpDataRes(uri, data);
                        this._Request.BeginGetResponse(new AsyncCallback(this._AsyncCallback), res);
                }
                public void SubmitPostJson(string uri, string json)
                {
                        this._InitRequest(uri, json, true);
                        this._Request.BeginGetResponse(new AsyncCallback(this._AsyncCallback), new HttpDataRes(uri, json));
                }
                public void SubmitGetData(string uri)
                {
                        this._InitRequest(uri, null, false);
                        this._Request.BeginGetResponse(new AsyncCallback(this._AsyncCallback), new HttpDataRes(uri, null));
                }
                private void _Exec(HttpDataRes res)
                {
                        try
                        {
                                this._AsyncFun(res);
                        }
                        catch (Exception e)
                        {
                                new ErrorLog(e, "Http异步请求回调发生错误", "http")
                                {
                                        { "Source",this._AsyncFun.Method.DeclaringType.FullName},
                                        { "StatusCode",res.StatusCode},
                                        { "Uri",res.Uri},
                                        { "Arg",res.Arg}
                                }.Save();
                        }
                }
                private void _AsyncCallback(IAsyncResult ar)
                {
                        HttpDataRes res = (HttpDataRes)ar.AsyncState;
                        res.Arg = this._Arg;
                        if (!ar.IsCompleted)
                        {
                                res.SetErrorMsg("异步执行失败!");
                                this._Request.Abort();
                        }
                        else
                        {
                                try
                                {
                                        using (HttpWebResponse response = (HttpWebResponse)this._Request.EndGetResponse(ar))
                                        {
                                                res.StatusCode = response.StatusCode;
                                                using (Stream stream = response.GetResponseStream())
                                                {
                                                        res.Content = ReadFully(stream, response.ContentLength);
                                                }
                                                response.Close();
                                        }
                                }
                                catch (WebException e)
                                {
                                        HttpWebResponse response = (HttpWebResponse)e.Response;
                                        res.StatusCode = response.StatusCode;
                                }
                                catch (Exception e)
                                {
                                        res.SetErrorMsg(e.Message);
                                }
                                finally
                                {
                                        if (this._Request != null)
                                        {
                                                this._Request.Abort();
                                        }
                                }
                        }
                        this._Exec(res);
                }
        }
}
