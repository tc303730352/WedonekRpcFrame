using RpcStore.Collect.LocalEvent.Model;
using RpcStore.DAL;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Client.Interface;

namespace RpcStore.Collect.LocalEvent
{
    [LocalEventName("Delete")]
    internal class ClearServerEvent : IEventHandler<Model.ServerEvent>
    {
        private readonly IServerRunStateDAL _RunState;
        private readonly IServerCurConfigDAL _CurConfig;
        private readonly IReduceInRankConfigDAL _ReduceInRank;
        private readonly IServerClientLimitDAL _ClientLimit;
        private readonly IServerEnvironmentDAL _Environment;
        private readonly IServerSignalStateDAL _SignalState;

        public ClearServerEvent (IServerRunStateDAL runState,
            IServerCurConfigDAL curConfig,
            IReduceInRankConfigDAL reduceInRank,
            IServerClientLimitDAL clientLimit,
            IServerEnvironmentDAL environment,
            IServerSignalStateDAL signalState)
        {
            this._RunState = runState;
            this._CurConfig = curConfig;
            this._ReduceInRank = reduceInRank;
            this._ClientLimit = clientLimit;
            this._Environment = environment;
            this._SignalState = signalState;
        }

        public void HandleEvent (ServerEvent data, string eventName)
        {
            long serverId = data.Server.Id;
            this._RunState.Delete(serverId);
            this._CurConfig.Delete(serverId);
            this._ReduceInRank.Clear(serverId);
            this._ClientLimit.Clear(serverId);
            this._Environment.Clear(serverId);
            this._SignalState.Clear(serverId);
            this._SignalState.ClearRemote(serverId);
        }
    }
}
