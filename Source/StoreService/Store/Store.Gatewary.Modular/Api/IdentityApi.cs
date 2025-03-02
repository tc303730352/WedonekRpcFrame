using RpcStore.RemoteModel.Identity.Model;
using Store.Gatewary.Modular.Interface;
using Store.Gatewary.Modular.Model.Identity;
using WeDonekRpc.ApiGateway.Attr;
using WeDonekRpc.Client;
using WeDonekRpc.Helper.Validate;
using WeDonekRpc.HttpApiGateway;

namespace Store.Gatewary.Modular.Api
{
    /// <summary>
    /// 身份标识管理
    /// </summary>
    internal class IdentityApi : ApiController
    {
        private readonly IIdentityService _Service;
        public IdentityApi (IIdentityService service)
        {
            this._Service = service;
        }
        /// <summary>
        /// 添加身份标识
        /// </summary>
        /// <param name="datum">身份标识资料</param>
        /// <returns></returns>
        [ApiPrower("rpc.store.admin")]
        public long AddApp ([NullValidate("rpc.store.identity.datum.null")] IdentityDatum datum)
        {
            return this._Service.AddIdentityApp(datum);
        }

        /// <summary>
        /// 删除身份标识
        /// </summary>
        /// <param name="id">标识ID</param>
        [ApiPrower("rpc.store.admin")]
        public void DeleteApp ([NumValidate("rpc.store.identity.id.null", 1)] long id)
        {
            this._Service.DeleteIdentityApp(id);
        }

        /// <summary>
        /// 获取身份标识
        /// </summary>
        /// <param name="id">数据ID</param>
        /// <returns></returns>
        public IdentityAppData GetApp ([NumValidate("rpc.store.identity.id.null", 1)] long id)
        {
            return this._Service.GetIdentityApp(id);
        }

        /// <summary>
        /// 查询身份标识
        /// </summary>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public PagingResult<IdentityApp> Query ([NullValidate("rpc.store.identity.param.null")] UI_QueryIdentity param)
        {
            IdentityApp[] results = this._Service.QueryIdentity(param.Query, param, out int count);
            return new PagingResult<IdentityApp>(count, results);
        }

        /// <summary>
        /// 修改身份标识
        /// </summary>
        /// <param name="param">参数</param>
        [ApiPrower("rpc.store.admin")]
        public void Set ([NullValidate("rpc.store.identity.param.null")] UI_SetIdentity param)
        {
            this._Service.SetIdentity(param.Id, param.Datum);
        }

        /// <summary>
        /// 设置身份标识启用状态
        /// </summary>
        /// <param name="param">参数</param>
        [ApiPrower("rpc.store.admin")]
        public void SetIsEnable ([NullValidate("rpc.store.identity.param.null")] UI_SetIdentityIsEnable param)
        {
            this._Service.SetIdentityIsEnable(param.Id, param.IsEnable);
        }

    }
}
