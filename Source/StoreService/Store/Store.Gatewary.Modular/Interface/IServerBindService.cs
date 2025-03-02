using RpcStore.RemoteModel.ContainerGroup.Model;
using RpcStore.RemoteModel.ServerBind.Model;
using WeDonekRpc.Model;

namespace Store.Gatewary.Modular.Interface
{
    public interface IServerBindService
    {
        ServerBindVer[] GetServerBindVer (long rpcMerId, bool? isHold);
        ContainerGroupItem[] GetContainerGroup (BindGetParam param);
        BindServerGroupType[] GetGroupAndType (BindGetParam param);
        long[] CheckIsBind (long rpcMerId, long[] serverId);

        /// <summary>
        /// 删除集群绑定的服务节点关系
        /// </summary>
        /// <param name="id">数据ID</param>
        void DeleteServerBind (long id);


        /// <summary>
        /// 查询集群绑定的服务节点
        /// </summary>
        /// <param name="rpcMerId">集群ID</param>
        /// <param name="query">查询参数</param>
        /// <param name="paging">分页参数</param>
        /// <param name="count">数据总数</param>
        /// <returns></returns>
        BindRemoteServer[] Query (long rpcMerId, BindQueryParam query, IBasicPage paging, out int count);

        /// <summary>
        /// 修改集群绑定的服务节点列表
        /// </summary>
        /// <param name="bind">绑定信息</param>
        void SetBindServer (BindServer bind);

        /// <summary>
        /// 设置绑定的服务节点负载均衡时的权重
        /// </summary>
        /// <param name="id">数据ID</param>
        /// <param name="weight">负载均衡权重</param>
        void SaveWeight (SaveWeight weight);
        BindServerItem[] GetItems (ServerBindQueryParam query);
    }
}
