namespace RpcManageClient
{
        public class RpcManageClient
        {
                public static void Init()
                {
                        RpcClient.RpcClient.Unity.Register(typeof(IRpcServerCollect), typeof(Collect.ServerCollect));
                }
        }
}
