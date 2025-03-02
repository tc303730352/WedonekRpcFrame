using RpcSync.Service.Interface;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Model;
using WeDonekRpc.Model.Config;
namespace RpcSync.Service.Event
{
    /// <summary>
    /// 系统配置服务
    /// </summary>
    internal class RpcSysConfigEvent : IRpcApiService
    {
        private readonly ISysConfigService _SysConfig;
        private readonly IConfigRefreshService _Refresh;
        public RpcSysConfigEvent (ISysConfigService sysConfig, IConfigRefreshService refresh)
        {
            this._Refresh = refresh;
            this._SysConfig = sysConfig;
        }
        /// <summary>
        /// 刷新系统配置
        /// </summary>
        public void RefreshSysConfig ()
        {
            this._Refresh.Refresh();
        }

        /// <summary>
        /// 获取系统配置
        /// </summary>
        /// <param name="source">事件原</param>
        /// <returns></returns>
        public RemoteSysConfig GetSysConfig (MsgSource source)
        {
            return this._SysConfig.GetSysConfig(source);
        }
    }
}
