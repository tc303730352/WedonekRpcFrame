using Wedonek.Demo.RemoteModel.User.Model;

namespace Wedonek.Demo.User.Service.Interface
{
    public interface IUserService
    {
        void SetOrderNo (long userId, string orderNo);
        void TryLockNum (long userId, int num);

        void SubmitNum (long userId, int num);
        void RollbackNum (long userId, int num);
        long AddUser (string name, string phone);
        UserDatum GetUser (long userId);
        long UserLogin (string phone);
        void SetOrderNo (long userId, string orderNo, string old);
    }
}