using RpcStore.RemoteModel.SignalState.Model;

namespace Store.Gatewary.Modular.Interface
{
    public interface ISignalStateService
    {
        /// <summary>
        /// 获取服务节点和各节点之间的通信状态
        /// </summary>
        /// <param name="serverId"></param>
        /// <returns></returns>
        ServerSignalState[] GetSignalState(long serverId);

    }
}
