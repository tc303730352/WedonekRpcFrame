using RpcClient.Collect;
using RpcClient.Model;
using RpcClient.Server;

using RpcModel;
using RpcModel.Model;
using RpcModel.Server;

namespace RpcClient.RpcApi
{
        internal class RpcServiceApi
        {
                #region RPC服务内部接口方法
                internal static bool GetAccessToken(out AccessToken token, out string error)
                {
                        if (RpcServerCollect.GetServer(out RpcServer server))
                        {
                                if (server.GetAccessToken(out token, out error))
                                {
                                        return true;
                                }
                                else
                                {
                                        return error.StartsWith("socket") && RpcServerCollect.GetServer(out server, server.ServerId) && server.GetAccessToken(out token, out error);
                                }
                        }
                        error = "rpc.server.no.con";
                        token = null;
                        return false;
                }

                public static bool GetControlServer(out RpcControlServer[] servers, out string error)
                {
                        if (RpcServerCollect.GetServer(out RpcServer server))
                        {
                                if (server.GetControlServer(out servers, out error))
                                {
                                        return true;
                                }
                                else if (!error.StartsWith("socket"))
                                {
                                        return false;
                                }
                                else if (!RpcServerCollect.GetServer(out server, server.ServerId))
                                {
                                        error = "rpc.server.no.con";
                                        return false;
                                }
                                else
                                {
                                        return server.GetControlServer(out servers, out error);
                                }
                        }
                        servers = null;
                        error = "rpc.server.no.con";
                        return false;
                }
                public static bool GetServerLimit(long serverId, out ServerClientLimit limit, out string error)
                {
                        if (RpcServerCollect.GetServer(out RpcServer server))
                        {
                                if (server.GetServerLimit(serverId, out limit, out error))
                                {
                                        return true;
                                }
                                else if (!error.StartsWith("socket"))
                                {
                                        return false;
                                }
                                else if (!RpcServerCollect.GetServer(out server, server.ServerId))
                                {
                                        error = "rpc.server.no.con";
                                        return false;
                                }
                                else
                                {
                                        return server.GetServerLimit(serverId, out limit, out error);
                                }
                        }
                        limit = null;
                        error = "rpc.server.no.con";
                        return false;
                }
                public static bool GetReduceInRank(long serverId, out ReduceInRank reduce, out string error)
                {
                        if (RpcServerCollect.GetServer(out RpcServer server))
                        {
                                if (server.GetReduceInRank(serverId, out reduce, out error))
                                {
                                        return true;
                                }
                                else if (!error.StartsWith("socket"))
                                {
                                        return false;
                                }
                                else if (!RpcServerCollect.GetServer(out server, server.ServerId))
                                {
                                        error = "rpc.server.no.con";
                                        return false;
                                }
                                else
                                {
                                        return server.GetReduceInRank(serverId, out reduce, out error);
                                }
                        }
                        reduce = null;
                        error = "rpc.server.no.con";
                        return false;
                }
                internal static void SendHeartbeat(ServiceHeartbeat obj)
                {
                        if (RpcServerCollect.GetServer(out RpcServer server))
                        {
                                if (server.SendHeartbeat(obj, out string error) || !error.StartsWith("socket"))
                                {
                                        return;
                                }
                                else if (RpcServerCollect.GetServer(out server, server.ServerId))
                                {
                                        server.SendHeartbeat(obj, out _);
                                }
                        }
                }
                public static bool GetServerLimit(RpcToken token, out LimitConfigRes config, out string error)
                {
                        if (RpcServerCollect.GetServer(out RpcServer server))
                        {
                                if (server.GetServerLimit(token, out config, out error))
                                {
                                        return true;
                                }
                                else if (!error.StartsWith("socket"))
                                {
                                        return false;
                                }
                                else if (!RpcServerCollect.GetServer(out server, server.ServerId))
                                {
                                        error = "rpc.server.no.con";
                                        return false;
                                }
                                else
                                {
                                        return server.GetServerLimit(token, out config, out error);
                                }
                        }
                        config = null;
                        error = "rpc.server.no.con";
                        return false;
                }
                internal static bool ServerLogin(RpcToken token, RemoteServerLogin login, out RpcConfig config, out RpcServerConfig serverConfig, out string error)
                {
                        if (RpcServerCollect.GetServer(out RpcServer server))
                        {
                                if (server.ServerLogin(token, login, out config, out serverConfig, out error))
                                {
                                        return true;
                                }
                                else if (!error.StartsWith("socket"))
                                {
                                        return false;
                                }
                                else if (!RpcServerCollect.GetServer(out server, server.ServerId))
                                {
                                        error = "rpc.server.no.con";
                                        return false;
                                }
                                else
                                {
                                        return server.ServerLogin(token, login, out config, out serverConfig, out error);
                                }
                        }
                        config = null;
                        serverConfig = null;
                        error = "rpc.server.no.con";
                        return false;
                }

                internal static bool GetRemoteServerData(RpcToken token, long serverId, out ServerConfigInfo config, out string error)
                {
                        if (RpcServerCollect.GetServer(out RpcServer server))
                        {
                                if (server.GetRemoteServerData(token, serverId, out config, out error))
                                {
                                        return true;
                                }
                                else if (!error.StartsWith("socket"))
                                {
                                        return false;
                                }
                                else if (!RpcServerCollect.GetServer(out server, server.ServerId))
                                {
                                        error = "rpc.server.no.con";
                                        return false;
                                }
                                else
                                {
                                        return server.GetRemoteServerData(token, serverId, out config, out error);
                                }
                        }
                        config = null;
                        error = "rpc.server.no.con";
                        return false;
                }
                internal static bool GetRemoteServer(string sysType, long merId, int regionId, out GetServerListRes res, out string error)
                {
                        if (RpcServerCollect.GetServer(out RpcServer server))
                        {
                                if (server.GetRemoteServer(sysType, merId, regionId, out res, out error))
                                {
                                        return true;
                                }
                                else if (!error.StartsWith("socket"))
                                {
                                        return false;
                                }
                                else if (!RpcServerCollect.GetServer(out server, server.ServerId))
                                {
                                        error = "rpc.server.no.con";
                                        return false;
                                }
                                else
                                {
                                        return server.GetRemoteServer(sysType, merId, regionId, out res, out error);
                                }
                        }
                        else
                        {
                                res = null;
                                error = "rpc.server.no.con";
                                return false;
                        }
                }
                #endregion
        }
}
