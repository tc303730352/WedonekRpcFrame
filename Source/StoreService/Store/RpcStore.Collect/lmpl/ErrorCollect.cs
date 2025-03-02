using RpcStore.DAL;
using RpcStore.Model.DB;
using RpcStore.RemoteModel.Error.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace RpcStore.Collect.lmpl
{
    internal class ErrorCollect : IErrorCollect
    {
        private readonly IErrorCollectDAL _BasicDAL;
        private readonly IErrorLangMsgDAL _ErrorLang;
        public ErrorCollect (IErrorCollectDAL basicDAL, IErrorLangMsgDAL errorLang)
        {
            this._BasicDAL = basicDAL;
            this._ErrorLang = errorLang;
        }

        public ErrorCollectModel[] Query (ErrorQuery query, IBasicPage paging, out int count)
        {
            return this._BasicDAL.Query(query, paging, out count);
        }
        public ErrorCollectModel SetErrorMsg (long errorId, string lang, string msg)
        {
            ErrorCollectModel error = this.GetError(errorId);
            if (this._SetErrorMsg(error, lang, msg))
            {
                return error;
            }
            return null;
        }
        private bool _SetErrorMsg (ErrorCollectModel error, string lang, string msg)
        {
            if (error.IsPerfect)
            {
                ErrorLangMsgModel errorLang = this._ErrorLang.GetErrorMsg(error.Id, lang);
                if (errorLang != null && errorLang.Msg != msg)
                {
                    this._ErrorLang.SetErrorMsg(errorLang.Id, msg);
                    return true;
                }
                return false;
            }
            this._ErrorLang.AddErrorMsg(new ErrorLangMsgModel
            {
                ErrorId = error.Id,
                Lang = lang,
                Msg = msg
            });
            return true;
        }
        public long SyncError (ErrorDatum add)
        {
            ErrorCollectModel error = this._SyncError(add.ErrorCode);
            if (add.LangMsg.Count == 0 && error.IsPerfect)
            {
                this._ErrorLang.Delete(error.Id);
            }
            else if (add.LangMsg.Count != 0)
            {
                if (error.IsPerfect)
                {
                    this._ErrorLang.SyncErrorMsg(error.Id, add.LangMsg);
                }
                else
                {
                    this._ErrorLang.AddErrorMsg(error.Id, add.LangMsg);
                }
            }
            return error.Id;
        }

        public Dictionary<long, string> GetErrorCode (long[] errorId)
        {
            return this._BasicDAL.GetErrorCode(errorId);
        }
        public ErrorCollectModel GetError (long id)
        {
            ErrorCollectModel error = this._BasicDAL.Get(id);
            if (error == null)
            {
                throw new ErrorException("rpc.store.error.not.find");
            }
            return error;
        }
        private ErrorCollectModel _AddError (string code)
        {
            ErrorCollectModel add = new ErrorCollectModel
            {
                ErrorCode = code,
                IsPerfect = false
            };
            this._BasicDAL.Add(add);
            return add;
        }
        public ErrorCollectModel _SyncError (string code)
        {
            ErrorCollectModel error = this._BasicDAL.Find(code);
            if (error != null)
            {
                return error;
            }
            return this._AddError(code);
        }
        public ErrorDatum FindError (string code)
        {
            ErrorCollectModel error = this._BasicDAL.Find(code);
            if (error != null && error.IsPerfect)
            {
                ErrorLangMsgModel[] msgs = this._ErrorLang.GetErrorMsg(error.Id);
                return new ErrorDatum
                {
                    ErrorCode = code,
                    LangMsg = msgs.ToDictionary(c => c.Lang, c => c.Msg)
                };
            }
            return new ErrorDatum
            {
                ErrorCode = code
            };
        }
    }
}
