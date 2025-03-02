using System;
using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.HttpApiGateway.Interface;

namespace WeDonekRpc.HttpApiGateway.Collect
{
    internal class UnityCollect
    {
        private static readonly IIocService _Unity = RpcClient.Ioc;

        public static IApiGateway GetGateway (Type type)
        {
            return (IApiGateway)_Unity.Resolve(PublicDict.IGatewayType, type.FullName);
        }
    }
}
