using WeDonekRpc.Client.Interface;

namespace Wedonek.Demo.Service.LocalEvent
{
    /// <summary>
    /// 本地事件-收到本地订单事件
    /// </summary>
    internal class LocalOrderEvent : IEventHandler<Event.LocalOrderEvent>
    {
        public void HandleEvent ( Event.LocalOrderEvent eventData, string eventName )
        {
            System.Console.WriteLine("EventName：" + eventName);
            System.Console.WriteLine("SerialNo：" + eventData.SerialNo);
            System.Console.WriteLine("LocalOrderEvent收到本地订单事件!");
        }
    }
}
