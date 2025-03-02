using RpcSync.Collect;
using RpcSync.Model;
using RpcSync.Service.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using WeDonekRpc.ModularModel.SysEvent.Model;
using WeDonekRpc.ModularModel.SysEvent.Msg;

namespace RpcSync.Service.Service
{
    internal class SysEventService : ISysEventService
    {
        private readonly IServerEventCollect _ServerEvent;
        private readonly IRemoteServerGroupCollect _RemoteGroup;
        public SysEventService ( IServerEventCollect ev, IRemoteServerGroupCollect remoteGroup )
        {
            this._ServerEvent = ev;
            this._RemoteGroup = remoteGroup;
        }
        public void Refresh ( long rpcMerId, string module )
        {
            long[] serverId = this._RemoteGroup.GetHoldServerId(rpcMerId);
            this._ServerEvent.Refresh(serverId, module);
            new RefreshEventModule
            {
                Module = module
            }.Send(serverId);
        }

        public ServiceSysEvent[] GetEnableEvents ( MsgSource source, string module )
        {
            EventSwitch[] evs = this._ServerEvent.GetsEnableEvent(source.ServerId, source.RpcMerId, module);
            if ( evs.IsNull() )
            {
                return null;
            }
            return evs.ConvertAll(a =>
            {
                return new ServiceSysEvent
                {
                    EventConfig = a.EventConfig,
                    Module = a.Module,
                    EventId = a.Id
                };
            });
        }
    }
}
