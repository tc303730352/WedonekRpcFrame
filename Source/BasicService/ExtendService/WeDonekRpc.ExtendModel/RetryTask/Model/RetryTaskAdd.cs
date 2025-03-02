namespace WeDonekRpc.ExtendModel.RetryTask.Model
{
    public class RetryTaskAdd
    {
        /// <summary>
        /// 说明
        /// </summary>
        public string Show { get; set; }

        /// <summary>
        /// 发送参数
        /// </summary>
        public RpcParamConfig SendBody { get; set; }
        /// <summary>
        /// 重试配置
        /// </summary>
        public RetryConfig RetryConfig { get; set; }

        /// <summary>
        /// 标识ID
        /// </summary>
        public string IdentityId { get; set; }
    }
}
