using RpcSync.Service.Accredit;

namespace RpcSync.Service.Interface
{
    public interface ISyncAccreditQueue
    {
        void Add (AccreditToken token);
    }
}