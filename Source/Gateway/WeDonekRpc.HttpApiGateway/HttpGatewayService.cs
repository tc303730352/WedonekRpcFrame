using WeDonekRpc.Client.Attr;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.HttpApiGateway.Interface;
namespace WeDonekRpc.HttpApiGateway
{
    [IocName("HttpGatewayService")]
    internal class HttpGatewayService : IRpcExtendService
    {
        private readonly IIdempotentService _Idempotent;
        private readonly IUrlRewriteService _UriRewrite;
        public HttpGatewayService (IIdempotentService idempotent, IUrlRewriteService urlRewrite)
        {
            this._UriRewrite = urlRewrite;
            this._Idempotent = idempotent;
        }

        public void Load (IRpcService service)
        {
            service.StartUpComplating += this.Service_StartUpComplating;
            service.StartUpComplate += this.Service_StartUpComplate;
        }

        private void Service_StartUpComplating ()
        {
            this._UriRewrite.Init();
        }

        private void Service_StartUpComplate ()
        {
            this._Idempotent.Init();
        }
    }
}
