using RpcService.Model;

namespace RpcService.Collect
{
        internal class RpcMerConfigCollect
        {
                public static bool GetConfig(long rpcMerId, long sysTypeId, out RpcMerConfig config, out string error)
                {
                        if (!new DAL.RpcMerConfigDAL().GetConfig(rpcMerId, sysTypeId, out config))
                        {
                                error = "rpc.mer.config.get.error";
                                return false;
                        }
                        else
                        {
                                if (config == null)
                                {
                                        config = new RpcMerConfig
                                        {
                                                IsRegionIsolate = true,
                                                IsolateLevel = false
                                        };
                                }
                                error = null;
                                return true;
                        }
                }
        }
}
