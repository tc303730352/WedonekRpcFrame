using System.Threading.Tasks;
using WeDonekRpc.Client.Collect;
using WeDonekRpc.Client.Log;
using WeDonekRpc.Helper.Config;
using WeDonekRpc.Model.Config;
using WeDonekRpc.Model.CurConfig;

namespace WeDonekRpc.Client.Resource
{
    /// <summary>
    /// 
    /// </summary>
    internal class ConfigRemote
    {
        public static bool GetConfigVal (string name, out string value, out string error)
        {
            GetSysConfigVal obj = new GetSysConfigVal()
            {
                Name = name
            };
            return RemoteCollect.Send(obj, out value, out error);
        }
        public static bool GetConfig (out RemoteSysConfig config, out string error)
        {
            return RemoteCollect.Send(new GetSysConfig(), out config, out error);
        }
        public static void SendConfig (IConfigCollect config)
        {
            _ = Task.Factory.StartNew(() =>
            {
                SetCurConfig send = new SetCurConfig
                {
                    Config = config.GetConfigItem()
                };
                if (!RemoteCollect.Send(send, out string error))
                {
                    RpcLogSystem.AddFatalError("远程节点配置保存失败!", string.Empty, error);
                }
            });
        }
    }
}
