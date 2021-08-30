using System.Data;

using RpcService.Config;
using RpcService.Model.DAL_Model;

using SqlExecHelper;
using SqlExecHelper.SetColumn;

using RpcHelper;

namespace RpcService.DAL
{
        internal class RemoteServerDAL : SqlExecHelper.SqlBasicClass
        {
                public RemoteServerDAL() : base("RemoteServerConfig")
                {

                }
                public bool GetRemoteServer(long id, out RemoteServerModel model)
                {
                        return this.GetRow(out model, new SqlWhere("Id", SqlDbType.BigInt) { Value = id });
                }
                public bool GetRemoteServerConfig(long[] ids, out BasicServer[] servers)
                {
                        return this.Get("Id", ids, out servers, new ISqlWhere[] {
                                new ComposeSqlWhere(new ISqlWhere[]{
                                        new SqlWhere("IsOnline", SqlDbType.Bit){Value=1},
                                        new SqlWhere("ServiceState", SqlDbType.SmallInt){ Value=0},
                                        new AndOrSqlWhere(new SqlWhere[]{
                                                new SqlWhere("IsOnline", SqlDbType.Bit){Value=0},
                                                new SqlWhere("LastOffliceDate", SqlDbType.Date, SqlExecHelper.QueryType.大等){Value=HeartbeatTimeHelper.CurrentDate},
                                         },false)
                                })
                        });
                }
                public bool LoadServer(out long[] servers)
                {
                        return this.Get<long>("Id", out servers, new ISqlWhere[] {
                                new SqlWhere("BindIndex", SqlDbType.Int){ Value=WebConfig.ServerIndex},
                                new SqlWhere("IsOnline", SqlDbType.Bit){ Value=1}
                        });
                }
                public bool SetServerApiVer(long serverId, string ver)
                {
                        return this.Update(new ISqlSetColumn[] {
                                new SqlSetColumn("ApiVer", SqlDbType.VarChar,20){ Value=ver}
                        }, new SqlWhere[] {
                                new SqlWhere("Id", SqlDbType.BigInt) { Value = serverId }
                        });
                }
                public bool ServerOnline(long serverId)
                {
                        return this.Update(new ISqlSetColumn[] {
                                new SqlSetColumn("IsOnline", SqlDbType.Bit){Value=1},
                                new SqlSetColumn("BindIndex", SqlDbType.Int){Value=WebConfig.ServerIndex}
                        }, new SqlWhere[] {
                                new SqlWhere("Id", SqlDbType.BigInt) { Value = serverId },
                                new SqlWhere("IsOnline", SqlDbType.Bit){Value=0},
                        });
                }
                public bool SetConIp(long serverId, string conIp)
                {
                        return this.Update(new ISqlSetColumn[] {
                                new SqlSetColumn("ConIp", SqlDbType.VarChar,15){Value=conIp}
                        }, new SqlWhere[] {
                                new SqlWhere("Id", SqlDbType.BigInt) { Value = serverId }
                        });
                }
                public bool ServerOffline(long serverId)
                {
                        return this.Update(new ISqlSetColumn[] {
                                new SqlSetColumn("IsOnline", SqlDbType.Bit){Value=0},
                                new SqlSetColumn("LastOffliceDate", SqlDbType.Date){Value=HeartbeatTimeHelper.CurrentDate}
                        }, new ISqlWhere[]
                        {
                               new SqlWhere("Id", SqlDbType.BigInt){Value=serverId},
                               new SqlWhere("IsOnline", SqlDbType.Bit){Value=1},
                                new SqlWhere("BindIndex", SqlDbType.Int){Value=WebConfig.ServerIndex}
                        });
                }

                internal bool FindServiceId(long systemType, string mac, int serverIndex, out long serverId)
                {
                        return this.ExecuteScalar("Id", out serverId, new ISqlWhere[]
                        {
                                new SqlWhere("SystemType", SqlDbType.BigInt){ Value=systemType},
                                new SqlWhere("ServerMac", SqlDbType.VarChar,17){ Value=mac},
                                new SqlWhere("ServerIndex", SqlDbType.Int){ Value=serverIndex}
                        });
                }
        }
}
