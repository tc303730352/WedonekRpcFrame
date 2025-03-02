using WeDonekRpc.Client;
using WeDonekRpc.Model;
using RpcStore.RemoteModel.Error.Model;

namespace RpcStore.Service.Interface
{
    public interface IErrorService
    {
        ErrorDatum GetError (string code);
        PagingResult<ErrorData> QueryError (ErrorQuery query, IBasicPage paging);
        void SetErrorMsg (ErrorSet set);
        long SyncError (ErrorDatum add);
    }
}