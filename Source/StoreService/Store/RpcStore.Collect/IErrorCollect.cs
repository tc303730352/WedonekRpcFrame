using WeDonekRpc.Model;
using RpcStore.Model.DB;
using RpcStore.RemoteModel.Error.Model;

namespace RpcStore.Collect
{
    public interface IErrorCollect
    {
        Dictionary<long, string> GetErrorCode (long[] errorId);
        ErrorDatum FindError (string code);
        ErrorCollectModel GetError (long id);
        ErrorCollectModel[] Query (ErrorQuery query, IBasicPage paging, out int count);
        ErrorCollectModel SetErrorMsg (long errorId, string lang, string msg);
        long SyncError (ErrorDatum add);
    }
}