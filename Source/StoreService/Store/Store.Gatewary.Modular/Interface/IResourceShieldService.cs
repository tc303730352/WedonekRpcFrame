using WeDonekRpc.Model;
using RpcStore.RemoteModel.ResourceShield.Model;

namespace Store.Gatewary.Modular.Interface
{
    public interface IResourceShieldService
    {
        /// <summary>
        /// 取消资源屏蔽
        /// </summary>
        /// <param name="resourceId">资源ID</param>
        void CancelResourceShieId (long resourceId);

        /// <summary>
        /// 取消屏蔽
        /// </summary>
        /// <param name="id">取消屏蔽</param>
        void CancelShieId (long id);


        /// <summary>
        /// 查询屏蔽的资源信息
        /// </summary>
        /// <param name="query">查询参数</param>
        /// <param name="paging">分页参数</param>
        /// <param name="count">数据总数</param>
        /// <returns></returns>
        ResourceShieldDatum[] Query (ResourceShieldQuery query, IBasicPage paging, out int count);

        void AddResourceShieId (ResourceShieldAdd datum);

        void AddShield (ShieldAddDatum datum);
    }
}
