using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

using RpcModel;

using RpcService.Controller;
using RpcService.Model;
using RpcService.Model.DAL_Model;

using RpcHelper;
using RpcHelper.TaskTools;

namespace RpcService.Collect
{
        internal class RpcServerCollect
        {
                private static readonly ConcurrentDictionary<long, RpcServerController> _ServerList = new ConcurrentDictionary<long, RpcServerController>();
                public static void Init()
                {
                        Task.Run(new Action(_LoadServerList));
                        TaskManage.AddTask(new TaskHelper("检查服务器状态!", new TimeSpan(0, 0, Tools.GetRandom(8, 20)), new Action(_CheckServerState)));
                        TaskManage.AddTask(new TaskHelper("刷新服务器!", new TimeSpan(0, 0, Tools.GetRandom(120, 180)), new Action(_RefreshServer)));
                }

                internal static void Refresh(long id, RefreshEventParam param)
                {
                        string key = string.Concat("Server_", id);
                        RpcService.Cache.Remove(key);
                        key = string.Join("_", "FindService", param["SystemType"], param["ServerMac"].Replace(":", string.Empty), param["ServerIndex"]);
                        RpcService.Cache.Remove(key);
                        ResetServer(id);
                }

                private static void _LoadServerList()
                {
                        if (new DAL.RemoteServerDAL().LoadServer(out long[] servers) && servers.Length > 0)
                        {
                                servers.ForEach(a =>
                                {
                                        if (!GetRpcServer(a, out RpcServerController server))
                                        {
                                                new WarnLog(server.Error)
                                                {
                                                        LogTitle = "加载服务端失败！",
                                                        LogContent = string.Concat("Id:", a)
                                                }.Save();
                                        }
                                });
                        }
                }
                private static void _CheckServerState()
                {
                        long[] keys = _ServerList.Keys.ToArray();
                        if (keys.Length > 0)
                        {
                                keys.ForEach(a =>
                                {
                                        if (_ServerList.TryGetValue(a, out RpcServerController server) && server.IsInit)
                                        {
                                                server.ChecklsOnline();
                                        }
                                });
                        }
                }
                internal static void ResetServer(long id)
                {
                        if (!_ServerList.TryGetValue(id, out RpcServerController service) || !service.IsInit)
                        {
                                return;
                        }
                        service.ResetLock();
                }
                private static void _DropServer(long id)
                {
                        if (_ServerList.TryRemove(id, out RpcServerController service))
                        {
                                service.Dispose();
                        }
                }
                private static void _RefreshServer()
                {
                        long[] keys = _ServerList.Keys.ToArray();
                        if (keys.Length > 0)
                        {
                                int time = HeartbeatTimeHelper.HeartbeatTime - 600;
                                keys.ForEach(a =>
                               {
                                       if (_ServerList.TryGetValue(a, out RpcServerController server))
                                       {
                                               if (server.ServerIsOnline == false && server.HeartbeatTime <= time)
                                               {
                                                       _DropServer(a);
                                               }
                                               else
                                               {
                                                       server.ResetLock();
                                               }
                                       }
                               });
                        }
                }

                public static bool GetRemoteServerConfig(long[] ids, out BasicServer[] servers, out string error)
                {
                        if (!new DAL.RemoteServerDAL().GetRemoteServerConfig(ids, out servers))
                        {
                                error = "rpc.server.get.error";
                                return false;
                        }
                        else
                        {
                                error = null;
                                return true;
                        }
                }


                public static bool GetRemoteServer(long id, out RemoteServerModel server, out string error)
                {
                        if (!new DAL.RemoteServerDAL().GetRemoteServer(id, out server))
                        {
                                error = "rpc.server.get.error";
                                return false;
                        }
                        else
                        {
                                error = null;
                                return true;
                        }
                }

                public static bool GetRpcServer(long serverId, out RpcServerController server)
                {
                        if (!_ServerList.TryGetValue(serverId, out server))
                        {
                                server = _ServerList.GetOrAdd(serverId, new RpcServerController(serverId));
                        }
                        if (!server.Init())
                        {
                                _ServerList.TryRemove(serverId, out server);
                                server.Dispose();
                                return false;
                        }
                        return server.IsInit;
                }

                private static bool _FindServiceId(ServiceLoginParam login, out long id, out string error)
                {
                        string key = string.Join("_", "FindService", login.SystemType, login.Mac.Replace(":", string.Empty), login.ServerIndex);
                        if (RpcService.Cache.TryGet(key, out id))
                        {
                                error = null;
                                return true;
                        }
                        else if (!new DAL.RemoteServerDAL().FindServiceId(login.SystemType, login.Mac, login.ServerIndex, out id))
                        {
                                error = "rpc.server.find.error";
                                return false;
                        }
                        else
                        {
                                TimeSpan ex = id == 0 ? new TimeSpan(0, 1, 0) : new TimeSpan(30, 0, 0, 0);
                                RpcService.Cache.Set(key, id, ex);
                                error = null;
                                return true;
                        }
                }

                public static bool ServerLogin(ServiceLoginParam login, out RpcServerController server, out string error)
                {
                        if (!_FindServiceId(login, out long serverId, out error))
                        {
                                server = null;
                                return false;
                        }
                        else if (serverId == 0)
                        {
                                error = "rpc.server.not.find";
                                server = null;
                                return false;
                        }
                        else if (!GetRpcServer(serverId, out server))
                        {
                                error = server.Error;
                                return false;
                        }
                        else if (!RpcServerStateCollect.SyncRunState(serverId, login.Process, out error))
                        {
                                return false;
                        }
                        else
                        {
                                server.ServerOnline(login.RemoteIp, login.ApiVer);
                                error = null;
                                return true;
                        }
                }
        }
}
