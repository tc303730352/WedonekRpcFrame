using RpcService.Model;

using SqlExecHelper;

namespace RpcService.DAL
{
        internal class RpcMerConfigDAL : SqlBasicClass
        {
                public RpcMerConfigDAL() : base("RpcMerConfig")
                {

                }
                public bool GetConfig(long rpcMerId, long sysTypeId, out RpcMerConfig config)
                {
                        return this.GetRow(out config, new ISqlWhere[] {
                                new SqlWhere("RpcMerId", System.Data.SqlDbType.BigInt){Value=rpcMerId},
                                new SqlWhere("SystemTypeId", System.Data.SqlDbType.BigInt) { Value = sysTypeId }
                        });
                }
        }
}
