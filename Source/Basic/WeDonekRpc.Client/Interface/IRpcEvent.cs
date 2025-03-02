
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Interface
{
    /// <summary>
    /// Rpc 事件
    /// </summary>
    [Attr.IgnoreIoc]
    public interface IRpcEvent
    {
        /// <summary>
        /// 服务加载
        /// </summary>
        /// <param name="option"></param>
        void Load(RpcInitOption option);
        /// <summary>
        /// 服务初始化事件
        /// </summary>
        void ServerInit(IIocService ioc);
        /// <summary>
        /// 服务关闭事件
        /// </summary>
        void ServerClose();
        /// <summary>
        /// 服务已启动
        /// </summary>
        void ServerStarted();
        /// <summary>
        /// 服务启动
        /// </summary>
        void ServerStarting();
        /// <summary>
        /// 刷新服务节点事件
        /// </summary>
        /// <param name="serverId">节点Id</param>
        void RefreshService(long serverId);
        /// <summary>
        /// 当前服务节点状态变更
        /// </summary>
        /// <param name="state"></param>
        void ServiceState(RpcServiceState state);
    }
}