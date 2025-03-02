using System;
using System.Reflection;
using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.HttpApiGateway.Collect;
using WeDonekRpc.HttpApiGateway.Helper;
using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.HttpApiGateway.Model;
using WeDonekRpc.HttpApiGateway.Route;

namespace WeDonekRpc.HttpApiGateway.Service
{
    internal class ModularRouteService : IModularRouteService
    {
        private readonly IIocService _Ioc;
        private readonly IApiModular _Modular;
        private IHttpRouteService _routeService;

        private IHttpRouteService _RouteService
        {
            get
            {
                if (this._routeService == null)
                {
                    this._routeService = this._Ioc.Resolve<IHttpRouteService>();
                }
                return this._routeService;
            }
        }

        public ModularRouteService (IApiModular modular)
        {
            this._Ioc = RpcClient.Ioc;
            this._Modular = modular;
        }

        public void Disable (string routeId)
        {
            this._RouteService.Disable(routeId);
        }

        public void DisableByPath (string path)
        {
            this._RouteService.DisableByPath(path);
        }

        public void Enable (string routeId)
        {
            this._RouteService.Enable(routeId);
        }

        public void EnableByPath (string path)
        {
            this._RouteService.EnableByPath(path);
        }

        public string RegApi (MethodInfo method, RouteSet routeSet)
        {
            ApiRouteBuffer buffer = ModularHelper.GetApiRoute(method, this._Modular, routeSet);
            return this._Reg(buffer);
        }

        public void Remove (string routeId)
        {
            this._RouteService.Remove(routeId);
        }

        public void RemoveByPath (string path)
        {
            this._RouteService.RemoveByPath(path);
        }
        private string _Reg (ApiRouteBuffer buffer)
        {
            if ( buffer.Apis.Count == 0 )
            {
                throw new ErrorException("rpc.gateway.api.null");
            }
            if (buffer.Form != null && !this._Ioc.IsRegistered(buffer.Form, buffer.Name))
            {
                throw new ErrorException("gateway.http.class.ioc.no.reg")
                {
                    Args = new System.Collections.Generic.Dictionary<string, string>
                   {
                        {"To",buffer.To.FullName },
                        {"Form",buffer.Form.FullName },
                        {"Name",buffer.Name },
                   }
                };
            }
            ApiBody route = buffer.Apis[0];
            if (route.ApiEventType != null && !this._Ioc.IsRegistered(typeof(IApiServiceEvent), route.Name))
            {
                throw new ErrorException("gateway.http.class.ioc.no.reg")
                {
                    Args = new System.Collections.Generic.Dictionary<string, string>
                   {
                        {"To",buffer.To.FullName },
                        {"Form",buffer.Form.FullName },
                        {"Name",buffer.Name },
                   }
                };
            }
            ApiRouteCollect.Add(route.Route);
            return route.Route.Id;
        }

        public string RegApi (RouteSet route, Action action)
        {
            ApiRouteBuffer buffer = ModularHelper.GetApiRoute(action.Method, this._Modular, route);
            return this._Reg(buffer);
        }

        public string RegApi (RouteSet route, Action<IApiService> action)
        {
            ApiRouteBuffer buffer = ModularHelper.GetApiRoute(action.Method, this._Modular, route);
            return this._Reg(buffer);
        }

        public string RegApi (RouteSet route, Func<IApiService, IResponse> action)
        {
            ApiRouteBuffer buffer = ModularHelper.GetApiRoute(action.Method, this._Modular, route);
            return this._Reg(buffer);
        }

        public string RegApi (RouteSet route, Func<IResponse> action)
        {
            ApiRouteBuffer buffer = ModularHelper.GetApiRoute(action.Method, this._Modular, route);
            return this._Reg(buffer);
        }

        public string RegApi<Result> (RouteSet route, Func<IApiService, Result> action)
        {
            ApiRouteBuffer buffer = ModularHelper.GetApiRoute(action.Method, this._Modular, route);
            return this._Reg(buffer);
        }

        public string RegApi<Result> (RouteSet route, Func<Result> action)
        {
            ApiRouteBuffer buffer = ModularHelper.GetApiRoute(action.Method, this._Modular, route);
            return this._Reg(buffer);
        }

        public string RegApi<T, Result> (RouteSet route, Func<IApiService, T, Result> action)
        {
            ApiRouteBuffer buffer = ModularHelper.GetApiRoute(action.Method, this._Modular, route);
            return this._Reg(buffer);
        }

        public string RegApi<T, Result> (RouteSet route, Func<T, Result> action)
        {
            ApiRouteBuffer buffer = ModularHelper.GetApiRoute(action.Method, this._Modular, route);
            return this._Reg(buffer);
        }

        public string RegApi<T> (RouteSet route, Action<IApiService, T> action)
        {
            ApiRouteBuffer buffer = ModularHelper.GetApiRoute(action.Method, this._Modular, route);
            return this._Reg(buffer);
        }

        public string RegApi<T> (RouteSet route, Action<T> action)
        {
            ApiRouteBuffer buffer = ModularHelper.GetApiRoute(action.Method, this._Modular, route);
            return this._Reg(buffer);
        }

        public string RegApi<T> (RouteSet route, Func<IApiService, T, IResponse> action)
        {
            ApiRouteBuffer buffer = ModularHelper.GetApiRoute(action.Method, this._Modular, route);
            return this._Reg(buffer);
        }

        public string RegApi<T> (RouteSet route, Func<T, IResponse> action)
        {
            ApiRouteBuffer buffer = ModularHelper.GetApiRoute(action.Method, this._Modular, route);
            return this._Reg(buffer);
        }
    }
}
