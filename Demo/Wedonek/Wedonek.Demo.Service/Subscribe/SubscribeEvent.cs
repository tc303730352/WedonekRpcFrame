using RpcClient;
using RpcClient.Interface;
using RpcHelper;
using Wedonek.Demo.RemoteModel.Order.Model;
using Wedonek.Demo.RemoteModel.Order.Msg;
using Wedonek.Demo.RemoteModel.Subscribe;
using Wedonek.Demo.Service.Interface;
using Wedonek.Demo.Service.Model;

namespace Wedonek.Demo.Service.Subscribe
{
        /// <summary>
        /// 订阅演示
        /// </summary>
        internal class SubscribeEvent : IRpcSubscribeService
        {
                private readonly IOrderService _Order = null;

                public SubscribeEvent(IOrderService order)
                {
                        this._Order = order;
                }
                /// <summary>
                /// 添加订单事件
                /// </summary>
                /// <param name="add"></param>
                public void AddOrderMsg(AddOrderMsg add)
                {
                        OrderInfo order = add.Order.ConvertMap<OrderData, OrderInfo>();
                        this._Order.AddOrder(order);
                }

                /// <summary>
                /// 订阅用户上线事件
                /// </summary>
                /// <param name="obj"></param>
                public void UserGoOnline(UserGoOnline obj)
                {
                        System.Console.WriteLine("用户上线了! ");
                        System.Console.WriteLine(string.Concat("user:", obj.ToJson()));
                }
        }
}
