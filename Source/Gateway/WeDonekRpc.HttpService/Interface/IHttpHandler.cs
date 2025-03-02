using System.Collections.Generic;

namespace WeDonekRpc.HttpService.Interface
{
    public interface IHttpHandler
    {
        /// <summary>
        /// 路由参数
        /// </summary>
        Dictionary<string, string> RouteArgs { get; }

        /// <summary>
        /// 路径参数
        /// </summary>
        Dictionary<string, string> PathArgs { get; }

        /// <summary>
        /// 请求
        /// </summary>
        IHttpRequest Request
        {
            get;
        }

        /// <summary>
        /// 响应
        /// </summary>
        IHttpResponse Response
        {
            get;
        }
    }
}
