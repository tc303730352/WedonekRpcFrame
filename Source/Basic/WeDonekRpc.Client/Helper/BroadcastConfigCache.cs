using WeDonekRpc.Client.Config;
using WeDonekRpc.Model;
using System;
using System.Collections.Concurrent;

namespace WeDonekRpc.Client.Helper
{
    internal class BroadcastConfigCache
    {
        private static readonly ConcurrentDictionary<string, IRemoteBroadcast> _BroadcastConfig = new ConcurrentDictionary<string, IRemoteBroadcast>();

        public static bool GetBroadcastConfig (Type type, out IRemoteBroadcast config)
        {
            string name = type.FullName;
            if (_BroadcastConfig.TryGetValue(name, out config))
            {
                return true;
            }
            object[] datas = type.GetCustomAttributes(ConfigDic.BroadcastType, true);
            if (datas == null || datas.Length == 0)
            {
                return false;
            }
            IRemoteBroadcast obj = (IRemoteBroadcast)datas[0];
            obj.InitConfig(type, WebConfig.RpcConfig.MaxRetryNum);
            if (RemoteConfigCache.GetRemoteConfig(type, out IRemoteConfig rConfig))
            {
                obj.RemoteConfig = rConfig.ToRemoteSet();
            }
            config = _BroadcastConfig.GetOrAdd(name, obj);
            return true;
        }

    }
}
