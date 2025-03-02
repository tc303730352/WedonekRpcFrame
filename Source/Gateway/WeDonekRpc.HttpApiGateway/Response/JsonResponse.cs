using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.HttpApiGateway.Model;

namespace WeDonekRpc.HttpApiGateway.Response
{
    public class JsonResponse : IResponse
    {
        private readonly string _JsonStr = null;
        public JsonResponse(string json)
        {
            this._JsonStr = json;
        }

        public ResponseType ResponseType => ResponseType.JSON;

        public virtual void InitResponse(IApiService service)
        {
            service.Response.ContentType = "application/json";
        }

        public bool Verification(IApiService service)
        {
            return true;
        }

        public virtual void WriteStream(IApiService service)
        {
            service.Response.Write(this._JsonStr);
        }
    }
}
