using ApiGateway;
using ApiGateway.Interface;

using HttpApiDocHelper;

using Wedonek.Gateway.Modular;
using Wedonek.Gateway.WebSocket;

namespace Wedonek.Gateway
{
        internal class Global : BasicGlobal
        {
                public override void ServiceStarting(IGatewayService service)
                {
                        //注册文档模块
                        service.RegDoc(new ApiDocModular());
                }
                public override void LoadModular(IGatewayService service)
                {
                        //注册网关模块
                        service.RegModular(new GatewayModular());
                        service.RegModular(new WebSocketGatewayModular());
                }
        }
}