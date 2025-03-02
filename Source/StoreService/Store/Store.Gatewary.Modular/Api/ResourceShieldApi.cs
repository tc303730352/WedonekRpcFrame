using RpcStore.RemoteModel.ResourceShield.Model;
using Store.Gatewary.Modular.Interface;
using Store.Gatewary.Modular.Model.ResourceShield;
using WeDonekRpc.ApiGateway.Attr;
using WeDonekRpc.Client;
using WeDonekRpc.Helper.Validate;
using WeDonekRpc.HttpApiGateway;

namespace Store.Gatewary.Modular.Api
{
    /// <summary>
    /// 网关资源屏蔽管理
    /// </summary>
    internal class ResourceShieldApi : ApiController
    {
        private readonly IResourceShieldService _Service;
        public ResourceShieldApi (IResourceShieldService service)
        {
            this._Service = service;
        }
        /// <summary>
        /// 取消资源屏蔽
        /// </summary>
        /// <param name="resourceId">资源ID</param>
        [ApiPrower("rpc.store.admin")]
        public void CancelResourceShieId ([NumValidate("rpc.store.resourceshield.resourceId.error", 1)] long resourceId)
        {
            this._Service.CancelResourceShieId(resourceId);
        }

        /// <summary>
        /// 取消屏蔽
        /// </summary>
        /// <param name="id">取消屏蔽</param>
        [ApiPrower("rpc.store.admin")]
        public void CancelShieId ([NumValidate("rpc.store.resourceshield.id.error", 1)] long id)
        {
            this._Service.CancelShieId(id);
        }


        /// <summary>
        /// 查询屏蔽的资源信息
        /// </summary>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public PagingResult<ResourceShieldDatum> Query ([NullValidate("rpc.store.resourceshield.param.null")] UI_QueryResourceShield param)
        {
            ResourceShieldDatum[] results = this._Service.Query(param.Query, param, out int count);
            return new PagingResult<ResourceShieldDatum>(count, results);
        }

        /// <summary>
        /// 屏蔽已有资源
        /// </summary>
        /// <param name="param">参数</param>
        /// <returns></returns>
        [ApiPrower("rpc.store.admin")]
        public void AddResourceShieId ([NullValidate("rpc.store.resourceshield.param.null")] ResourceShieldAdd param)
        {
            this._Service.AddResourceShieId(param);
        }

        /// <summary>
        /// 新增屏蔽(添加或修改)
        /// </summary>
        /// <param name="datum">需要屏蔽的资料</param>
        /// <returns></returns>
        [ApiPrower("rpc.store.admin")]
        public void AddShield ([NullValidate("rpc.store.resourceshield.datum.null")] ShieldAddDatum datum)
        {
            this._Service.AddShield(datum);
        }

    }
}
