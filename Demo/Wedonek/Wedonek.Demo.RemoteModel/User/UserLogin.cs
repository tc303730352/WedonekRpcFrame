using WeDonekRpc.Client;
using WeDonekRpc.Model;
namespace Wedonek.Demo.RemoteModel.User
{
    [IRemoteConfig("demo.user")]
    public class UserLogin : RpcRemote<long>
    {
        public string Phone
        {
            get;
            set;
        }
    }
}
