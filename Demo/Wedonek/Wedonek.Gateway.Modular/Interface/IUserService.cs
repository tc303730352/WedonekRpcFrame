using Wedonek.Demo.RemoteModel.User.Model;
using Wedonek.Gateway.Modular.Model;
using WeDonekRpc.Modular;

namespace Wedonek.Gateway.Modular.Interface
{
    public interface IUserService
    {
        IUserState GetAccredit (string accreditId);
        void KickOutUser (long userId);
        UserDatum GetUser (IUserState state);
        long Reg (UserRegParam reg);
        string UserLogin (string phone);
    }
}