
using RpcModel;

using RpcModularModel.Identity;
using RpcModularModel.Identity.Model;

using RpcSyncService.Logic;

namespace RpcSyncService.Event
{
        /// <summary>
        /// 身份标识
        /// </summary>
        internal class IdentityEvent : RpcClient.Interface.IRpcApiService
        {
                /// <summary>
                /// 获取身份标识
                /// </summary>
                /// <param name="obj">参数</param>
                /// <param name="source">事件原</param>
                /// <returns>身份标识</returns>
                public IdentityDatum GetIdentity(GetIdentity obj, MsgSource source)
                {
                        return IdentityLogic.GetIdentity(obj.IdentityId, source);
                }
        }
}
