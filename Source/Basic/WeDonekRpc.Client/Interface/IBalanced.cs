using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Interface
{
    /// <summary>
    /// 服务节点的负载均衡接口
    /// </summary>
    internal interface IBalanced
    {
        /// <summary>
        /// 负载方式
        /// </summary>
        BalancedType BalancedType { get; }
        /// <summary>
        /// 分配节点
        /// </summary>
        /// <param name="config">节点配置</param>
        /// <param name="server">分配的服务节点</param>
        /// <returns>是否成功分配了节点</returns>
        bool DistributeNode (IRemoteConfig config, out IRemote server);
        /// <summary>
        /// 获取所有节点
        /// </summary>
        /// <returns></returns>
        IRemoteCursor GetAllNode ();
    }
}