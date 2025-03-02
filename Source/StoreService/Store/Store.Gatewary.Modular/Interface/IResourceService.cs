using WeDonekRpc.Model;
using RpcStore.RemoteModel.Resource.Model;

namespace Store.Gatewary.Modular.Interface
{
    public interface IResourceService
    {
        /// <summary>
        /// 查询资源信息
        /// </summary>
        /// <param name="query">查询参数</param>
        /// <param name="paging">分页参数</param>
        /// <param name="count">数据总数</param>
        /// <returns></returns>
        ResourceDatum[] QueryResource (ResourceQuery query, IBasicPage paging, out int count);

        ResourceDto Get (long id);

    }
}
