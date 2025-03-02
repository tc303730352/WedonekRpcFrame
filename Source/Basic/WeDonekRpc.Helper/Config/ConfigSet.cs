using System;

namespace WeDonekRpc.Helper.Config
{
    internal class ConfigSet
    {
        private readonly Action<ConfigSet, string> _RefreshEvent;
        public ConfigSet (IConfigSection section, Action<ConfigSet, string> refresh)
        {
            this._RefreshEvent = refresh;
            section.AddRefreshEvent(this._Refresh);
        }

        private void _Refresh (IConfigSection section, string name)
        {
            this.AutoLoad = section.GetValue<bool>("AutoLoad", true);
            this.CheckTime = section.GetValue<int>("CheckTime", 10);
            this.Prower = section.GetValue<short>("Prower", 10);
            this.AutoSave = section.GetValue<bool>("AutoSave", true);
            this.SaveTime = section.GetValue<int>("SaveTime", 10);
            this._RefreshEvent(this, name);
        }

        /// <summary>
        /// 检查变更加载
        /// </summary>
        public bool AutoLoad
        {
            get;
            private set;
        } = true;
        /// <summary>
        /// 检查间隔
        /// </summary>
        public int CheckTime
        {
            get;
            private set;
        } = 10;
        /// <summary>
        /// 覆盖的权限值
        /// </summary>
        public short Prower
        {
            get;
            private set;
        } = 10;
        /// <summary>
        /// 自动保存
        /// </summary>
        public bool AutoSave
        {
            get;
            private set;
        } = true;
        /// <summary>
        /// 自动保存间隔(秒)
        /// </summary>
        public int SaveTime
        {
            get;
            private set;
        } = 10;
    }
}
