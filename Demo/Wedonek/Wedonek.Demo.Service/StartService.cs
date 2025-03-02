using WeDonekRpc.Client;

namespace Wedonek.Demo.Service
{
    public class StartService
    {
        public static void Start ()
        {
            RpcClient.RpcEvent = new RpcEvent();
            RpcClient.Start();
        }
    }
}
