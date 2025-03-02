using WeDonekRpc.Client.Interface;
using RpcStore.RemoteModel.SignalState;
using RpcStore.RemoteModel.SignalState.Model;
using RpcStore.Service.Interface;

namespace RpcStore.Service.Event
{
    internal class SignalStateEvent : IRpcApiService
    {
        private ISignalStateService _Service;
        public SignalStateEvent(ISignalStateService service)
        {
            _Service = service;
        }

        public ServerSignalState[] GetSignalState(GetSignalState obj)
        {
            return _Service.Gets(obj.ServerId);
        }
    }
}
