using RpcSync.Service.Accredit;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Model;
using WeDonekRpc.ModularModel.Accredit;
using WeDonekRpc.ModularModel.Accredit.Model;

namespace RpcSync.Service.Interface
{
    [ClassLifetimeAttr(ClassLifetimeType.Scope)]
    public interface IAccreditToken
    {
        AccreditToken Token { get; }
        string AccreditId { get; }
        string CheckKey { get; }

        bool Cancel ();
        bool CheckPrower (MsgSource source);
        void Create (AccreditToken token);
        bool Init (string accreditId);

        TimeSpan? Refresh ();
        bool Remove (out string[] subs);
        bool SetState (string state, long verNum);

        AccreditDatum Get ();
        void Set (SetAccredit obj);
    }
}