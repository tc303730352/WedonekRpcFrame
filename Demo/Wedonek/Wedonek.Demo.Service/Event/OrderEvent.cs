using RpcClient;
using System;
using Wedonek.Demo.RemoteModel.Order;
using Wedonek.Demo.RemoteModel.Order.Model;
using Wedonek.Demo.Service.Interface;
using Wedonek.Demo.Service.Model;

namespace Wedonek.Demo.Service.Event
{
        internal class OrderEvent : RpcClient.Interface.IRpcApiService
        {
                private readonly IOrderService _Order = null;

                public OrderEvent(IOrderService order)
                {
                        this._Order = order;
                }
                /// <summary>
                /// 添加订单
                /// </summary>
                /// <param name="add">订单实体</param>
                /// <param name="orderId">订单号</param>
                /// <param name="error">错误信息</param>
                /// <returns>是否添加成功</returns>
                public bool AddOrder(AddOrder add, out Guid orderId, out string error)
                {
                        OrderAddModel order = add.ConvertMap<AddOrder, OrderAddModel>();
                        return this._Order.AddOrder(order, out orderId, out error);
                }
                /// <summary>
                /// 删除订单
                /// </summary>
                /// <param name="drop"></param>
                public void DropOrder(DropOrder drop)
                {
                        this._Order.DropOrder(drop.OrderId);
                }
                /// <summary>
                /// 获取订单
                /// </summary>
                /// <param name="obj"></param>
                /// <returns></returns>
                public OrderData GetOrder(GetOrder obj)
                {
                        OrderInfo order = this._Order.GetOrder(obj.OrderId);
                        return order.ConvertMap<OrderInfo, OrderData>();
                }

                /// <summary>
                /// 查询订单
                /// </summary>
                /// <param name="obj"></param>
                /// <param name="count"></param>
                /// <returns></returns>
                public OrderData[] QueryOrder(QueryOrder obj, out long count)
                {
                        OrderInfo[] orders = this._Order.Query(obj.UserId, obj.ToBasicPage(), out count);
                        if (orders == null)
                        {
                                return null;
                        }
                        return orders.ConvertMap<OrderInfo, OrderData>();
                }
        }
}
