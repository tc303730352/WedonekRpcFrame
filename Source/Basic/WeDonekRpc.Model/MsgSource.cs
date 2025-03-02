namespace WeDonekRpc.Model
{
    public class MsgSource
    {
        /// <summary>
        /// 服务节点类型ID
        /// </summary>
        public long SystemTypeId { get; set; }
        /// <summary>
        /// 服务节点类型
        /// </summary>
        public string SystemType { get; set; }
        /// <summary>
        /// 服务节组
        /// </summary>
        public string SysGroup { get; set; }
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
        /// 容器类型
        /// </summary>
        public long? ContGroup { get; set; }
        /// <summary>
        /// 应用版本号
        /// </summary>
        public int VerNum { get; set; }
    }
}
