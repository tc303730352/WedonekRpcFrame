using System.Data;

using RpcSyncService.Model;

using SqlExecHelper;
namespace RpcSyncService.DAL
{
        internal class RemoteServerGroupDAL : SqlBasicClass
        {
                public RemoteServerGroupDAL() : base("RemoteServerGroup")
                {

                }
                public bool GetAllServer(long merId, out MerServer[] servers)
                {
                        return this.Get(out servers, new SqlWhere("RpcMerId", SqlDbType.BigInt) { Value = merId });
                }

        }
}
