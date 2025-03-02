using WeDonekRpc.HttpApiGateway;
using WeDonekRpc.Client;
using WeDonekRpc.Helper.Validate;
using RpcStore.RemoteModel.Resource.Model;
using Store.Gatewary.Modular.Interface;
using Store.Gatewary.Modular.Model.Resource;

namespace Store.Gatewary.Modular.Api
{
    /// <summary>
    /// 服务资源
    /// </summary>
    internal class ResourceApi : ApiController
    {
        private readonly IResourceService _Service;
        public ResourceApi (IResourceService service)
        {
            this._Service = service;
        }
        /// <summary>
        /// 获取服务资源信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResourceDto Get ([NumValidate("rpc.store.resource.id.error", 1)] long id)
        {
            return this._Service.Get(id);
        }

        /// <summary>
        /// 查询资源信息
        /// </summary>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public PagingResult<ResourceDatum> Query ([NullValidate("rpc.store.resource.param.null")] UI_QueryResource param)
        {
            ResourceDatum[] results = this._Service.QueryResource(param.Query, param, out int count);
            return new PagingResult<ResourceDatum>(count, results);
        }

    }
}
