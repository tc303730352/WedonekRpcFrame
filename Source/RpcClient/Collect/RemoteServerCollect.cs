using System;
using System.Collections.Concurrent;
using System.Linq;

using RpcClient.Broadcast;
using RpcClient.Interface;
using RpcClient.Remote;
using RpcClient.Track;

using RpcModel.Server;

using RpcHelper;
using RpcHelper.TaskTools;
namespace RpcClient.Collect
{
        internal class RemoteServerCollect
        {
                private static readonly IRemote _Local = null;
                private static readonly ConcurrentDictionary<long, IRemoteHelper> _RemoteServerList = new ConcurrentDictionary<long, IRemoteHelper>();

                static RemoteServerCollect()
                {
                        RpcClient.Route.AddRoute<RefreshClientLimit>("RefreshClientLimit", _RefreshLimit);
                        RpcClient.Route.AddRoute<RefreshReduce>("RefreshReduce", _RefreshReduce);
                        RpcClient.Route.AddRoute<RefreshService>("RefreshService", _Refresh);
                        TaskManage.AddTask(new TaskHelper("检查服务器链接!", new TimeSpan(0, 0, Tools.GetRandom(2, 5)), new Action(_CheckServerCon)));
                        TaskManage.AddTask(new TaskHelper("刷新服务状态!", new TimeSpan(0, 0, 1), new Action(_RefreshState)));
                        _Local = new LocalServer();
                }
                #region 私有方法
                private static void _RefreshState()
                {
                        if (_RemoteServerList.IsEmpty)
                        {
                                return;
                        }
                        long[] keys = _RemoteServerList.Keys.ToArray();
                        int now = HeartbeatTimeHelper.HeartbeatTime;
                        keys.ForEach(a =>
                        {
                                if (_RemoteServerList.TryGetValue(a, out IRemoteHelper remote) && remote.IsInit)
                                {
                                        remote.RefreshState(now);
                                }
                        });
                }
                private static void _RefreshReduce(RefreshReduce obj)
                {
                        if (_RemoteServerList.TryGetValue(obj.ServerId, out IRemoteHelper remote))
                        {
                                remote.RefreshReduce();
                        }
                }
                private static void _RefreshLimit(RefreshClientLimit obj)
                {
                        if (_RemoteServerList.TryGetValue(obj.ServerId, out IRemoteHelper remote))
                        {
                                remote.RefreshLimit();
                        }
                }
                private static void _Refresh(RefreshService obj)
                {
                        if (_RemoteServerList.TryGetValue(obj.ServerId, out IRemoteHelper remote))
                        {
                                remote.ResetLock();
                        }
                        RpcClient.RpcEvent.RefreshService(obj.ServerId);
                }

                private static void _CheckServerCon()
                {
                        if (_RemoteServerList.IsEmpty)
                        {
                                return;
                        }
                        long[] keys = _RemoteServerList.Keys.ToArray();
                        int time = HeartbeatTimeHelper.HeartbeatTime - 600;
                        keys.ForEach(a =>
                        {
                                if (_RemoteServerList.TryGetValue(a, out IRemoteHelper remote) && remote.IsInit)
                                {
                                        if (!remote.CheckIsUsable() && remote.OfflineTime < time)
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
                public static int GetAvgTime(long serverId)
                {
                        if (_RemoteServerList.TryGetValue(serverId, out IRemoteHelper server) && server.IsInit)
                        {
                                return server.GetAvgTime();
                        }
                        return 0;
                }
                public static RemoteState[] GetRemoteState()
                {
                        long[] ids = _RemoteServerList.Keys.ToArray();
                        return ids.Convert(a =>
                        {
                                if (_RemoteServerList.TryGetValue(a, out IRemoteHelper server) && server.IsInit)
                                {
                                        return server.GetRemoteState();
                                }
                                return null;
                        });
                }
                private static bool _GetRemoteServer(long serverId, out IRemoteHelper remote)
                {
                        if (!_RemoteServerList.TryGetValue(serverId, out remote))
                        {
                                remote = _RemoteServerList.GetOrAdd(serverId, new RemoteServer(serverId));
                        }
                        if (!remote.Init())
                        {
                                if (_RemoteServerList.TryRemove(serverId, out _))
                                {
                                        remote.Dispose();
                                }
                                return false;
                        }
                        return remote.IsInit;
                }
                public static bool GetRemoteServer(long serverId, out IRemote server)
                {
                        if (!_GetServer(serverId, out server))
                        {
                                return false;
                        }
                        else if (MsgTrackCollect.CheckIsTrace(out long spanId))
                        {
                                server = new TrackRemoteHelper(server, spanId);
                        }
                        return true;
                }
                private static bool _GetServer(long serverId, out IRemote server)
                {
                        if (serverId == RpcStateCollect.ServerId)
                        {
                                server = _Local;
                                return true;
                        }
                        else if (_GetRemoteServer(serverId, out IRemoteHelper remote))
                        {
                                server = remote;
                                return true;
                        }
                        server = null;
                        return false;
                }

        }
}
