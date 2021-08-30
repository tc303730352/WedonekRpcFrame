using RpcModel;

using RpcService.Model;

namespace RpcService.Collect
{
        internal class ReduceInRankCollect
        {
                public static bool GetReduceInRank(long rpcMerId, long serverId, out ReduceInRank config, out string error)
                {
                        string key = string.Format("Reduce_{0}_{1}", rpcMerId, serverId);
                        if (RpcService.Cache.TryGet(key, out config))
                        {
                                error = null;
                                return true;
                        }
                        else if (!new DAL.ReduceInRankConfigDAL().GetReduceInRank(rpcMerId, serverId, out ReduceInRankConfig data))
                        {
                                error = "rpc.server.relation.get.error";
                                return false;
                        }
                        else
                        {
                                if (data == null)
                                {
                                        config = new ReduceInRank { IsEnable = false, FusingErrorNum = 1 };
                                }
                                else
                                {
                                        config = data.ConvertMap<ReduceInRankConfig, ReduceInRank>();
                                }
                                error = null;
                                RpcService.Cache.Set(key, config, new System.TimeSpan(10, 0, 0, 0));
                                return true;
                        }
                }

                internal static void Refresh(long rpcMerId, long serverId)
                {
                        string key = string.Format("Reduce_{0}_{1}", rpcMerId, serverId);
                        RpcService.Cache.Remove(key);
                }
        }
}
