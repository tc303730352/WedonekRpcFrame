using System.Data;

using RpcService.Model.DAL_Model;

using SqlExecHelper;

namespace RpcService.DAL
{
        internal class RpcMerDAL : SqlBasicClass
        {
                public RpcMerDAL() : base("RpcMer")
                {

                }
                public bool GetRpcMer(string appId, out RpcMerInfo mer)
                {
                        return this.GetRow(out mer, new SqlWhere("AppId", SqlDbType.VarChar, 32)
                        {
                                Value = appId
                        });
                }

                internal bool GetMerAppId(long id, out string appId)
                {
                        return this.ExecuteScalar("AppId", out appId, "Id", id);
                }
        }
}
