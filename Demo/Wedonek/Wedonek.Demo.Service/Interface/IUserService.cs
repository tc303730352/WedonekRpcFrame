using Wedonek.Demo.RemoteModel.User.Model;

namespace Wedonek.Demo.Service.Interface
{
        internal interface IUserService
        {
                UserDatum GetUser(long userId);
                void SetUserOrderNo(long userId, string orderNo);
        }
}