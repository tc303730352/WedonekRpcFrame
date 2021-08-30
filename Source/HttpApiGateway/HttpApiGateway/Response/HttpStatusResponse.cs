using System.Net;

using HttpApiGateway.Interface;

namespace HttpApiGateway.Response
{
        public sealed class HttpStatusResponse : IResponse
        {
                private readonly HttpStatusCode _StatusCode = HttpStatusCode.OK;
                public HttpStatusResponse(HttpStatusCode code)
                {
                        this._StatusCode = code;
                }
                public bool Verification(IApiService route)
                {
                        return true;
                }
                public void InitResponse(IApiService service)
                {
                        service.Response.SetHttpStatus(this._StatusCode);
                }

                public void WriteStream(IApiService route)
                {
                }

        }
}
