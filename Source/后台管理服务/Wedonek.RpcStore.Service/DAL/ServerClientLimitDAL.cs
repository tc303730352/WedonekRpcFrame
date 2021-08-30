using RpcModel.Model;

using SqlExecHelper;

using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Service.DAL
{
        internal class ServerClientLimitDAL : SqlBasicClass
        {
                public ServerClientLimitDAL() : base("ServerClientLimit")
                {

                }
                public bool AddConfig(ServerClientLimitData add)
                {
                        return this.Insert(add);
                }
                public bool SetConfig(long id, ServerClientLimit config)
                {
                        return this.Update(config, "id", id);
                }
                public bool DropConfig(long id, out ServerClientLimitData config)
                {
                        return this.Drop("Id", id, out config);
                }
                public bool GetConfig(long rpcMerId, long serverId, out ServerClientLimitData config)
                {
                        return this.GetRow(out config, new ISqlWhere[] {
                                new SqlWhere("RpcMerId", System.Data.SqlDbType.BigInt){Value=rpcMerId},
                                new SqlWhere("ServerId", System.Data.SqlDbType.BigInt){Value=serverId}
                        });
                }
                public bool FindLimitId(long rpcMerId, long serverId, out long limitId)
                {
                        return this.ExecuteScalar("Id", out limitId, new ISqlWhere[] {
                                new SqlWhere("RpcMerId", System.Data.SqlDbType.BigInt){Value=rpcMerId},
                                new SqlWhere("ServerId", System.Data.SqlDbType.BigInt){Value=serverId}
                        });
                }
        }
}
