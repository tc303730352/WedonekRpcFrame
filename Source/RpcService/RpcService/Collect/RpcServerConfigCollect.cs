using System.Collections.Concurrent;

using RpcService.Controller;

using RpcHelper;

namespace RpcService.Collect
{
        internal class RpcServerConfigCollect
        {
                private static readonly ConcurrentDictionary<string, RpcServerConfigController> _ServerConfig = new ConcurrentDictionary<string, RpcServerConfigController>();
                static RpcServerConfigCollect()
                {
                        AutoResetDic.AutoReset(_ServerConfig, 1800);
                }


                public static void Refresh(long sysTypeId)
                {
                        if (RemoteServerGroupCollect.GetRpcMer(sysTypeId, out long[] rpcMerId, out string error))
                        {
                                rpcMerId.ForEach(a =>
                                {
                                        Refresh(a, sysTypeId);
                                });
                        }
                }

                internal static void Refresh(long rpcMerId, long sysTypeId)
                {
                        string key = string.Concat(rpcMerId, "_", sysTypeId);
                        if (_ServerConfig.TryGetValue(key, out RpcServerConfigController config))
                        {
                                config.ResetLock();
                        }
                }

                public static bool GetServerConfig(long rpcMerId, long sysTypeId, out RpcServerConfigController config)
                {
                        string key = string.Concat(rpcMerId, "_", sysTypeId);
                        if (!_ServerConfig.TryGetValue(key, out config))
                        {
                                config = _ServerConfig.GetOrAdd(key, new RpcServerConfigController(rpcMerId, sysTypeId));
                        }
                        if (!config.Init())
                        {
                                _ServerConfig.TryRemove(key, out config);
                                config.Dispose();
                                return false;
                        }
                        return config.IsInit;
                }
        }
}
