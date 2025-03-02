using RpcStore.RemoteModel.ResourceModular.Model;
using Store.Gatewary.Modular.Interface;
using Store.Gatewary.Modular.Model.ResourceModular;
using WeDonekRpc.ApiGateway.Attr;
using WeDonekRpc.Client;
using WeDonekRpc.Helper.Validate;
using WeDonekRpc.HttpApiGateway;

namespace Store.Gatewary.Modular.Api
{
    /// <summary>
    /// 资源模块
    /// </summary>
    internal class ResourceModularApi : ApiController
    {
        private readonly IResourceModularService _Service;
        public ResourceModularApi (IResourceModularService service)
        {
            this._Service = service;
        }
        /// <summary>
        /// 获取资源模块
        /// </summary>
        /// <param name="param">参数</param>
        /// <returns>资源模块</returns>
        public BasicModular[] GetBasicModular ([NullValidate("rpc.store.resourcemodular.param.null")] UI_GetBasicModular param)
        {
            return this._Service.GetBasicModular(param.RpcMerId, param.SystemType);
        }

        /// <summary>
        /// 查询资源模块
        /// </summary>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public PagingResult<ResourceModularDatum> QueryModular ([NullValidate("rpc.store.resourcemodular.param.null")] UI_QueryModular param)
        {
            ResourceModularDatum[] results = this._Service.QueryModular(param.Query, param, out int count);
            return new PagingResult<ResourceModularDatum>(count, results);
        }

        /// <summary>
        /// 设置资源模块备注信息
        /// </summary>
        /// <param name="param">参数</param>
        [ApiPrower("rpc.store.admin")]
        public void SetModularRemark ([NullValidate("rpc.store.resourcemodular.param.null")] UI_SetModularRemark param)
        {
            this._Service.SetModularRemark(param.Id, param.Remark);
        }

    }
}
