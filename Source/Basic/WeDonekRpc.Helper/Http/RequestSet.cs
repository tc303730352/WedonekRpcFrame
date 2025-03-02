using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace WeDonekRpc.Helper.Http
{
    public class RequestSet
    {
        public static readonly string DefAccept = "*/*";
        public static readonly string DefUAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/132.0.0.0 Safari/537.36 Edg/132.0.0.0";
        public static readonly TimeSpan DefTimeout = new TimeSpan(0, 0, 10);

        public static readonly Encoding DefEncoding = Encoding.UTF8;

        public string Accept
        {
            get;
            set;
        } = DefAccept;

        /// <summary>
        /// 请求编码
        /// </summary>
        public Encoding RequestEncoding
        {
            get;
            set;
        } = DefEncoding;

        /// <summary>
        /// 响应编码
        /// </summary>
        public Encoding ResponseEncoding
        {
            get;
            set;
        } = DefEncoding;

        /// <summary>
        /// 请求Cookies
        /// </summary>
        public Dictionary<string, string> Cookies
        {
            get;
            set;
        }
        /// <summary>
        /// 请求头集合
        /// </summary>
        public Dictionary<string, string> HeadList
        {
            get;
            set;
        }
        /// <summary>
        /// 超时时间
        /// </summary>
        public TimeSpan Timeout
        {
            get;
            set;
        } = DefTimeout;

        /// <summary>
        /// 用户代理头
        /// </summary>
        public string UserAgent
        {
            get;
            set;
        } = DefUAgent;
        public Uri Referer
        {
            get;
            set;
        }
        /// <summary>
        /// 每次读取的字节数
        /// </summary>
        public int ReadBufferSize
        {
            get;
            set;
        } = 1024*1024*10;

        /// <summary>
        /// 写入的缓冲字节数 0 取默认
        /// </summary>
        public int WriteBufferSize { get; set; } = 0;

        internal void Init ( HttpClient client, HttpContent content, Uri uri )
        {
            client.DefaultRequestHeaders.Host = uri.Host;
            client.Timeout = this.Timeout;
            client.DefaultRequestHeaders.Referrer = this.Referer;
            if ( this.UserAgent != null )
            {
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(this.UserAgent));
            }
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(this.Accept));
            content.Headers.ContentEncoding.Add(this.RequestEncoding.EncodingName);
            if ( this.HeadList != null )
            {
                this.HeadList.ForEach(( n, c ) =>
                {
                    content.Headers.Add(n, c);
                });
            }
            if ( this.Cookies != null )
            {
                StringBuilder cookie = new StringBuilder();
                this.Cookies.ForEach(( n, c ) =>
                {
                    cookie.Append(n);
                    cookie.Append('=');
                    cookie.Append(c);
                    cookie.Append(';');
                });
                content.Headers.Add("Cookie", cookie.ToString());
            }
        }
    }
}
