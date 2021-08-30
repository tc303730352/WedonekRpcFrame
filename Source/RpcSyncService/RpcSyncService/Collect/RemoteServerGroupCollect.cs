using RpcSyncService.Model;

namespace RpcSyncService.Collect
{
        internal class RemoteServerGroupCollect
        {
                public static bool GetAllServer(long merId, out MerServer[] servers, out string error)
                {
                        string key = string.Concat("Server_", merId);
                        if (RpcClient.RpcClient.Cache.TryGet(key, out servers))
                        {
                                error = null;
                                return true;
                        }
                        else if (!new DAL.RemoteServerGroupDAL().GetAllServer(merId, out servers))
                        {
                                error = "rpc.sync.mer.service.get.error";
                                return false;
                        }
                        else
                        {
                                RpcClient.RpcClient.Cache.Set(key, servers);
                                error = null;
                                return true;
                        }
                }
                public static void Refresh(long merId)
                {
                        string key = string.Concat("Server_", merId);
                        RpcClient.RpcClient.Cache.Remove(key);
                }
        }
}
