using RpcClient;

using RpcHelper;

using RpcManageClient;

using Wedonek.RpcStore.Service.DAL;
using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Service.Collect
{
        internal class ReduceInRankCollect : BasicCollect<ReduceInRankConfigDAL>, IReduceInRankCollect
        {
                private IRpcServerCollect _RpcServer => this.GetCollect<IRpcServerCollect>();
                public ReduceInRankConfig GetReduceInRank(long rpcMerId, long serverId)
                {
                        if (!this.BasicDAL.GetReduceInRank(rpcMerId, serverId, out ReduceInRankConfig config))
                        {
                                throw new ErrorException("rpc.reduce.get.error");
                        }
                        return config;
                }
                public long GetReduceInRankId(long rpcMerId, long serverId)
                {
                        if (!this.BasicDAL.GetReduceInRankId(rpcMerId, serverId, out long id))
                        {
                                throw new ErrorException("rpc.reduce.get.error");
                        }
                        return id;
                }

                public void SyncReduceInRank(AddReduceInRank datum)
                {
                        long id = this.GetReduceInRankId(datum.RpcMerId, datum.ServerId);
                        if (id != 0)
                        {
                                this._SetReduceInRank(id, datum.ConvertMap<AddReduceInRank, ReduceInRankDatum>());
                        }
                        else
                        {
                                this._AddReduceInRank(datum);
                        }
                        this._RpcServer.RefreshReduce(datum.RpcMerId, datum.ServerId);
                }
                private void _SetReduceInRank(long id, ReduceInRankDatum datum)
                {
                        if (!this.BasicDAL.SetReduceInRank(id, datum))
                        {
                                throw new ErrorException("rpc.reduce.set.error");
                        }
                }
                private void _AddReduceInRank(AddReduceInRank add)
                {
                        if (!this.BasicDAL.AddReduceInRank(add))
                        {
                                throw new ErrorException("rpc.reduce.set.error");
                        }
                }
                public void DropReduceInRank(long id)
                {
                        if (!this.BasicDAL.Drop(id))
                        {
                                throw new ErrorException("rpc.reduce.drop.error");
                        }
                }
        }
}
