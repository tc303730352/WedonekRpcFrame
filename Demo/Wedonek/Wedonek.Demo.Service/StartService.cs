using RpcClient.Model;

namespace Wedonek.Demo.Service
{
        public class StartService
        {
                public static void Start()
                {
                        RpcClient.RpcClient.RpcEvent = new RpcEvent();
                        RpcClient.RpcClient.Start();
                        RpcClient.RpcClient.Load("Wedonek.Demo.Service");
                }
        }
}
