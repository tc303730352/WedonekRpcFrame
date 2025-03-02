using RpcStore.RemoteModel.MerConfig.Model;
using Store.Gatewary.Modular.Interface;
using WeDonekRpc.ApiGateway.Attr;
using WeDonekRpc.Helper.Validate;
using WeDonekRpc.HttpApiGateway;

namespace Store.Gatewary.Modular.Api
{
    /// <summary>
    /// 服务集群配置管理
    /// </summary>
    internal class MerConfigApi : ApiController
    {
        private readonly IMerConfigService _Service;
        public MerConfigApi (IMerConfigService service)
        {
            this._Service = service;
        }
        /// <summary>
        /// 添加集群系统类别配置
        /// </summary>
        /// <param name="config">集群系统类别配置</param>
        /// <returns></returns>
        [ApiPrower("rpc.store.admin")]
        public long Set ([NullValidate("rpc.store.merconfig.config.null")] MerConfigArg config)
        {
            return this._Service.SetMerConfig(config);
        }

        /// <summary>
        /// 删除集群配置
        /// </summary>
        /// <param name="id">数据ID</param>
        [ApiPrower("rpc.store.admin")]
        public void Delete ([NumValidate("rpc.store.merconfig.id.null", 1)] long id)
        {
            this._Service.DeleteMerConfig(id);
        }

        /// <summary>
        /// 获取单个集群配置
        /// </summary>
        /// <param name="id">数据ID</param>
        /// <returns></returns>
        public RpcMerConfig Get ([NumValidate("rpc.store.mer.id.null", 1)] long rpcMerId, [NumValidate("rpc.store.server.type.null", 1)] long systemTypeId)
        {
            return this._Service.GetMerConfig(rpcMerId, systemTypeId);
        }

        /// <summary>
        /// 获取集群下的所有配置
        /// </summary>
        /// <param name="rpcMerId">集群ID</param>
        /// <returns>服务集群配置</returns>
        public RpcMerConfigDatum[] GetByMerId ([NumValidate("rpc.store.merconfig.rpcMerId.error", 1)] long rpcMerId)
        {
            return this._Service.GetMerConfigByMerId(rpcMerId);
        }

    }
}
