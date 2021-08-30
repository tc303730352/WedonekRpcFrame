using Wedonek.Demo.RemoteModel.User.Model;

namespace Wedonek.Demo.User.Service.Interface
{
        internal interface IUserService
        {
                void SetOrderNo(long userId, string orderNo);
                void AddOrderNum(long userId, int num);
                long AddUser(string name, string phone);
                UserDatum GetUser(long userId);
                long UserLogin(string phone);
                void SetOrderNo(long userId, string orderNo, string old);
        }
}