using RpcStore.RemoteModel.ServerPublic.Model;
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
    /// 发布方案
    /// </summary>
    internal class PublicSchemeApi : ApiController
    {
        private readonly IPublicSchemeService _Service;

        public PublicSchemeApi (IPublicSchemeService service)
        {
            this._Service = service;
        }
        /// <summary>
        /// 新增方案
        /// </summary>
        /// <param name="add"></param>
        /// <returns></returns>
        [ApiPrower("rpc.store.admin")]
        public long Add (PublicSchemeAdd add)
        {
            return this._Service.Add(add);
        }
        /// <summary>
        /// 删除方案
        /// </summary>
        /// <param name="id"></param>
        [ApiPrower("rpc.store.admin")]
        public void Delete ([NumValidate("rpc.store.public.scheme.id.error", 1)] long id)
        {
            this._Service.Delete(id);
        }
        /// <summary>
        /// 禁用方案
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        [ApiPrower("rpc.store.admin")]
        public bool SetIsEnable (SetIsEnable<long> set)
        {
            return this._Service.SetIsEnable(set.Id, set.IsEnable);
        }

        /// <summary>
        /// 获取方案详细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PublicScheme Get ([NumValidate("rpc.store.public.scheme.id.error", 1)] long id)
        {
            return this._Service.Get(id);
        }
        /// <summary>
        /// 查询方案列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public PagingResult<ServerPublicScheme> Query (PagingParam<PublicSchemeQuery> query)
        {
            return this._Service.Query(query.Query, query.ToBasicPaging());
        }
        /// <summary>
        /// 设置方案信息
        /// </summary>
        /// <param name="set"></param>
        [ApiPrower("rpc.store.admin")]
        public void Set (LongParam<PublicScheme> set)
        {
            this._Service.Set(set.Id, set.Value);
        }
    }
}
