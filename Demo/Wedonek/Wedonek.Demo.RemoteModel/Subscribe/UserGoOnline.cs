using RpcModel;

namespace Wedonek.Demo.RemoteModel.Subscribe
{
        /// <summary>
        /// 用户上线事件(演示订阅的接收方法)
        /// </summary>
        [IRemoteBroadcast(true, "demo", BroadcastType = BroadcastType.订阅)]
        public class UserGoOnline : RpcClient.RpcBroadcast
        {
                public long UserId
                {
                        get;
                        set;
                }

                public string Phone
                {
                        get;
                        set;
                }
        }
}
