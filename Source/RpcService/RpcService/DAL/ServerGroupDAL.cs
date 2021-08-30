using System.Data;

using SqlExecHelper;

namespace RpcService.DAL
{
        internal class ServerGroupDAL : SqlBasicClass
        {
                public ServerGroupDAL() : base("ServerGroup")
                {

                }
                public bool GetTypeVal(long groupId, out string typeVal)
                {
                        return this.ExecuteScalar("TypeVal", out typeVal, new SqlWhere("Id", SqlDbType.BigInt)
                        {
                                Value = groupId
                        });
                }
        }
}
