using Wedonek.Demo.RemoteModel.Order.Model;
using WeDonekRpc.Client;
using WeDonekRpc.Model;

namespace Wedonek.Demo.RemoteModel.Order.Msg
{
    /// <summary>
    /// 发布订阅消息-添加订单(已订阅的方式推送)
    /// </summary>
    [IRemoteBroadcast("AddOrderMsg", false, "demo.order", BroadcastType = BroadcastType.订阅)]
    public class AddOrderMsg : RpcBroadcast
    {
        /// <summary>
        /// 订单信息
        /// </summary>
        public OrderData Order
        {
            get;
            set;
        }
    }
}
