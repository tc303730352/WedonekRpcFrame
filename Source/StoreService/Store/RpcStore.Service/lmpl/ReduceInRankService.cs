using WeDonekRpc.Client;
using RpcManageClient;
using RpcStore.Collect;
using RpcStore.Model.DB;
using RpcStore.RemoteModel.ReduceInRank.Model;
using RpcStore.Service.Interface;

namespace RpcStore.Service.lmpl
{
    internal class ReduceInRankService : IReduceInRankService
    {
        private IRpcServerCollect _RpcServer;
        private readonly IReduceInRankCollect _ReduceInRank;
        public ReduceInRankService(IReduceInRankCollect reduce, IRpcServerCollect rpcServer)
        {
            _RpcServer = rpcServer;
            _ReduceInRank = reduce;
        }

        public void Delete(long id)
        {
            ReduceInRankConfigModel config = _ReduceInRank.Get(id);
            _ReduceInRank.Delete(config);
            _RpcServer.RefreshReduce(config.RpcMerId, config.ServerId);
        }

        public ReduceInRankConfig Get(long rpcMerId, long serverId)
        {
            ReduceInRankConfigModel config = _ReduceInRank.Get(rpcMerId, serverId);
            if (config == null)
            {
                return null;
            }
            return config.ConvertMap<ReduceInRankConfigModel, ReduceInRankConfig>();
        }


        public void Sync(ReduceInRankAdd add)
        {
            if (_ReduceInRank.Sync(add))
            {
                _RpcServer.RefreshReduce(add.RpcMerId, add.ServerId);
            }
        }
    }
}
