using WeDonekRpc.Client.Interface;
using RpcStore.RemoteModel.ReduceInRank;
using RpcStore.RemoteModel.ReduceInRank.Model;
using RpcStore.Service.Interface;

namespace RpcStore.Service.Event
{
    /// <summary>
    /// 服务节点降级配置
    /// </summary>
    internal class ReduceInRankEvent : IRpcApiService
    {
        private IReduceInRankService _Service;

        public ReduceInRankEvent(IReduceInRankService service)
        {
            _Service = service;
        }

        public void DeleteReduceInRank(DeleteReduceInRank obj)
        {
            _Service.Delete(obj.Id);
        }

        public ReduceInRankConfig GetReduceInRank(GetReduceInRank obj)
        {
            return _Service.Get(obj.RpcMerId, obj.ServerId);
        }

        public void SyncReduceInRank(SyncReduceInRank add)
        {
            _Service.Sync(add.Datum);
        }
    }
}
