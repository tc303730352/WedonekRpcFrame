
using System;
using System.Reflection;

using RpcClient.Attr;
using RpcClient.Config;
using RpcClient.Interface;
using RpcClient.Route;

using SocketTcpServer.FileUp;
using SocketTcpServer.Interface;

using RpcHelper;

namespace RpcClient.Helper
{
        internal class RpcRouteHelper
        {
                private ITcpRoute _CreateRoute(string name, MethodInfo method)
                {
                        if (method.ReturnType == ConfigDic.VoidType)
                        {
                                return new VoidFuncRoute(name, method);
                        }
                        else if (method.ReturnType == ConfigDic.ReturnType)
                        {
                                return new Route.Route(name, method);
                        }
                        else if (RpcClientHelper.CheckIsOutRoute(method))
                        {
                                return new OutFuncRoute(name, method);
                        }
                        else
                        {
                                return new ReturnFuncRoute(name, method);
                        }
                }
                private void _LoadRoute(MethodInfo[] methods, Type form, Type to)
                {
                        if (methods.Length == 0)
                        {
                                return;
                        }
                        RpcClient.Unity.Register(form, to, to.FullName);
                        methods.ForEach(b =>
                        {
                                if (b.DeclaringType == to && b.IsPublic && !b.IsConstructor && !b.IsVirtual)//限定 是声明该方法的类 不能是虚方法和构造函数 需是公开的方法 
                                {
                                        string name = RpcClientHelper.GetRouteName(b);
                                        if (!RpcClient.Route.CheckIsExists(name))
                                        {
                                                ITcpRoute route = this._CreateRoute(name, b);
                                                if (route.VerificationRoute())
                                                {
                                                        RpcClient.Route.AddRoute(route);
                                                        RpcLogSystem.AddRouteLog(b, name, to);
                                                }
                                                else
                                                {
                                                        RpcLogSystem.AddRouteLog(b, name, to, "rpc.route.format.error");
                                                }
                                        }
                                        else
                                        {
                                                RpcLogSystem.AddRouteLog(b, name, to, "rpc.route.repeat");
                                        }
                                }
                        });
                }

                private void _LoadMethod(Type form, Type to)
                {
                        MethodInfo[] methods = to.GetMethods();
                        this._LoadRoute(methods, form, to);
                        if (to.BaseType.FullName != "System.Object")
                        {
                                this._LoadMethod(to.BaseType, to.BaseType);
                        }
                }
                public void AddRoute(Assembly assembly)
                {
                        if (assembly != null)
                        {
                                assembly.GetTypes().ForEach(a =>
                                {
                                        try
                                        {
                                                Type type = a.GetInterface(ConfigDic.RpcApiService.FullName);
                                                if (type != null)
                                                {
                                                        this._LoadMethod(ConfigDic.RpcApiService, a);
                                                        return;
                                                }
                                                RpcRouteGroup attr = a.GetCustomAttribute<RpcRouteGroup>();
                                                if (attr != null)
                                                {
                                                        this._LoadMethod(a, a);
                                                        return;
                                                }
                                                type = a.GetInterface(ConfigDic.IStreamAllot.FullName);
                                                if (type != null)
                                                {
                                                        if (RpcClient.Unity.Register(ConfigDic.IStreamAllot, a, a.FullName))
                                                        {
                                                                IStreamAllot stream = RpcClient.Unity.Resolve<IStreamAllot>(a.FullName);
                                                                FileUpRouteCollect.AddAllot(stream);
                                                        }
                                                }
                                        }
                                        catch (Exception e)
                                        {
                                                RpcLogSystem.AddErrorLog("路由加载错误!", a, ErrorException.FormatError(e));
                                        }
                                });
                        }
                }
        }
}
