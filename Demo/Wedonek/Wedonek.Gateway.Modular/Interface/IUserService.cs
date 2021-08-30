using Wedonek.Demo.RemoteModel.User.Model;
using Wedonek.Gateway.Modular.Model;

namespace Wedonek.Gateway.Modular.Interface
{
        internal interface IUserService
        {
                void KickOutUser(long userId);
                UserDatum GetUser(UserLoginState state);
                long Reg(UserRegParam reg);
                string UserLogin(string phone);
                string PressureTest();
        }
}