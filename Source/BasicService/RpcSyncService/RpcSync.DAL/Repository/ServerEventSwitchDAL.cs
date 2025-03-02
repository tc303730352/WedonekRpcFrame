using RpcSync.Model;
using RpcSync.Model.DB;

namespace RpcSync.DAL.Repository
{
    internal class ServerEventSwitchDAL : IServerEventSwitchDAL
    {
        private readonly IRpcExtendResource<ServerEventSwitch> _BasicDAL;
        public ServerEventSwitchDAL ( IRpcExtendResource<ServerEventSwitch> dal )
        {
            this._BasicDAL = dal;
        }
        public EventSwitch[] GetsEnableEvent ( long serverId, long rpcMerId )
        {
            return this._BasicDAL.Gets<EventSwitch>(a => ( a.ServerId == serverId || ( a.RpcMerId == rpcMerId && a.ServerId == 0 ) ) && a.IsEnable).OrderByDescending(a => a.ServerId).Distinct().ToArray();
        }
        public EventSwitch[] GetsEnableEvent ( long serverId, long rpcMerId, string module )
        {
            return this._BasicDAL.Gets<EventSwitch>(a => a.Module == module && a.IsEnable && ( a.ServerId == serverId || ( a.RpcMerId == rpcMerId && a.ServerId == 0 ) )).OrderByDescending(a => a.ServerId).Distinct().ToArray();
        }

        public ServiceEventDatum[] Gets ( long[] ids )
        {
            return this._BasicDAL.Join<SystemEventConfig, ServiceEventDatum>(( a, b ) => a.SysEventId == b.Id, ( a, b ) => ids.Contains(a.Id), ( a, b ) => new ServiceEventDatum
            {
                Id = a.Id,
                SysEventId = a.SysEventId,
                EventName = b.EventName,
                EventType = a.EventType,
                EventLevel = a.EventLevel,
                MsgTemplate = a.MsgTemplate
            });
        }
    }
}
