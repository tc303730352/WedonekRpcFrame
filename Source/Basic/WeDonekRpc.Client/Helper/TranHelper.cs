using System;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Helper
{
    internal class TranHelper
    {
        /// <summary>
        /// 重指令对象中获取事务名称
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="ErrorException"></exception>
        public static string GetTranName (Type type)
        {
            if (RemoteConfigCache.GetRemoteConfig(type, out IRemoteConfig config))
            {
                return config.SysDictate;
            }
            else if (BroadcastConfigCache.GetBroadcastConfig(type, out IRemoteBroadcast broadcast))
            {
                return broadcast.SysDictate;
            }
            throw new ErrorException("rpc.tran.name.not.find");
        }
    }
}
