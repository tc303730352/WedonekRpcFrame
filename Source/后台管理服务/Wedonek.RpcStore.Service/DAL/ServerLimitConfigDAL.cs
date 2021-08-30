using RpcModel.Model;

using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Service.DAL
{
        internal class ServerLimitConfigDAL : SqlExecHelper.SqlBasicClass
        {
                public ServerLimitConfigDAL() : base("ServerLimitConfig")
                {

                }
                public bool AddConfig(AddServerLimitConfig add)
                {
                        return this.Insert(add);
                }
                public bool SetConfig(long serverId, ServerLimitConfig config)
                {
                        return this.Update(config, "ServerId", serverId);
                }
                public bool DropConfig(long serverId)
                {
                        return this.Drop("ServerId", serverId) > 0;
                }
                public bool GetConfig(long serverId, out ServerLimitConfig config)
                {
                        return this.GetRow("ServerId", serverId, out config);
                }
        }
}
