using RpcModel;

namespace RpcClient.Interface
{
        /// <summary>
        /// 远程服务节点
        /// </summary>
        internal interface IRemoteServer
        {
                /// <summary>
                /// 分配节点
                /// </summary>
                /// <typeparam name="T">消息体</typeparam>
                /// <param name="config">配置</param>
                /// <param name="model">消息体</param>
                /// <param name="server">分配的节点</param>
                /// <returns>是否分配了节点</returns>
                bool DistributeNode<T>(IRemoteConfig config, T model, out IRemote server);

                /// <summary>
                /// 分配节点
                /// </summary>
                /// <param name="config">配置</param>
                /// <param name="server">分配的节点</param>
                /// <returns>是否分配了节点</returns>
                bool DistributeNode(IRemoteConfig config, out IRemote server);
        }
}
