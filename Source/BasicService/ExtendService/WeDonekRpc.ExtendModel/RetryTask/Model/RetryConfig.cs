namespace WeDonekRpc.ExtendModel.RetryTask.Model
{
    /// <summary>
    /// 重试配置
    /// </summary>
    public class RetryConfig
    {
        /// <summary>
        /// 最大重试数量
        /// </summary>
        public int? MaxRetry
        {
            get;
            set;
        }
        /// <summary>
        /// 重试间隔(秒)
        /// </summary>
        public int[] Interval
        {
            get;
            set;
        }
        /// <summary>
        /// 重试开始时间
        /// </summary>
        public long? Begin
        {
            get;
            set;
        }
        /// <summary>
        /// 重试截止时间
        /// </summary>
        public long? End
        {
            get;
            set;
        }
        /// <summary>
        /// 限定停止重试的错误码
        /// </summary>
        public string[] StopErrorCode
        {
            get;
            set;
        }
    }
}
