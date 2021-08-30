using RpcModel;
using RpcModel.Model;
using RpcModel.Server;

using RpcService.Collect;
using RpcService.Controller;
using RpcService.Model;
using RpcService.Save;

namespace RpcService.Logic
{
        internal class ServiceLogic
        {
                /// <summary>
                /// 远程服务登陆
                /// </summary>
                /// <param name="tokenId"></param>
                /// <param name="obj"></param>
                /// <returns></returns>
                public static bool ServerLogin(RpcServerLogin obj, string remoteIp, out RpcServerLoginRes res, out string error)
                {
                        if (!RpcTokenCollect.GetOAuthToken(obj.AccessToken, out RpcToken token, out error))
                        {
                                res = null;
                                return false;
                        }
                        else if (!RpcMerCollect.GetOAuthMer(token.AppId, out RpcMerController mer))
                        {
                                res = null;
                                error = mer.Error;
                                return false;
                        }
                        else if (!SystemTypeCollect.GetSystemType(obj.ServerLogin.SystemType, out SystemTypeController systemType))
                        {
                                res = null;
                                error = systemType.Error;
                                return false;
                        }
                        else if (!RpcServerCollect.ServerLogin(new ServiceLoginParam
                        {
                                ServerIndex = obj.ServerLogin.ServerIndex,
                                SystemType = systemType.Id,
                                Mac = obj.ServerLogin.ServerMac,
                                ApiVer = obj.ApiVer,
                                RemoteIp = remoteIp,
                                Process = obj.ServerLogin.Process
                        }, out RpcServerController server, out error))
                        {
                                res = null;
                                return false;
                        }
                        else
                        {
                                token.SetConServerId(server.ServerId);
                                res = new RpcServerLoginRes
                                {
                                        OAuthConfig = new RpcConfig
                                        {
                                                AllowConIp = mer.AllowServerIp
                                        },
                                        ServerConfig = new RpcServerConfig
                                        {
                                                Name = server.ServerName,
                                                ConfigPrower = server.ConfigPrower,
                                                RegionId = server.RegionId,
                                                ServerId = server.ServerId,
                                                ServerPort = server.ServerPort,
                                                ServerIp = server.ServerIp,
                                                GroupId = server.GroupId,
                                                GroupTypeVal = server.TypeVal,
                                                ServiceState = server.ServiceState,
                                                SystemType = server.SystemType,
                                                PublicKey = server.PublicKey
                                        }
                                };
                                return true;
                        }
                }

                internal static bool ServiceHeartbeat(ServiceHeartbeat obj, string conIp, out string error)
                {
                        if (!RpcServerCollect.GetRpcServer(obj.ServerId, out RpcServerController server))
                        {
                                error = server.Error;
                                return false;
                        }
                        else
                        {
                                server.ServerOnline(conIp);
                                SaveServiceState.Add(obj.RunState, obj.ServerId);
                                SaveSignalState.Add(obj.ServerId, obj.RemoteState);
                                error = null;
                                return true;
                        }
                }

                public static bool GetRemoteServer(GetRemoteServer obj, out ServerConfigInfo config, out string error)
                {
                        if (!RpcTokenCollect.GetOAuthToken(obj.AccessToken, out RpcToken token, out error))
                        {
                                config = null;
                                return false;
                        }
                        else if (!RpcServerCollect.GetRpcServer(obj.ServerId, out RpcServerController server))
                        {
                                config = null;
                                error = server.Error;
                                return false;
                        }
                        else if (!ServerClientLimitCollect.GetClientLimit(token.OAuthMerId, server.ServerId, out ServerClientLimit limit, out error))
                        {
                                config = null;
                                return false;
                        }
                        else if (!ReduceInRankCollect.GetReduceInRank(token.OAuthMerId, server.ServerId, out ReduceInRank reduce, out error))
                        {
                                config = null;
                                return false;
                        }
                        else
                        {
                                config = new ServerConfigInfo
                                {
                                        ServerId = server.ServerId,
                                        ServerPort = server.ServerPort,
                                        Name = server.ServerName,
                                        ServerIp = server.RegionId == obj.RegionId ? server.ServerIp : server.RemoteIp,
                                        GroupId = server.GroupId,
                                        RegionId = server.RegionId,
                                        ConfigPrower = server.ConfigPrower,
                                        GroupTypeVal = server.TypeVal,
                                        SystemType = server.SystemType,
                                        ServiceState = server.ServiceState,
                                        PublicKey = server.PublicKey,
                                        Reduce = reduce,
                                        ClientLimit = limit
                                };
                                return true;
                        }
                }

                public static bool GetServerList(GetServerList obj, out GetServerListRes res, out string error)
                {
                        if (!SystemTypeCollect.GetSystemType(obj.SystemType, out SystemTypeController type))
                        {
                                res = null;
                                error = type.Error;
                                return false;
                        }
                        else if (!RpcServerConfigCollect.GetServerConfig(obj.RpcMerId, type.Id, out RpcServerConfigController config))
                        {
                                res = null;
                                error = config.Error;
                                return false;
                        }
                        else if (obj.LimitRegionId != 0)
                        {
                                res = new GetServerListRes
                                {
                                        BalancedType = type.BalancedType,
                                        Servers = config.GetServers(obj.LimitRegionId)
                                };
                                error = null;
                                return true;
                        }
                        else
                        {
                                ServerConfig[] locals = config.GetServers(obj.RegionId, out ServerConfig[] backs);
                                res = new GetServerListRes
                                {
                                        BalancedType = type.BalancedType,
                                        Servers = locals,
                                        BackUp = backs
                                };
                                error = null;
                                return true;
                        }

                }
        }
}