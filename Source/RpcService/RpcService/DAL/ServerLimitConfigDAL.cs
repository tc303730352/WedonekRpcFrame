using RpcModel.Model;

namespace RpcService.DAL
{
        internal class ServerLimitConfigDAL : SqlExecHelper.SqlBasicClass
        {
                public ServerLimitConfigDAL() : base("ServerLimitConfig")
                {

                }

                public bool GetLimitConfig(long serverId, out ServerLimitConfig config)
                {
                        return this.GetRow("ServerId", serverId, out config);
                }

        }
}
