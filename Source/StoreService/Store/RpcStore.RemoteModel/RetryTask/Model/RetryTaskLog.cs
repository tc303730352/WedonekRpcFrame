namespace RpcStore.RemoteModel.RetryTask.Model
{
    public class RetryTaskLog
    {
        /// <summary>
        /// 日志ID
        /// </summary>
        public long Id { get; set; }
        
        /// <summary>
        /// 是否失败
        /// </summary>
        public bool IsFail { get; set; }

        /// <summary>
        /// 当前次数
        /// </summary>
        public int RetryNum { get; set; }

        /// <summary>
        /// 最后次错误码
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// 时长
        /// </summary>
        public int Duration { get; set; }
        /// <summary>
        /// 运行时间
        /// </summary>
        public DateTime RunTime { get; set; }
    }
}
