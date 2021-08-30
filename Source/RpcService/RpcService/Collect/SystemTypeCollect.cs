using System.Collections.Concurrent;

using RpcService.Controller;

namespace RpcService.Collect
{
        internal class SystemTypeCollect
        {
                private static readonly ConcurrentDictionary<string, SystemTypeController> _SystemType = new ConcurrentDictionary<string, SystemTypeController>();

                public static bool GetSystemType(string type, out SystemTypeController systemType)
                {
                        if (!_SystemType.TryGetValue(type, out systemType))
                        {
                                systemType = _SystemType.GetOrAdd(type, new SystemTypeController(type));
                        }
                        if (!systemType.Init())
                        {
                                _SystemType.TryRemove(type, out systemType);
                                systemType.Dispose();
                                return false;
                        }
                        return systemType.IsInit;
                }


        }
}
