using SqlExecHelper;
using SqlExecHelper.SetColumn;

using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Service.DAL
{
        internal class ErrorLangMsgDAL : SqlExecHelper.SqlBasicClass
        {
                public ErrorLangMsgDAL() : base("ErrorLangMsg")
                {

                }
                public bool GetErrorLangId(long errorId, string lang, out long id)
                {
                        return this.ExecuteScalar("Id", out id, new ISqlWhere[] {
                                new SqlWhere("ErrorId", System.Data.SqlDbType.BigInt){Value=errorId},
                                new SqlWhere("Lang", System.Data.SqlDbType.VarChar,20){Value=lang}
                        });
                }
                public bool GetErrorMsg(long[] errorId, out ErrorLang[] msgs)
                {
                        return this.Get("ErrorId", errorId, out msgs);
                }
                public bool GetErrorMsg(long errorId, out ErrorLang[] msgs)
                {
                        return this.Get("ErrorId", errorId, out msgs);
                }
                public bool DropError(long errorId)
                {
                        return this.Drop("ErrorId", errorId) != -2;
                }
                public bool SetErrorMsg(long id, string msg)
                {
                        return this.Update(new ISqlSetColumn[] {
                                new SqlSetColumn("Msg",  System.Data.SqlDbType.NVarChar,100){Value=msg}
                        }, "Id", id);
                }
                public bool AddErrorMsg(ErrorLangAddParam add)
                {
                        return this.Insert(add);
                }
                public bool AddErrorMsg(ErrorLangAddParam[] adds)
                {
                        return this.Insert(adds);
                }

                internal bool GetErrorMsg(long errorId, string lang, out string msg)
                {
                        return this.ExecuteScalar("Msg", out msg, new ISqlWhere[] {
                                new SqlWhere("ErrorId",  System.Data.SqlDbType.BigInt){Value=errorId},
                                new SqlWhere("Lang",  System.Data.SqlDbType.VarChar,20){Value=lang}
                        });
                }
        }
}
