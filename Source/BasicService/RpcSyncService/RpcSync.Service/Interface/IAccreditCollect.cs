using RpcSync.Service.Accredit;

namespace RpcSync.Service.Interface
{
    public interface IAccreditCollect
    {
        bool Get(string accreditId, out IAccreditToken token);
        void Accredit(AccreditToken token);
    }
}
