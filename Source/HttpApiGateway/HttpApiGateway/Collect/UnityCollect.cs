using System;

using HttpApiGateway.Interface;

using RpcClient.Interface;

namespace HttpApiGateway.Collect
{
        internal class UnityCollect
        {
                private static readonly IUnityCollect _Unity = RpcClient.RpcClient.Unity;
                public static bool RegisterApi(Type type)
                {
                        Type apiType = type.GetInterface(PublicDict.IApiType.FullName);
                        if (apiType == null)
                        {
                                return false;
                        }
                        return _Unity.Register(PublicDict.IGatewayType, type, type.FullName);
                }
                public static bool RegisterGateway(Type type)
                {
                        Type apiType = type.GetInterface(PublicDict.IGatewayType.FullName);
                        if (apiType == null)
                        {
                                return false;
                        }
                        return _Unity.Register(PublicDict.IGatewayType, type, type.FullName);
                }
                public static IApiController GetApi(Type type)
                {
                        return (IApiController)_Unity.Resolve(PublicDict.IGatewayType, type.FullName);
                }
                public static IApiGateway GetGateway(Type type)
                {
                        return (IApiGateway)_Unity.Resolve(PublicDict.IGatewayType, type.FullName);
                }
        }
}
