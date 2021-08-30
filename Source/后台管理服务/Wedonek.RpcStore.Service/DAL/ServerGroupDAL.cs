using System.Collections.Generic;
using System.Data;

using RpcModel;

using SqlExecHelper;
using SqlExecHelper.SetColumn;

using RpcHelper;

using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Service.DAL
{
        internal class ServerGroupDAL : SqlExecHelper.SqlBasicClass
        {
                public ServerGroupDAL() : base("ServerGroup")
                {

                }
                public bool CheckIsRepeat(string typeVal, out bool isExists)
                {
                        return this.CheckIsExists(out isExists, new SqlWhere("TypeVal", SqlDbType.VarChar, 50) { Value = typeVal });
                }
                public bool AddGroup(ServerGroupDatum datum, out long id)
                {
                        return this.Insert(datum, out id);
                }

                public bool SetGroupName(long id, string name)
                {
                        return this.Update(new ISqlSetColumn[] {
                                new SqlSetColumn("GroupName", SqlDbType.NVarChar,50){Value=name}
                        }, new SqlWhere("Id", SqlDbType.BigInt) { Value = id });
                }
                public bool QueryGroup(string name, IBasicPage paging, out ServerGroup[] groups, out long count)
                {
                        List<ISqlWhere> wheres = new List<ISqlWhere>();
                        if (!name.IsNull())
                        {
                                wheres.Add(new LikeSqlWhere("GroupName", 50) { Value = name });
                        }
                        return this.Query("Id desc", paging.Index, paging.Size, out groups, out count, wheres.ToArray());
                }
                public bool GetGroups(out ServerGroup[] groups)
                {
                        return this.Get(out groups);
                }
                public bool GetGroup(long id, out ServerGroup datum)
                {
                        return this.GetRow("Id", id, out datum);
                }

                public bool DropGroup(long id)
                {
                        return this.Drop("Id", id) > 0;
                }

                public bool GetServiceGroup(long[] ids, out ServerGroup[] groups)
                {
                        return this.Get("Id", ids, out groups);
                }
        }
}
