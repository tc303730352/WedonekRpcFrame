using RpcSync.Model;

namespace RpcSync.DAL
{
    public interface IErrorCodeDAL
    {
        string FindErrorCode(long errorId);
        long GetErroMaxId();
        ErrorDatum SyncError(string code);
    }
}