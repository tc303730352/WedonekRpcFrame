
using RpcClient.Interface;

using Wedonek.Demo.Service.LocalEvent.Event;

namespace Wedonek.Demo.Service.LocalEvent
{
        public class UserAddOrder : IEventHandler<AddOrderEvent>
        {
                public void HandleEvent(AddOrderEvent eventData, string eventName)
                {
                        System.Console.WriteLine("UserAddOrder收到添加订单事件!");
                }
        }
}
