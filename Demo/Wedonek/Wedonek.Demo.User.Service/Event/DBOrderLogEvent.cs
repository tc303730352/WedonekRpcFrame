using Wedonek.Demo.RemoteModel.DBOrderLog;
using Wedonek.Demo.User.Service.Interface;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.SqlSugarDbTran.Attr;

namespace Wedonek.Demo.User.Service.Event
{
    internal class DBOrderLogEvent : IRpcApiService
    {
        private readonly IDBOrderLogService _Service;

        public DBOrderLogEvent (IDBOrderLogService service)
        {
            this._Service = service;
        }
        [RpcDbTransaction]
        public long AddOrderLog (AddOrderLog order)
        {
            return this._Service.AddOrder(order);
        }
    }
}
