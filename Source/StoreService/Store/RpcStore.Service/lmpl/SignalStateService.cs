using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using RpcStore.Collect;
using RpcStore.Model.DB;
using RpcStore.Model.ServerConfig;
using RpcStore.RemoteModel.SignalState.Model;
using RpcStore.Service.Interface;

namespace RpcStore.Service.lmpl
{
    internal class SignalStateService : ISignalStateService
    {
        private readonly IServerSignalStateCollect _SignalState;
        private readonly IServerCollect _Server;
        public SignalStateService(IServerSignalStateCollect signalState,
            IServerCollect server)
        {
            _Server = server;
            _SignalState = signalState;
        }
        public ServerSignalState[] Gets(long serverId)
        {
            ServerSignalStateModel[] states = _SignalState.Gets(serverId);
            if (states.IsNull())
            {
                return null;
            }
            ServerConfigDatum[] servers = _Server.Gets(states.ConvertAll(a => a.RemoteId));
            return states.ConvertMap<ServerSignalStateModel, ServerSignalState>((a, b) =>
            {
                ServerConfigDatum server = servers.Find(c => c.Id == a.RemoteId);
                b.IsContainer = server.IsContainer;
                b.IsOnline = server.IsOnline;
                b.ServiceState = server.ServiceState;
                b.ServerName = server.ServerName;
                b.ServerId = server.Id;
                return b;
            });
        }
    }
}
