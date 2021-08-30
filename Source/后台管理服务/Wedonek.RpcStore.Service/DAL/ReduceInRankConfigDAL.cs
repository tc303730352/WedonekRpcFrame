using SqlExecHelper;

using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Service.DAL
{
        internal class ReduceInRankConfigDAL : SqlExecHelper.SqlBasicClass
        {
                public ReduceInRankConfigDAL() : base("ReduceInRankConfig")
                {

                }
                public bool GetReduceInRank(long rpcMerId, long serverId, out ReduceInRankConfig config)
                {
                        return this.GetRow(out config, new ISqlWhere[] {
                                new SqlWhere("RpcMerId", System.Data.SqlDbType.BigInt){Value=rpcMerId},
                                new SqlWhere("ServerId", System.Data.SqlDbType.BigInt){Value=serverId},
                        });
                }
                public bool GetReduceInRankId(long rpcMerId, long serverId, out long id)
                {
                        return this.ExecuteScalar("Id", out id, new ISqlWhere[] {
                                new SqlWhere("RpcMerId", System.Data.SqlDbType.BigInt){Value=rpcMerId},
                                new SqlWhere("ServerId", System.Data.SqlDbType.BigInt){Value=serverId},
                        });
                }
                public bool SetReduceInRank(long id, ReduceInRankDatum datum)
                {
                        return this.Update(datum, "Id", id);
                }
                public bool AddReduceInRank(AddReduceInRank add)
                {
                        return this.Insert(add);
                }
                public bool Drop(long id)
                {
                        return this.Drop("Id", id) > 0;
                }
        }
}
