using ApiGateway.Interface;
using RpcModel;
using System;
using Wedonek.Demo.RemoteModel.Order.Model;
using Wedonek.Gateway.WebSocket.Model;

namespace Wedonek.Gateway.WebSocket.Interface
{
        internal interface IOrderService
        {
                Guid AddOrder(UserLoginState state, OrderParam add,IClientIdentity identity);
                void DropOrder(Guid orderId);
                OrderData GetOrder(Guid orderId);
                OrderData[] Query(UserLoginState state, BasicPage paging, out long count);
        }
}