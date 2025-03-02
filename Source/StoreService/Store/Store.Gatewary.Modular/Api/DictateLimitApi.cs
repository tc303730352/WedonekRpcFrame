using RpcStore.RemoteModel.DictateLimit.Model;
using Store.Gatewary.Modular.Interface;
using Store.Gatewary.Modular.Model.DictateLimit;
using WeDonekRpc.ApiGateway.Attr;
using WeDonekRpc.Client;
using WeDonekRpc.Helper.Validate;
using WeDonekRpc.HttpApiGateway;

namespace Store.Gatewary.Modular.Api
{
    /// <summary>
    /// 服务节点指令限流配置 
    /// </summary>
    internal class DictateLimitApi : ApiController
    {
        private readonly IDictateLimitService _Service;
        public DictateLimitApi (IDictateLimitService service)
        {
            this._Service = service;
        }
        /// <summary>
        /// 添加服务节点指令限流配置
        /// </summary>
        /// <param name="datum">指令限流配置</param>
        /// <returns></returns>
        [ApiPrower("rpc.store.admin")]
        public long Add ([NullValidate("rpc.store.dictatelimit.datum.null")] DictateLimitAdd datum)
        {
            return this._Service.AddDictateLimit(datum);
        }

        /// <summary>
        /// 删除指令限流配置
        /// </summary>
        /// <param name="id">数据ID</param>
        [ApiPrower("rpc.store.admin")]
        public void Delete ([NumValidate("rpc.store.dictatelimit.id.error", 1)] long id)
        {
            this._Service.DeleteDictateLimit(id);
        }

        /// <summary>
        /// 获取指令限流配置
        /// </summary>
        /// <param name="id">数据ID</param>
        /// <returns>指令限流配置</returns>
        public DictateLimit Get ([NumValidate("rpc.store.dictatelimit.id.error", 1)] long id)
        {
            return this._Service.GetDictateLimit(id);
        }

        /// <summary>
        /// 查询指令限流配置
        /// </summary>
        /// <param name="param">参数</param>
        /// <returns>指令限流配置</returns>
        public PagingResult<DictateLimit> Query ([NullValidate("rpc.store.dictatelimit.param.null")] UI_QueryDictateLimit param)
        {
            DictateLimit[] results = this._Service.QueryDictateLimit(param.Query, param, out int count);
            return new PagingResult<DictateLimit>(count, results);
        }

        /// <summary>
        /// 设置指令限流配置
        /// </summary>
        /// <param name="param">参数</param>
        [ApiPrower("rpc.store.admin")]
        public void Set ([NullValidate("rpc.store.dictatelimit.param.null")] UI_SetDictateLimit param)
        {
            this._Service.SetDictateLimit(param.Id, param.Datum);
        }

    }
}
