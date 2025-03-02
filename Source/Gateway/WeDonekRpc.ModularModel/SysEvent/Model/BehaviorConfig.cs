namespace WeDonekRpc.ModularModel.SysEvent.Model
{
    /// <summary>
    /// 性能配置
    /// </summary>
    public class BehaviorConfig
    {
        /// <summary>
        /// 发送超时阈值
        /// </summary>
        public uint Threshold
        {
            get;
            set;
        }

        /// <summary>
        /// 日志记录范围
        /// </summary>
        public LogRecordRange RecordRange
        {
            get;
            set;
        }
    }
}
