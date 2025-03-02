using System.Threading.Tasks;
using Wedonek.Demo.RemoteModel.User.Model;
using WeDonekRpc.Modular;

namespace Wedonek.Gateway.WebSocket.Interface
{
    public interface IUserService
    {
        Task<UserDatum> GetUser (IUserState state);
        int SendMsg (string text);
    }
}