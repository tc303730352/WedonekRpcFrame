using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using RpcStore.Collect;
using RpcStore.Model.DB;
using RpcStore.Model.ServerConfig;
using RpcStore.RemoteModel.RunState.Model;
using RpcStore.Service.Interface;

namespace RpcStore.Service.lmpl
{
    internal class ServerRunStateService : IServerRunStateService
    {
        private readonly IServerRunStateCollect _RunState;
        private readonly IServerCollect _Server;

        public ServerRunStateService(IServerRunStateCollect runState, IServerCollect server)
        {
            _Server = server;
            _RunState = runState;
        }
        public ServerRunState Get(long serverId)
        {
            ServerRunStateModel state = _RunState.Get(serverId);
            if (state == null)
            {
                return null;
            }
            return state.ConvertMap<ServerRunStateModel, ServerRunState>();
        }

        public PagingResult<RunState> Query(IBasicPage paging)
        {
            ServerRunStateModel[] list = _RunState.Query(paging, out int count);
            if (list.IsNull())
            {
                return new PagingResult<RunState>();
            }
            ServerConfigDatum[] services = _Server.Gets(list.ConvertAll(c => c.ServerId));
            RunState[] states = list.ConvertMap<ServerRunStateModel, RunState>((a, b) =>
            {
                ServerConfigDatum datum = services.Find(c => c.Id == a.ServerId);
                b.ServerName = datum.ServerName;
                b.IsContainer = datum.IsContainer;
                b.ServiceState = datum.ServiceState;
                b.IsOnline = datum.IsOnline;
                return b;
            });
            return new PagingResult<RunState>(count, states);
        }
    }
}
