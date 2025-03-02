using System.Collections.Concurrent;
using RpcManageClient.Model;
using RpcManageClient.Server;
using WeDonekRpc.Client;
using WeDonekRpc.Client.Broadcast;
using WeDonekRpc.Client.Collect;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Server;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using WeDonekRpc.Model.Model;

namespace RpcManageClient.Collect
{
    internal class ServerCollect : IRpcServerCollect
    {
        private static IRpcControlCollect _RpcServer => RpcClient.Ioc.Resolve<IRpcControlCollect>();

        private static readonly ConcurrentDictionary<int, RpcServer> _ServerList = new ConcurrentDictionary<int, RpcServer>();

        private static RpcServer _GetRpcServer (RpcToken token, int id)
        {
            if (_ServerList.TryGetValue(id, out RpcServer server))
            {
                return server;
            }
            else if (!_RpcServer.GetServer(id, out RpcControlServer control, out string error))
            {
                throw new ErrorException(error);
            }
            else
            {
                server = new RpcServer(token, control);
                return _ServerList.GetOrAdd(id, server);
            }
        }
        private static void _RefreshRpc (int id, string key, RefreshEventParam param)
        {
            RpcToken token = RpcClient.GetAccessToken();
            RpcServer server = _GetRpcServer(token, id);
            if (server == null)
            {
                throw new ErrorException("rpc.server.on.config");
            }
            RefreshRpc obj = new RefreshRpc
            {
                EventKey = key,
                Param = param,
                TokenId = token.Access_Token
            };
            server.Send("RefreshRpc", obj);
        }
        private static void _RefreshRpc (string key, RefreshEventParam param)
        {
            if (!_RpcServer.GetServers(out RpcControlServer[] servers, out string error))
            {
                throw new ErrorException(error);
            }
            servers.ForEach(a =>
            {
                _RefreshRpc(a.Id, key, param);
            });

        }
        private static void _RefreshRpc (string key, int regionId, RefreshEventParam param)
        {
            if (!_RpcServer.GetServers(out RpcControlServer[] servers, out string error))
            {
                throw new ErrorException(error);
            }
            RpcControlServer server = servers.Find(c => c.RegionId == regionId);
            if (server == null)
            {
                throw new ErrorException("rpc.server.on.config");
            }
            _RefreshRpc(server.Id, key, param);
        }
        /// <summary>
        /// 刷新集群节点配置
        /// </summary>
        /// <param name="serverId"></param>
        /// <param name="remoteId"></param>
        public void RefreshMerConfig (long rpcMerId, long systemTypeId)
        {
            RefreshEventParam param = new RefreshEventParam
            {
                ["RpcMerId"] = rpcMerId.ToString(),
                ["SystemTypeId"] = systemTypeId.ToString()
            };
            _RefreshRpc("RefreshMerConfig", param);
        }
        public void RefreshReduce (long rpcMerId, long serverId)
        {
            RefreshEventParam param = new RefreshEventParam
            {
                ["RpcMerId"] = rpcMerId.ToString(),
                ["ServerId"] = serverId.ToString()
            };
            _RefreshRpc("RefreshReduce", param);
            new RefreshReduce { ServerId = serverId }.Send();
        }
        public void RefreshLimit (long serverId)
        {
            RefreshEventParam param = new RefreshEventParam
            {
                ["ServerId"] = serverId.ToString()
            };
            _RefreshRpc("RefreshLimit", param);
            RemoteCollect.BroadcastMsg("RefreshLimit", serverId);
        }
        public void RefreshDictateLimit (long serverId, string dictate)
        {
            RefreshEventParam param = new RefreshEventParam
            {
                ["ServerId"] = serverId.ToString(),
                ["Dictate"] = dictate
            };
            _RefreshRpc("RefreshDictateLimit", param);
            RemoteCollect.BroadcastMsg("RefreshLimit", serverId);
        }
        public void RefreshClientLimit (long rpcMerId, long serverId)
        {
            RefreshEventParam param = new RefreshEventParam
            {
                ["ServerId"] = serverId.ToString(),
                ["RpcMerId"] = rpcMerId.ToString()
            };
            _RefreshRpc("RefreshClientLimit", param);
            new RefreshClientLimit { ServerId = serverId }.Send();
        }
        /// <summary>
        /// 刷新集群
        /// </summary>
        /// <param name="merId"></param>
        public void RefreshMer (long merId)
        {
            RefreshEventParam param = new RefreshEventParam
            {
                ["RpcMerId"] = merId.ToString()
            };
            _RefreshRpc("RefreshMer", param);
            new RefreshMer
            {
                RpcMerId = merId
            }.Send();
        }
        /// <summary>
        /// 刷新集群配置
        /// </summary>
        /// <param name="merId"></param>
        /// <param name="systypeId"></param>
        public void RefreshConfig (long merId, long systypeId)
        {
            RefreshEventParam param = new RefreshEventParam
            {
                ["RpcMerId"] = merId.ToString(),
                ["SystemType"] = systypeId.ToString()
            };
            _RefreshRpc("RefreshConfig", param);
            new RefreshMer
            {
                RpcMerId = merId
            }.Send();
        }

