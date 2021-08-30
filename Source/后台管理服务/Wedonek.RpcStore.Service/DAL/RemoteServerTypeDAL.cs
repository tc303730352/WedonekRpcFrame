using System.Collections.Generic;
using System.Data;

using RpcModel;

using SqlExecHelper;

using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Service.DAL
{
        internal class RemoteServerTypeDAL : SqlBasicClass
        {
                public RemoteServerTypeDAL() : base("RemoteServerType")
                {

                }

                public bool AddServiceType(ServerTypeDatum add, out long id)
                {
                        return this.Insert(add, out id);
                }
                public bool CheckIsRepeat(string typeVal, out bool isExists)
                {
                        return this.CheckIsExists(out isExists, new SqlWhere("TypeVal", SqlDbType.VarChar, 50) { Value = typeVal });
                }

                public bool CheckIsExists(long groupId, out bool isExists)
                {
                        return this.CheckIsExists(out isExists, new SqlWhere("GroupId", SqlDbType.BigInt) { Value = groupId });
                }

                public bool SetServiceType(long id, ServerTypeSetParam param)
                {
                        return this.Update(param, "Id", id);
                }

                public bool DropServiceType(long id)
                {
                        return this.Drop("Id", id) > 0;
                }

                public bool GetServiceType(long id, out ServerType type)
                {
                        return this.GetRow("Id", id, out type);
                }
                public bool GetServiceTypes(long groupId, out ServerType[] types)
                {
                        return this.Get("GroupId", groupId, out types);
                }
                public bool QuerySystemType(ServerTypeQueryParam query, IBasicPage paging, out ServerType[] datas, out long count)
                {
                        List<ISqlWhere> param = new List<ISqlWhere>(3);
                        if (query.GroupId != 0)
                        {
                                param.Add(new SqlWhere("GroupId", SqlDbType.BigInt) { Value = query.GroupId });
                        }
                        if (!string.IsNullOrEmpty(query.Name))
                        {
                                param.Add(new LikeSqlWhere("SystemName", 50, LikeQueryType.全) { Value = query.Name });
                        }
                        if (query.BalancedType.HasValue)
                        {
                                param.Add(new SqlWhere("BalancedType", SqlDbType.SmallInt) { Value = query.BalancedType });
                        }
                        return this.Query("Id desc", paging.Index, paging.Size, out datas, out count, param.ToArray());
                }

                internal bool GetServiceTypes(long[] ids, out ServerType[] types)
                {
                        return this.Get<long, ServerType>("id", ids, out types);
                }

                internal bool Clear(long groupId)
                {
                        return this.Drop("GroupId", groupId) != -2;
                }
        }
}
