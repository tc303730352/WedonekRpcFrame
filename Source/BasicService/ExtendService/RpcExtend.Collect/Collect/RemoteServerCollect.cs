using System.Collections.Concurrent;
using RpcExtend.DAL;
using RpcExtend.Model;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Helper;

namespace RpcExtend.Collect.Collect
{
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    internal class RemoteServerCollect : IRemoteServerCollect
    {
        private class _CacheServer
        {
            public RemoteServerConfig Config;
            public int CacheTime;
        }
        private static readonly ConcurrentDictionary<long, _CacheServer> _RemoteConfig = new ConcurrentDictionary<long, _CacheServer>();
        private static readonly Timer _ClearCacheTimer;
        private readonly IIocService _Unity;
        public RemoteServerCollect (IIocService unity)
        {
            this._Unity = unity;
        }
        static RemoteServerCollect ()
        {
            _ClearCacheTimer = new Timer(new TimerCallback(_ClearCache), null, 2000, 2500);
        }

        private static void _ClearCache (object? state)
        {
            if (_RemoteConfig.IsEmpty)
            {
                return;
            }
            long[] keys = _RemoteConfig.Keys.ToArray();
            int time = HeartbeatTimeHelper.HeartbeatTime;
            keys.ForEach(c =>
            {
                if (_RemoteConfig[c].CacheTime <= time)
                {
                    _ = _RemoteConfig.TryRemove(c, out _);
                }
            });
        }

        public RemoteServerConfig GetServer (long serverId)
        {
            if (_RemoteConfig.TryGetValue(serverId, out _CacheServer cache))
            {
                return cache.Config;
            }
            RemoteServerConfig config = this._Unity.Resolve<IRemoteServerConfigDAL>().GetServer(serverId);
            if (config == null)
            {
                throw new ErrorException("rpc.server.config.not.find");
            }
            _ = _RemoteConfig.TryAdd(serverId, new _CacheServer
            {
                CacheTime = HeartbeatTimeHelper.HeartbeatTime + 1800,
                Config = config
            });
            return config;
        }
    }
}
