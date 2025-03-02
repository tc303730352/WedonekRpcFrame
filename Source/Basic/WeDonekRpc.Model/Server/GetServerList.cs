namespace WeDonekRpc.Model.Server
{
    /// <summary>
    /// 获取节点服务列表
    /// </summary>
    public class GetServerList
    {
        /// <summary>
        /// 节点类型
        /// </summary>
        public string SystemType
        {
            get;
            set;
        }

        /// <summary>
        /// 集群Id
        /// </summary>
        public long RpcMerId { get; set; }
        /// <summary>
        /// 限定使用区域
        /// </summary>
        public int? LimitRegionId { get; set; }

        public Source Source { get; set; }
        /// <summary>
        /// 负载均衡版本号
        /// </summary>
        public string TransmitVer { get; set; }
        /// <summary>
        /// 配置版本
        /// </summary>
        public int ConfigVer { get; set; }
    }
    public class Source
    {
        public long RpcMerId { get; set; }
        public long SystemTypeId { get; set; }
        public int Ver { get; set; }
        public int RegionId { get; set; }
    }
}
