using System;
using WeDonekRpc.ApiGateway;
using WeDonekRpc.ApiGateway.Interface;
using WeDonekRpc.ApiGateway.Json;
using WeDonekRpc.ApiGateway.Model;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.HttpApiGateway.Interface;

namespace WeDonekRpc.HttpApiGateway
{
    internal class ApiGatewayService
    {
        private static IApiDocModular _ApiDoc;

        public static IHttpConfig Config { get; } = new Config.HttpConfig();

        public static void Init ()
        {
            ApiGatewayService.Config.RefreshEvent(_Refresh);
            GatewayServer.LoadEv += GatewayServer_Load;
            GatewayServer.InitEv += GatewayServer_InitEv;
            GatewayServer.Starting += GatewayServer_Starting;
            GatewayServer.Closeing += GatewayServer_Closeing;
        }

        private static void _Refresh ( IHttpConfig config, string name )
        {
            if ( config.IsEnableLongToString )
            {
                GatewayJsonTools.Enable();
            }
            else
            {
                GatewayJsonTools.Stop();
            }
        }

        private static void GatewayServer_InitEv ( IIocService obj )
        {
            _ApiDoc = obj.Resolve<IApiDocModular>();
        }

        private static void GatewayServer_Load ( IGatewayOption obj )
        {
            _ = obj.IocBuffer.RegisterInstance<IHttpConfig>(ApiGatewayService.Config);
            _ = obj.IocBuffer.RegisterInstance<IGatewayUpConfig>(ApiGatewayService.Config.UpConfig);
            obj.RegModular(new GatewayModular());
        }

        private static void GatewayServer_Closeing ()
        {
            HttpService.HttpService.StopService();
        }

        private static void GatewayServer_Starting ()
        {
            ApiPlugInService.Init();
            HttpService.HttpService.RegService(Config.Url);
        }

        internal static string GetApiShow ( Uri uri )
        {
            if ( _ApiDoc == null )
            {
                return string.Empty;
            }
            return _ApiDoc.GetApiShow(uri);
        }

        internal static void RegModular ( string name, Type source )
        {
            _ApiDoc?.RegModular(name, source, Config.RealRequestUri);
        }
        internal static void RegApi ( ApiFuncBody api )
        {
            _ApiDoc?.RegApi(api);
        }
    }
}
