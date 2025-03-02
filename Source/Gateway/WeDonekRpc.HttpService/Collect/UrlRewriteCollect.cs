using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WeDonekRpc.Helper;
using WeDonekRpc.HttpService.Interface;
using WeDonekRpc.HttpService.Rewrite;
namespace WeDonekRpc.HttpService.Collect
{
    public class UrlRewriteCollect
    {
        private static IRewriteRoute[] _Routes = Array.Empty<IRewriteRoute>();
        private static readonly ConcurrentDictionary<string, IRewriteRoute> _RouteDic = new ConcurrentDictionary<string, IRewriteRoute>();

        public static bool CheckIsExist (string path, bool isRegex, out string endPath)
        {
            if (isRegex || path.EndsWith("/"))
            {
                endPath = _Routes.Find(a => a.Path == path, a => a.EndPath);
                return endPath != null;
            }
            else if (_RouteDic.TryGetValue(path, out IRewriteRoute route))
            {
                endPath = route.EndPath;
                return true;
            }
            endPath = null;
            return false;
        }
        public static void Add (string path, string endPoint)
        {
            IRewriteRoute route = new RewriteRoute(path, endPoint);
            _Add(route);
        }
        public static void Add (Regex regex, string endPoint)
        {
            IRewriteRoute route = new RewriteRoute(regex, endPoint);
            _Add(route);
        }
        public static void Add (string path, Func<Uri, string> endPoint)
        {
            IRewriteRoute route = new ActionRoute(path, endPoint);
            _Add(route);
        }
        public static void RemoveRegex (string regex)
        {
            if (_Routes.IsExists(c => c.Path == regex))
            {
                _Routes = _Routes.RemoveOne(a => a.Path == regex);
            }
        }
        public static void Remove (Regex regex)
        {
            string path = regex.ToString();
            if (_Routes.IsExists(c => c.Path == path))
            {
                _Routes = _Routes.RemoveOne(a => a.Path == path);
            }
        }
        public static void Remove (string path)
        {
            if (path.EndsWith("/"))
            {
                if (_Routes.IsExists(c => c.Path == path))
                {
                    _Routes = _Routes.RemoveOne(a => a.Path == path);
                }
                return;
            }
            _ = _RouteDic.TryRemove(path, out _);
        }
        public static void Add (Regex regex, Func<Uri, string> endPoint)
        {
            IRewriteRoute route = new ActionRoute(regex, endPoint);
            _Add(route);
        }
        private static void _Add (IRewriteRoute route)
        {
            if (!route.IsFullPath)
            {
                if (_Routes.IsExists(a => a.Path == route.Path))
                {
                    throw new ErrorException("http.dynamic.path.repeat");
                }
                _Routes = _Routes.Add(route);
            }
            else if (_RouteDic.ContainsKey(route.Path))
            {
                throw new ErrorException("http.dynamic.path.repeat");
            }
            else
            {
                _ = _RouteDic.TryAdd(route.Path, route);
            }
        }

        internal static bool GetRoute (string path, Uri uri, out RouteParam param)
        {
            if (_RouteDic.TryGetValue(path, out IRewriteRoute route))
            {
                param = route.GetRouteParam(uri);
                return true;
            }
            route = _Routes.Find(a => a.IsUsable(path));
            if (route != null)
            {
                param = route.GetRouteParam(uri);
                return param != null;
            }
            param = null;
            return false;
        }
        public static void Exclude (string[] paths)
        {
            if (paths.IsNull())
            {
                _Routes = Array.Empty<IRewriteRoute>();
                _RouteDic.Clear();
                return;
            }
            int num = _RouteDic.Count + _Routes.Length - paths.Length;
            if (num == 0)
            {
                return;
            }
            string[] keys = _RouteDic.Keys.ToArray().Remove(paths);
            keys.ForEach(c =>
            {
                _ = _RouteDic.TryRemove(c, out IRewriteRoute route);
            });
            if (keys.Length != num)
            {
                _Routes = _Routes.FindAll(c => paths.IsExists(c.Path));
            }
        }
    }
}
