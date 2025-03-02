using WeDonekRpc.HttpApiGateway.Collect;

namespace WeDonekRpc.HttpApiGateway
{
    public class GatewayModular : BasicApiModular
    {
        public static string ModularName = "Http_Gateway";
        public GatewayModular () : base("Http_Gateway", "网关服务")
        {

        }

        protected override void Init ()
        {
            BlockUpCollect.Init(this);
            base.Init();
        }
    }
}
