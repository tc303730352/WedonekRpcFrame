using WeDonekRpc.Client;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Helper.Config;

namespace RpcSync.Service.ConfigService
{
    [IocName("RpcSyncConfig")]
    internal class ConfigExtendService : IRpcExtendService
    {
        public void Load (IRpcService service)
        {
            service.BeginIniting += this.Service_BeginIniting;
        }

        private void Service_BeginIniting ()
        {
            LocalConfig.Local.SetValue<bool>("rpc_config:IsEnableError", false, int.MaxValue);
            RpcClient.Config.SetLocalServer(new ConfigServer());
        }
    }
}
