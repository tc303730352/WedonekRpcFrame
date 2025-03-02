using WeDonekRpc.Model;
using RpcStore.RemoteModel.Error;
using RpcStore.RemoteModel.Error.Model;
using Store.Gatewary.Modular.Interface;

namespace Store.Gatewary.Modular.Services
{
    internal class ErrorService : IErrorService
    {
        public ErrorDatum GetError (string code)
        {
            return new GetError
            {
                ErrorCode = code
            }.Send();
        }

        public ErrorData[] QueryError (ErrorQuery query, IBasicPage paging, out int count)
        {
            return new QueryError
            {
                Query = query,
                Index = paging.Index,
                Size = paging.Size,
                NextId = paging.NextId,
                SortName = paging.SortName,
                IsDesc = paging.IsDesc
            }.Send(out count);
        }

        public void SetErrorMsg (ErrorSet datum)
        {
            new SetErrorMsg
            {
                Datum = datum,
            }.Send();
        }

        public long SyncError (ErrorDatum datum)
        {
            return new SyncError
            {
                Datum = datum,
            }.Send();
        }

    }
}
