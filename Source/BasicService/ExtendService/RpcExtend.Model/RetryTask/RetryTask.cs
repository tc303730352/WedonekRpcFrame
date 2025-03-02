using WeDonekRpc.ExtendModel.RetryTask.Model;

namespace RpcExtend.Model.RetryTask
{
    public class RetryTask
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 添加任务的集群ID
        /// </summary>
        public long RpcMerId { get; set; }

        /// <summary>
        /// 区域ID
        /// </summary>
        public int RegionId { get; set; }

        /// <summary>
        /// 发送参数
        /// </summary>
        [SqlSugar.SugarColumn(IsJson = true)]
        public RpcParamConfig SendBody { get; set; }
        /// <summary>
        /// 重试配置
        /// </summary>
        [SqlSugar.SugarColumn(IsJson = true)]
        public RetryConfig RetryConfig { get; set; }

        /// <summary>
        /// 已经重试次数
        /// </summary>
        public int RetryNum { get; set; }

        public long NextRetryTime { get; set; }

    }
}
