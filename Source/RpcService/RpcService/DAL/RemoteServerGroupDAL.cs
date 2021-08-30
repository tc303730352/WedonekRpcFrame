using System.Data;

using RpcService.Model;

using SqlExecHelper;
namespace RpcService.DAL
{
        internal class RemoteServerGroupDAL : SqlBasicClass
        {
                public RemoteServerGroupDAL() : base("RemoteServerGroup")
                {

                }
                internal bool GetRpcMer(long systemTypeId, out long[] rpcMerId)
                {
                        return this.GroupByOne("RpcMerId", out rpcMerId, new ISqlWhere[] {
                                new SqlWhere("SystemType", SqlDbType.BigInt){ Value=systemTypeId},
                        });
                }
                internal bool GetRemoteServer(long rpcMerId, long systemTypeId, out RemoteConfig[] services)
                {
                        return this.Get(out services, new ISqlWhere[] {
                                 new SqlWhere("RpcMerId", SqlDbType.BigInt){ Value=rpcMerId},
                                new SqlWhere("SystemType", SqlDbType.BigInt){ Value=systemTypeId},
                        });
                }
        }
}
