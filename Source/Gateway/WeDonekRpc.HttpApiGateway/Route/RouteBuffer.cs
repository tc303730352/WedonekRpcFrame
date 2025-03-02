using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WeDonekRpc.Client.Ioc;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.HttpApiGateway.Collect;
using WeDonekRpc.HttpApiGateway.Helper;
using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.HttpApiGateway.Model;

namespace WeDonekRpc.HttpApiGateway.Route
{
    public class RouteBuffer
    {
        private readonly Dictionary<string, IApiRoute> _Routes = [];
        private readonly Dictionary<string, Type> _Controllers = [];
        private readonly IocBuffer _Ioc;
        private readonly IApiModular _Modular;
        internal RouteBuffer ( IocBuffer ioc, IApiModular modular )
        {
            this._Modular = modular;
            this._Ioc = ioc;
        }
        internal void Load ( Assembly assembly )
        {
            ApiRouteBuffer[] apis = ModularHelper.GetApiBody(assembly.GetTypes(), this._Modular);
            if ( !apis.IsNull() )
            {
                apis.ForEach(c =>
                {
                    IocBody body = this._Ioc.Register(c.Form, c.To, c.Name);
                    if ( body == null )
                    {
                        throw new ErrorException("gateway.http.controller.reg.fail")
                        {
                            Args =
                            {
                                  {"type",body.Name }
                            }
                        };
                    }
                    if ( c.Apis.Count > 0 )
                    {
                        if ( !this._Controllers.ContainsKey(c.Name) )
                        {
                            this._Controllers.Add(c.Name, c.To);
                        }
                        c.Apis.ForEach(a =>
                        {
                            if ( !this._Routes.ContainsKey(a.Route.ApiUri) )
                            {
                                if ( a.ApiEventType != null )
                                {
                                    _ = this._Ioc.Register(typeof(IApiServiceEvent), a.ApiEventType, a.Name);
                                }
                                if ( a.UpConfigType != null )
                                {
                                    _ = this._Ioc.Register(typeof(IUpFileConfig), a.UpConfigType, a.Route.Id);
                                }
                                this._Routes.Add(a.Route.ApiUri, a.Route);
                            }
                        });
                    }
                });
            }
        }

        internal async void Init ()
        {
            if ( this._Routes.Count > 0 )
            {
                await Task.Factory.StartNew(this._Init);
            }
        }
        private void _Init ()
        {
            this._Controllers.ForEach(( key, val ) =>
            {
                ApiGatewayService.RegModular(this._Modular.Show ?? this._Modular.ServiceName, val);
            });
            IApiRoute[] routes = this._Routes.Values.ToArray();
            using ( IResourceCollect resource = ResourceCollect.Create(this._Modular) )
            {
                ApiRouteCollect.Adds(routes, resource.RegRoute);
            }
        }
        private string _Reg ( ApiRouteBuffer buffer )
        {
            if ( buffer.Form != null )
            {
                _ = this._Ioc.Register(buffer.Form, buffer.To, buffer.Name);
            }
            ApiBody route = buffer.Apis[0];
            if ( !this._Routes.ContainsKey(route.Route.ApiUri) )
            {
                if ( route.ApiEventType != null )
                {
                    _ = this._Ioc.Register(typeof(IApiServiceEvent), route.ApiEventType, route.Name);
                }
                if ( route.UpConfigType != null )
                {
                    _ = this._Ioc.Register(typeof(IUpFileConfig), route.UpConfigType, route.Route.Id);
                }
                this._Routes.Add(route.Route.ApiUri, route.Route);
                return route.Route.Id;
            }
            else
            {
                throw new ErrorException("rpc.route.path.repeat");
            }
        }
        public string RegApi ( RouteSet route, MethodInfo method )
        {
            ApiRouteBuffer buffer = ModularHelper.GetApiRoute(method, this._Modular, route);
            return this._Reg(buffer);
        }

        public string RegApi ( RouteSet route, Action action )
        {
            ApiRouteBuffer buffer = ModularHelper.GetApiRoute(action.Method, this._Modular, route);
            return this._Reg(buffer);
        }

        public string RegApi ( RouteSet route, Action<IApiService> action )
        {
            ApiRouteBuffer buffer = ModularHelper.GetApiRoute(action.Method, this._Modular, route);
            return this._Reg(buffer);
        }

        public string RegApi ( RouteSet route, Func<IApiService, IResponse> action )
        {
            ApiRouteBuffer buffer = ModularHelper.GetApiRoute(action.Method, this._Modular, route);
            return this._Reg(buffer);
        }

        public string RegApi ( RouteSet route, Func<IResponse> action )
        {
            ApiRouteBuffer buffer = ModularHelper.GetApiRoute(action.Method, this._Modular, route);
            return this._Reg(buffer);
        }

        public string RegApi<Result> ( RouteSet route, Func<IApiService, Result> action )
        {
            ApiRouteBuffer buffer = ModularHelper.GetApiRoute(action.Method, this._Modular, route);
            return this._Reg(buffer);
        }

        public string RegApi<Result> ( RouteSet route, Func<Result> action )
        {
            ApiRouteBuffer buffer = ModularHelper.GetApiRoute(action.Method, this._Modular, route);
            return this._Reg(buffer);
        }

        public string RegApi<T, Result> ( RouteSet route, Func<IApiService, T, Result> action )
        {
            ApiRouteBuffer buffer = ModularHelper.GetApiRoute(action.Method, this._Modular, route);
            return this._Reg(buffer);
        }

        public string RegApi<T, Result> ( RouteSet route, Func<T, Result> action )
        {
            ApiRouteBuffer buffer = ModularHelper.GetApiRoute(action.Method, this._Modular, route);
            return this._Reg(buffer);
        }

        public string RegApi<T> ( RouteSet route, Action<IApiService, T> action )
        {
            ApiRouteBuffer buffer = ModularHelper.GetApiRoute(action.Method, this._Modular, route);
            return this._Reg(buffer);
        }

        public string RegApi<T> ( RouteSet route, Action<T> action )
        {
            ApiRouteBuffer buffer = ModularHelper.GetApiRoute(action.Method, this._Modular, route);
            return this._Reg(buffer);
        }

        public string RegApi<T> ( RouteSet route, Func<IApiService, T, IResponse> action )
        {
            ApiRouteBuffer buffer = ModularHelper.GetApiRoute(action.Method, this._Modular, route);
            return this._Reg(buffer);
        }

        public string RegApi<T> ( RouteSet route, Func<T, IResponse> action )
        {
            ApiRouteBuffer buffer = ModularHelper.GetApiRoute(action.Method, this._Modular, route);
            return this._Reg(buffer);
        }

    }
}
