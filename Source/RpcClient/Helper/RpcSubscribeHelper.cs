using System;
using System.Reflection;

using RpcClient.Collect;
using RpcClient.Config;

using RpcHelper;

namespace RpcClient.Helper
{
        internal class RpcSubscribeHelper
        {
                private void _LoadRoute(MethodInfo[] methods, Type form, Type to)
                {
                        if (methods.Length == 0)
                        {
                                return;
                        }
                        RpcClient.Unity.Register(form, to, to.FullName);
                        methods.ForEach(b =>
                        {
                                if (b.DeclaringType == to)//限定 是声明该方法的类 不能是虚方法和构造函数 需是公开的方法 
                                {
                                        string name = RpcClientHelper.GetRouteName(b);
                                        if (!RpcSubscribeCollect.Add(new Subscribe.SubscribeEvent(name, b)))
                                        {
                                                RpcLogSystem.AddRouteLog(b, name, to, "rpc.subscribe.add.fail");
                                                return;
                                        }
                                        RpcLogSystem.AddRouteLog(b, name, to);
                                }
                        });
                }
                private void _LoadMethod(Type form, Type to)
                {
                        MethodInfo[] methods = to.GetMethods().FindAll(a => a.ReturnType == PublicDataDic.VoidType && a.IsPublic && !a.IsConstructor && !a.IsVirtual);
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
                                                Type type = a.GetInterface(ConfigDic.RpcSubscribeService.FullName);
                                                if (type != null)
                                                {
                                                        this._LoadMethod(ConfigDic.RpcSubscribeService, a);
                                                }
                                        }
                                        catch (Exception e)
                                        {
                                                RpcLogSystem.AddErrorLog("订阅类加载错误!", a, ErrorException.FormatError(e));
                                        }
                                });
                        }
                }
        }
}
