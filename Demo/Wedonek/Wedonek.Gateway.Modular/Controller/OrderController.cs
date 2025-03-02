using Wedonek.Gateway.Modular.Interface;
using Wedonek.Gateway.Modular.Model;
using WeDonekRpc.HttpApiGateway;

namespace Wedonek.Gateway.Modular.Controller
{
    /// <summary>
    /// 订单
    /// </summary>
    internal class OrderController : ApiController
    {
        private readonly IOrderService _Service;

        public OrderController ( IOrderService service )
        {
            this._Service = service;
        }

        public long AddOrder ( OrderParam add )
        {
            return this._Service.AddOrder(add, base.UserState.GetValue<long>("UserId"));
        }

    }
}
