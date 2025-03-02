using RpcStore.RemoteModel.ContainerGroup.Model;
using Store.Gatewary.Modular.Interface;
using WeDonekRpc.ApiGateway.Attr;
using WeDonekRpc.Client;
using WeDonekRpc.Helper.Validate;
using WeDonekRpc.HttpApiGateway;
using WeDonekRpc.HttpApiGateway.Model;

namespace Store.Gatewary.Modular.Api
{
    /// <summary>
    /// 容器组
    /// </summary>
    internal class ContainerGroupApi : ApiController
    {
        private readonly IContainerGroupService _Service;
        public ContainerGroupApi (IContainerGroupService service)
        {
            this._Service = service;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [ApiPrower("rpc.store.admin")]
        public int Add (ContainerGroupAdd data)
        {
            return this._Service.Add(data);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        [ApiPrower("rpc.store.admin")]
        public void Delete ([NumValidate("rpc.store.container.group.id.error", 1)] long id)
        {
            this._Service.Delete(id);
        }

        /// <summary>
        /// 获取详细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ContainerGroupDatum Get ([NumValidate("rpc.store.container.group.id.error", 1)] long id)
        {
            return this._Service.Get(id);
        }

        /// <summary>
        /// 获取所有容器组
        /// </summary>
        /// <returns></returns>
        public ContainerGroupItem[] GetItems (int? regionId)
        {
            return this._Service.GetItems(regionId);
        }

        /// <summary>
        /// 查询容器组
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public PagingResult<ContainerGroup> Query (PagingParam<ContainerGroupQuery> param)
        {
            return this._Service.Query(param.Query, param);
        }
        /// <summary>
        /// 修改容器信息
        /// </summary>
        /// <param name="set"></param>
        [ApiPrower("rpc.store.admin")]
        public void Set (LongParam<ContainerGroupSet> set)
        {
            this._Service.Set(set.Id, set.Value);
        }
    }
}
