using System;

using HttpApiGateway.Interface;

namespace HttpApiGateway
{
        internal class PublicDict
        {
                public static readonly Type IApiType = typeof(IApiController);

                public static readonly Type IGatewayType = typeof(IApiGateway);

                public static readonly Type ResponseType = typeof(IResponse);
        }
}
