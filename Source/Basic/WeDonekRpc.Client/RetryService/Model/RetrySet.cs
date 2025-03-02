namespace WeDonekRpc.Client.RetryService.Model
{
    public class RetrySet
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
        /// 限定停止重试的错误码
        /// </summary>
        public string[] StopErrorCode
        {
            get;
            set;
        }
        /// <summary>
        /// 启动延迟
        /// </summary>
        public int? StartInterval { get; set; }

        /// <summary>
        /// 最大重试时间
        /// </summary>
        public int? MaxRetryTime { get; set; }
    }
}
