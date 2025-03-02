using RpcStore.RemoteModel.Tran.Model;
using Store.Gatewary.Modular.Interface;
using Store.Gatewary.Modular.Model.Tran;
using WeDonekRpc.Client;
using WeDonekRpc.Helper.Validate;
using WeDonekRpc.HttpApiGateway;

namespace Store.Gatewary.Modular.Api
{
    /// <summary>
    /// 事务查询
    /// </summary>
    internal class TranApi : ApiController
    {
        private readonly ITranService _Service;
        public TranApi (ITranService service)
        {
            this._Service = service;
        }
        /// <summary>
        /// 获取事务详细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TransactionData Get ([NumValidate("rpc.store.tran.id.error", 1)] long id)
        {
            return this._Service.Get(id);
        }

        /// <summary>
        /// 获取事务日志树形列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TransactionTree[] GetTree ([NumValidate("rpc.store.tran.id.error", 1)] long id)
        {
            return this._Service.GetTree(id);
        }

        /// <summary>
        /// 查询事务日志
        /// </summary>
        /// <param name="param">参数</param>
        /// <returns>事务日志</returns>
        public PagingResult<TransactionLog> Query ([NullValidate("rpc.store.tran.param.null")] UI_QueryTran param)
        {
            return this._Service.Query(param.Query, param);
        }

    }
}
