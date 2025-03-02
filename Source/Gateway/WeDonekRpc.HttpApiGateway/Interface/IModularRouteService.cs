using System;
using System.Reflection;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.HttpApiGateway.Model;
namespace WeDonekRpc.HttpApiGateway.Interface
{
    [IgnoreIoc]
    public interface IModularRouteService
    {
        void EnableByPath ( string path );

        void DisableByPath ( string path );

        void Enable ( string routeId );

        void Disable ( string routeId );

        void RemoveByPath ( string path );

        void Remove ( string routeId );

        string RegApi (MethodInfo method, RouteSet routeSet);

        string RegApi (RouteSet route, Action action);

        string RegApi (RouteSet route, Action<IApiService> action);

        string RegApi (RouteSet route, Func<IApiService, IResponse> action);

        string RegApi (RouteSet route, Func<IResponse> action);

        string RegApi<Result> (RouteSet route, Func<IApiService, Result> action);

        string RegApi<Result> (RouteSet route, Func<Result> action);

        string RegApi<T, Result> (RouteSet route, Func<IApiService, T, Result> action);

        string RegApi<T, Result> (RouteSet route, Func<T, Result> action);

        string RegApi<T> (RouteSet route, Action<IApiService, T> action);

        string RegApi<T> (RouteSet route, Action<T> action);

        string RegApi<T> (RouteSet route, Func<IApiService, T, IResponse> action);

        string RegApi<T> (RouteSet route, Func<T, IResponse> action);
    }
}