using WeDonekRpc.Client;
using WeDonekRpc.Model;
namespace Wedonek.Demo.RemoteModel.User
{
    [IRemoteConfig("demo.user")]
    public class GetUser : RpcRemote<Model.UserDatum>
    {
        public long UserId
        {
            get;
            set;
        }
    }
}
