using RpcCentral.Collect;
using RpcCentral.Collect.Model;
using RpcCentral.Common;
using RpcCentral.Service.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using System.Reflection;

namespace RpcCentral.Service.RpcEvent
{
    internal class RpcEventService : IRpcEventService
    {
        private IRpcTokenCollect _Token;
        public RpcEventService(IRpcTokenCollect token)
        {
            this._Token = token;
        }

        public void Execate(RefreshRpc obj)
        {
            RpcTokenCache token = _Token.Get(obj.TokenId);
            IRpcEvent rpcEvent = UnityHelper.Resolve<IRpcEvent>(obj.EventKey);
            if (rpcEvent != null)
            {
                rpcEvent.Refresh(token, obj.Param);
            }
        }
        public static void InitEvent(IocBuffer buffer)
        {
            Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().Find(a => a.GetName().Name == "RpcCentral.Service");
            _LoadRoute(assembly, buffer);
        }
        private static void _LoadRoute(Assembly assembly,IocBuffer buffer)
        {
            Type iType = typeof(IRpcEvent);
            Type[] types = assembly.GetTypes();
            types.ForEach(a =>
            {
                Type type = a.GetInterface(iType.Name);
                if (type != null && !a.Name.StartsWith("RpcEvent") && a.IsClass && a.GetConstructors().Count(b =>
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
