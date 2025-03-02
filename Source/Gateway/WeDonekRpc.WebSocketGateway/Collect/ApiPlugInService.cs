using WeDonekRpc.HttpWebSocket.Interface;
using WeDonekRpc.HttpWebSocket.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.WebSocketGateway.Interface;
using WeDonekRpc.WebSocketGateway.PlugIn;
using WeDonekRpc.Helper.Lock;

namespace WeDonekRpc.WebSocketGateway.Collect
{
    internal class ApiPlugInService : IPlugInService
    {
        private static IWebSocketPlugin[] _Plugins = System.Array.Empty<IWebSocketPlugin>();
        private static IWebSocketPlugin[] _Auths = System.Array.Empty<IWebSocketPlugin>();
        private static IWebSocketPlugin[] _Request = System.Array.Empty<IWebSocketPlugin>();
        private static IWebSocketPlugin[] _Execs = System.Array.Empty<IWebSocketPlugin>();
        private static readonly LockHelper _Lock = new LockHelper();
        public static void Init()
        {
            _Add(new LimitPlugIn());
            _Add(new IpBackPlugIn());
            _Add(new IpLimitPlugIn());
            _Add(new RouteShieIdPlugIn());
            _Add(new NodeLimitPlugIn());
        }
        private static void _Add(IWebSocketPlugin plugIn, int index)
        {
            if (_Lock.GetLock())
            {
                if (!_Plugins.IsExists(a => a.Name == plugIn.Name))
                {
                    plugIn.Init();
                    if (plugIn.IsEnable)
                    {
                        _Plugins = _Plugins.Insert(plugIn, index);
                        _Init(plugIn);
                    }
                }
                _Lock.Exit();
            }
        }
        private static void _Init(IWebSocketPlugin plugin)
        {
            if ((ExecStage.请求 & plugin.ExecStage) == ExecStage.请求)
            {
                _Request = _Request.Add(plugin);
            }
            if ((ExecStage.认证 & plugin.ExecStage) == ExecStage.认证)
            {
                _Auths = _Auths.Add(plugin);
            }
            if ((ExecStage.执行 & plugin.ExecStage) == ExecStage.执行)
            {
                _Execs = _Execs.Add(plugin);
            }
        }
        private static void _Add(IWebSocketPlugin plugIn)
        {
            if (_Lock.GetLock())
            {
                if (!_Plugins.IsExists(a => a.Name == plugIn.Name))
                {
                    plugIn.Init();
                    if (plugIn.IsEnable)
                    {
                        _Plugins = _Plugins.Add(plugIn);
                        _Init(plugIn);
                    }
                }
                _Lock.Exit();
            }
        }



        public void Add(IWebSocketPlugin plugIn, int index)
        {
            _Add(plugIn, index);
        }
        public void Add(IWebSocketPlugin plugIn)
        {
            _Add(plugIn);
        }

        public void Delete(string name)
        {
            _Drop(name);
        }
        private static void _Drop(string name)
        {
            if (_Lock.GetLock())
            {
                _Plugins = _Plugins.RemoveOne(a => a.Name == name, out IWebSocketPlugin plugIn);
                if (plugIn != null)
                {
                    _Drop(plugIn);
                    plugIn.Dispose();
                }
                _Lock.Exit();
            }
        }
        private static void _Drop(IWebSocketPlugin plugin)
        {
            if ((ExecStage.请求 & plugin.ExecStage) == ExecStage.请求)
            {
                _Request = _Request.Remove(plugin);
            }
            if ((ExecStage.认证 & plugin.ExecStage) == ExecStage.认证)
            {
                _Auths = _Auths.Remove(plugin);
            }
            if ((ExecStage.执行 & plugin.ExecStage) == ExecStage.执行)
            {
                _Execs = _Execs.Remove(plugin);
            }
        }
        public static bool Authorize(RequestBody request)
        {
            return _Authorize(_Auths, request);
        }
        public static bool RequestInit(IApiService service, out string error)
        {
            return _RequestInit(_Request, service, out error);
        }
        public static bool Exec(IApiService service, ApiHandler handler, out string error)
        {
            return _Exec(_Execs, service, handler, out error);
        }
        private static bool _RequestInit(IWebSocketPlugin[] plugIns, IApiService service, out string error)
        {
            if (plugIns.Length == 0)
            {
                error = null;
                return true;
            }
            foreach (IWebSocketPlugin i in plugIns)
            {
                if (!i.RequestInit(service, out error))
                {
                    return false;
                }
            }
            error = null;
            return true;
        }
        private static bool _Exec(IWebSocketPlugin[] plugIns, IApiService service, ApiHandler handler, out string error)
        {
            if (plugIns.Length == 0)
            {
                error = null;
                return true;
            }
            foreach (IWebSocketPlugin i in plugIns)
            {
                if (!i.Exec(service, handler, out error))
                {
                    return false;
                }
            }
            error = null;
            return true;
        }
        private static bool _Authorize(IWebSocketPlugin[] plugIns, RequestBody request)
        {
            if (plugIns.Length == 0)
            {
                return true;
            }
            foreach (IWebSocketPlugin i in plugIns)
            {
                if (!i.Authorize(request))
                {
                    return false;
                }
            }
            return true;
        }
    }
}