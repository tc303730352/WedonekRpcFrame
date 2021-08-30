
using ApiGateway.Interface;

namespace ApiGateway
{
        internal class GatewayService : IGatewayService
        {
                public void RegDoc(IApiDocModular doc)
                {
                        GatewayServer.RegDoc(doc);
                }

                public void RegModular(IModular modular)
                {
                        GatewayServer.RegModular(modular);
                }
        }
}
