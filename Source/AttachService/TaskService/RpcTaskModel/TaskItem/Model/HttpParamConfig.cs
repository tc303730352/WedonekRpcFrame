using System;
using System.Net.Http;
using System.Net;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using WeDonekRpc.Helper.Validate;
namespace RpcTaskModel.TaskItem.Model
{
    /// <summary>
    /// Http配置参数
    /// </summary>
    public class HttpParamConfig
    {
        /// <summary>
        /// 请求的完整URI
        /// </summary>
        [NullValidate("task.item.http.uri.null")]
        public Uri Uri
        {
            get;
            set;
        }
        /// <summary>
        /// 请求方式
        /// </summary>
        [NullValidate("task.item.http.method.null")]
        public string RequestMethod
        {
            get;
            set;
        }
       
       
        /// <summary>
        /// Post参数
        /// </summary>
        public string PostParam
        {
            get;
            set;
        }
        /// <summary>
        /// 媒体类型
        /// </summary>
        public string ContentType
        {
            get;
            set;
        } = "application/json";

        /// <summary>
        /// 一旦响应标头操作应立即完成
        /// </summary>
        public bool ResponseHeadersRead
        {
            get;
            set;
        }
        /// <summary>
        /// Http配置
        /// </summary>
        public HttpConfig Config
        {
            get;
            set;
        }
    }
    public class HttpConfig
    {
        /// <summary>
        /// 获取或设置一个值，该值指示处理程序是否应跟随重定向响应。
        /// </summary>
        public bool AllowAutoRedirect { get; set; } = false;

        /// <summary>
        /// 获取或设置 HttpClientHandler 对象管理的 HttpClient 对象所用的 TLS/SSL 协议。
        /// </summary>
        public SslProtocols SslProtocols { get; set; } = SslProtocols.Tls12;

        /// <summary>
        /// 获取或设置处理程序用于自动解压缩 HTTP 内容响应的解压缩方法类型。
        /// </summary>
        public DecompressionMethods AutomaticDecompression { get; set; } = DecompressionMethods.None;

        /// <summary>
        /// 获取或设置处理程序遵循的重定向的最大数目。
        /// </summary>
        public int MaxAutomaticRedirections { get; set; } = 3;
    }
}
