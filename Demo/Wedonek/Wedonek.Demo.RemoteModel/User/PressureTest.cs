
using RpcClient;

using RpcModel;

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
        }
}
