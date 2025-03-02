using WeDonekRpc.Client;

namespace Wedonek.Demo.Service.LocalEvent.Event
{
    /// <summary>
    /// 本地事件-本地订单消息
    /// </summary>
    public class LocalOrderEvent : RpcLocalEvent
    {
        public long UserId
        {
            get;
            set;
        }
        public string SerialNo
        {
            get;
            set;
        }
    }
}
