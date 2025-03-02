using WeDonekRpc.Model;
using WeDonekRpc.ModularModel.Accredit;
using WeDonekRpc.ModularModel.Accredit.Model;

namespace RpcSync.Service.Interface
{
    public interface IAccreditServer
    {
        AccreditDatum GetAccredit (string accreditId, MsgSource source);
        ApplyAccreditRes ApplyAccredit (ApplyAccredit apply, MsgSource source);
        void CancelAccredit (string accreditId, string checkKey);
        int CheckAccredit (CheckAccredit obj);
        void KickOutAccredit (string checkKey);
        SetUserStateRes SetUserState (SetUserState obj);
        ApplyAccreditRes ToUpdate (SetAccredit obj);
    }
}