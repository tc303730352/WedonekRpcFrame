using WeDonekRpc.Model;
using RpcSync.Collect;
using RpcSync.Collect.Model;
using RpcSync.Service.ConfigService;
using RpcSync.Service.Interface;
using WeDonekRpc.Model.Config;

namespace RpcSync.Service.Service
{
    internal class SysConfigService : ISysConfigService
    {
        private readonly ISysConfigCollect _SysConfig;
        private readonly ISystemTypeCollect _SystemType;

        public SysConfigService (ISysConfigCollect sysConfig, ISystemTypeCollect systemType)
        {
            this._SysConfig = sysConfig;
            this._SystemType = systemType;
        }

        public RemoteSysConfig GetSysConfig (MsgSource source)
        {
            RpcServerType serverType = this._SystemType.GetServerType(source.SystemTypeId);
            SysConfigItem item = this._SysConfig.GetSysConfig(source.SystemType);
            return new RemoteSysConfig
            {
                ConfigMd5 = item.Md5,
                ConfigData = item.Filters(source, serverType)
            };

        }
    }
}
