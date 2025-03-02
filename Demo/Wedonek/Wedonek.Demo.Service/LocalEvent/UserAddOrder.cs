using WeDonekRpc.Client.Interface;

namespace Wedonek.Demo.Service.LocalEvent
{
    /// <summary>
    /// 本地事件-收到添加订单事件
    /// </summary>
    [WeDonekRpc.Client.Attr.LocalEventName("Add")]
    public class UserAddOrder : IEventHandler<Event.LocalOrderEvent>
    {
        public void HandleEvent ( Event.LocalOrderEvent eventData, string eventName )
        {
            System.Console.WriteLine("SerialNo：" + eventData.SerialNo);
            System.Console.WriteLine("UserAddOrder收到添加订单事件!");
        }
    }
}
