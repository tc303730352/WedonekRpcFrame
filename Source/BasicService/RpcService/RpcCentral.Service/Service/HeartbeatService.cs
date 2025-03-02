using RpcCentral.Collect;
using RpcCentral.Collect.Controller;
using RpcCentral.Service.Interface;
using WeDonekRpc.Model.Server;

namespace RpcCentral.Service.Service
{
    internal class HeartbeatService : IHeartbeatService
    {
        private readonly IRpcServerCollect _Server;
        private readonly ISyncRunStateCollect _RunState;
        private readonly ISyncSignalStateCollect _SignalState;
        public HeartbeatService (IRpcServerCollect server,
            ISyncSignalStateCollect signalState,
            ISyncRunStateCollect runState)
        {
            this._SignalState = signalState;
            this._Server = server;
            this._RunState = runState;
        }

        public int Heartbeat (ServiceHeartbeat obj, string conIp)
        {
            RpcServerController server = this._Server.GetRpcServer(obj.ServerId);
            _ = server.ServerOnline(conIp);
            this._RunState.Sync(obj.RunState, obj.ServerId);
            this._SignalState.Sync(obj.ServerId, obj.RemoteState);
            return server.SyncVerNum(obj.VerNum);
        }
    }
}
