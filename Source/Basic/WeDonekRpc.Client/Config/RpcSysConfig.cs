using WeDonekRpc.Helper.Config;

namespace WeDonekRpc.Client.Config
{

    /// <summary>
    /// 系统配置
    /// </summary>
    public class RpcSysConfig
    {
        public RpcSysConfig ()
        {
            RpcClient.Config.GetSection("rpc:Config").AddRefreshEvent(this._InitConfig);
        }

        private void _InitConfig (IConfigSection section, string name)
        {
            this.TranOverTime = section.GetValue("TranOverTime", 90);
            this.CloseDelayTime = section.GetValue("CloseDelayTime", 10);
            this.ExpireTime = section.GetValue("ExpireTime", 10);
            this.LockTimeOut = section.GetValue("LockTimeOut", 15);
            this.LockValidTime = section.GetValue("LockValidTime", 2);
            this.MaxRetryNum = section.GetValue("MaxRetryNum", 3);
            this.IsValidateData = section.GetValue("IsValidateData", false);
            this.IsEnableQueue = section.GetValue("IsEnableQueue", true);
        }

        /// <summary>
        /// 远程事务超时时间
        /// </summary>
        public int TranOverTime
        {
            get;
            private set;
        }
        /// <summary>
        /// 关闭服务延迟时间(秒)
        /// </summary>
        public int CloseDelayTime { get; private set; }

        /// <summary>
        /// 消息过期时间(秒)
        /// </summary>
        public int ExpireTime { get; private set; }


        /// <summary>
        /// 锁超时时间
        /// </summary>
        public int LockTimeOut { get; private set; }

        /// <summary>
        /// 远程锁超时时间
        /// </summary>
        public int LockValidTime { get; private set; }

        /// <summary>
        /// 消息最大重试数
        /// </summary>
        public int MaxRetryNum { get; private set; }
        /// <summary>
        /// 是否验证数据
        /// </summary>
        public bool IsValidateData { get; private set; }
        /// <summary>
        /// 是否启用队列
        /// </summary>
        public bool IsEnableQueue { get; private set; }

    }
}
