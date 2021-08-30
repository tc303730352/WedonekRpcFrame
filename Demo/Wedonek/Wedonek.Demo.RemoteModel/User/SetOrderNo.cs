using RpcModel;

namespace Wedonek.Demo.RemoteModel.User
{
        [IRemoteConfig("demo.user")]
        public class SetOrderNo : RpcClient.RpcRemote
        {
                /// <summary>
                /// 用户名
                /// </summary>
                public long UserId
                {
                        get;
                        set;
                }
                /// <summary>
                /// 订单号
                /// </summary>
                public string OrderNo
                {
                        get;
                        set;
                }
        }
}
