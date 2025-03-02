using System;
using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.WebSocketGateway.Interface;

namespace WeDonekRpc.WebSocketGateway.Collect
{
    internal class UnityCollect
    {
        private static readonly IIocService ioc = RpcClient.Ioc;

        public static IApiGateway GetGateway (Type type)
        {
            return (IApiGateway)ioc.Resolve(PublicDict.IGatewayType, type.FullName);
        }
    }
}
