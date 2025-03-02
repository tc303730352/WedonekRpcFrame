using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using RpcStore.RemoteModel.Error;
using RpcStore.RemoteModel.Error.Model;
using IErrorService = RpcStore.Service.Interface.IErrorService;

namespace RpcStore.Service.Event
{
    /// <summary>
    /// 错误码管理
    /// </summary>
    internal class ErrorEvent : IRpcApiService
    {
        private readonly IErrorService _Service;
        public ErrorEvent (IErrorService service)
        {
            this._Service = service;
        }

        public ErrorDatum GetError (GetError obj)
        {
            return this._Service.GetError(obj.ErrorCode);
        }

        public PagingResult<ErrorData> QueryError (QueryError query)
        {
            return this._Service.QueryError(query.Query, query.ToBasicPage());
        }

        public void SetErrorMsg (SetErrorMsg set)
        {
            this._Service.SetErrorMsg(set.Datum);
        }

        public long SyncError (SyncError obj)
        {
            return this._Service.SyncError(obj.Datum);
        }
    }
}
