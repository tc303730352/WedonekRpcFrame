using System;
using System.Collections.Concurrent;
using System.Threading;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Helper;
using WeDonekRpc.HttpApiGateway.Interface;

namespace WeDonekRpc.HttpApiGateway.Collect
{
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    internal class ApiRouteCollect : IHttpRouteService
    {
        private static readonly ConcurrentDictionary<string, IApiRoute> _RouteApi = new ConcurrentDictionary<string, IApiRoute>();
        private static readonly ConcurrentDictionary<string, string> _RouteUri = new ConcurrentDictionary<string, string>();
        private static int _Id = 0;

        internal static string CreateApiId ()
        {
            return Interlocked.Add(ref _Id, 1).ToString();
        }
        public void Remove ( string routeId )
        {
            if ( _RouteApi.TryRemove(routeId, out IApiRoute route) )
            {
                _ = _RouteUri.TryRemove(route.ApiUri, out _);
                route.Dispose();
            }
        }
        public void RemoveByPath ( string path )
        {
            if ( !_RouteUri.TryRemove(path, out string id) )
            {
                return;
            }
            else if ( _RouteApi.TryRemove(id, out IApiRoute route) )
            {
                route.Dispose();
            }
        }
        public static void Adds ( IApiRoute[] routes, Action<IRoute> action )
        {
            routes.ForEach(a =>
            {
                if ( !_RouteUri.ContainsKey(a.ApiUri) && _RouteUri.TryAdd(a.ApiUri, a.Id) && _RouteApi.TryAdd(a.Id, a) )
                {
                    a.InitRoute();
                    action(a);
                }
            });
        }
        public static void Add ( IApiRoute route )
        {
            if ( _RouteUri.ContainsKey(route.ApiUri) )
            {
                throw new ErrorException("gateway.http.route.path.repeat");
            }
            if ( _RouteUri.TryAdd(route.ApiUri, route.Id) && _RouteApi.TryAdd(route.Id, route) )
            {
                route.InitRoute();
            }
            else
            {
                throw new ErrorException("gateway.http.route.reg.fail");
            }
        }

        public void EnableByPath ( string path )
        {
            if ( !_RouteUri.TryGetValue(path, out string id) )
            {
                throw new ErrorException("gateway.http.path.not.find");
            }
            this.Enable(id);
        }

        public void DisableByPath ( string path )
        {
            if ( !_RouteUri.TryGetValue(path, out string id) )
            {
                throw new ErrorException("gateway.http.path.not.find");
            }
            this.Disable(id);
        }

        public void Enable ( string routeId )
        {
            if ( _RouteApi.TryGetValue(routeId, out IApiRoute route) )
            {
                route.Enable();
            }
        }

        public void Disable ( string routeId )
        {
            if ( _RouteApi.TryGetValue(routeId, out IApiRoute route) )
            {
                route.Disable();
            }
        }

    }
}
