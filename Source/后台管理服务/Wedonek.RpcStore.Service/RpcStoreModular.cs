namespace Wedonek.RpcStore.Service
{
        public class RpcStoreModular
        {
                public static void InitModular()
                {
                        RpcClient.RpcClient.Unity.Load("Wedonek.RpcStore.Service");
                        RpcManageClient.RpcManageClient.Init();
                }
        }
}
