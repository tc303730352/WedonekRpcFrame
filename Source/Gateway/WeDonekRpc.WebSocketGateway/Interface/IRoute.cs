namespace WeDonekRpc.WebSocketGateway.Interface
{
    /// <summary>
    /// 路由
    /// </summary>
    public interface IRoute
    {
        /// <summary>
        /// 地址
        /// </summary>
        string LocalPath { get; }
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