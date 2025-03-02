using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Modular;

namespace WeDonekRpc.ApiGateway
{
    internal class GatewayExtendService : IRpcInitModular
    {
        public void Init (IIocService ioc)
        {

        }

        public void InitEnd (IIocService ioc, IRpcService service)
        {
        }

        public void Load (RpcInitOption option)
        {
            option.LoadModular<ExtendModular>();
        }
    }
}
