
using RpcClient.Interface;

using Wedonek.Demo.Service.LocalEvent.Event;

namespace Wedonek.Demo.Service.LocalEvent
{
        internal class LocalAddOrderEvent : IEventHandler<AddOrderEvent>
        {
                public void HandleEvent(AddOrderEvent eventData, string eventName)
                {
                        System.Console.WriteLine("LocalAddOrderEvent收到添加订单事件!");
                }
        }
}
