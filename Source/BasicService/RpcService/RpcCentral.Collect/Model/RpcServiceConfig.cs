namespace RpcCentral.Collect.Model
{
    /// <summary>
    /// 节点配置
    /// </summary>
    public class RpcServiceConfig
    {
        /// <summary>
        /// 节点Id
        /// </summary>
        public long ServerId
        {
            get;
            set;
        }
        /// <summary>
        /// 服务编号
        /// </summary>
        public string ServerCode { get; set; }
        /// <summary>
        /// 权重
        /// </summary>
        public int Weight
        {
            get;
            set;
        }
        /// <summary>
        /// 节点版本号
        /// </summary>
        public int VerNum
        {
            get;
            set;
        }
        /// <summary>
        /// 区域Id
        /// </summary>
        public int RegionId
        {
            get;
            set;
        }

    }
}
