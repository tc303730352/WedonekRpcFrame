using System.Data;

using RpcSyncService.Model.DAL_Model;

using SqlExecHelper;

namespace RpcSyncService.DAL
{
        internal class ErrorDAL : SqlExecHelper.SqlBasicClass
        {
                public ErrorDAL() : base("ErrorLangMsg")
                {

                }


                public bool GetErrorLang(long errorId, out ErrorLang[] langs)
                {
                        return this.Get(out langs, new SqlWhere("ErrorId", SqlDbType.BigInt) { Value = errorId });
                }

        }
}
