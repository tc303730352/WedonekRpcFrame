using WeDonekRpc.Model;
using RpcStore.RemoteModel.RunState;
using RpcStore.RemoteModel.RunState.Model;
using Store.Gatewary.Modular.Interface;

namespace Store.Gatewary.Modular.Services
{
    internal class RunStateService : IRunStateService
    {
        public ServerRunState GetRunState(long serverId)
        {
            return new GetRunState
            {
                ServerId = serverId,
            }.Send();
        }

        public RunState[] QueryRunState(IBasicPage paging, out int count)
        {
            return new QueryRunState
            {
                Index = paging.Index,
                Size = paging.Size,
                NextId = paging.NextId,
                SortName = paging.SortName,
                IsDesc = paging.IsDesc
            }.Send(out count);
        }

    }
}
