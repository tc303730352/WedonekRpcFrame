using System.Collections.Concurrent;
using System.Linq;

using ApiGateway.Interface;

using RpcHelper;
namespace ApiGateway.Collect
{
        internal class ModularCollect
        {
                private static readonly ConcurrentDictionary<string, IModular> _ModularList = new ConcurrentDictionary<string, IModular>();


                public static T GetModular<T>(string name) where T : IModular
                {
                        if (_ModularList.TryGetValue(name, out IModular modular))
                        {
                                return (T)modular;
                        }
                        return default;
                }
                public static void RegModular(IModular modular)
                {
                        if (_ModularList.ContainsKey(modular.ServiceName))
                        {
                                throw new ErrorException("http.modular.repeat");
                        }
                        else if (!_ModularList.TryAdd(modular.ServiceName, modular))
                        {
                                throw new ErrorException("http.modular.reg.fail");
                        }
                        else
                        {
                                modular.InitModular();
                        }
                }
                public static void Start()
                {
                        if (_ModularList.IsEmpty)
                        {
                                return;
                        }
                        string[] keys = _ModularList.Keys.ToArray();
                        keys.ForEach(a =>
                        {
                                _ModularList[a].Start();
                        });
                }
                public static void Close()
                {
                        if (_ModularList.IsEmpty)
                        {
                                return;
                        }
                        string[] keys = _ModularList.Keys.ToArray();
                        keys.ForEach(a =>
                        {
                                if (_ModularList.TryRemove(a, out IModular modular))
                                {
                                        modular.Dispose();
                                }
                        });
                }
        }
}
