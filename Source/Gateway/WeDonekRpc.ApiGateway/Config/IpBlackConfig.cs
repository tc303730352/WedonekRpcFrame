using System;
using WeDonekRpc.ApiGateway.Interface;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Helper.Config;

namespace WeDonekRpc.ApiGateway.Config
{
    /// <summary>
    /// IP黑名单配置
    /// </summary>
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    public class IpBlackConfig : IIpBlackConfig
    {
        private Action<string> _Refresh;
        public IpBlackConfig (IConfigCollect config)
        {
            IConfigSection section = config.GetSection("gateway:ipBack");
            section.AddRefreshEvent(this._Init);
        }

        private void _Init (IConfigSection section, string name)
        {
            this.IsEnable = section.GetValue<bool>("IsEnable", false);
            this.IsLocal = section.GetValue<bool>("IsLocal", true);
            this.Local = section.GetValue("Local", new IpBlackLocal());
            this.Remote = section.GetValue("Remote", new IpBlackRemote());
            if (this._Refresh != null)
            {
                this._Refresh(name);
            }
        }
        public void AddRefreshEvent (Action<string> action)
        {
            this._Refresh = action;
            action(null);
        }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable
        {
            get;
            private set;
        } = false;
        /// <summary>
        /// 是否使用本地名单
        /// </summary>
        public bool IsLocal
        {
            get;
            private set;
        } = true;
        /// <summary>
        /// 本地配置
        /// </summary>
        public IpBlackLocal Local
        {
            get;
            private set;
        }

        /// <summary>
        /// 远程
        /// </summary>
        public IpBlackRemote Remote
        {
            get;
            private set;
        }
    }
    /// <summary>
    /// 远端黑名单配置
    /// </summary>
    public class IpBlackRemote
    {
        /// <summary>
        /// 启用本地缓存
        /// </summary>
        public bool EnableCache
        {
            get;
            set;
        } = true;
        /// <summary>
        /// 黑名单缓存路径
        /// </summary>
        public string CachePath
        {
            get;
            set;
        } = "BlackCache";

        /// <summary>
        /// 同步缓存的时间(秒)
        /// </summary>
        public int SyncVerTime
        {
            get;
            set;
        } = 120;
    }
    /// <summary>
    /// 本地IP黑名单
    /// </summary>
    public class IpBlackLocal
    {
        /// <summary>
        /// 保存黑名单的目录
        /// </summary>
        public string DirPath
        {
            get;
            set;
        } = "Black";
        /// <summary>
        /// 数据同步时间(秒)
        /// </summary>
        public int SyncTime
        {
            get;
            set;
        } = 120;
    }
}
