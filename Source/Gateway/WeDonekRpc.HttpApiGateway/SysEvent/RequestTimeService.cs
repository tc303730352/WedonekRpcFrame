using WeDonekRpc.Client.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.HttpApiGateway.SysEvent.Module;
using WeDonekRpc.Modular;
using WeDonekRpc.ModularModel.SysEvent.Model;

namespace WeDonekRpc.HttpApiGateway.SysEvent
{
    internal class RequestTimeService
    {
        private static IGatewayApiService _ApiService;

        private static IRpcEventService _RpcEvent;

        private static IRpcEventLogService _LogService;

        private static readonly string _ModuleName = "HttpRequestTime";

        private static RequestDurationModule _Module;


        public static void Init ( IIocService ioc )
        {
            _ApiService = ioc.Resolve<IGatewayApiService>();
            _RpcEvent = ioc.Resolve<IRpcEventService>();
            _LogService = ioc.Resolve<IRpcEventLogService>();
            _RpcEvent.RefreshEvent(_Refresh);
            _InitService();
        }
        private static void _InitService ()
        {
            ServiceSysEvent[] events = _RpcEvent.GetSysEvents(_ModuleName);
            if ( events.IsNull() && _Module != null )
            {
                _Module.Dispose();
                _Module = null;
                return;
            }
            _Module?.Dispose();
            if ( !events.IsNull() )
            {
                _Module = new RequestDurationModule(events, _LogService, _ApiService);
                _Module.Init();
            }
        }


        private static void _Refresh ( string module )
        {
            if ( module != _ModuleName )
            {
                return;
            }
            _InitService();
        }
    }
}
