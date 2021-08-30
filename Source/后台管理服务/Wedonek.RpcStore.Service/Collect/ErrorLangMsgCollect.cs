using RpcHelper;

using Wedonek.RpcStore.Service.DAL;
using Wedonek.RpcStore.Service.Interface;
using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Service.Collect
{
        internal class ErrorLangMsgCollect : BasicCollect<ErrorLangMsgDAL>, IErrorLangMsgCollect
        {
                public ErrorLang[] GetErrorMsg(long[] errorId)
                {
                        if (!this.BasicDAL.GetErrorMsg(errorId, out ErrorLang[] langs))
                        {
                                throw new ErrorException("rpc.error.lang.get.error");
                        }
                        return langs;
                }
                private long _FindErrorLangId(long errorId, string lang)
                {
                        if (!this.BasicDAL.GetErrorLangId(errorId, lang, out long id))
                        {
                                throw new ErrorException("rpc.error.lang.get.error");
                        }
                        return id;
                }
                private void _SetErrorMsg(long id, string msg)
                {
                        if (!this.BasicDAL.SetErrorMsg(id, msg))
                        {
                                throw new ErrorException("rpc.error.lang.set.error");
                        }
                }
                public void SyncError(long errorId, string lang, string msg)
                {
                        long id = this._FindErrorLangId(errorId, lang);
                        if (id != 0)
                        {
                                this._SetErrorMsg(id, msg);
                                return;
                        }
                        else if (!this.BasicDAL.AddErrorMsg(new Model.ErrorLangAddParam
                        {
                                ErrorId = errorId,
                                Lang = lang,
                                Msg = msg
                        }))
                        {
                                throw new ErrorException("rpc.error.lang.add.error");
                        }
                }
                public string GetErrorMsg(long errorId, string lang)
                {
                        if (!this.BasicDAL.GetErrorMsg(errorId, lang, out string msg))
                        {
                                throw new ErrorException("rpc.error.lang.get.error");
                        }
                        return msg;
                }
        }
}
