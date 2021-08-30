using System.Data;

using RpcSyncService.Model;

using SqlExecHelper;

using RpcHelper;

namespace RpcSyncService.DAL
{
        internal class RemoteServerConfigDAL : SqlExecHelper.SqlBasicClass
        {
                public RemoteServerConfigDAL() : base("RemoteServerConfig")
                {

                }
                public bool GetServerState(out ServerState state)
                {
                        return this.GetRow(out state);
                }

                public bool GetServer(out RemoteServer[] server)
                {
                        return this.Get(out server, new ISqlWhere[] {
                                new SqlWhere("ServiceState", SqlDbType.SmallInt){ Value=0},
                                new SqlWhere("IsOnline", SqlDbType.SmallInt){ Value=1},
                                new AndOrSqlWhere(new SqlWhere[]{
                                        new SqlWhere("IsOnline", SqlDbType.Bit){Value=0},
                                        new SqlWhere("LastOffliceDate", SqlDbType.Date, QueryType.大等){Value=HeartbeatTimeHelper.CurrentDate.AddDays(-1)},
                                 },false)
                        });
                }
                public bool GetServer(long serverId, out RemoteServerConfig server)
                {
                        return this.GetRow(out server, new SqlWhere("Id", System.Data.SqlDbType.BigInt) { Value = serverId });
                }
        }
}
