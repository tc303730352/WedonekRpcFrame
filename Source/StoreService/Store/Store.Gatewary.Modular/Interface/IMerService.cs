using WeDonekRpc.Model;
using RpcStore.RemoteModel.Mer.Model;

namespace Store.Gatewary.Modular.Interface
{
    public interface IMerService
    {
        BasicRpcMer[] GetMerItems ();
        /// <summary>
        /// 添加服务集群
        /// </summary>
        /// <param name="datum">集群资料</param>
        /// <returns></returns>
        long AddMer (RpcMerAdd datum);

        /// <summary>
        /// 删除服务集群
        /// </summary>
        /// <param name="rpcMerId">集群ID</param>
        void DeleteMer (long rpcMerId);

        /// <summary>
        /// 获取服务集群资料
        /// </summary>
        /// <param name="rpcMerId">服务集群ID</param>
        /// <returns></returns>
        RpcMerDatum GetMer (long rpcMerId);

        /// <summary>
        /// 查询服务集群
        /// </summary>
        /// <param name="name">集群名</param>
        /// <param name="paging">分页参数</param>
        /// <param name="count">数据总数</param>
        /// <returns></returns>
        RpcMer[] QueryMer (string name, IBasicPage paging, out int count);

        /// <summary>
        /// 修改服务集群资料
        /// </summary>
        /// <param name="rpcMerId">集群ID</param>
        /// <param name="datum">集群资料</param>
        void SetMer (long rpcMerId, RpcMerSet datum);

    }
}
