using System;

namespace WeDonekRpc.HttpApiGateway.Interface
{
    public interface IHttpConfig
    {
        /// <summary>
        /// 响应模板
        /// </summary>
        IApiResponseTemplate ApiTemplate { get; set; }

        /// <summary>
        /// 监听地址
        /// </summary>
        string Url { get; }

        /// <summary>
        /// 接口路由格式
        /// </summary>
        string ApiRouteFormat { get; }

        /// <summary>
        /// 最大请求长度
        /// </summary>
        public long MaxRequstLength { get; }

        /// <summary>
        /// 上传配置
        /// </summary>
        public IGatewayUpConfig UpConfig { get; }
        /// <summary>
        /// 客户端真实请求地址
        /// </summary>
        Uri RealRequestUri { get; }
        /// <summary>
        /// 是否启用JSON 序列化时 Long 转 String
        /// </summary>
        bool IsEnableLongToString { get; }
        bool CheckContentLen ( long len );

        void RefreshEvent ( Action<IHttpConfig, string> action );
    }
}