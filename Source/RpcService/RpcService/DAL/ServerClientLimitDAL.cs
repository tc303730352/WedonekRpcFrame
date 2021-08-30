using RpcModel.Model;

using SqlExecHelper;

namespace RpcService.DAL
{
        internal class ServerClientLimitDAL : SqlBasicClass
        {
                public ServerClientLimitDAL() : base("ServerClientLimit")
                {

                }


                public bool GetClientLimit(long rpcMerId, long serverId, out ServerClientLimit limit)
                {
                        return this.GetRow(out limit, new ISqlWhere[] {
                                new SqlWhere("RpcMerId", System.Data.SqlDbType.BigInt){Value=rpcMerId},
                                new SqlWhere("ServerId", System.Data.SqlDbType.BigInt){Value=serverId},
                        });
                }
        }
}
