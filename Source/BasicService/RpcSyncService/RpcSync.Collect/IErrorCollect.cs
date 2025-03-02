using WeDonekRpc.Helper.Error;

namespace RpcSync.Collect
{
    public interface IErrorCollect
    {
        void Refresh(long errorId);
        ErrorMsg FindError(string code,string lang);
        string FindErrorCode(long errorId);

        long GetOrAdd(string code);
    }
}