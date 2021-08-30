using Wedonek.Demo.RemoteModel.User.Model;

namespace Wedonek.Gateway.WebSocket.Interface
{
        internal interface IUserService
        {
                UserDatum GetUser(UserLoginState state);
                int SendMsg(string text);
        }
}