        /// <summary>
        /// 刷新服务节点
        /// </summary>
        /// <param name="serverId"></param>
        /// <param name="systypeId"></param>
        public void RefreshService (ServiceDatum service)
        {
            RefreshEventParam arg = new RefreshEventParam()
            {
                ["SystemType"] = service.SystemType.ToString(),
                ["Id"] = service.Id.ToString(),
                ["State"] = ( (int)service.ServiceState ).ToString(),
                ["ServerMac"] = service.ServerMac,
                ["ServerIndex"] = service.ServerIndex.ToString(),
                ["ServerPort"] = service.ServerPort.ToString(),
                ["HoldRpcMerId"] = service.HoldRpcMerId.ToString(),
                ["ContainerGroupId"] = service.ContainerGroupId.ToString(),
            };
            new RefreshService { ServerId = service.Id }.SyncSend();
            _RefreshRpc("RefreshService", arg);
            if (service.IsOnline)
            {
                new ResetServiceState
                {
                    ServiceState = service.ServiceState
                }.Send(service.Id);
            }
        }
        public void RefreshVerNum (long serverId, int verNum, int oldVerNum)
        {
            RefreshEventParam param = new RefreshEventParam
            {
                ["VerNum"] = verNum.ToString(),
                ["OldVerNum"] = oldVerNum.ToString(),
                ["ServerId"] = serverId.ToString()
            };
            _RefreshRpc("RefreshVerNum", param);
        }
        public void RefreshVerConfig (long merId, long sysTypeId, long verNum)
        {
            RefreshEventParam param = new RefreshEventParam
            {
                ["VerNum"] = verNum.ToString(),
                ["RpcMerId"] = merId.ToString(),
                ["SystemType"] = sysTypeId.ToString()
            };
            _RefreshRpc("RefreshVerConfig", param);
        }

        public void RefreshControl (int regionId)
        {
            RefreshEventParam param = new RefreshEventParam
            {
                ["RegionId"] = regionId.ToString()
            };
            _RefreshRpc("RefreshControl", regionId, param);
        }

        public void RefreshTransmit (long rpcMerId, long sysTypeId)
        {
            RefreshEventParam param = new RefreshEventParam
            {
                ["RpcMerId"] = rpcMerId.ToString(),
                ["SystemTypeId"] = sysTypeId.ToString()
            };
            _RefreshRpc("RefreshTransmit", param);
        }

        public void RefreshVerNum (long rpcMerId, long systemTypeId, int verNum)
        {
            RefreshEventParam param = new RefreshEventParam
            {
                ["SystemTypeId"] = systemTypeId.ToString(),
                ["VerNum"] = verNum.ToString(),
                ["RpcMerId"] = rpcMerId.ToString()
            };
            _RefreshRpc("RefreshCurVerNum", param);
        }
    }
}
