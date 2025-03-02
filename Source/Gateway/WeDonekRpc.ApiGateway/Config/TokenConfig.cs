namespace WeDonekRpc.ApiGateway.Config
{
    /// <summary>
    /// 临牌配置
    /// </summary>
    public class TokenConfig
    {
        /// <summary>
        /// 限定令牌数
        /// </summary>
        public short TokenNum
        {
            get;
            set;
        } = 1000;

        /// <summary>
        /// 令牌每秒新增数
        /// </summary>
        public short TokenInNum { get; set; } = 10;
    }
}
