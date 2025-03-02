using RpcStore.Collect.LocalEvent.Model;
using RpcStore.DAL;
using RpcStore.Model.DB;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Client.Interface;

namespace RpcStore.Collect.LocalEvent
{
    [LocalEventName("Delete")]
    internal class DeleteServerGroupEvent : IEventHandler<Model.RemoteServerGroupEvent>
    {
        private readonly IReduceInRankConfigDAL _ReduceInRank;
        private readonly IServerClientLimitDAL _ClientLimit;

        public DeleteServerGroupEvent (IReduceInRankConfigDAL reduceInRank, IServerClientLimitDAL clientLimit)
        {
            this._ReduceInRank = reduceInRank;
            this._ClientLimit = clientLimit;
        }

        public void HandleEvent (RemoteServerGroupEvent data, string eventName)
        {
            RemoteServerGroupModel source = data.Source;
            this._ReduceInRank.Clear(source.RpcMerId, source.ServerId);
            this._ClientLimit.Clear(source.RpcMerId, source.ServerId);
        }
    }
}
