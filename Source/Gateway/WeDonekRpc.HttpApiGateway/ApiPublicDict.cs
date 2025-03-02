using System;
using WeDonekRpc.HttpApiGateway.Interface;

namespace WeDonekRpc.HttpApiGateway
{
    internal class PublicDict
    {
        public static readonly Type IApiType = typeof(IApiController);

        public static readonly Type IGatewayType = typeof(IApiGateway);
        public static readonly Type IUpBlockFileType = typeof(IUpBlockFileTask);

        public static readonly Type ResponseType = typeof(IResponse);
    }
}
