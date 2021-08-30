using RpcService.Model;

using SqlExecHelper;

namespace RpcService.DAL
{
        internal class ReduceInRankConfigDAL : SqlExecHelper.SqlBasicClass
        {
                public ReduceInRankConfigDAL() : base("ReduceInRankConfig")
                {

                }

                public bool GetReduceInRank(long rpcMerId, long servrId, out ReduceInRankConfig config)
                {
                        return this.GetRow(out config, new ISqlWhere[] {
                                new SqlWhere("RpcMerId", System.Data.SqlDbType.BigInt){Value=rpcMerId},
                                new  SqlWhere("ServerId", System.Data.SqlDbType.BigInt){Value=servrId}
                        });
                }

        }
}
