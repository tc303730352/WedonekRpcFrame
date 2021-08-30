using System;

using RpcService.Model;

namespace RpcService.Collect
{
        internal class RemoteServerGroupCollect
        {
                public static bool GetRemoteServer(long merId, long systemTypeId, out RemoteConfig[] services, out string error)
                {
                        if (!new DAL.RemoteServerGroupDAL().GetRemoteServer(merId, systemTypeId, out services))
                        {
                                error = "rpc.server.group.query.error";
                                return false;
                        }
                        else
                        {
                                Array.Sort(services);
                                error = null;
                                return true;
                        }
                }
                public static bool GetRpcMer(long systemTypeId, out long[] rpcMerId, out string error)
                {
                        if (!new DAL.RemoteServerGroupDAL().GetRpcMer(systemTypeId, out rpcMerId))
                        {
                                error = "rpc.server.group.get.error";
                                return false;
                        }
                        else
                        {
                                error = null;
                                return true;
                        }
                }
        }
}
