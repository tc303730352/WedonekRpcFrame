using RpcStore.RemoteModel.LimitConfig.Model;
using Store.Gatewary.Modular.Interface;
using WeDonekRpc.ApiGateway.Attr;
using WeDonekRpc.Helper.Validate;
using WeDonekRpc.HttpApiGateway;
using WeDonekRpc.Model.Model;

namespace Store.Gatewary.Modular.Api
{
    /// <summary>
    /// 服务节点限流配置
    /// </summary>
    internal class LimitConfigApi : ApiController
    {
        private readonly ILimitConfigService _Service;
        public LimitConfigApi (ILimitConfigService service)
        {
            this._Service = service;
        }
        /// <summary>
        /// 删除服务节点全局限流配置
        /// </summary>
        /// <param name="serverId">服务节点ID</param>
        [ApiPrower("rpc.store.admin")]
        public void Delete ([NumValidate("rpc.store.limitconfig.serverId.error", 1)] long serverId)
        {
            this._Service.DeleteLimitConfig(serverId);
        }

        /// <summary>
        /// 获取服务节点全局限流配置
        /// </summary>
        /// <param name="serverId">服务节点ID</param>
        /// <returns></returns>
        public ServerLimitConfig Get ([NumValidate("rpc.store.limitconfig.serverId.error", 1)] long serverId)
        {
            return this._Service.GetLimitConfig(serverId);
        }

        /// <summary>
        /// 同步服务节点全局限流配置(添加或修改)
        /// </summary>
        /// <param name="config">全局限流配置</param>
        [ApiPrower("rpc.store.admin")]
        public void Sync ([NullValidate("rpc.store.limitconfig.config.null")] LimitConfigDatum config)
        {
            this._Service.SyncLimitConfig(config);
        }

    }
}
