using System;

using RpcModel.Model;

namespace RpcService.Collect
{
        internal class ServerLimitConfigCollect
        {
                internal static void Refresh(long serverId)
                {
                        string key = string.Concat("Limit_", serverId);
                        RpcService.Cache.Remove(key);

                }
                internal static bool GetServerLimit(long serverId, out ServerLimitConfig config, out string error)
                {
                        string key = string.Concat("Limit_", serverId);
                        if (RpcService.Cache.TryGet(key, out config))
                        {
                                error = null;
                                return true;
                        }
                        else if (!new DAL.ServerLimitConfigDAL().GetLimitConfig(serverId, out config))
                        {
                                error = "rpc.limit.config.get.error";
                                return false;
                        }
                        else
                        {
                                if (config == null)
                                {
                                        config = new ServerLimitConfig
                                        {
                                                IsEnable = false,
                                                IsEnableBucket = false
                                        };
                                }
                                RpcService.Cache.Add(key, config, new TimeSpan(10, 0, 0, 0));
                                error = null;
                                return true;
                        }
                }
        }
}
