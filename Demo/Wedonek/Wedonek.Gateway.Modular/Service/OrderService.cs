using Wedonek.Demo.RemoteModel.DBOrderLog;
using Wedonek.Demo.RemoteModel.Msg;
using Wedonek.Demo.RemoteModel.Order;
using Wedonek.Demo.RemoteModel.User;
using Wedonek.Gateway.Modular.Interface;
using Wedonek.Gateway.Modular.Model;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Tran;
using WeDonekRpc.Helper;

namespace Wedonek.Gateway.Modular.Service
{
    internal class OrderService : IOrderService
    {
        public long AddOrder ( OrderParam add, long userId )
        {
            string orderNo = Tools.GetSerialNo("DE01");
            long id;
            using ( IRpcTransaction tran = new RpcTransaction() )
            {
                //添加一个订单
                id = new AddDbOrder
                {
                    OrderNo = orderNo,
                    OrderPrice = add.OrderPrice,
                    OrderTitle = add.OrderTitle,
                    UserId = userId
                }.Send();
                long logId = new AddOrderLog
                {
                    OrderId = id,
                    OrderPrice = add.OrderPrice,
                    UserId = userId
                }.Send();
                new LockUserMoney
                {
                    Money = add.OrderPrice,
                    UserId = userId
                }.Send();
                new PlaceAnOrder
                {
                    OrderNo = orderNo,
                    UserId = userId
                }.Send();
                tran.Complate();//提交事务
            }
            return id;
        }
    }
}
