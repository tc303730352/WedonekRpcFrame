using RpcStore.Collect;
using RpcStore.Model.DB;
using RpcStore.RemoteModel.Error.Model;
using RpcStore.Service.Interface;
using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace RpcStore.Service.lmpl
{
    internal class ErrorService : IErrorService
    {
        private readonly IErrorCollect _Error;
        private readonly WeDonekRpc.Client.Interface.IErrorService _ErrorService;
        private readonly IErrorLangMsgCollect _ErrorLang;

        public ErrorService (IErrorLangMsgCollect lang,
            WeDonekRpc.Client.Interface.IErrorService errorService,
            IErrorCollect error)
        {
            this._ErrorService = errorService;
            this._Error = error;
            this._ErrorLang = lang;
        }
        public PagingResult<ErrorData> QueryError (ErrorQuery query, IBasicPage paging)
        {
            ErrorCollectModel[] errors = this._Error.Query(query, paging, out int count);
            if (errors == null)
            {
                return new PagingResult<ErrorData>();
            }
            long[] ids = errors.Convert(a => a.IsPerfect, a => a.Id);
            if (ids.Length == 0)
            {
                ErrorData[] list = errors.ConvertMap<ErrorCollectModel, ErrorData>();
                return new PagingResult<ErrorData>(list, count);
            }
            else
            {
                ErrorLangMsgModel[] langs = this._ErrorLang.GetErrorMsg(ids);
                ErrorData[] list = errors.ConvertMap<ErrorCollectModel, ErrorData>((a, b) =>
                 {
                     b.Zh = langs.Find(c => c.ErrorId == a.Id && c.Lang == "zh", c => c.Msg);
                     b.En = langs.Find(c => c.ErrorId == a.Id && c.Lang == "en", c => c.Msg);
                     return b;
                 });
                return new PagingResult<ErrorData>(list, count);
            }
        }
        public long SyncError (ErrorDatum add)
        {
            return this._Error.SyncError(add);
        }
        public void SetErrorMsg (ErrorSet set)
        {
            ErrorCollectModel error = this._Error.SetErrorMsg(set.ErrorId, set.Lang, set.Msg);
            if (error != null)
            {
                this._ErrorService.Reset(error.Id, error.ErrorCode);
            }
        }
        public ErrorDatum GetError (string code)
        {
            return this._Error.FindError(code);
        }
    }
}
