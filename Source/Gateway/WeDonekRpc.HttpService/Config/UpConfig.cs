using WeDonekRpc.Helper.Config;

namespace WeDonekRpc.HttpService.Config
{
    public class UpConfig
    {
        public UpConfig ()
        {
            IConfigSection section = LocalConfig.Local.GetSection("gateway:up");
            section.AddRefreshEvent(this.Local_RefreshEvent);
        }
        private void Local_RefreshEvent (IConfigSection config, string name)
        {
            if (name == string.Empty || name == "MemoryUpSize")
            {
                this.MemoryUpSize = config.GetValue<long>("MemoryUpSize", 10485760);
            }
        }
        /// <summary>
        /// 限制使用内存做存储介质的文件大小
        /// </summary>
        public long MemoryUpSize
        {
            private set;
            get;
        }
    }
}
