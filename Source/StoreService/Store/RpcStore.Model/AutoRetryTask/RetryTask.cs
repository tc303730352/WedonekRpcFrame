using WeDonekRpc.ExtendModel;

namespace RpcStore.Model.AutoRetryTask
{
    public class RetryTask
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 所在机房
        /// </summary>
        public int RegionId { get; set; }
        /// <summary>
        /// 添加任务的节点
        /// </summary>
        public long ServerId { get; set; }

        /// <summary>
        /// 标识ID
        /// </summary>
        public string IdentityId { get; set; }

        /// <summary>
        /// 节点类型
        /// </summary>
        public string SystemType { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Show { get; set; }

        /// <summary>
        /// 重试状态
        /// </summary>
        public AutoRetryTaskStatus Status { get; set; }


        /// <summary>
        /// 负责重试节点ID
        /// </summary>
        public long RegServiceId { get; set; }


        public bool IsLock { get; set; }

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
        /// 添加时间
        /// </summary>
        public DateTime AddTime { get; set; }
    }
}
