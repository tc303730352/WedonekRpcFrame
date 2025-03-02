using RpcStore.RemoteModel.TransmitScheme.Model;
using Store.Gatewary.Modular.Interface;
using Store.Gatewary.Modular.Model;
using WeDonekRpc.ApiGateway.Attr;
using WeDonekRpc.Client;
using WeDonekRpc.Helper.Validate;
using WeDonekRpc.HttpApiGateway;
using WeDonekRpc.HttpApiGateway.Model;

namespace Store.Gatewary.Modular.Api
{
    /// <summary>
    /// 负载均衡方案管理
    /// </summary>
    internal class TransmitSchemeApi : ApiController
    {
        private readonly ITransmitSchemeService _Service;

        public TransmitSchemeApi (ITransmitSchemeService service)
        {
            this._Service = service;
        }
        /// <summary>
        /// 新增方案
        /// </summary>
        /// <param name="datum"></param>
        /// <returns></returns>
        [ApiPrower("rpc.store.admin")]
        public long Add (TransmitSchemeAdd datum)
        {
            return this._Service.Add(datum);
        }
        /// <summary>
        /// 删除方案
        /// </summary>
        /// <param name="id"></param>
        [ApiPrower("rpc.store.admin")]
        public void Delete ([NumValidate("rpc.store.transmit.scheme.id.error", 1)] long id)
        {
            this._Service.Delete(id);
        }
        public TransmitDatum[] Generate (TransmitGenerate param)
        {
            return this._Service.Generate(param);
        }
        /// <summary>
        /// 获取方案信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TransmitSchemeData Get ([NumValidate("rpc.store.transmit.scheme.id.error", 1)] long id)
        {
            return this._Service.Get(id);
        }
        /// <summary>
        /// 获取负载方案详细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TransmitSchemeDetailed GetDetailed ([NumValidate("rpc.store.transmit.scheme.id.error", 1)] long id)
        {
            return this._Service.GetDetailed(id);
        }
        /// <summary>
        /// 查询方案列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public PagingResult<TransmitScheme> Query (PagingParam<TransmitSchemeQuery> query)
        {
            return this._Service.Query(query.Query, query);
        }
        /// <summary>
        /// 设置负载项
        /// </summary>
        /// <param name="set"></param>
        [ApiPrower("rpc.store.admin")]
        public void SetItem (LongParam<TransmitDatum[]> set)
        {
            this._Service.SetItem(set.Id, set.Value);
        }
        /// <summary>
        /// 修改方案信息
        /// </summary>
        /// <param name="set"></param>
        [ApiPrower("rpc.store.admin")]
        public void Set (LongParam<TransmitSchemeSet> set)
        {
            this._Service.Set(set.Id, set.Value);
        }
        /// <summary>
        /// 设置方案启用状态
        /// </summary>
        /// <param name="set"></param>
        [ApiPrower("rpc.store.admin")]
        public void SetIsEnable (SetIsEnable<long> set)
        {
            this._Service.SetIsEnable(set.Id, set.IsEnable);
        }
    }
}
