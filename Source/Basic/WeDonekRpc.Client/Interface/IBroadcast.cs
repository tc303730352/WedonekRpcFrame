using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Interface
{
    /// <summary>
    /// 消息广播接口工厂
    /// </summary>
    [Attr.IgnoreIoc]
    internal interface IBroadcast
    {
        bool BroadcastMsg<T>(IRemoteBroadcast config, T model, out string error);
        /// <summary>
        /// 广播消息
        /// </summary>
        /// <param name="config">广播配置</param>
        /// <param name="body">消息实体</param>
        void BroadcastMsg(IRemoteBroadcast config, DynamicModel body);
        /// <summary>
        /// 广播消息
        /// </summary>
        /// <typeparam name="T">消息体</typeparam>
        /// <param name="config">广播配置</param>
        /// <param name="model">消息实体</param>
        /// <param name="serverId">接收服务节点ID</param>
        void BroadcastMsg<T>(IRemoteBroadcast config, T model, long[] serverId);
        /// <summary>
        /// 广播消息
        /// </summary>
        /// <typeparam name="T">消息体</typeparam>
        /// <param name="config">广播配置</param>
        /// <param name="model">消息实体</param>
        void BroadcastMsg<T>(IRemoteBroadcast config, T model);
        /// <summary>
        /// 广播消息
        /// </summary>
        /// <typeparam name="T">消息体</typeparam>
        /// <param name="config">广播配置</param>
        /// <param name="model">消息实体</param>
        /// <param name="typeVal">接收的服务节点类型</param>
        void BroadcastMsg<T>(IRemoteBroadcast config, T model, string[] typeVal);
        /// <summary>
        /// 广播消息
        /// </summary>
        /// <typeparam name="T">消息体</typeparam>
        /// <param name="config">广播配置</param>
        /// <param name="model">消息实体</param>
        /// <param name="rpcMerId">接收所在集群ID</param>
        /// <param name="typeVal">接收的服务节点类型</param>
        void BroadcastMsg<T>(IRemoteBroadcast config, T model, long rpcMerId, string[] typeVal);
    }
}