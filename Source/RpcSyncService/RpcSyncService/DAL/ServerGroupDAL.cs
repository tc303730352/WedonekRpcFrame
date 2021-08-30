using RpcSyncService.Model;

namespace RpcSyncService.DAL
{
        internal class ServerGroupDAL : SqlExecHelper.SqlBasicClass
        {
                public ServerGroupDAL() : base("ServerGroup")
                {

                }
                public bool GetServerGroup(out ServerGroup[] groups)
                {
                        return this.Get(out groups);
                }
        }
}
