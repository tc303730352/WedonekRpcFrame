namespace Wedonek.Demo.User.Service
{
        public class StartService
        {
                public static void Start()
                {
                        RpcClient.RpcClient.RpcEvent = new RpcEvent();
                        RpcClient.RpcClient.Start();
                        RpcClient.RpcClient.Load("Wedonek.Demo.User.Service");
                }
        }
}
