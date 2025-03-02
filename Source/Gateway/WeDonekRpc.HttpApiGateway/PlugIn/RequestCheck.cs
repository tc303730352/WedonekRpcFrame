using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.HttpService.Interface;

namespace WeDonekRpc.HttpApiGateway.PlugIn
{
    internal class RequestCheck : IHttpPlugIn
    {
        public bool IsEnable => true;

        public string Name => "RequestCheck";

        public void Init()
        {

        }
        public void Exec(IRoute route, IHttpHandler handler)
        {
            if (!ApiGatewayService.Config.CheckContentLen(handler.Request.ContentLength))
            {
                handler.Response.SetHttpStatus(System.Net.HttpStatusCode.RequestEntityTooLarge);
                handler.Response.End();
            }
        }

        public void Dispose()
        {

        }
    }
}
