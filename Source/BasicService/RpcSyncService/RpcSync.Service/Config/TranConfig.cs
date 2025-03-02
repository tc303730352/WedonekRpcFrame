using RpcSync.Service.Interface;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Helper.Config;

namespace RpcSync.Service.Config
{
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    internal class TranConfig : ITranConfig
    {
        public TranConfig (IConfigCollect config)
        {
            IConfigSection section = config.GetSection("tran");
            section.AddRefreshEvent(this._InitConfig);
        }
        /// <summary>
        /// 初始化配置(进入条件 第一次初始化或配置项中值变更)
        /// </summary>
        /// <param name="config">当前配置项</param>
        /// <param name="name">更改的配置名(第一次初始化时为string.Empty)</param>
        private void _InitConfig (IConfigSection config, string name)
        {
            this.TranRollbackRetryNum = config.GetValue<short>("RollbackRetryNum", 3);
            this.TranCommitRetryNum = config.GetValue<short>("CommitRetryNum", 3);
        }

        /// <summary>
        /// 事务回滚重试数
        /// </summary>
        public short TranRollbackRetryNum { get; private set; } = 3;

        public short TranCommitRetryNum { get; private set; } = 3;
    }
}
