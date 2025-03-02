using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Interface
{
    /// <summary>
    /// 远程服务节点
    /// </summary>
    internal interface IRemoteNode
    {
        /// <summary>
        /// 分配节点
        /// </summary>
        /// <typeparam name="T">消息体</typeparam>
        /// <param name="config">配置</param>
        /// <param name="model">消息体</param>
        /// <param name="server">分配的节点</param>
        /// <returns>是否分配了节点</returns>
        bool DistributeNode<T> (IRemoteConfig config, T model, out IRemote server);

        /// <summary>
        /// 分配节点
        /// </summary>
        /// <param name="config">配置</param>
        /// <param name="server">分配的节点</param>
        /// <returns>是否分配了节点</returns>
        bool DistributeNode (IRemoteConfig config, out IRemote server);

        /// <summary>
        /// 获取满足条件的所有节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="config"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        IRemoteCursor DistributeNode<T> (IRemoteConfig config, T model);
    }
}
