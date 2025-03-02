using RpcCentral.Common;
using RpcCentral.Service.Interface;
using WeDonekRpc.Helper;
using System.Reflection;

namespace RpcCentral.Service.TcpService
{
    internal class TcpRouteService
    {
        public static ITcpRoute GetRoute(string name)
        {
            return UnityHelper.Resolve<ITcpRoute>(name);
        }
        public static void InitRoute(IocBuffer buffer)
        {
            Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().Find(a => a.GetName().Name == "RpcCentral.Service");
            _LoadRoute(assembly, buffer);
        }
        private static void _LoadRoute(Assembly assembly, IocBuffer buffer)
        {
            Type iType = typeof(ITcpRoute);
            Type[] types = assembly.GetTypes();
            types.ForEach(a =>
            {
                Type type = a.GetInterface(iType.Name);
                if (type != null && !a.Name.StartsWith("TcpRoute") && a.IsClass && a.GetConstructors().Count(b =>
                {
                    ParameterInfo[] param = b.GetParameters();
                    return param.IsNull() || !param.IsExists(c => !c.ParameterType.IsInterface);
                }) != 0)
                {
                    string name = a.Name;
                    if (name.EndsWith("Event"))
                    {
                        name = name.Remove(name.Length - 5);
                    }
                    buffer.Register(iType, a, name);
                }
            });
        }
    }
}
