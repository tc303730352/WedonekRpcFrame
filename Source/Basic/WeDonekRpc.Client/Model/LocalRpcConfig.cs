namespace WeDonekRpc.Client.Model
{
    public class LocalRpcConfig
    {
        /// <summary>
        /// 服务组值
        /// </summary>
        public string SysGroup { get; set; }
        /// <summary>
        /// 服务节点类型值
        /// </summary>
        public string SystemType { get; set; }
        /// <summary>
        /// 服务节点类型ID
        /// </summary>
        public long SystemTypeId { get; set; }
        /// <summary>
        /// 服务节组ID
        /// </summary>
        public long SysGroupId { get; set; }
        /// <summary>
        /// 服务节点ID
        /// </summary>
        public long ServerId { get; set; }
        /// <summary>
        /// 服务集群ID
        /// </summary>
        public long RpcMerId { get; set; }
        /// <summary>
        /// 所在区域Id
        /// </summary>
        public int RegionId { get; set; }
        /// <summary>
        /// 容器组ID
        /// </summary>
        public long? ContGroup { get; set; }
    }
}
