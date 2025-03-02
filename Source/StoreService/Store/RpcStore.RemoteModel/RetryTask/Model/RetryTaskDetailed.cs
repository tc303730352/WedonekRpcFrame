using WeDonekRpc.ExtendModel.RetryTask.Model;

namespace RpcStore.RemoteModel.RetryTask.Model
{
    public class RetryTaskDetailed : RetryTaskDatum
    {
        /// <summary>
        /// 发送参数
        /// </summary>
        public RpcParamConfig SendBody { get; set; }

        /// <summary>
        /// 重试配置
        /// </summary>
        public RetryConfig RetryConfig { get; set; }

        /// <summary>
        /// 日志列表
        /// </summary>
        public RetryTaskLog[] Logs { get; set; }
    }
}
