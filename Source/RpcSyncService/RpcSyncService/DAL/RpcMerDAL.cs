using SqlExecHelper;
namespace RpcSyncService.DAL
{
        internal class RpcMerDAL : SqlBasicClass
        {
                public RpcMerDAL() : base("RpcMer")
                {

                }
                public bool GetVerNum(long merId, out int verNum)
                {
                        return this.ExecuteScalar("VerNum", out verNum, new SqlWhere("Id", System.Data.SqlDbType.BigInt) { Value = merId });
                }
        }
}
