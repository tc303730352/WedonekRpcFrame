using Wedonek.Gateway.Modular;
using Wedonek.Gateway.WebSocket;
using WeDonekRpc.ApiGateway;
using WeDonekRpc.ApiGateway.Interface;
using WeDonekRpc.HttpApiDoc;

namespace Wedonek.Gateway
{
    internal class Global : BasicGlobal
    {
        public override void Load (IGatewayOption option)
        {
            option.RegDoc(new ApiDocModular());
            option.RegModular(new GatewayModular());
            option.RegModular(new WebSocketGatewayModular());
        }
        public override void ServiceStarting ()
        {
        }
    }
}