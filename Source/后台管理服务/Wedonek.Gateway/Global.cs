using ApiGateway;
using ApiGateway.Interface;

using HttpApiDocHelper;

using Wedonek.RpcStore.Gateway;

namespace Wedonek.Gateway
{
        internal class Global : BasicGlobal
        {
                public override void LoadModular(IGatewayService service)
                {
                        service.RegModular(new GatewayModular());

                }
                public override void ServiceStarting(IGatewayService service)
                {
                        //注册文档模块
                        service.RegDoc(new ApiDocModular());
                }
        }
}
