using WeDonekRpc.ApiGateway;

namespace WeDonekRpc.HttpApiGateway.Interface
{
    /// <summary>
    /// 路由
    /// </summary>
    public interface IRoute
    {
        string Id { get; }
        /// <summary>
        /// Api类型
        /// </summary>
        ApiType ApiType { get; }
        /// <summary>
        /// 地址
        /// </summary>
        string ApiUri { get; }
        /// <summary>
        /// 是否需要授权
        /// </summary>
        bool IsAccredit { get; }
        /// <summary>
        /// 权限值
        /// </summary>
        string[] Prower { get; }
        /// <summary>
        /// 服务名称
        /// </summary>
        string ServiceName { get; }

    }
}