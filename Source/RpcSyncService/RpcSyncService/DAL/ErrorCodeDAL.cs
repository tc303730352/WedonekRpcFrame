using System.Data;

using RpcSyncService.Model;

using SqlExecHelper;

namespace RpcSyncService.DAL
{
        internal class ErrorCodeDAL : SqlBasicClass
        {
                public ErrorCodeDAL() : base("ErrorCollect")
                {

                }
                public bool SyncError(string code, out ErrorDatum error)
                {
                        return this.GetRow("RegError", out error, new SqlBasicParameter[] {
                                new SqlBasicParameter("@Code", SqlDbType.VarChar, 50) { Value = code }
                        });
                }
                public bool FindErrorCode(long errorId, out string error)
                {
                        return this.ExecuteScalar("ErrorCode", out error, new SqlWhere("Id", SqlDbType.BigInt) { Value = errorId });
                }

                public bool GetErroMaxId(out long id)
                {
                        return this.ExecuteScalar("Id", SqlFuncType.max, out id);
                }
        }
}
