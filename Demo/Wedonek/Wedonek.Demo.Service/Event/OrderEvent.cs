using Wedonek.Demo.RemoteModel.Order;
using Wedonek.Demo.Service.Interface;
using Wedonek.Demo.Service.Model;
using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.SqlSugarDbTran.Attr;

namespace Wedonek.Demo.Service.Event
{
    internal class OrderEvent : IRpcApiService
    {

        private readonly IOrderService _Service;

        public OrderEvent ( IOrderService service )
        {
            this._Service = service;
        }
        /// <summary>
        /// 新增DB事务订单
        /// </summary>
        /// <param name="add"></param>
        /// <returns></returns>
        [RpcDbTransaction]
        public long AddDbOrder ( AddDbOrder add )
        {
            OrderAddModel data = add.ConvertMap<AddDbOrder, OrderAddModel>();
            return this._Service.AddOrder(data);
        }
    }
}
