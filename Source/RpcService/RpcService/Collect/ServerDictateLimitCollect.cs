using System;

using RpcModel.Model;

namespace RpcService.Collect
{
        internal class ServerDictateLimitCollect
        {
                internal static void Refresh(long serverId)
                {
                        string key = string.Concat("DicateLimit_", serverId);
                        RpcService.Cache.Remove(key);

                }
                internal static bool GetDictateLimit(long serverId, out ServerDictateLimit[] limits, out string error)
                {
                        string key = string.Concat("DicateLimit_", serverId);
                        if (RpcService.Cache.TryGet(key, out limits))
                        {
                                error = null;
                                return true;
                        }
                        else if (!new DAL.ServerDictateLimitDAL().GetDictateLimit(serverId, out limits))
                        {
                                error = "rpc.dictate.limit.get.error";
                                return false;
                        }
                        else
                        {
                                RpcService.Cache.Add(key, limits, new TimeSpan(10, 0, 0, 0));
                                error = null;
                                return true;
                        }
                }
        }
}
