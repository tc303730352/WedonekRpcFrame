using RpcSync.DAL;
using RpcSync.Model;
using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.Helper;

namespace RpcSync.Collect.Collect
{
    internal class ServerEventCollect : IServerEventCollect
    {
        private readonly IServerEventSwitchDAL _EventSwitch;
        private readonly ICacheController _Cache;
        public ServerEventCollect ( IServerEventSwitchDAL eventSwitch, ICacheController cache )
        {
            this._Cache = cache;
            this._EventSwitch = eventSwitch;
        }
        public void Refresh ( long[] serverId, string module )
        {
            serverId.ForEach(c =>
            {
                string key = "ServiceEvent_" + c;
                _ = this._Cache.Remove(key);
                if ( module.IsNotNull() )
                {
                    key = string.Join("_", "EventSwitch", c, module);
                    _ = this._Cache.Remove(key);
                }
                key = "EventSwitch_" + c;
                _ = this._Cache.Remove(key);
            });
        }
        public ServiceEventDatum[] Gets ( long[] ids )
        {
            Array.Sort(ids);
            string key = "ServiceEvent_" + ids.Join(',');
            if ( this._Cache.TryGet(key, out ServiceEventDatum[] datas) )
            {
                return datas;
            }
            datas = this._EventSwitch.Gets(ids);
            _ = this._Cache.Set(key, datas);
            return datas;
        }
        public EventSwitch[] GetsEnableEvent ( long serverId, long rpcMerId, string module )
        {
            if ( module.IsNull() )
            {
                return this.GetsEnableEvent(serverId, rpcMerId);
            }
            string key = string.Join("_", "EventSwitch", serverId, module);
            if ( this._Cache.TryGet(key, out EventSwitch[] datas) )
            {
                return datas;
            }
            datas = this._EventSwitch.GetsEnableEvent(serverId, rpcMerId, module);
            _ = this._Cache.Set(key, datas);
            return datas;
        }
        public EventSwitch[] GetsEnableEvent ( long serverId, long rpcMerId )
        {
            string key = "EventSwitch_" + serverId;
            if ( this._Cache.TryGet(key, out EventSwitch[] datas) )
            {
                return datas;
            }
            datas = this._EventSwitch.GetsEnableEvent(serverId, rpcMerId);
            _ = this._Cache.Set(key, datas);
            return datas;
        }
    }
}
