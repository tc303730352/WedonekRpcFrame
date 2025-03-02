using WeDonekRpc.Client;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Config;

namespace WeDonekRpc.Modular.Config
{
    /// <summary>
    /// 授权配置
    /// </summary>
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    public class AccreditConfig : IAccreditConfig
    {
        public AccreditConfig ()
        {
            RpcClient.Config.GetSection("rpcassembly:Accredit").AddRefreshEvent(this._Refresh);
        }

        private void _Refresh (IConfigSection section, string name)
        {
            string roleType = section.GetValue("RoleType");
            if (roleType.IsNull() && RpcClient.IsInit)
            {
                roleType = RpcClient.GroupTypeVal;
            }
            this._RoleType = roleType;
            this.RefreshTime = section.GetValue<int>("RefreshTime", 120);
            this.ErrorVaildTime = section.GetValue<int>("ErrorVaildTime", 5);
            this.MinCheckTime = section.GetValue<int>("MinCheckTime", 5);
            this.MaxCheckime = section.GetValue<int>("MaxCheckime", 60);
            this.MinCacheTime = section.GetValue<int>("MinCacheTime", 240);
            this.MaxCacheTime = section.GetValue<int>("MaxCacheTime", 360);
        }
        private string _RoleType = null;
        /// <summary>
        /// 角色类型
        /// </summary>
        public string RoleType
        {
            get
            {
                if (this._RoleType.IsNull())
                {
                    this._RoleType = RpcClient.GroupTypeVal;
                }
                return this._RoleType;
            }
        }
        /// <summary>
        /// 授权刷新时间(秒)
        /// </summary>
        public int RefreshTime { get; private set; } = 120;

        /// <summary>
        /// 授权信息本地过期时间
        /// </summary>
        public int ErrorVaildTime { get; private set; } = 5;
        /// <summary>
        /// 授权信息最小同步时间
        /// </summary>
        public int MinCheckTime { get; private set; } = 5;

        /// <summary>
        /// 授权信息最晚同步时间
        /// </summary>
        public int MaxCheckime { get; private set; } = 60;
        /// <summary>
        /// 授权信息本地最小缓存时间
        /// </summary>
        public int MinCacheTime { get; private set; } = 240;
        /// <summary>
        /// 授权信息本地最大缓存时间
        /// </summary>
        public int MaxCacheTime { get; private set; } = 360;

        public int GetNextCheckTime (int time)
        {
            return time + Tools.GetRandom(this.MinCheckTime, this.MaxCheckime);
        }
        public int GetCacheVaildTime ()
        {
            return HeartbeatTimeHelper.HeartbeatTime + Tools.GetRandom(this.MinCacheTime, this.MaxCacheTime);
        }
    }
}
