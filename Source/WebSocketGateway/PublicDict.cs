using System;

using HttpWebSocket.Model;

using RpcModular;

using WebSocketGateway.Interface;

namespace WebSocketGateway
{
        internal class PublicDict
        {
                public static readonly Type IGatewayType = typeof(IApiGateway);
                public static readonly Type ISession = typeof(ISession);
                public static readonly Type IUserStateType = typeof(IUserState);
                public static readonly Type RequestBody = typeof(RequestBody);
                public static readonly Type IApiSocketService = typeof(IApiSocketService);
                public static readonly Type ICurrentModular = typeof(ICurrentModular);
                public static readonly Type IApiType = typeof(IApiController);
        }
}
