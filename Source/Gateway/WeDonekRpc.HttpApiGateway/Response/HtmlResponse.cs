
using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.HttpApiGateway.Model;
using WeDonekRpc.HttpService.Model;

namespace WeDonekRpc.HttpApiGateway.Response
{
    public class HtmlResponse : IResponse
    {
        private readonly string _Html = null;
        private readonly HttpCacheSet _Cache;
        public HtmlResponse(string html)
        {
            this._Html = html;
        }
        public HtmlResponse(string html, HttpCacheSet cache)
        {
            this._Cache = cache;
            this._Html = html;
        }
        public ResponseType ResponseType => ResponseType.Html;

        public virtual void InitResponse(IApiService service)
        {
            if(this._Cache != null)
            {
                service.Response.SetCache(this._Cache);
            }
            service.Response.ContentType = "text/html";
        }

        public bool Verification(IApiService service)
        {
            return true;
        }

        public virtual void WriteStream(IApiService service)
        {
            service.Response.Write(this._Html);
        }
    }
}
