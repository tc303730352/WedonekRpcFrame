using RpcStore.RemoteModel.ReduceInRank.Model;
using Store.Gatewary.Modular.Interface;
using WeDonekRpc.ApiGateway.Attr;
using WeDonekRpc.Helper.Validate;
using WeDonekRpc.HttpApiGateway;

namespace Store.Gatewary.Modular.Api
{
    /// <summary>
    /// 服务节点降级配置
    /// </summary>
    internal class ReduceInRankApi : ApiController
    {
        private readonly IReduceInRankService _Service;
        public ReduceInRankApi (IReduceInRankService service)
        {
            this._Service = service;
        }
        /// <summary>
        /// 删除服务节点降级配置
        /// </summary>
        /// <param name="id">数据ID</param>
        [ApiPrower("rpc.store.admin")]
        public void Delete ([NumValidate("rpc.store.reduceinrank.id.error", 1)] long id)
        {
            this._Service.DeleteReduceInRank(id);
        }

        /// <summary>
        /// 获取集群下某个服务节点降级配置
        /// </summary>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public ReduceInRankConfig Get ([NullValidate("rpc.store.mer.id.null")] long rpcMerId, [NullValidate("rpc.store.server.id.null")] long serverId)
        {
            return this._Service.GetReduceInRank(rpcMerId, serverId);
        }

        /// <summary>
        /// 服务节点降级配置
        /// </summary>
        /// <param name="datum">降级配置</param>
        [ApiPrower("rpc.store.admin")]
        public void Sync ([NullValidate("rpc.store.reduceinrank.datum.null")] ReduceInRankAdd datum)
        {
            this._Service.SyncReduceInRank(datum);
        }

    }
}
