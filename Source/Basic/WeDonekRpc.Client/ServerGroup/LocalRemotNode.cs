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
    internal class LocalRemotNode : IRemoteGroup
    {
        private static readonly ConcurrentDictionary<string, RemoteNode> _RemoteServer = new ConcurrentDictionary<string, RemoteNode>();


        static LocalRemotNode ()
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
            int time = now - 900;
            keys.ForEach(a =>
            {
                if (_RemoteServer.TryGetValue(a, out RemoteNode remote))
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

        private static bool _GetRemoteServer (string sysType, out RemoteNode server)
        {
            if (!_RemoteServer.TryGetValue(sysType, out server))
            {
                server = _RemoteServer.GetOrAdd(sysType, new RemoteNode(sysType, RpcStateCollect.RpcMerId));
            }
            if (!server.Init())
            {
                if (_RemoteServer.TryRemove(sysType, out _))
                {
                    server.Dispose();
                }
                return false;
            }
            return server.IsInit;
        }
        public bool FindServer (IRemoteConfig config, out IRemote server)
        {
            if (_GetRemoteServer(config.SystemType, out RemoteNode remote))
            {
                return remote.DistributeNode(config, out server);
            }
            server = null;
            return false;
        }

        public bool FindServer<T> (string sysType, IRemoteConfig config, T model, out IRemote server)
        {
            if (_GetRemoteServer(sysType, out RemoteNode remote))
            {
                return remote.DistributeNode<T>(config, model, out server);
            }
            server = null;
            return false;
        }

        public IRemoteCursor FindAllServer<T> (string systemType, IRemoteConfig config, T model)
        {
            if (_GetRemoteServer(systemType, out RemoteNode remote))
            {
                return remote.DistributeNode<T>(config, model);
            }
            return null;
        }
    }
}
