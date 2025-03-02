using System;
using WeDonekRpc.ApiGateway.Interface;
using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Helper.Log;

namespace WeDonekRpc.ApiGateway
{
    public class GatewayServer
    {
        private static readonly IApiDocModular _ApiDoc = null;
        private static GatewayOption _Option;
        public static event Action Starting;
        public static event Action<IGatewayOption> LoadEv;
        public static event Action<IIocService> InitEv;
        public static event Action Closeing;
        /// <summary>
        /// 配置信息
        /// </summary>
        public static IGatewayConfig Config
        {
            get;
            private set;
        }

        public static WeDonekRpc.Modular.IIdentityService UserIdentity
        {
            get;
            private set;
        }

        /// <summary>
        /// 全局事件
        /// </summary>
        public static IGlobal Global
        {
            get;
            set;
        } = new BasicGlobal();

        internal static string GetApiShow ( Uri uri )
        {
            if ( _ApiDoc == null )
            {
                return string.Empty;
            }
            return _ApiDoc.GetApiShow(uri);
        }



        public static T GetModular<T> ( string name ) where T : IModular
        {
            return ModularService.GetModular<T>(name);
        }
        /// <summary>
        /// 停止服务
        /// </summary>
        public static void StopApiService ()
        {
            ModularService.Close();
            Closeing?.Invoke();
            RpcClient.Close();
            Global.ServiceClose();
        }
        /// <summary>
        /// 初始化Api服务
        /// </summary>
        public static void InitApiService ()
        {
            RpcClient.InitComplate += RpcClient_InitComplate;
            RpcClient.Start(_Load, _Init);
        }
        private static void _Load ( RpcInitOption option )
        {
            _Option = new GatewayOption(option);
            Global.Load(_Option);
            LoadEv?.Invoke(_Option);
        }
        private static void _Init ( IIocService ioc )
        {
            GatewayServer.UserIdentity = ioc.Resolve<WeDonekRpc.Modular.IIdentityService>();
            GatewayServer.Config = ioc.Resolve<IGatewayConfig>();
            InitEv?.Invoke(ioc);
        }

        private static void RpcClient_InitComplate ()
        {
            Starting?.Invoke();
            _Option.Init();
            _Option = null;
            Global.ServiceStarting();
            ModularService.Start();
            Global.ServiceStarted();
            new InfoLog("服务已启动!").Save();
        }

    }
}
