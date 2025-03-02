using WeDonekRpc.Client;
using WeDonekRpc.Model;

namespace Wedonek.Demo.RemoteModel.User
{
    [IRemoteConfig("demo.user")]
    public class PressureTest : RpcRemote<int>
    {
        public int Num
        {
            get;
            set;
        }
        public int Time { get; set; }
    }
}
