using RpcModel.Model;

using SqlExecHelper;
namespace RpcService.DAL
{
        internal class ServerDictateLimitDAL : SqlExecHelper.SqlBasicClass
        {
                public ServerDictateLimitDAL() : base("ServerDictateLimit")
                {

                }

                public bool GetDictateLimit(long serverId, out ServerDictateLimit[] limts)
                {
                        return this.Get(out limts, new SqlWhere("ServerId", System.Data.SqlDbType.BigInt) { Value = serverId });
                }
        }
}
