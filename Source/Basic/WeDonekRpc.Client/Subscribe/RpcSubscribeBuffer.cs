using WeDonekRpc.Client.Collect;
using WeDonekRpc.Client.Config;
using WeDonekRpc.Client.Ioc;
using WeDonekRpc.Client.Log;
using WeDonekRpc.Helper;
using System;
using System.Reflection;

namespace WeDonekRpc.Client.Subscribe
{
    internal class RpcSubscribeBuffer
    {
        private IocBuffer _Ioc;
        public RpcSubscribeBuffer(IocBuffer ioc)
        {
            _Ioc = ioc;
        }
        private void _LoadRoute(MethodInfo[] methods, Type form, Type to)
        {
            if (methods.Length == 0)
            {
                return;
            }
            _Ioc.Register(form, to, to.FullName);
            methods.ForEach(b =>
            {
                if (b.DeclaringType == to)//限定 是声明该方法的类 不能是虚方法和构造函数 需是公开的方法 
                {
                    string name = SubscribeHelper.GetRouteName(b);
                    if (!RpcSubscribeCollect.Add(new SubscribeEvent(name, b)))
                    {
                        RouteLog.AddRouteLog(b, name, to, "rpc.subscribe.add.fail");
                        return;
                    }
                    RouteLog.AddRouteLog(b, name, to);
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
        internal void AddRoute(Type type)
        {
            Type subType= type.GetInterface(ConfigDic.RpcSubscribeService.FullName);
            if (subType != null)
            {
                this._LoadMethod(ConfigDic.RpcSubscribeService, type);
            }
        }
    }
}
