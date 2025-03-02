using System.Collections.Concurrent;
using RpcCentral.Collect.Controller;
using RpcCentral.Common;
using RpcCentral.DAL;
using RpcCentral.Model;
using RpcCentral.Model.DB;
using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Log;
using WeDonekRpc.Model;

namespace RpcCentral.Collect.Collect
{
    internal class RpcServerCollect : IRpcServerCollect
    {
        private readonly ICacheController _Cache;
        private readonly IRemoteServerDAL _Server;
        private static readonly ConcurrentDictionary<long, RpcServerController> _ServerList = new ConcurrentDictionary<long, RpcServerController>();
        private static readonly Timer _CheckTimer;
        private static readonly Timer _RefreshTimer;
        public RpcServerCollect ( ICacheController cache, IRemoteServerDAL server )
        {
            this._Cache = cache;
            this._Server = server;
        }
        static RpcServerCollect ()
        {
            int time = Tools.GetRandom(5000, 10000);
            _CheckTimer = new Timer(new TimerCallback(_CheckServerState), null, time, time);
            time = Tools.GetRandom(120000, 180000);
            _RefreshTimer = new Timer(new TimerCallback(_RefreshServer), null, time, time);
        }
        private static void _CheckServerState ( object state )
        {
            long[] keys = _ServerList.Keys.ToArray();
            if ( keys.Length > 0 )
            {
                using ( IocScope scope = UnityHelper.CreateScore() )
                {
                    keys.ForEach(a =>
                    {
                        if ( _ServerList.TryGetValue(a, out RpcServerController server) && server.IsInit )
                        {
                            server.ChecklsOnline(scope);
                        }
                    });
                }
            }
        }

        private static void _ResetServer ( long id )
        {
            if ( !_ServerList.TryGetValue(id, out RpcServerController service) || !service.IsInit )
            {
                return;
            }
            service.ResetLock();
        }
        private static void _DropServer ( long id )
        {
            if ( _ServerList.TryRemove(id, out RpcServerController service) )
            {
                service.Dispose();
            }
        }
        private static void _RefreshServer ( object state )
        {
            long[] keys = _ServerList.Keys.ToArray();
            if ( keys.Length > 0 )
            {
                int time = HeartbeatTimeHelper.HeartbeatTime - 600;
                keys.ForEach(a =>
               {
                   if ( _ServerList.TryGetValue(a, out RpcServerController server) )
                   {
                       if ( server.ServerIsOnline == false && server.HeartbeatTime <= time )
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
        public void Refresh ( long id, RefreshEventParam param )
        {
            string key = string.Join("_", "FindService", param["SystemType"], param["ServerMac"].Replace(":", string.Empty), param["ServerIndex"]);
            _ = this._Cache.Remove(key);
            key = string.Join("_", "FindService", param["ContainerGroupId"], param["ServerPort"], param["HoldRpcMerId"], param["SystemType"]);
            _ = this._Cache.Remove(key);
            _ResetServer(id);
        }
        private void _Clear ( RemoteServerConfig config )
        {
            string key = string.Join("_", "FindService", config.ContainerGroupId, config.ServerPort, config.HoldRpcMerId, config.SystemType);
            _ = this._Cache.Remove(key);
            key = string.Join("_", "FindService", config.SystemType, config.ServerMac.Replace(":", string.Empty), config.ServerIndex);
            _ = this._Cache.Remove(key);
        }

        public void SetContainerId ( long serverId, long containerId )
        {
            this._Server.SetContainerId(serverId, containerId);
            _ResetServer(serverId);
        }
        public long AddContainer ( RemoteServerConfig config, string sysType )
        {
            if ( this._Server.CheckContainerPort(config.ContainerGroupId.Value, config.ServerPort) )
            {
                throw new ErrorException("rpc.server.port.repeat");
            }
            long id = this._Server.Add(config, sysType);
            this._Clear(config);
            return id;
        }
        public void LoadServer ()
        {
            long[] servers = this._Server.LoadServer(RpcContralConfig.ServerIndex);
            if ( servers.Length > 0 )
            {
                servers.ForEachByParallel(a =>
                {
                    if ( !this._GetRpcServer(a, out RpcServerController server) )
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


        private bool _GetRpcServer ( long serverId, out RpcServerController server )
        {
            if ( !_ServerList.TryGetValue(serverId, out server) )
            {
                server = _ServerList.GetOrAdd(serverId, new RpcServerController(serverId));
            }
            if ( !server.Init() )
            {
                _ = _ServerList.TryRemove(serverId, out server);
                server.Dispose();
                return false;
            }
            return server.IsInit;
        }

        public RpcServerController GetRpcServer ( long serverId )
        {
            if ( this._GetRpcServer(serverId, out RpcServerController server) )
            {
                return server;
            }
            throw new ErrorException(server.Error);
        }
        public RpcServerController FindRpcServer ( long sysTypeId, string mac, int serverIndex )
        {
            long id = this._FindServiceId(sysTypeId, mac, serverIndex);
            if ( id == 0 )
            {
                throw new ErrorException("rpc.server.not.find");
            }
            else if ( this._GetRpcServer(id, out RpcServerController server) )
            {
                return server;
            }
            else
            {
                throw new ErrorException(server.Error);
            }
        }

        private long _FindServiceId ( long sysTypeId, string mac, int serverIndex )
        {
            string key = string.Join("_", "FindService", sysTypeId, mac.Replace(":", string.Empty), serverIndex);
            if ( this._Cache.TryGet(key, out long id) )
            {
                return id;
            }
            id = this._Server.FindServiceId(sysTypeId, mac, serverIndex);
            TimeSpan ex = id == 0 ? new TimeSpan(0, 1, 0) : new TimeSpan(30, 0, 0, 0);
            _ = this._Cache.Set(key, id, ex);
            return id;
        }
        public ServerCont FindService ( ContainerGetArg arg )
        {
            string key = string.Join("_", "FindService", arg.ContainerGroupId, arg.ServerPort, arg.HoldRpcMerId, arg.SystemType);
            if ( this._Cache.TryGet(key, out ServerCont obj) )
            {
                return obj;
            }
            obj = this._Server.FindService(arg);
            _ = obj == null
                ? this._Cache.Set(key, new ServerCont
                {
                    Id = 0
                }, new TimeSpan(0, 1, 0))
                : this._Cache.Set(key, obj, new TimeSpan(30, 0, 0, 0));
            return obj;
        }

        public void RefreshVerNum ( long serverId, int verNum, int oldVerNum )
        {
            if ( _ServerList.TryGetValue(serverId, out RpcServerController server) )
            {
                server.SetVerNum(verNum, oldVerNum);
            }
        }
    }
}
