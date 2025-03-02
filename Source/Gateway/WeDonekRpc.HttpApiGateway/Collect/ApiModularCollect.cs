using System.Collections.Concurrent;
using WeDonekRpc.ApiGateway;
using WeDonekRpc.Helper;
using WeDonekRpc.HttpApiGateway.Interface;

namespace WeDonekRpc.HttpApiGateway.Collect
{
    internal class ApiModularCollect : IApiModularService
    {
        private static readonly ConcurrentDictionary<string, IApiModular> _ModularList = new ConcurrentDictionary<string, IApiModular>();

        public static void RegModular (IApiModular modular)
        {
            if (_ModularList.ContainsKey(modular.ServiceName))
            {
                throw new ErrorException("gateway.http.modular.repeat");
            }
            else if (!_ModularList.TryAdd(modular.ServiceName, modular))
            {
                throw new ErrorException("gateway.http.reg.fail");
            }
            ModularService.TryAddModular(modular);
        }

        public IApiModular GetModular (string serviceName)
        {
            if (!_ModularList.TryGetValue(serviceName, out IApiModular modular))
            {
                throw new ErrorException("gateway.http.modular.not.find");
            }
            return modular;
        }
    }
}
