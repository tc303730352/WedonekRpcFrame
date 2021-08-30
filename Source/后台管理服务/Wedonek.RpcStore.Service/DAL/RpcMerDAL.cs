using System.Collections.Generic;

using RpcModel;

using SqlExecHelper;

using RpcHelper;

using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Service.DAL
{
        internal class RpcMerDAL : SqlExecHelper.SqlBasicClass
        {
                public RpcMerDAL() : base("RpcMer")
                {

                }
                public bool SetMer(long id, RpcMerSetParam param)
                {
                        return this.Update(param, "Id", id);
                }
                public bool AddMer(RpcMerDatum add, out long id)
                {
                        return this.Insert(add, out id);
                }
                public bool GetMer(long id, out RpcMer mer)
                {
                        return this.GetRow("Id", id, out mer);
                }
                public bool DropMer(long id)
                {
                        return this.Drop("Id", id) > 0;
                }
                public bool Query(string name, IBasicPage paging, out RpcMer[] mers, out long count)
                {
                        List<ISqlWhere> wheres = new List<ISqlWhere>();
                        if (!name.IsNull())
                        {
                                wheres.Add(new LikeSqlWhere("SystemName", 50) { Value = name });
                        }
                        return this.Query("Id", paging.Index, paging.Size, out mers, out count, wheres.ToArray());
                }
        }
}
