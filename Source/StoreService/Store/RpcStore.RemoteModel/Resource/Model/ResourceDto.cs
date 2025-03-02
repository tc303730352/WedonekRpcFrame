namespace RpcStore.RemoteModel.Resource.Model
{
    public class ResourceDto
    {
        /// <summary>
        /// 资源Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 模块Id
        /// </summary>
        public long ModularId { get; set; }

        /// <summary>
        /// 系统类别
        /// </summary>
        public string SystemType { get; set; }
        /// <summary>
        /// 系统类别ID
        /// </summary>
        public long SystemTypeId { get; set; }

        /// <summary>
        /// 集群ID
        /// </summary>
        public long RpcMerId { get; set; }
        /// <summary>
        /// 资源路径
        /// </summary>
        public string ResourcePath { get; set; }
        /// <summary>
        /// 完整路径
        /// </summary>
        public string FullPath { get; set; }
        /// <summary>
        /// 资源说明
        /// </summary>
        public string ResourceShow { get; set; }
        /// <summary>
        /// 资源状态
        /// </summary>
        public ResourceState ResourceState { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public string VerNumStr { get; set; }
        /// <summary>
        /// 资源版本
        /// </summary>
        public string ResourceVer { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime? LastTime { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime { get; set; }
    }
}
