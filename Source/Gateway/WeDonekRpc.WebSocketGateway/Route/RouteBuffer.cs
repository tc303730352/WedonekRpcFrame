using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WeDonekRpc.Client.Ioc;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.WebSocketGateway.Collect;
using WeDonekRpc.WebSocketGateway.Helper;
using WeDonekRpc.WebSocketGateway.Interface;
using WeDonekRpc.WebSocketGateway.Model;

namespace WeDonekRpc.WebSocketGateway.Route
{
    public class RouteBuffer
    {
        private readonly Dictionary<string, ApiHandler> _Routes = [];
        private readonly Dictionary<string, Type> _Controllers = [];
        private readonly IocBuffer _Ioc;
        private readonly IApiModular _Modular;
        internal RouteBuffer (IocBuffer ioc, IApiModular modular)
        {
            this._Modular = modular;
            this._Ioc = ioc;
        }
        internal void Load (Assembly assembly)
        {
            ApiRouteBuffer[] apis = ModularHelper.GetApiBody(assembly.GetTypes(), this._Modular);
            if (!apis.IsNull())
            {
                apis.ForEach(c =>
                {
                    IocBody body = this._Ioc.Register(c.Form, c.To, c.Name);
                    if (body == null)
                    {
                        throw new ErrorException("gateway.http.controller.reg.fail")
                        {
                            Args =
                            {
                                  {"type",body.Name }
                            }
                        };
                    }
                    if (c.Apis.Count > 0)
                    {
                        if (!this._Controllers.ContainsKey(c.Name))
                        {
                            this._Controllers.Add(c.Name, c.To);
                        }
                        c.Apis.ForEach(a =>
                        {
                            if (!this._Routes.ContainsKey(a.LocalPath))
                            {

                                this._Routes.Add(a.LocalPath, a);
                            }
                        });
                    }
                });
            }
        }

        internal void Init ()
        {
            if (this._Routes.Count > 0)
            {
                this._Controllers.ForEach((key, val) =>
                {
                    GatewayService.RegModular(this._Modular, val);
                });
                ApiHandler[] routes = this._Routes.Values.ToArray();
                using (IResourceCollect resource = ResourceCollect.Create(this._Modular))
                {
                    RouteCollect.Adds(routes, resource.RegRoute);
                }
            }
        }
        private string _Reg (ApiRouteBuffer buffer)
        {
            if (buffer.Form != null)
            {
                _ = this._Ioc.Register(buffer.Form, buffer.To, buffer.Name);
            }
            ApiHandler route = buffer.Apis[0];
            if (!this._Routes.ContainsKey(route.Route.LocalPath))
            {
                this._Routes.Add(route.LocalPath, route);
                return route.Id;
            }
            else
            {
                throw new ErrorException("rpc.route.path.repeat");
            }
        }
        public string RegApi (RouteSet route, MethodInfo method)
        {
            ApiRouteBuffer buffer = ModularHelper.GetApiRoute(method, this._Modular, route);
            return this._Reg(buffer);
        }

        public string RegApi (RouteSet route, Action action)
        {
            ApiRouteBuffer buffer = ModularHelper.GetApiRoute(action.Method, this._Modular, route);
            return this._Reg(buffer);
        }

        public string RegApi (RouteSet route, Action<IApiSocketService> action)
        {
            ApiRouteBuffer buffer = ModularHelper.GetApiRoute(action.Method, this._Modular, route);
            return this._Reg(buffer);
        }


        public string RegApi<Result> (RouteSet route, Func<IApiSocketService, Result> action)
        {
            ApiRouteBuffer buffer = ModularHelper.GetApiRoute(action.Method, this._Modular, route);
            return this._Reg(buffer);
        }

        public string RegApi<Result> (RouteSet route, Func<Result> action)
        {
            ApiRouteBuffer buffer = ModularHelper.GetApiRoute(action.Method, this._Modular, route);
            return this._Reg(buffer);
        }

        public string RegApi<T, Result> (RouteSet route, Func<IApiSocketService, T, Result> action)
        {
            ApiRouteBuffer buffer = ModularHelper.GetApiRoute(action.Method, this._Modular, route);
            return this._Reg(buffer);
        }

        public string RegApi<T, Result> (RouteSet route, Func<T, Result> action)
        {
            ApiRouteBuffer buffer = ModularHelper.GetApiRoute(action.Method, this._Modular, route);
            return this._Reg(buffer);
        }

        public string RegApi<T> (RouteSet route, Action<IApiSocketService, T> action)
        {
            ApiRouteBuffer buffer = ModularHelper.GetApiRoute(action.Method, this._Modular, route);
            return this._Reg(buffer);
        }

        public string RegApi<T> (RouteSet route, Action<T> action)
        {
            ApiRouteBuffer buffer = ModularHelper.GetApiRoute(action.Method, this._Modular, route);
            return this._Reg(buffer);
        }
    }
}
