using WeDonekRpc.Client.Attr;

namespace RpcSync.Service.Interface
{
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    public interface ICommitTranService : IDisposable
    {
        void AddQueue (long tranId);
    }
}