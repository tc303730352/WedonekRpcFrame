using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.HttpApiGateway.PlugIn;
using WeDonekRpc.HttpService.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Lock;
namespace WeDonekRpc.HttpApiGateway
{
    internal class ApiPlugInService : IPlugInService
    {
        private static IHttpPlugIn[] _PlugIns = System.Array.Empty<IHttpPlugIn>();
        private static readonly LockHelper _Lock = new LockHelper();
        public static void Init()
        {
            _Add(new RequestCheck());
            _Add(new RouteShieIdPlugIn());
            _Add(new IpBackPlugIn());
            _Add(new IpLimitPlugIn());
            _Add(new LimitPlugIn());
            _Add(new NodeLimitPlugIn());
            _Add(new CorsPlugIn());
        }
        private static void _Add(IHttpPlugIn plugIn, int index)
        {
            if (_Lock.GetLock())
            {
                if (!_PlugIns.IsExists(a => a.Name == plugIn.Name))
                {
                    plugIn.Init();
                    if (plugIn.IsEnable)
                    {
                        _PlugIns = _PlugIns.Insert(plugIn, index);
                    }
                }
                _Lock.Exit();
            }
        }

        private static void _Add(IHttpPlugIn plugIn)
        {
            if (_Lock.GetLock())
            {
                if (!_PlugIns.IsExists(a => a.Name == plugIn.Name))
                {
                    plugIn.Init();
                    if (plugIn.IsEnable)
                    {
                        _PlugIns = _PlugIns.Add(plugIn);
                    }
                }
                _Lock.Exit();
            }
        }
        public void Add(IHttpPlugIn plugIn, int index)
        {
            _Add(plugIn, index);
        }
        public void Add(IHttpPlugIn plugIn)
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
                _PlugIns = _PlugIns.RemoveOne(a => a.Name == name, out IHttpPlugIn plugIn);
                if (plugIn != null)
                {
                    plugIn.Dispose();
                }
                _Lock.Exit();
            }
        }
        public static bool Request_Init(IRoute route, IHttpHandler handler)
        {
            return _Request_Init(_PlugIns, route, handler);
        }
        private static bool _Request_Init(IHttpPlugIn[] plugIns, IRoute route, IHttpHandler handler)
        {
            foreach (IHttpPlugIn i in plugIns)
            {
                i.Exec(route, handler);
                if (handler.Response.IsEnd)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
