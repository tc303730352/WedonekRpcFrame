using Wedonek.Demo.RemoteModel.User;
using Wedonek.Demo.RemoteModel.User.Model;
using Wedonek.Demo.Service.Interface;

namespace Wedonek.Demo.Service.Service
{
    internal class UserService : IUserService
    {
        public UserDatum GetUser (long userId)
        {
            return new GetUser { UserId = userId }.Send();
        }

        public void SetUserOrderNo (long userId, string orderNo)
        {
            new SetOrderNo
            {
                OrderNo = orderNo,
                UserId = userId
            }.Send();
        }
    }
}
