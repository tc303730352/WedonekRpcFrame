using RpcModel.Model;

using RpcHelper.Validate;

using Wedonek.RpcStore.Gateway.Interface;
using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Gateway.Controller
{
        /// <summary>
        /// 服务节点限流配置
        /// </summary>
        internal class LimitConfigController : HttpApiGateway.ApiController
        {
                private readonly IServerLimitConfigService _Limit = null;

                public LimitConfigController(IServerLimitConfigService service)
                {
                        this._Limit = service;
                }
                /// <summary>
                /// 删除配置
                /// </summary>
                /// <param name="serverId">节点Id</param>
                public void Drop([NumValidate("rpc.server.id.error", 1)] long serverId)
                {
                        this._Limit.DropConfig(serverId);
                }

                /// <summary>
                /// 获取配置
                /// </summary>
                /// <param name="serverId">节点Id</param>
                /// <returns>节点配置</returns>
                public ServerLimitConfig Get([NumValidate("rpc.server.id.error", 1)] long serverId)
                {
                        return this._Limit.GetLimitConfig(serverId);
                }
                /// <summary>
                /// 同步配置
                /// </summary>
                /// <param name="add"></param>
                public void Sync(AddServerLimitConfig add)
                {
                        this._Limit.SyncConfig(add);
                }
        }
}
