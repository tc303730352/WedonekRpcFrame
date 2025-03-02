using System.Collections.Concurrent;
using RpcCentral.Collect.Controller;
using RpcCentral.DAL;
using WeDonekRpc.Helper;

namespace RpcCentral.Collect.Collect
{
    internal class RpcServerConfigCollect : IRpcServerConfigCollect
    {
        private static readonly ConcurrentDictionary<string, RpcServerConfigController> _ServerConfig = new ConcurrentDictionary<string, RpcServerConfigController>();
        static RpcServerConfigCollect ()
        {
            AutoResetDic.AutoReset(_ServerConfig, 1800);
        }

        private readonly IRemoteServerGroupDAL _ServerGroup;

        public RpcServerConfigCollect (IRemoteServerGroupDAL serverGroup)
        {
            this._ServerGroup = serverGroup;
        }

        public void Refresh (long sysTypeId)
        {
            long[] rpcMerId = this._ServerGroup.GetRpcMer(sysTypeId);
            rpcMerId.ForEach(a =>
             {
                 this.Refresh(a, sysTypeId);
             });
        }

        public void Refresh (long rpcMerId, long sysTypeId)
        {
            string key = string.Concat(rpcMerId, "_", sysTypeId);
            if (_ServerConfig.TryGetValue(key, out RpcServerConfigController config))
            {
                config.ResetLock();
            }
        }
        public RpcServerConfigController Get (long rpcMerId, long sysTypeId)
        {
            if (_GetServerConfig(rpcMerId, sysTypeId, out RpcServerConfigController config))
            {
                return config;
            }
            throw new ErrorException(config.Error);
        }
        private static bool _GetServerConfig (long rpcMerId, long sysTypeId, out RpcServerConfigController config)
        {
            string key = string.Concat(rpcMerId, "_", sysTypeId);
            if (!_ServerConfig.TryGetValue(key, out config))
            {
                config = _ServerConfig.GetOrAdd(key, new RpcServerConfigController(rpcMerId, sysTypeId));
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
