using RpcHelper.Validate;

using Wedonek.RpcStore.Gateway.Interface;
using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Gateway.Controller
{
        /// <summary>
        /// 客户端节点限流配置
        /// </summary>
        internal class ClientLimitController : HttpApiGateway.ApiController
        {
                private readonly IClientLimitService _Service = null;
                public ClientLimitController(IClientLimitService service)
                {
                        this._Service = service;
                }
                /// <summary>
                /// 删除配置
                /// </summary>
                /// <param name="rpcMerId">集群Id</param>
                /// <param name="serverId">服务节点Id</param>
                public void Drop([NumValidate("rpc.mer.id.null", 1)] long rpcMerId, [NumValidate("rpc.server.id.null", 1)] long serverId)
                {
                        this._Service.Drop(rpcMerId, serverId);
                }
                /// <summary>
                /// 获取配置
                /// </summary>
                /// <param name="rpcMerId">集群Id</param>
                /// <param name="serverId">服务节点Id</param>
                /// <returns></returns>
                public ServerClientLimitData Get([NumValidate("rpc.mer.id.null", 1)] long rpcMerId, [NumValidate("rpc.server.id.null", 1)] long serverId)
                {
                        return this._Service.Get(rpcMerId, serverId);
                }
                /// <summary>
                /// 添加或设置
                /// </summary>
                /// <param name="add"></param>
                public void Sync(ServerClientLimitData add)
                {
                        this._Service.Sync(add);
                }
        }
}
