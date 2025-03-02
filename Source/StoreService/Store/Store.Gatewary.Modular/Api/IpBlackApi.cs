using RpcStore.RemoteModel.IpBlack.Model;
using Store.Gatewary.Modular.Interface;
using Store.Gatewary.Modular.Model.IpBlack;
using WeDonekRpc.ApiGateway.Attr;
using WeDonekRpc.Client;
using WeDonekRpc.Helper.Validate;
using WeDonekRpc.HttpApiGateway;

namespace Store.Gatewary.Modular.Api
{
    /// <summary>
    /// 网关IP黑名单管理
    /// </summary>
    internal class IpBlackApi : ApiController
    {
        private readonly IIpBlackService _Service;
        public IpBlackApi (IIpBlackService service)
        {
            this._Service = service;
        }
        /// <summary>
        /// 添加Ip黑名单
        /// </summary>
        /// <param name="datum">Ip黑名单</param>
        /// <returns></returns>
        [ApiPrower("rpc.store.admin")]
        public long AddIpBack ([NullValidate("rpc.store.ipblack.datum.null")] IpBlackAddData datum)
        {
            return this._Service.AddIpBack(datum);
        }

        /// <summary>
        /// 删除Ip黑名单
        /// </summary>
        /// <param name="id">黑名单ID</param>
        [ApiPrower("rpc.store.admin")]
        public void DropIpBack ([NumValidate("rpc.store.ipblack.id.error", 1)] long id)
        {
            this._Service.DropIpBack(id);
        }

        /// <summary>
        /// 获取Ip黑名单
        /// </summary>
        /// <param name="id">数据ID</param>
        /// <returns></returns>
        public IpBlackDatum GetIpBack ([NumValidate("rpc.store.ipblack.id.error", 1)] long id)
        {
            return this._Service.GetIpBack(id);
        }

        /// <summary>
        /// 查询Ip黑名单
        /// </summary>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public PagingResult<IpBlack> Query ([NullValidate("rpc.store.ipblack.param.null")] UI_QueryIpBlack param)
        {
            IpBlack[] results = this._Service.QueryIpBlack(param.Query, param, out int count);
            return new PagingResult<IpBlack>(count, results);
        }

        /// <summary>
        /// 修改Ip黑名单
        /// </summary>
        /// <param name="param">参数</param>
        [ApiPrower("rpc.store.admin")]
        public void Set ([NullValidate("rpc.store.ipblack.param.null")] UI_SetIpBlack param)
        {
            this._Service.SetIpBlack(param.Id, param.Datum);
        }

        /// <summary>
        /// 修改Ip黑名单备注
        /// </summary>
        /// <param name="param">参数</param>
        [ApiPrower("rpc.store.admin")]
        public void SetRemark ([NullValidate("rpc.store.ipblack.param.null")] UI_SetIpBlackRemark param)
        {
            this._Service.SetIpBlackRemark(param.Id, param.Remark);
        }

    }
}
