using System.Data;

using RpcModel;

using SqlExecHelper;
using SqlExecHelper.SetColumn;
using SqlExecHelper.SqlValue;

using RpcHelper;

using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Service.DAL
{
        internal class ErrorCollectDAL : SqlBasicClass
        {
                public ErrorCollectDAL() : base("ErrorCollect")
                {

                }

                public bool QueryError(string code, IBasicPage paging, out ErrorDatum[] errors, out long count)
                {
                        ISqlWhere where = null;
                        if (!code.IsNull())
                        {
                                where = new LikeSqlWhere("ErrorCode", 50, LikeQueryType.右) { Value = code };
                        }
                        return this.Query("Id desc", paging.Index, paging.Size, out errors, out count, where);
                }
                public bool GetError(long id, out ErrorDatum error)
                {
                        return this.GetRow("Id", id, out error);
                }
                public bool FindErrorId(string code, out long errorId)
                {
                        return this.ExecuteScalar("Id", out errorId, new SqlWhere("ErrorCode", SqlDbType.VarChar, 50) { Value = code });
                }

                public bool SetError(long id)
                {
                        return this.Update(new ISqlSetColumn[] {
                                new SqlSetColumn("IsPerfect", SqlDbType.Bit){Value=1}
                        }, new ISqlWhere[] {
                                new SqlWhere("Id", SqlDbType.BigInt){Value=id}
                        });
                }

                public bool AddError(string errorCode, out long id)
                {
                        return this.Insert(new IInsertSqlValue[] {
                                new InsertSqlValue("ErrorCode", SqlDbType.VarChar,50){ Value=errorCode},
                                new InsertSqlValue("IsPerfect", SqlDbType.Bit){Value=0}
                        }, out id);
                }
        }
}
