
using RpcModel;

namespace RpcClient
{
        /// <summary>
        /// 服务节点事件
        /// </summary>
        public class RpcEvent : Interface.IRpcEvent
        {
                /// <summary>
                /// 服务关闭事件
                /// </summary>
                public virtual void ServerClose()
                {
                }

                /// <summary>
                /// 服务节点已启动
                /// </summary>
                public virtual void ServerStarted()
                {
                }
                /// <summary>
                /// 服务节点启动中
                /// </summary>
                public virtual void ServerStarting()
                {
                }
                /// <summary>
                /// 服务节点事件
                /// </summary>
                /// <param name="serverId"></param>
                public virtual void RefreshService(long serverId)
                {

                }
                /// <summary>
                /// 服务节点状态变更
                /// </summary>
                /// <param name="state"></param>
                public virtual void ServiceState(RpcServiceState state)
                {

                }
        }
}
