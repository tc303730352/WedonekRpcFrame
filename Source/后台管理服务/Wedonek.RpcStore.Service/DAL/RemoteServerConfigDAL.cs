using System.Collections.Generic;
using System.Data;

using RpcModel;

using SqlExecHelper;
using SqlExecHelper.SetColumn;

using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Service.DAL
{
        internal class RemoteServerConfigDAL : SqlBasicClass
        {
                public RemoteServerConfigDAL() : base("RemoteServerConfig")
                {

                }
                public bool CheckIsExists(long sysTypeId, out bool isExists)
                {
                        return this.CheckIsExists(out isExists, new ISqlWhere[]{
                                new SqlWhere("SystemType", SqlDbType.BigInt) { Value = sysTypeId }
                        });
                }
                public bool AddService(ServerConfigAddParam add, out long id)
                {
                        return this.Insert(add, out id);
                }
                public bool GetServiceIndex(long sysTypeId, string mac, out int index)
                {
                        return this.ExecuteScalar("ServerIndex", SqlFuncType.max, out index, new ISqlWhere[] {
                                new SqlWhere("SystemType", SqlDbType.BigInt){Value=sysTypeId},
                                new SqlWhere("ServerMac", SqlDbType.VarChar,17){Value=mac}
                        });
                }

                internal bool SetServiceState(long id, RpcServiceState state)
                {
                        return this.Update(new ISqlSetColumn[] {
                        new SqlSetColumn("ServiceState", SqlDbType.SmallInt){Value=state}
                       }, "Id", id);
                }

                public bool SetService(long id, ServerConfigSetParam data)
                {
                        return this.Update(data, "Id", id);
                }
                public bool DropService(long id)
                {
                        return this.Drop("Id", id) > 0;
                }
                public bool QueryService(QueryServiceParam query, IBasicPage paging, out ServerConfigDatum[] services, out long count)
                {
                        List<ISqlWhere> list = new List<ISqlWhere>(5);
                        if (query.SystemTypeId != 0)
                        {
                                list.Add(new SqlWhere("SystemType", SqlDbType.BigInt) { Value = query.SystemTypeId });
                        }
                        else if (query.GroupId != 0)
                        {
                                list.Add(new SqlWhere("GroupId", SqlDbType.BigInt) { Value = query.GroupId });
                        }
                        if (query.IsOnline != null)
                        {
                                list.Add(new SqlWhere("IsOnline", SqlDbType.Bit) { Value = (bool)query.IsOnline ? 1 : 0 });
                        }
                        if (!string.IsNullOrEmpty(query.ServiceMac))
                        {
                                list.Add(new SqlWhere("ServiceMac", SqlDbType.VarChar, 17) { Value = query.ServiceMac });
                        }
                        else if (!string.IsNullOrEmpty(query.ServiceName))
                        {
                                list.Add(new LikeSqlWhere("ServerName", 50, LikeQueryType.全) { Value = query.ServiceName });
                        }
                        return this.Query("Id desc", paging.Index, paging.Size, out services, out count, list.ToArray());
                }
                public bool GetService(long id, out RemoteServerConfig datum)
                {
                        return this.GetRow("Id", id, out datum);
                }
                public bool GetServices(long[] ids, out ServerConfigDatum[] datum)
                {
                        return this.Get("Id", ids, out datum);
                }
                public bool CheckIsOnline(long id, out bool isOnline)
                {
                        return this.ExecuteScalar("IsOnline", out isOnline, new SqlWhere("Id", SqlDbType.BigInt) { Value = id });
                }

                public bool CheckServerPort(string mac, int serverPort, out bool isExists)
                {
                        return this.CheckIsExists(out isExists, new ISqlWhere[] {
                                new SqlWhere("ServerMac", SqlDbType.VarChar,17){Value=mac},
                                new SqlWhere("ServerPort", SqlDbType.Int){Value=serverPort}
                        });
                }

                internal bool CheckIsExistsByGroup(long groupId, out bool isExists)
                {
                        return this.CheckIsExists(out isExists, new ISqlWhere[]{
                                new SqlWhere("GroupId", SqlDbType.BigInt) { Value = groupId }
                        });
                }

                internal bool CheckRegion(int regionId, out bool isExists)
                {
                        return this.CheckIsExists(out isExists, new ISqlWhere[]{
                                new SqlWhere("RegionId", SqlDbType.Int) { Value = regionId }
                        });
                }
        }
}
