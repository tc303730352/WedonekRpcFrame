using System;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace WeDonekRpc.Helper.Http
{
    public class HttpClientConfig
    {
        internal HttpClientConfig ()
        {
            this.CertificateValidation = new Func<HttpRequestMessage, X509Certificate2, X509Chain, SslPolicyErrors, bool>(_ValidationCert);
        }

        private bool _ValidationCert ( HttpRequestMessage message, X509Certificate2 certificate, X509Chain chain, SslPolicyErrors errors )
        {
            return true;
        }

        /// <summary>
        /// 获取或设置一个值，该值指示处理程序是否应跟随重定向响应。
        /// </summary>
        public bool AllowAutoRedirect { get; set; } = false;

        /// <summary>
        /// 获取或设置一个值，该值指示处理程序是否随请求发送授权标头。
        /// </summary>
        public bool PreAuthenticate { get; set; } = false;
        /// <summary>
        /// 获取或设置 HttpClientHandler 对象管理的 HttpClient 对象所用的 TLS/SSL 协议。
        /// </summary>
        public SslProtocols SslProtocols { get; set; } = SslProtocols.Tls12;
        /// <summary>
        /// 获取或设置处理程序用于自动解压缩 HTTP 内容响应的解压缩方法类型。
        /// </summary>
        public DecompressionMethods AutomaticDecompression { get; set; } = DecompressionMethods.None;
        /// <summary>
        ///  获取或设置一个值，该值指示是否根据证书颁发机构吊销列表检查证书。
        /// </summary>
        public bool CheckCertificateRevocationList { get; set; } = false;

        /// <summary>
        /// 获取或设置一个值，该值指示是否从证书存储自动挑选证书，或者是否允许调用方通过特定的客户端证书。
        /// </summary>
        public ClientCertificateOption ClientCertificateOptions { get; set; } = ClientCertificateOption.Automatic;

        /// <summary>
        /// 证书信息
        /// </summary>
        public X509Certificate2 Cert { get; set; }

        /// <summary>
        /// 获取或设置处理程序遵循的重定向的最大数目。
        /// </summary>
        public int MaxAutomaticRedirections { get; set; } = 3;
        /// <summary>
        /// 获取或设置使用 HttpClient 对象发出请求时允许的最大并发连接数（每个服务器终结点）。 
        /// 请注意，该限制针对每个服务器终结点
        /// 例如: 值为 256 表示允许 256 个到 http://www.adatum.com/ 的并发连接，以及另外 256 个到 http://www.adventure-works.com/ 的并发连接。
        /// </summary>
        public int MaxConnectionsPerServer { get; set; } = 5;
        /// <summary>
        /// 获取或设置处理程序使用的最大请求内容缓冲区大小 默认：5MB。
        /// </summary>
        public int MaxRequestContentBufferSize { get; set; } = 1024 * 1024 * 5;
        /// <summary>
        /// 获取或设置响应标头的最大长度，以千字节（1024 字节）为单位。
        /// 例如，如果该值为 64，那么允许的最大响应标头长度为 65536 字节。
        /// </summary>
        public int MaxResponseHeadersLength { get; set; } = 64;

        /// <summary>
        /// 获取或设置一个值，该值指示处理程序是否使用 CookieContainer 属性来存储服务器 Cookie，并在发送请求时使用这些 Cookie。
        /// </summary>
        public bool UseCookies { get; set; } = false;
        /// <summary>
        /// 获取或设置一个值，该值控制处理程序是否随请求一起发送默认凭据
        /// </summary>
        public bool UseDefaultCredentials { get; set; } = false;

        /// <summary>
        /// 代理对象
        /// </summary>
        public WebProxy Proxy { get; set; }

        /// <summary>
        /// 获取或设置用于验证服务器证书的回调方法。
        /// </summary>
        public Func<HttpRequestMessage, X509Certificate2, X509Chain, SslPolicyErrors, bool> CertificateValidation
        {
            get;
            set;
        }

        internal HttpClientHandler Create ()
        {
            HttpClientHandler handler = new HttpClientHandler
            {
                PreAuthenticate = this.PreAuthenticate,
                AllowAutoRedirect = this.AllowAutoRedirect,
                SslProtocols = this.SslProtocols,
                ServerCertificateCustomValidationCallback = this.CertificateValidation,
                AutomaticDecompression = this.AutomaticDecompression,
                CheckCertificateRevocationList = this.CheckCertificateRevocationList,
                ClientCertificateOptions = this.ClientCertificateOptions,
                MaxAutomaticRedirections = this.MaxAutomaticRedirections,
                MaxConnectionsPerServer = this.MaxConnectionsPerServer,
                MaxRequestContentBufferSize = this.MaxRequestContentBufferSize,
                MaxResponseHeadersLength = this.MaxResponseHeadersLength,
                UseCookies = this.UseCookies,
                UseDefaultCredentials = this.UseDefaultCredentials
            };
            if ( this.Proxy != null )
            {
                handler.Proxy = this.Proxy;
                handler.UseProxy = true;
            }
            else
            {
                handler.UseProxy = false;
            }
            if ( this.Cert != null )
            {
                handler.ClientCertificates.Add(this.Cert);
            }
            return handler;
        }
    }
}
