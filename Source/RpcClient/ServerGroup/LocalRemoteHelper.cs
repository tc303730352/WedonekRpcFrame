using System;
using System.Collections.Concurrent;
using System.Linq;

using RpcClient.Collect;
using RpcClient.Interface;
using RpcClient.Remote;

using RpcModel;

using RpcHelper;
using RpcHelper.TaskTools;
namespace RpcClient.ServerGroup
{
        internal class LocalRemoteHelper : IRemoteGroup
        {
                private static readonly ConcurrentDictionary<string, RemoteHelper> _RemoteServer = new ConcurrentDictionary<string, RemoteHelper>();


                static LocalRemoteHelper()
                {
                        TaskManage.AddTask(new TaskHelper("刷新服务器设置!", new TimeSpan(0, 0, Tools.GetRandom(30, 60)), new Action(_RefreshServer)));
                }

                private static void _RefreshServer()
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
                                if (_RemoteServer.TryGetValue(a, out RemoteHelper remote))
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

                private static bool _GetRemoteServer(string sysType, out RemoteHelper server)
                {
                        if (!_RemoteServer.TryGetValue(sysType, out server))
                        {
                                server = _RemoteServer.GetOrAdd(sysType, new RemoteHelper(sysType, RpcStateCollect.RpcMerId));
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
                public bool FindServer(IRemoteConfig config, out IRemote server)
                {
                        if (_GetRemoteServer(config.SystemType, out RemoteHelper remote))
                        {
                                return remote.DistributeNode(config, out server);
                        }
                        server = null;
                        return false;
                }

                public bool FindServer<T>(string sysType, IRemoteConfig config, T model, out IRemote server)
                {
                        if (_GetRemoteServer(sysType, out RemoteHelper remote))
                        {
                                return remote.DistributeNode<T>(config, model, out server);
                        }
                        server = null;
                        return false;
                }
        }
}
