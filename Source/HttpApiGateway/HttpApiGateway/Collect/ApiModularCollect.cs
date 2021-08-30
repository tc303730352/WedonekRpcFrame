using System.Collections.Concurrent;

using HttpApiGateway.Interface;

namespace HttpApiGateway.Collect
{
        internal class ApiModularCollect
        {
                private static readonly ConcurrentDictionary<string, IApiModular> _ModularList = new ConcurrentDictionary<string, IApiModular>();


                public static bool RegModular(IApiModular modular, out string error)
                {
                        if (_ModularList.ContainsKey(modular.ServiceName))
                        {
                                error = "http.api.modular.repeat";
                                return false;
                        }
                        else if (!_ModularList.TryAdd(modular.ServiceName, modular))
                        {
                                error = "http.api.modular.reg.error";
                                return false;
                        }
                        else
                        {
                                error = null;
                                return true;
                        }
                }
        }
}
