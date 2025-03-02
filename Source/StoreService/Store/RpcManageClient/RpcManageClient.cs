using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;

namespace RpcManageClient
{
    public class RpcManageClient : IRpcInitModular
    {

        public void Init (IIocService ioc)
        {

        }

        public void InitEnd (IIocService ioc, IRpcService service)
        {
        }

        public void Load (RpcInitOption option)
        {
            _ = option.Ioc.Register(typeof(IRpcServerCollect), typeof(Collect.ServerCollect));
        }
    }
}
