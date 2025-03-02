using WeDonekRpc.Helper.Config;

namespace WeDonekRpc.Client.Config
{
    internal class EnvironmentUploadConfig
    {
        public EnvironmentUploadConfig ()
        {
            IConfigSection section = LocalConfig.Local.GetSection("rpc:Environment");
            this.DelayInterval = section.GetValue<int>("Delay", 30) * 1000;
            this.CheckTime = section.GetValue<int>("Time", 2);
        }
        /// <summary>
        /// 检查间隔
        /// </summary>
        public int DelayInterval
        {
            get;
            private set;
        }
        /// <summary>
        /// 检查次数
        /// </summary>
        public int CheckTime { get; private set; }
    }
}
