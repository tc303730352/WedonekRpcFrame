using WeDonekRpc.Model;
using RpcStore.Model.DB;
using RpcStore.RemoteModel.Error.Model;

namespace RpcStore.DAL
{
    public interface IErrorCollectDAL
    {
        void Add(ErrorCollectModel add);
        ErrorCollectModel Find(string code);
        ErrorCollectModel Get(long id);
        Dictionary<long, string> GetErrorCode (long[] errorId);
        ErrorCollectModel[] Gets(long[] ids);
        ErrorCollectModel[] Query(ErrorQuery query, IBasicPage paging, out int count);
        void SetIsPerfect(long id);
    }
}