using Wedonek.RpcStore.Service;

namespace Wedonek.RpcStore.Gateway
{
        public class GatewayModular : HttpApiGateway.BasicApiModular
        {
                public GatewayModular() : base("rpc")
                {

                }
                protected override void Init()
                {
                        this.Config.RegUserState<UserLoginState>();
                        RpcStoreModular.InitModular();
                }
        }
}
