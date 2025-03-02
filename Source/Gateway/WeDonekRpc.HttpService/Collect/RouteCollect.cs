using System;
using System.Collections.Concurrent;
using WeDonekRpc.HttpService.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Lock;
namespace WeDonekRpc.HttpService.Collect
{
    internal class RouteCollect
    {
        private static readonly ConcurrentDictionary<string, IBasicHandler> _PathRoute = new ConcurrentDictionary<string, IBasicHandler>();

        private static IBasicHandler[] _RouteList = new IBasicHandler[0];
        private static readonly LockHelper _lock = new LockHelper();


        internal static IBasicHandler GetHandler(string path)
        {
            if (_PathRoute.TryGetValue(path, out IBasicHandler temp))
            {
                return (IBasicHandler)temp.Clone();
            }
            temp = _RouteList.Find(a => a.IsUsable(path));
            if (temp != null)
            {
                return (IBasicHandler)temp.Clone();
            }
            return null;
        }

        private static void _AddRouteList(IBasicHandler handler)
        {
            if (_lock.GetLock())
            {
                _RouteList = _RouteList.Add(handler, (a, b) => a.SortNum <= b.SortNum);
                _lock.Exit();
            }
        }
        public static void AddRoute(IBasicHandler handler)
        {
            if (!handler.IsFullPath)
            {
                _AddRouteList(handler);
            }
            else
            {
                _PathRoute.TryAdd(handler.RequestPath, handler);
            }
        }
        public static void ReplaceRoute(IBasicHandler old, IBasicHandler route)
        {
            RemoveRoute(old.RequestPath, old.IsRegex);
            AddRoute(route);
        }
        private static void _RemoveRoute(string uri)
        {
            uri = uri.ToLower();
            if (_lock.GetLock())
            {
                _RouteList = _RouteList.RemoveOne(a => a.RequestPath == uri);
                _lock.Exit();
            }
        }
        public static void RemoveRoute(string uri, bool isRegex)
        {
            if (isRegex || uri.EndsWith("/"))
            {
                _RemoveRoute(uri);
            }
            else
            {
                _PathRoute.TryRemove(uri, out _);
            }
        }
    }
}
