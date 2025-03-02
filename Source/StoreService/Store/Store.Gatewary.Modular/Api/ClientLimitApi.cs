using RpcStore.RemoteModel.ClientLimit.Model;
using Store.Gatewary.Modular.Interface;
using WeDonekRpc.ApiGateway.Attr;
using WeDonekRpc.Helper.Validate;
using WeDonekRpc.HttpApiGateway;

namespace Store.Gatewary.Modular.Api
{
    /// <summary>
    /// 服务节点限流配置
    /// </summary>
    internal class ClientLimitApi : ApiController
    {
        private readonly IClientLimitService _Service;
        public ClientLimitApi (IClientLimitService service)
        {
            this._Service = service;
        }
        /// <summary>
        /// 删除服务节点限流配置
        /// </summary>
        /// <param name="id">数据ID</param>
        [ApiPrower("rpc.store.admin")]
        public void Delete ([NumValidate("rpc.store.clientlimit.id.error", 1)] long id)
        {
            this._Service.DeleteClientLimit(id);
        }
        /// <summary>
        /// 获取所有客户端限制
        /// </summary>
        /// <param name="serverId"></param>
        /// <returns></returns>
        public ClientLimitModel[] GetAll ([NumValidate("rpc.store.server.id.error", 1)] long serverId)
        {
            return this._Service.GetAllClientLimit(serverId);
        }
        /// <summary>
        /// 获取服务节点限流配置
        /// </summary>
        /// <param name="rpcMerId">参数</param>
        /// <param name="serverId">节点ID</param>
        /// <returns>服务节点限流配置</returns>
        public ClientLimitData Get ([NullValidate("rpc.store.mer.id.null")] long rpcMerId, [NullValidate("rpc.store.server.id.null")] long serverId)
        {
            return this._Service.GetClientLimit(rpcMerId, serverId);
        }

        /// <summary>
        /// 添加或设置服务节点限流配置
        /// </summary>
        /// <param name="datum">限流配置</param>
        [ApiPrower("rpc.store.admin")]
        public void Sync ([NullValidate("rpc.store.clientlimit.datum.null")] ClientLimitDatum datum)
        {
            this._Service.SyncClientLimit(datum);
        }

    }
}
