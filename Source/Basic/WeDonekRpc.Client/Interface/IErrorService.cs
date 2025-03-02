using WeDonekRpc.Client.Controller;

namespace WeDonekRpc.Client.Interface
{
    public interface IErrorService
    {
        bool GetError(string code, out ErrorController error);
        void Drop(string code);
        void Refresh(long errorId);
        void Reset(long errorId, string code);
    }
}