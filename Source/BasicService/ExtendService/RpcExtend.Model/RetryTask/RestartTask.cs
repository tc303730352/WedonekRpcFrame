using WeDonekRpc.ExtendModel;

namespace RpcExtend.Model.RetryTask
{
    public class RestartTask
    {
        /// <summary>
        /// 重试状态
        /// </summary>
        public AutoRetryTaskStatus Status { get; set; }


        /// <summary>
        /// 下次重试时间
        /// </summary>
        public long NextRetryTime { get; set; }

        public bool IsLock { get; set; }
    }
}
