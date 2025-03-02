using RpcStore.RemoteModel.Mer.Model;
using Store.Gatewary.Modular.Interface;
using Store.Gatewary.Modular.Model.Mer;
using WeDonekRpc.ApiGateway.Attr;
using WeDonekRpc.Client;
using WeDonekRpc.Helper.Validate;
using WeDonekRpc.HttpApiGateway;

namespace Store.Gatewary.Modular.Api
{
    /// <summary>
    /// 服务集群管理
    /// </summary>
    internal class MerApi : ApiController
    {
        private readonly IMerService _Service;
        public MerApi (IMerService service)
        {
            this._Service = service;
        }
        /// <summary>
        /// 添加服务集群
        /// </summary>
        /// <param name="datum">集群资料</param>
        /// <returns></returns>
        [ApiPrower("rpc.store.admin")]
        public long Add ([NullValidate("rpc.store.mer.datum.null")] RpcMerAdd datum)
        {
            return this._Service.AddMer(datum);
        }
        /// <summary>
        /// 获取集群列表
        /// </summary>
        /// <returns></returns>
        public BasicRpcMer[] GetItems ()
        {
            return this._Service.GetMerItems();
        }

        /// <summary>
        /// 删除服务集群
        /// </summary>
        /// <param name="rpcMerId">集群ID</param>
        [ApiPrower("rpc.store.admin")]
        public void Delete ([NumValidate("rpc.store.mer.rpcMerId.error", 1)] long rpcMerId)
        {
            this._Service.DeleteMer(rpcMerId);
        }

        /// <summary>
        /// 获取服务集群资料
        /// </summary>
        /// <param name="rpcMerId">服务集群ID</param>
        /// <returns></returns>
        public RpcMerDatum Get ([NumValidate("rpc.store.mer.rpcMerId.error", 1)] long rpcMerId)
        {
            return this._Service.GetMer(rpcMerId);
        }

        /// <summary>
        /// 查询服务集群
        /// </summary>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public PagingResult<RpcMer> Query ([NullValidate("rpc.store.mer.param.null")] UI_QueryMer param)
        {
            RpcMer[] results = this._Service.QueryMer(param.Name, param, out int count);
            return new PagingResult<RpcMer>(count, results);
        }

        /// <summary>
        /// 修改服务集群资料
        /// </summary>
        /// <param name="param">参数</param>
        [ApiPrower("rpc.store.admin")]
        public void Set ([NullValidate("rpc.store.mer.param.null")] UI_SetMer param)
        {
            this._Service.SetMer(param.RpcMerId, param.Datum);
        }

    }
}
