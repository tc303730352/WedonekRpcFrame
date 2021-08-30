using RpcModel;

namespace Wedonek.Demo.RemoteModel.Msg
{
        /// <summary>
        /// 用户下单事件广播(演示使用消息队列广播消息)
        /// </summary>
        [IRemoteBroadcast(false, "demo", BroadcastType = BroadcastType.消息)]
        public class PlaceAnOrder : RpcClient.RpcBroadcast
        {
                public long UserId
                {
                        get;
                        set;
                }
                public string OrderNo
                {
                        get;
                        set;
                }
        }
}
