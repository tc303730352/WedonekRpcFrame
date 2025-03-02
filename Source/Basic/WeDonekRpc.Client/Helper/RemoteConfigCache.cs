using WeDonekRpc.Client.Config;
using WeDonekRpc.Model;
using System;
using System.Collections.Concurrent;

namespace WeDonekRpc.Client.Helper
{
    internal class RemoteConfigCache
    {
        private static readonly ConcurrentDictionary<string, IRemoteConfig> _RemoteConfig = new ConcurrentDictionary<string, IRemoteConfig>();

       
        public static bool GetRemoteConfig(Type type, out IRemoteConfig config)
        {
            string name = type.FullName;
            if (_RemoteConfig.TryGetValue(name, out config))
            {
                return true;
            }
            object[] datas = type.GetCustomAttributes(ConfigDic.RemoteConfigType, true);
            if (datas == null || datas.Length == 0)
            {
                return false;
            }
            config = _RemoteConfig.GetOrAdd(name, (IRemoteConfig)datas[0]);
            config.InitConfig(type, WebConfig.RpcConfig.MaxRetryNum);
            return true;
        }
    }
}
