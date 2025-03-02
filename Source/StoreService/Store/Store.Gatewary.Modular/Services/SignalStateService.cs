using RpcStore.RemoteModel.SignalState;
using RpcStore.RemoteModel.SignalState.Model;
using Store.Gatewary.Modular.Interface;

namespace Store.Gatewary.Modular.Services
{
    internal class SignalStateService : ISignalStateService
    {
        public ServerSignalState[] GetSignalState(long serverId)
        {
            return new GetSignalState
            {
                ServerId = serverId,
            }.Send();
        }

    }
}
