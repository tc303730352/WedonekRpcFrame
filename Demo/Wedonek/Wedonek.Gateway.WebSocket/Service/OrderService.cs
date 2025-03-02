using Wedonek.Demo.RemoteModel.DBOrderLog;
using Wedonek.Demo.RemoteModel.Msg;
using Wedonek.Demo.RemoteModel.Order;
using Wedonek.Demo.RemoteModel.User;
using Wedonek.Gateway.WebSocket.Interface;
using Wedonek.Gateway.WebSocket.Model;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Tran;
using WeDonekRpc.Helper;
namespace Wedonek.Gateway.WebSocket.Service
{
    /// <summary>
    /// 订单接口
    /// </summary> 
    internal class OrderService : IOrderService
    {
        /// <summary>
        /// 添加订单
        /// </summary>
        /// <param name="add">订单信息</param>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
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
