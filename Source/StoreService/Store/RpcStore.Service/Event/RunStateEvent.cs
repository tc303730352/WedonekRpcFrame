using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using RpcStore.RemoteModel.RunState;
using RpcStore.RemoteModel.RunState.Model;
using RpcStore.Service.Interface;

namespace RpcStore.Service.Event
{
    internal class RunStateEvent: IRpcApiService
    {
        private IServerRunStateService _Service;

        public RunStateEvent(IServerRunStateService service)
        {
            _Service = service;
        }

        public ServerRunState GetRunState(GetRunState obj)
        {
            return _Service.Get(obj.ServerId);
        }

        public PagingResult<RunState> QueryRunState(QueryRunState query)
        {
            return _Service.Query(query.ToBasicPage());
        }
    }
}
