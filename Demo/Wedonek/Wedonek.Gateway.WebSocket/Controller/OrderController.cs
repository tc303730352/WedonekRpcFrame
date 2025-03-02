using Wedonek.Gateway.WebSocket.Interface;
using Wedonek.Gateway.WebSocket.Model;
using WeDonekRpc.WebSocketGateway;

namespace Wedonek.Gateway.WebSocket.Controller
{
    /// <summary>
    /// WebSocket订单接口
    /// </summary>
    internal class OrderController : WebSocketController
    {
        private readonly IOrderService _Service;

        public OrderController ( IOrderService service )
        {
            this._Service = service;
        }

        /// <summary>
        /// 获取用户资料
        /// </summary>
        /// <returns>用户资料</returns>
        public long AddOrder ( OrderParam add )
        {
            return this._Service.AddOrder(add, base.UserState.GetValue<long>("UserId"));
        }
    }
}
