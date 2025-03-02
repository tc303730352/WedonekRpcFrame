using Wedonek.Demo.RemoteModel.User.Model;

namespace Wedonek.Demo.Service.Interface
{
    public interface IUserService
    {
        UserDatum GetUser (long userId);
        void SetUserOrderNo (long userId, string orderNo);
    }
}