using System.Collections.Concurrent;
using RpcCentral.Collect.Controller;
using WeDonekRpc.Helper;

namespace RpcCentral.Collect.Collect
{
    internal class RpcConfigCollect : IRpcConfigCollect
    {
        private static readonly ConcurrentDictionary<string, RpcConfigController> _ServerConfig = new ConcurrentDictionary<string, RpcConfigController>();
        static RpcConfigCollect ()
        {
            AutoResetDic.AutoReset(_ServerConfig, 1800);
        }


        public void RefreshVerNum (long rpcMerId, long sysTypeId, int verNum)
        {
            string key = string.Concat(rpcMerId, "_", sysTypeId);
            if (_ServerConfig.TryGetValue(key, out RpcConfigController config))
            {
                config.RefreshVer(verNum);
            }
        }
        public void Refresh (long rpcMerId, long sysTypeId)
        {
            string key = string.Concat(rpcMerId, "_", sysTypeId);
            if (_ServerConfig.TryGetValue(key, out RpcConfigController config))
            {
                config.ResetLock();
            }
        }
        public RpcConfigController Get (long rpcMerId, long sysTypeId)
        {
            if (_GetConfig(rpcMerId, sysTypeId, out RpcConfigController config))
            {
                return config;
            }
            throw new ErrorException(config.Error);
        }
        private static bool _GetConfig (long rpcMerId, long sysTypeId, out RpcConfigController config)
        {
            string key = rpcMerId + "_" + sysTypeId;
            if (!_ServerConfig.TryGetValue(key, out config))
            {
                config = _ServerConfig.GetOrAdd(key, new RpcConfigController(rpcMerId, sysTypeId));
            }
            if (!config.Init())
            {
                _ = _ServerConfig.TryRemove(key, out config);
                config.Dispose();
                return false;
            }
            return config.IsInit;
        }
    }
}
