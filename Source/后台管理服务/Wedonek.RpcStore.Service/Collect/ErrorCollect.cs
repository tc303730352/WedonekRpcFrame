using System.Transactions;

using RpcHelper;

using RpcModel;

using Wedonek.RpcStore.Service.DAL;
using Wedonek.RpcStore.Service.Interface;
using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Service.Collect
{
        internal class ErrorCollect : BasicCollect<ErrorCollectDAL>, IErrorCollect
        {
                public ErrorDatum[] QueryError(string code, IBasicPage paging, out long count)
                {
                        if (!this.BasicDAL.QueryError(code, paging, out ErrorDatum[] errors, out count))
                        {
                                throw new ErrorException("rpc.error.query.error");
                        }
                        return errors;
                }
                public long SetErrorMsg(long errorId, string lang, string msg)
                {
                        ErrorDatum error = this.GetError(errorId);
                        IErrorLangMsgCollect msgCollect = this.GetCollect<IErrorLangMsgCollect>();
                        using (TransactionScope tran = new TransactionScope())
                        {
                                msgCollect.SyncError(errorId, lang, msg);
                                if (!this.BasicDAL.SetError(errorId))
                                {
                                        throw new ErrorException("rpc.error.set.error");
                                }
                                else
                                {
                                        tran.Complete();
                                }
                        }
                        RpcClient.RpcClient.Error.ResetError(errorId, error.ErrorCode);
                        return errorId;
                }

                public long SyncError(ErrorAddDatum add)
                {
                        long errorId = this._SyncError(add.ErrorCode);
                        IErrorLangMsgCollect lang = this.GetCollect<IErrorLangMsgCollect>();
                        using (TransactionScope tran = new TransactionScope())
                        {
                                lang.SyncError(errorId, add.Lang, add.Msg);
                                if (!this.BasicDAL.SetError(errorId))
                                {
                                        throw new ErrorException("rpc.error.set.error");
                                }
                                else
                                {
                                        tran.Complete();
                                }
                        }
                        RpcClient.RpcClient.Error.ResetError(errorId, add.ErrorCode);
                        return errorId;
                }


                public ErrorDatum GetError(long id)
                {
                        if (!this.BasicDAL.GetError(id, out ErrorDatum datum))
                        {
                                throw new ErrorException("rpc.error.get.error");
                        }
                        return datum;
                }
                private long _AddError(string code)
                {
                        if (!this.BasicDAL.AddError(code, out long errorId))
                        {
                                throw new ErrorException("rpc.error.add.error");
                        }
                        return errorId;
                }
                private long _SyncError(string code)
                {
                        if (!this.BasicDAL.FindErrorId(code, out long errorId))
                        {
                                throw new ErrorException("rpc.error.find.error");
                        }
                        else if (errorId != 0)
                        {
                                return errorId;
                        }
                        return this._AddError(code);
                }
        }
}
