using System;

using ApiGateway.Interface;
using ApiGateway.Model;

using RpcClient;

using WebSocketGateway.Interface;

namespace WebSocketGateway
{
        internal class GatewayService
        {
                private static IApiDocModular _Doc => RpcClient.RpcClient.Unity.Resolve<IApiDocModular>();

                static GatewayService()
                {
                        RpcClient.RpcClient.Unity.RegisterType(typeof(ICurrentSession), typeof(Model.CurrentSession));
                        RpcClient.RpcClient.Unity.RegisterType(typeof(ICurrentService), typeof(Model.CurrentService));
                        RpcClient.RpcClient.Unity.RegisterType(typeof(ICurrentModular), typeof(Model.CurrentModular));
                }
                internal static void RegModular(IApiModular modular, Type type)
                {
                        if (_Doc != null)
                        {
                                Uri uri = new Uri(modular.Config.SocketConfig.ToUri(), "/{accreditId}/{identityId}");
                                _Doc.RegModular(modular.ServiceName, type, uri);
                        }
                }

                internal static string GetApiShow(string localPath)
                {
                        if (_Doc != null)
                        {
                                _Doc.GetApiShow(localPath, ApiGateway.GatewayType.WebSocket);
                        }
                        return string.Empty;
                }

                internal static void RegApi(ApiFuncBody api)
                {
                        if (_Doc != null)
                        {
                                _Doc.RegApi(api);
                        }

                }
        }
}
