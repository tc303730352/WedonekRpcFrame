using WeDonekRpc.Model;
using RpcStore.RemoteModel.RunState.Model;

namespace Store.Gatewary.Modular.Interface
{
    public interface IRunStateService
    {
        /// <summary>
        /// 获取服务节点运行状态
        /// </summary>
        /// <param name="serverId">服务节点ID</param>
        /// <returns>运行状态</returns>
        ServerRunState GetRunState(long serverId);

        /// <summary>
        /// 查询服务节点运行状态
        /// </summary>
        /// <param name="paging">分页参数</param>
        /// <param name="count">数据总数</param>
        /// <returns>运行状态</returns>
        RunState[] QueryRunState(IBasicPage paging, out int count);

    }
}
