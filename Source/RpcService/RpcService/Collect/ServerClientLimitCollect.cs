using System;

using RpcModel.Model;

namespace RpcService.Collect
{
        internal class ServerClientLimitCollect
        {
                internal static void Refresh(long rpcMerId, long serverId)
                {
                        string key = string.Join("_", "ClientLimit", rpcMerId, serverId);
                        RpcService.Cache.Remove(key);

                }
                internal static bool GetClientLimit(long rpcMerId, long serverId, out ServerClientLimit config, out string error)
                {
                        string key = string.Join("_", "ClientLimit", rpcMerId, serverId);
                        if (RpcService.Cache.TryGet(key, out config))
                        {
                                error = null;
                                return true;
                        }
                        else if (!new DAL.ServerClientLimitDAL().GetClientLimit(rpcMerId, serverId, out config))
                        {
                                error = "rpc.limit.config.get.error";
                                return false;
                        }
                        else
                        {
                                if (config == null)
                                {
                                        config = new ServerClientLimit
                                        {
                                                IsEnable = false
                                        };
                                }
                                RpcService.Cache.Add(key, config, new TimeSpan(10, 0, 0, 0));
                                error = null;
                                return true;
                        }
                }
        }
}
