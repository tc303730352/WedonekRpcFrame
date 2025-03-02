using System;
using WeDonekRpc.ApiGateway;
using WeDonekRpc.ApiGateway.Interface;
using WeDonekRpc.ApiGateway.Model;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.WebSocketGateway.Interface;

namespace WeDonekRpc.WebSocketGateway
{
    internal class GatewayService
    {
        private static IApiDocModular _Doc;
        private static readonly ISocketGatewayConfig _Config = new Config.SocketGatewayConfig();

        public static ISocketGatewayConfig Config { get => _Config; }

        static GatewayService ()
        {
            GatewayServer.LoadEv += GatewayServer_LoadEv;
            GatewayServer.InitEv += GatewayServer_InitEv;
        }

        private static void GatewayServer_InitEv (IIocService service)
        {
            _Doc = service.Resolve<IApiDocModular>();
        }

        private static void GatewayServer_LoadEv (IGatewayOption obj)
        {
            _ = obj.IocBuffer.RegisterInstance<ISocketGatewayConfig>(_Config);
            _ = obj.IocBuffer.RegisterType(typeof(ICurrentSession), typeof(Model.CurrentSession));
            _ = obj.IocBuffer.RegisterType(typeof(ICurrentService), typeof(Model.CurrentService));
            _ = obj.IocBuffer.RegisterType(typeof(ICurrentModular), typeof(Model.CurrentModular));
        }

        internal static void RegModular (IApiModular modular, Type type)
        {
            if (_Doc != null)
            {
                Uri uri = new Uri(modular.Config.SocketConfig.ToUri(), "/{accreditId}/{identityId}");
                _Doc.RegModular(modular.ServiceName, type, uri);
            }
        }

        internal static string GetApiShow (string localPath)
        {
            return _Doc?.GetApiShow(localPath, ApiGateway.GatewayType.WebSocket);
        }

        internal static void RegApi (ApiFuncBody api)
        {
            _Doc?.RegApi(api);

        }
    }
}
