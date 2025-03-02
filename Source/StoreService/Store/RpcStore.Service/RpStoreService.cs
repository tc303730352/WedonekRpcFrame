using WeDonekRpc.Client;
using WeDonekRpc.Modular;

namespace RpcStore.Service
{
    public class RpcStoreService
    {
        public static void Start ()
        {
            RpcClient.Start((option) =>
            {
                option.LoadModular<ExtendModular>();
                option.Load("RpcStore.Service");
            });
        }
    }
}
