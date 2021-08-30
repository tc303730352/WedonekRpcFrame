using System.Collections.Generic;
using System.Data;

using RpcModel;
using RpcModel.Model;

using SqlExecHelper;

using RpcHelper;

using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Service.DAL
{
        internal class ServerDictateLimitDAL : SqlExecHelper.SqlBasicClass
        {
                public ServerDictateLimitDAL() : base("ServerDictateLimit")
                {

                }
                public bool AddDictateLimit(AddDictateLimit add, out long id)
                {
                        return this.Insert(add, out id);
                }

                public bool GetDictateLimit(long id, out ServerDictateLimitData limit)
                {
                        return this.GetRow("Id", id, out limit);
                }
                public bool DropDictateLimit(long id)
                {
                        return this.Drop("Id", id) > 0;
                }
                public bool SetDictateLimit(long id, ServerDictateLimit limit)
                {
                        return this.Update(limit, "Id", id);
                }
                public bool Query(long serverId, string dictate, IBasicPage paging, out ServerDictateLimitData[] limits, out long count)
                {
                        List<ISqlWhere> wheres = new List<ISqlWhere>() {
                                new SqlWhere("ServerId", SqlDbType.BigInt){Value=serverId}
                        };
                        if (!dictate.IsNull())
                        {
                                wheres.Add(new SqlWhere("Dictate", SqlDbType.VarChar, 50) { Value = dictate });
                        }
                        return this.Query("Id desc", paging.Index, paging.Size, out limits, out count, wheres.ToArray());
                }
        }
}
