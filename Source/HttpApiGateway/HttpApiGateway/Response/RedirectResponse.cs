using System;

using HttpApiGateway.Interface;

namespace HttpApiGateway.Response
{
        public sealed class RedirectResponse : IResponse
        {
                private readonly Uri _JumpUri = null;
                public RedirectResponse(Uri uri)
                {
                        this._JumpUri = uri;
                }
                public RedirectResponse(string uri)
                {
                        this._JumpUri = new Uri(uri);
                }
                public bool Verification(IApiService route)
                {
                        return true;
                }
                public void InitResponse(IApiService service)
                {
                }

                public void WriteStream(IApiService route)
                {
                        route.Response.Redirect(this._JumpUri);
                }

        }
}
