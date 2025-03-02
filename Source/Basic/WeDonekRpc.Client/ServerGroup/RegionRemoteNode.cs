using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using WeDonekRpc.Client.Collect;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Remote;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.ServerGroup
{
    /// <summary>
    /// 区域服务节点
    /// </summary>
    internal class RegionRemoteNode : IRemoteGroup
    {
        private static readonly ConcurrentDictionary<string, RemoteNode> _RemoteServer = new ConcurrentDictionary<string, RemoteNode>();

        private readonly long _RpcMerId;
        private readonly int? _RegionId;
        private readonly string _Key = null;

        public RegionRemoteNode (long? merId, int? regionId)
        {
            this._RpcMerId = merId.HasValue ? merId.Value : RpcStateCollect.RpcMerId;
            this._Key = string.Join("_", this._RpcMerId, regionId.GetValueOrDefault());
            this._RegionId = regionId;
        }

        static RegionRemoteNode ()
        {
            int time = Tools.GetRandom(30000, 60000);
            _ = new Timer(_RefreshServer, null, time, time);
        }

        private static void _RefreshServer (object state)
        {
            if (_RemoteServer.IsEmpty)
            {
                return;
            }
            string[] keys = _RemoteServer.Keys.ToArray();
            int now = HeartbeatTimeHelper.HeartbeatTime;
            int time = now - 1800;
            keys.ForEach(a =>
            {
                if (_RemoteServer.TryGetValue(a, out RemoteNode remote) && remote.IsInit)
                {
                    if (remote.HeartbeatTime <= time)
                    {
                        if (_RemoteServer.TryRemove(a, out remote))
                        {
                            remote.Dispose();
                        }
                    }
                    else
                    {
                        remote.ResetLock(now);
                    }
                }
            });
        }

        private static bool _GetRemoteServer (string sysType, RegionRemoteNode remote, out RemoteNode server)
        {
            string key = string.Concat(sysType, remote._Key);
            if (!_RemoteServer.TryGetValue(key, out server))
            {
                server = _RemoteServer.GetOrAdd(key, new RemoteNode(sysType, remote._RpcMerId, remote._RegionId));
            }
            if (!server.Init())
            {
                if (_RemoteServer.TryRemove(key, out _))
                {
                    server.Dispose();
                }
                return false;
            }
            return server.IsInit;
        }
        public bool FindServer (IRemoteConfig config, out IRemote server)
        {
            if (_GetRemoteServer(config.SystemType, this, out RemoteNode remote))
            {
                return remote.DistributeNode(config, out server);
            }
            server = null;
            return false;
        }

        public bool FindServer<T> (string sysType, IRemoteConfig config, T model, out IRemote server)
        {
            if (_GetRemoteServer(sysType, this, out RemoteNode remote))
            {
                return remote.DistributeNode(config, model, out server);
            }
            server = null;
            return false;
        }

        public IRemoteCursor FindAllServer<T> (string systemType, IRemoteConfig config, T model)
        {
            if (_GetRemoteServer(systemType, this, out RemoteNode remote))
            {
                return remote.DistributeNode(config, model);
            }
            return null;
        }
    }
}
