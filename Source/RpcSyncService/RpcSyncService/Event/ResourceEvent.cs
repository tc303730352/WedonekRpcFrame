
using RpcModel;

using RpcModularModel.Resource;

using RpcSyncService.Collect;
using RpcSyncService.Logic;

namespace RpcSyncService.Event
{
        /// <summary>
        /// 资源服务
        /// </summary>
        internal class ResourceEvent : RpcClient.Interface.IRpcApiService
        {
                /// <summary>
                /// 清理资源
                /// </summary>
                public void ClearResource()
                {
                        ResourceCollect.ClearResource();
                }
                /// <summary>
                /// 检查无效资源
                /// </summary>
                public void InvalidResource()
                {
                        ResourceCollect.InvalidResource();
                }
                /// <summary>
                /// 同步资源
                /// </summary>
                /// <param name="obj">参数</param>
                /// <param name="source">来源</param>
                public void SyncResource(SyncResource obj, MsgSource source)
                {
                        ResourceLogic.SyncResource(obj, source);
                }
        }
}
