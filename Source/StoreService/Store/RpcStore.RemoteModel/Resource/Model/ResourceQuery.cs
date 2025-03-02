using WeDonekRpc.Helper.Validate;
namespace RpcStore.RemoteModel.Resource.Model
{
    /// <summary>
    /// 资源查询参数
    /// </summary>
    public class ResourceQuery
    {
        /// <summary>
        /// 模块Id
        /// </summary>
        [NumValidate("rpc.store.resource.modular.Id.error", 1)]
        public long ModularId
        {
            get;
            set;
        }
        /// <summary>
        /// 查询关键字
        /// </summary>
        public string QueryKey
        {
            get;
            set;
        }
        /// <summary>
        /// 资源状态
        /// </summary>
        public ResourceState? ResourceState
        {
            get;
            set;
        }
    }
}
