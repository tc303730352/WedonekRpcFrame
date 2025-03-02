using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.HttpApiGateway.SysEvent;
using WeDonekRpc.HttpService.Interface;

namespace WeDonekRpc.HttpApiGateway
{
    internal class HttpApiGatewayModular : IRpcInitModular
    {
        public void Init ( IIocService ioc )
        {
        }

        public void Load ( RpcInitOption option )
        {
            _ = option.Ioc.RegisterInstance<IGatewayApiService>(GatewayApiService.Service);
            _ = option.Ioc.RegisterInstance<ICrossConfig>(HttpService.HttpService.Config.Cross);
            ApiGatewayService.Init();
            option.Load("ApiGateway");
        }
        public void InitEnd ( IIocService ioc, IRpcService service )
        {
            service.StartUpComplating += this.Service_StartUpComplating;
        }

        private void Service_StartUpComplating ()
        {
            RequestTimeService.Init(RpcClient.Ioc);
        }
    }
}
