using RpcModel;

namespace Wedonek.Demo.RemoteModel.User
{
        [IRemoteConfig("demo.user")]
        public class GetUser : RpcClient.RpcRemote<Model.UserDatum>
        {
                public long UserId
                {
                        get;
                        set;
                }
        }
}
