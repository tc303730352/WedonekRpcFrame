using RpcModel;

namespace Wedonek.Demo.RemoteModel.User
{
        [IRemoteConfig("demo.user")]
        public class AddOrderNum : RpcClient.RpcRemote
        {
                public long UserId
                {
                        get;
                        set;
                }
                public int OrderNum
                {
                        get;
                        set;
                }
        }
}
