using System.Collections.Concurrent;

using RpcSyncService.Controller;

namespace RpcSyncService.Collect
{
        public class RemoteServerTypeCollect
        {
                private static readonly ConcurrentDictionary<string, SystemTypeController> _SystemTypeList = new ConcurrentDictionary<string, SystemTypeController>();
                public static bool GetSystemType(string type, out SystemTypeController systemType)
                {
                        if (!_SystemTypeList.TryGetValue(type, out systemType))
                        {
                                systemType = _SystemTypeList.GetOrAdd(type, new SystemTypeController(type));
                        }
                        if (!systemType.Init())
                        {
                                _SystemTypeList.TryRemove(type, out _);
                                systemType.Dispose();
                                return false;
                        }
                        return systemType.IsInit;
                }
        }
}
