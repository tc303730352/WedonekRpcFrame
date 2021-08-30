using SqlExecHelper;
using SqlExecHelper.SetColumn;

using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Service.DAL
{
        internal class RemoteServerGroupDAL : SqlBasicClass
        {
                public RemoteServerGroupDAL() : base("RemoteServerGroup")
                {

                }

                public bool Clear(long serverId)
                {
                        return this.Drop("ServerId", serverId) != -2;
                }
                public bool ClearByMerId(long merId)
                {
                        return this.Drop("RpcMerId", merId) != -2;
                }

                internal bool GetServers(long merId, out RemoteGroup[] servers)
                {
                        return this.Get("RpcMerId", merId, out servers);
                }

                internal bool CheckIsExists(long merId, out bool isExists)
                {
                        return this.CheckIsExists(out isExists, new ISqlWhere[] {
                                new SqlWhere("RpcMerId", System.Data.SqlDbType.BigInt){Value=merId}
                        });
                }
                internal bool Get(long id, out RemoteGroupAddParam datum)
                {
                        return this.GetRow("Id", id, out datum);
                }

                internal bool CheckIsExists(long merId, long serverId, out bool isExists)
                {
                        return this.CheckIsExists(out isExists, new ISqlWhere[] {
                                new SqlWhere("RpcMerId", System.Data.SqlDbType.BigInt){Value=merId},
                                new SqlWhere("ServerId", System.Data.SqlDbType.BigInt){Value=serverId}
                        });
                }

                internal bool DropBind(long id)
                {
                        return this.Drop("Id", id) > 0;
                }
                public bool SetWeight(long id, int weight)
                {
                        return this.Update(new ISqlSetColumn[]
                        {
                                new SqlSetColumn("Weight", System.Data.SqlDbType.Int){Value=weight}
                        }, "Id", id);
                }
                internal bool Add(RemoteGroupAddParam add)
                {
                        return this.Insert(add);
                }

                internal bool GetServers(long rpcMerId, long[] serverId, out RemoteGroup[] binds)
                {
                        return this.Get("ServerId", serverId, out binds, new SqlWhere("RpcMerId", System.Data.SqlDbType.BigInt) { Value = rpcMerId });
                }
        }
}
