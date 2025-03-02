using Wedonek.Demo.RemoteModel.DBOrderLog;

namespace Wedonek.Demo.User.Service.Interface
{
    public interface IDBOrderLogService
    {
        long AddOrder (AddOrderLog order);
    }
}