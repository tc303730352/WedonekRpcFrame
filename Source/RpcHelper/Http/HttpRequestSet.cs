using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace RpcHelper
{
        public enum AnalogType
        {
                无 = 0,
                PC = 1,
                手机 = 2,
                微信 = 3
        }
        public delegate string GetImgExtension(Uri uri, WebResponse response);
        public delegate string GetSaveFileName(Uri uri, string savePath, string extension);
        public class HttpRequestSet : System.ICloneable
        {
                public const string DefAccept = "*/*";
                public const int DefTimeout = 10000;
                public const int DefReadWriteTimeout = 10000;
                public static readonly Version DefProtocolVersion = HttpVersion.Version11;
                public const int DefContinueTimeout = 3000;
                public const SecurityProtocolType DefSecurityProtocolType = SecurityProtocolType.Tls;
                public static readonly Encoding DefEncoding = Encoding.UTF8;
                public const string DefPhoneUserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 10_3 like Mac OS X) AppleWebKit/602.1.50 (KHTML, like Gecko) CriOS/56.0.2924.75 Mobile/14E5239e Safari/602.1";
                public const string HtmlAccept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                public const string DefPcUserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36";
                public const string DefWxUserAgent = "Mozilla/5.0 (iphone x Build/MXB48T; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/53.0.2785.49 Mobile MQQBrowser/6.2 TBS/043632 Safari/537.36 MicroMessenger/6.6.1.1220(0x26060135) NetType/WIFI Language/zh_CN";
                public HttpRequestSet(string method, HttpReqType type, AnalogType analogType, Uri uri) : this(type, analogType, uri)
                {
                        this.SubmitMethod = method;
                }
                public HttpRequestSet(string method, HttpReqType type) : this(type)
                {
                        this.SubmitMethod = method;
                }
                public HttpRequestSet(HttpReqType type)
                {
                        this._InitContentType(type);
                }
                public HttpRequestSet(HttpReqType type, AnalogType analogType, Uri uri) : this(type)
                {
                        if (analogType == AnalogType.无)
                        {
                                return;
                        }
                        else if (analogType == AnalogType.PC)
                        {
                                this.Referer = uri.GetLeftPart(UriPartial.Authority);
                                this.UserAgent = DefPcUserAgent;
                        }
                        else if (analogType == AnalogType.手机)
                        {
                                this.Referer = uri.GetLeftPart(UriPartial.Authority);
                                this.UserAgent = DefPhoneUserAgent;
                        }
                        else if (analogType == AnalogType.微信)
                        {
                                this.Referer = uri.GetLeftPart(UriPartial.Authority);
                                this.UserAgent = DefWxUserAgent;
                        }
                }

                private void _InitContentType(HttpReqType type)
                {
                        if (type == HttpReqType.Json)
                        {
                                this.ContentType = "application/json";
                        }
                        else if (type == HttpReqType.basic)
                        {
                                this.ContentType = "application/x-www-form-urlencoded";
                        }
                        else if (type == HttpReqType.image)
                        {
                                this.ContentType = "image/*;";
                        }
                        else if (type == HttpReqType.File)
                        {
                                this.ContentType = "application/octet-stream";
                                this.IsResetFileName = false;
                        }
                        else if (type == HttpReqType.Html)
                        {
                                this.ContentType = "text/html";
                        }
                        else if (type == HttpReqType.XML)
                        {
                                this.ContentType = "text/xml";
                                this.Accept = HtmlAccept;
                        }
                }

                public object Clone()
                {
                        return this.MemberwiseClone();
                }

                public string Accept
                {
                        get;
                        set;
                } = DefAccept;
                public string SubmitMethod
                {
                        get;
                        set;
                }
                public SecurityProtocolType SecurityProtocolType
                {
                        get;
                        set;
                } = HttpRequestSet.DefSecurityProtocolType;
                public Encoding RequestEncoding
                {
                        get;
                        set;
                } = HttpRequestSet.DefEncoding;
                public Encoding HtmlEncoding
                {
                        get;
                        set;
                } = HttpRequestSet.DefEncoding;
                public bool IsResetFileName
                {
                        get;
                        set;
                }
                public Dictionary<string, string> Cookies
                {
                        get;
                        set;
                } = new Dictionary<string, string>();
                public Dictionary<string, string> HeadList
                {
                        get;
                        set;
                } = new Dictionary<string, string>();
                public int Timeout
                {
                        get;
                        set;
                } = HttpRequestSet.DefTimeout;
                /// <summary>
                /// 读写超时时间
                /// </summary>
                public int ReadWriteTimeout
                {
                        get;
                        set;
                } = HttpRequestSet.DefReadWriteTimeout;
                public string Referer
                {
                        get;
                        set;
                }
                public string UserAgent
                {
                        get;
                        set;
                }

                public string ContentType
                {
                        get;
                        set;
                }
                public GetImgExtension ImageExten
                {
                        get;
                        set;
                }
                public GetSaveFileName FileSave
                {
                        get;
                        set;
                }
                public int MinFileSize
                {
                        get;
                        set;
                }
                public CertInfo HttpsCert
                {
                        get;
                        set;
                }

                public int ContinueTimeout { get; set; } = HttpRequestSet.DefContinueTimeout;
                public Version ProtocolVersion { get; set; } = HttpRequestSet.DefProtocolVersion;
                public bool SendChunked { get; set; } = false;
        }
}
