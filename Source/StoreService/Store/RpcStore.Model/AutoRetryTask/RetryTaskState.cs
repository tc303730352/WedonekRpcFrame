using WeDonekRpc.ExtendModel;

namespace RpcStore.Model.AutoRetryTask
{
    public class RetryTaskState
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        public long Id { get; set; }


        /// <summary>
        /// 标识ID
        /// </summary>
        public string IdentityId { get; set; }

        /// <summary>
        /// 重试状态
        /// </summary>
        public AutoRetryTaskStatus Status { get; set; }


        /// <summary>
        /// 负责重试节点ID
        /// </summary>
        public long RegServiceId { get; set; }
    }
}
