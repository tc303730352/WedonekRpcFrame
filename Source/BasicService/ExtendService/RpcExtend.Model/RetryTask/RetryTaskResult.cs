using WeDonekRpc.ExtendModel;

namespace RpcExtend.Model.RetryTask
{
    public class RetryTaskResult
    {
        public long Id { get; set; }

        /// <summary>
        /// 重试状态
        /// </summary>
        public AutoRetryTaskStatus Status { get; set; }


        /// <summary>
        /// 下次重试时间
        /// </summary>
        public long NextRetryTime { get; set; }

        /// <summary>
        /// 已经重试次数
        /// </summary>
        public int RetryNum { get; set; }

        /// <summary>
        /// 最后次错误码
        /// </summary>
        public string ErrorCode { get; set; }


        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime? ComplateTime { get; set; }
        /// <summary>
        /// 是否锁定
        /// </summary>
        public bool IsLock { get; set; }
    }
}
