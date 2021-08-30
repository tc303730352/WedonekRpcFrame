using RpcModel;
using System;
using Wedonek.Demo.Service.Model;

namespace Wedonek.Demo.Service.Interface
{
        internal interface IOrderService
        {
                bool AddOrder(OrderAddModel order, out Guid orderId, out string error);
                void DropOrder(Guid id);
                OrderInfo GetOrder(Guid orderId);
                OrderInfo[] Query(long userId, IBasicPage paging, out long count);
                void AddOrder(OrderInfo order);
                bool DropOrder(string orderNo);
        }
}