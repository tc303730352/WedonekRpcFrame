using RpcSync.Service.Model;

namespace RpcSync.Service.Interface
{
    public interface IAccreditKeyCollect
    {
        string KickOut(string checkKey);
        void Renewal(IAccreditToken token, TimeSpan time);
        string Set(ref SyncAccredit obj, TimeSpan time);
        bool TryRemove(string checkKey, string accreditId);
        bool Check(string checkKey, string accreditId, out int stateVer);
    }
}