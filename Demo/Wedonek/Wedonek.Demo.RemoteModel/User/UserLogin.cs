using RpcModel;

namespace Wedonek.Demo.RemoteModel.User
{
        [IRemoteConfig("demo.user")]
        public  class UserLogin : RpcClient.RpcRemote<long>
        {
                public string Phone
                {
                        get;
                        set;
                }
        }
}
