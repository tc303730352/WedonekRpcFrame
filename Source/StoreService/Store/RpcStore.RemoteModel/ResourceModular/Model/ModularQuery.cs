namespace RpcStore.RemoteModel.ResourceModular.Model
{
    /// <summary>
    /// 模块查询
    /// </summary>
    public class ModularQuery
    {
        /// <summary>
        /// 查询关键字
        /// </summary>
        public string QueryKey { get; set; }
        /// <summary>
        /// 服务集群ID
        /// </summary>
        public long? RpcMerId { get; set; }
        /// <summary>
        /// 系统类型
        /// </summary>
        public string SystemType { get; set; }
        /// <summary>
        /// 资源类型
        /// </summary>
        public ResourceType? ResourceType { get; set; }
    }
}
