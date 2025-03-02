using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using WeDonekRpc.Client.Broadcast;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Remote;
using WeDonekRpc.Client.Track;
using WeDonekRpc.Helper;
using WeDonekRpc.Model.Server;
namespace WeDonekRpc.Client.Collect
{
    internal class RemoteServerCollect
    {
        private static readonly IRemote _Local = null;
        private static readonly ConcurrentDictionary<long, IRemoteRootNode> _RemoteServerList = new ConcurrentDictionary<long, IRemoteRootNode>();
        private static readonly Timer _CheckConTimer = null;
        private static readonly Timer _RefreshStateTimer = null;
        static RemoteServerCollect ()
        {
            _Local = new LocalServer();
            RpcClient.Route.AddRoute<RefreshClientLimit>("RefreshClientLimit", _RefreshLimit);
            RpcClient.Route.AddRoute<RefreshReduce>("RefreshReduce", _RefreshReduce);
            RpcClient.Route.AddRoute<RefreshService>("RefreshService", _Refresh);
            int time = Tools.GetRandom(2, 5) * 1000;
            _CheckConTimer = new Timer(_CheckServerCon, null, time, time);
            _RefreshStateTimer = new Timer(_RefreshState, null, 1000, 1000);
        }
        #region 私有方法
        private static void _RefreshState (object state)
        {
            if (_RemoteServerList.IsEmpty)
            {
                return;
            }
            long[] keys = _RemoteServerList.Keys.ToArray();
            int now = HeartbeatTimeHelper.HeartbeatTime;
            keys.ForEach(a =>
            {
                if (_RemoteServerList.TryGetValue(a, out IRemoteRootNode remote) && remote.IsInit)
                {
                    remote.RefreshState(now);
                }
            });
        }
        private static void _RefreshReduce (RefreshReduce obj)
        {
            if (_RemoteServerList.TryGetValue(obj.ServerId, out IRemoteRootNode remote))
            {
                remote.RefreshReduce();
            }
        }
        private static void _RefreshLimit (RefreshClientLimit obj)
        {
            if (_RemoteServerList.TryGetValue(obj.ServerId, out IRemoteRootNode remote))
            {
                remote.RefreshLimit();
            }
        }
        private static void _Refresh (RefreshService obj)
        {
            if (_RemoteServerList.TryGetValue(obj.ServerId, out IRemoteRootNode remote))
            {
                remote.ResetLock();
            }
            RpcClient.RpcEvent.RefreshService(obj.ServerId);
        }

        private static void _CheckServerCon (object state)
        {
            if (_RemoteServerList.IsEmpty)
            {
                return;
            }
            long[] keys = _RemoteServerList.Keys.ToArray();
            int time = HeartbeatTimeHelper.HeartbeatTime - 600;
            keys.ForEach(a =>
            {
                if (_RemoteServerList.TryGetValue(a, out IRemoteRootNode remote) && remote.IsInit)
                {
                    if (!remote.CheckIsUsable(time))
                    {
                        if (_RemoteServerList.TryRemove(a, out remote))
                        {
                            remote.Dispose();
                        }
                    }
                }
            });
        }

        #endregion
        public static int GetAvgTime (long serverId)
        {
            if (_RemoteServerList.TryGetValue(serverId, out IRemoteRootNode server) && server.IsInit)
            {
                return server.GetAvgTime();
            }
            return 0;
        }
        public static RemoteState[] GetRemoteState ()
        {
            long[] ids = _RemoteServerList.Keys.ToArray();
            return ids.Convert(a =>
            {
                if (_RemoteServerList.TryGetValue(a, out IRemoteRootNode server) && server.IsInit)
                {
                    return server.GetRemoteState();
                }
                return null;
            });
        }
        private static bool _GetRemoteServer (long serverId, out IRemoteRootNode remote)
        {
            if (!_RemoteServerList.TryGetValue(serverId, out remote))
            {
                remote = _RemoteServerList.GetOrAdd(serverId, new RemoteRootNode(serverId));
            }
            if (!remote.Init())
            {
                if (_RemoteServerList.TryRemove(serverId, out remote))
                {
                    remote.Dispose();
                }
                return false;
            }
            return remote.IsInit;
        }
        public static bool GetRemoteServer (long serverId, bool isCloseTrace, out IRemote server)
        {
            if (!_GetServer(serverId, out server))
            {
                return false;
            }
            else if (!isCloseTrace && MsgTrackCollect.CheckIsTrace(out long spanId))
            {
                server = new TrackRemoteNode(server, spanId);
            }
            return true;
        }
        public static bool GetUsableServer (long serverId, bool isCloseTrace, out IRemote server)
        {
            if (!_GetServer(serverId, out server) || !server.IsUsable)
            {
                return false;
            }
            else if (!isCloseTrace && MsgTrackCollect.CheckIsTrace(out long spanId))
            {
                server = new TrackRemoteNode(server, spanId);
            }
            return true;
        }
        private static bool _GetServer (long serverId, out IRemote server)
        {
            if (serverId == RpcStateCollect.ServerId)
            {
                server = _Local;
                return true;
            }
            else if (_GetRemoteServer(serverId, out IRemoteRootNode remote))
            {
                server = remote;
                return true;
            }
            server = null;
            return false;
        }

    }
}
