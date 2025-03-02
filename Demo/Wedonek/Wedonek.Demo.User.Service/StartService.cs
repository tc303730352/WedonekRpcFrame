using WeDonekRpc.Client;

namespace Wedonek.Demo.User.Service
{
    public class StartService
    {
        public static void Start ()
        {
            RpcClient.RpcEvent = new LocalRpcEvent();
            RpcClient.Start();
        }

    }
}
