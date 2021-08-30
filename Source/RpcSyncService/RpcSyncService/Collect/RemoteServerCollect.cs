using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

using RpcSyncService.Model;

using RpcHelper;
using RpcHelper.TaskTools;

namespace RpcSyncService.Collect
{
        internal class RemoteServerCollect
        {
                private static readonly ConcurrentDictionary<long, RemoteServerConfig> _RemoteConfig = new ConcurrentDictionary<long, RemoteServerConfig>();
                private static RemoteServer[] _Server = null;

                static RemoteServerCollect()
                {
                        TaskManage.AddTask(new TaskHelper("刷新配置", new TimeSpan(0, 1, 0), () => { _Server = null; }));
                }

                public static void Refresh(long serverId)
                {
                        _RemoteConfig.TryRemove(serverId, out _);
                }

                public static bool GetServer(long serverId, out RemoteServerConfig server)
                {
                        if (_RemoteConfig.TryGetValue(serverId, out server))
                        {
                                return true;
                        }
                        else if (!new DAL.RemoteServerConfigDAL().GetServer(serverId, out server))
                        {
                                return false;
                        }
                        else
                        {
                                _RemoteConfig.TryAdd(serverId, server);
                                return true;
                        }
                }

                public static bool GetServerId(List<RootNode> nodes, int regionId, out long[] serverId, out string error)
                {
                        if (!_GetAllServer(out RemoteServer[] server, out error))
                        {
                                serverId = null;
                                return false;
                        }
                        else if (regionId == 0)
                        {
                                serverId = server.Convert(a => nodes.IsExists(b => b.Id == a.SystemType), a => a.Id);
                                return true;
                        }
                        else
                        {
                                serverId = server.Convert(a => a.RegionId == regionId && nodes.IsExists(b => b.Id == a.SystemType), a => a.Id);
                                return true;
                        }
                }

                public static bool GetAllServer(int regionId, out long[] serverId, out string error)
                {
                        if (!_GetAllServer(out RemoteServer[] server, out error))
                        {
                                serverId = null;
                                return false;
                        }
                        else if (regionId == 0)
                        {
                                serverId = server.ConvertAll(a => a.Id);
                                return true;
                        }
                        else
                        {
                                serverId = server.Convert(a => a.RegionId == regionId, a => a.Id);
                                return true;
                        }
                }
                private static bool _GetAllServer(out RemoteServer[] server, out string error)
                {
                        if (_Server != null)
                        {
                                error = null;
                                server = _Server;
                                return true;
                        }
                        else if (!new DAL.RemoteServerConfigDAL().GetServer(out server))
                        {
                                error = "rpc.sync.server.list.get.error";
                                return false;
                        }
                        else
                        {
                                _Server = server;
                                error = null;
                                return true;
                        }
                }
        }
}
