using System;
using System.Collections.Concurrent;
using System.Linq;

using RpcClient.Interface;
using RpcClient.Remote;

using RpcModel;

using RpcHelper;
using RpcHelper.TaskTools;

namespace RpcClient.ServerGroup
{
        internal class OtherRemoteHelper : IRemoteGroup
        {
                private static readonly ConcurrentDictionary<string, RemoteHelper> _RemoteServer = new ConcurrentDictionary<string, RemoteHelper>();

                private long _RpcMerId { get; }
                private int _RegionId { get; }
                private readonly string _Key = null;

                public OtherRemoteHelper(long merId, int regionId)
                {
                        this._Key = string.Join("_", merId, regionId);
                        this._RegionId = regionId;
                        this._RpcMerId = merId;
                }
                static OtherRemoteHelper()
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
                        int time = now - 1800;
                        keys.ForEach(a =>
                        {
                                if (_RemoteServer.TryGetValue(a, out RemoteHelper remote) && remote.IsInit)
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

                private static bool _GetRemoteServer(string sysType, OtherRemoteHelper remote, out RemoteHelper server)
                {
                        string key = string.Concat(sysType, remote._Key);
                        if (!_RemoteServer.TryGetValue(key, out server))
                        {
                                server = _RemoteServer.GetOrAdd(key, new RemoteHelper(sysType, remote._RpcMerId, remote._RegionId));
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
                public bool FindServer(IRemoteConfig config, out IRemote server)
                {
                        if (_GetRemoteServer(config.SystemType, this, out RemoteHelper remote))
                        {
                                return remote.DistributeNode(config, out server);
                        }
                        server = null;
                        return false;
                }

                public bool FindServer<T>(string sysType, IRemoteConfig config, T model, out IRemote server)
                {
                        if (_GetRemoteServer(sysType, this, out RemoteHelper remote))
                        {
                                return remote.DistributeNode(config, model, out server);
                        }
                        server = null;
                        return false;
                }
        }
}
