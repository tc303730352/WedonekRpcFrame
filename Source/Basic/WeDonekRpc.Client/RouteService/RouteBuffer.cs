using System;
using System.Collections.Generic;
using System.Reflection;
using WeDonekRpc.Client.Config;
using WeDonekRpc.Client.FileUp;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Ioc;
using WeDonekRpc.Client.Log;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Client.Route;
using WeDonekRpc.Helper;
using WeDonekRpc.TcpServer.FileUp;
using WeDonekRpc.TcpServer.Interface;

namespace WeDonekRpc.Client.RouteService
{
    public class RouteBuffer : IDisposable
    {
        private readonly IocBuffer _Ioc;
        private readonly IRouteService _Route;
        private readonly List<string> _FileUpRoutes = [];
        internal RouteBuffer (IocBuffer Ioc, IRouteService route)
        {
            this._Ioc = Ioc;
            this._Route = route;
        }
        /// <summary>
        /// 获取所有已注册的路由
        /// </summary>
        /// <returns></returns>
        public IRoute[] GetAllRoutes ()
        {
            return this._Route.GetRoutes();
        }
        public void RegUpFileRoute<Result> (string name, Type type)
        {
            if (this._Route.CheckIsExists(name))
            {
                throw new ErrorException("rpc.route.name.repeat");
            }
            IocBody body = this._Ioc.Register(typeof(IFileUpEvent<Result>), type, name);
            if (body != null)
            {
                SocketFileUpEvent<Result> socketUp = new SocketFileUpEvent<Result>(name);
                socketUp.Install(this._Ioc);
            }
        }
        public void AddRoute (Type type)
        {
            Type apiType = type.GetInterface(ConfigDic.RpcApiService.FullName);
            if (apiType != null)
            {
                this._LoadMethod(ConfigDic.RpcApiService, type);
            }
            else if (type.GetInterface(ConfigDic.IStreamAllot.FullName) != null)
            {
                IocBody body = this._Ioc.Register(ConfigDic.IStreamAllot, type, type.FullName);
                if (body != null)
                {
                    this._FileUpRoutes.Add(body.Name);
                }
            }
        }
        internal void Init ()
        {
            if (this._FileUpRoutes.Count > 0)
            {
                this._FileUpRoutes.ForEach(a =>
                {
                    IStreamAllot stream = RpcClient.Ioc.Resolve<IStreamAllot>(a);
                    _ = FileUpRouteCollect.AddAllot(stream);
                });
            }
        }
        private bool _CheckFormat (MethodInfo method)
        {
            ParameterInfo[] param = method.GetParameters();
            if (param.Length == 0)
            {
                return true;
            }
            return !param.IsExists(a => a.IsOut || a.ParameterType.IsByRef);
        }
        private void _LoadRoute (MethodInfo[] methods, Type form, Type to)
        {
            if (methods.Length == 0)
            {
                return;
            }
            IocBody ioc = this._Ioc.Register(form, to, to.FullName);
            if (ioc == null)
            {
                return;
            }
            methods.ForEach(b =>
            {
                if (b.DeclaringType == to && b.IsPublic && !b.IsConstructor && !b.IsVirtual)//限定 是声明该方法的类 不能是虚方法和构造函数 需是公开的方法 
                {
                    if (!this._CheckFormat(b))
                    {
                        //new 
                        RouteLog.AddRouteLog(b, b.Name, to, "rpc.route.format.error");
                        return;
                    }
                    string name = RouteHelper.GetRouteName(b);
                    if (!this._Route.CheckIsExists(name))
                    {
                        ITcpRoute route = this._CreateRoute(name, b);
                        this._Route.AddRoute(route);
                    }
                    else
                    {
                        RouteLog.AddRouteLog(b, name, to, "rpc.route.repeat");
                    }
                }
            });
        }
        private ITcpRoute _CreateRoute (string name, MethodInfo method)
        {
            if (method.ReturnType == ConfigDic.TaskType)
            {
                return new VoidTaskFuncRoute(name, method);
            }
            else if (method.ReturnType == ConfigDic.TaskReturnType)
            {
                return new BasicTaskRoute(name, method);
            }
            else if (method.ReturnType == ConfigDic.VoidType)
            {
                return new VoidFuncRoute(name, method);
            }
            else if (method.ReturnType == ConfigDic.ReturnType)
            {
                return new BasicRoute(name, method);
            }
            else if (method.ReturnType.Name == ConfigDic.TaskFuncType)
            {
                Type type = method.ReturnType.GetGenericArguments()[0];
                if (type.GetInterface(ConfigDic.ReturnType.FullName) != null)
                {
                    return new BasicTaskRoute(name, method);
                }
                return new ReturnTaskFuncRoute(name, method);
            }
            else
            {
                return new ReturnFuncRoute(name, method);
            }
        }
        private void _LoadMethod (Type form, Type to)
        {
            MethodInfo[] methods = to.GetMethods();
            this._LoadRoute(methods, form, to);
            if (to.BaseType.FullName != "System.Object")
            {
                this._LoadMethod(to.BaseType, to.BaseType);
            }
        }

        public void Dispose ()
        {
            this._FileUpRoutes.Clear();
        }
    }
}
