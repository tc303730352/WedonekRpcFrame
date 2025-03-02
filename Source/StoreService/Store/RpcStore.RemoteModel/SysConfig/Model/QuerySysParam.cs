using WeDonekRpc.Model;

namespace RpcStore.RemoteModel.SysConfig.Model
{
    /// <summary>
    /// 查询参数
    /// </summary>
    public class QuerySysParam
    {
        /// <summary>
        /// 集群ID
        /// </summary>
        public long? RpcMerId
        {
            get;
            set;
        }
        /// <summary>
        /// 服务节点ID
        /// </summary>
        public long? ServerId
        {
            get;
            set;
        }
        /// <summary>
        /// 机房ID
        /// </summary>
        public int? RegionId
        {
            get;
            set;
        }
        /// <summary>
        /// 容器组
        /// </summary>
        public long? ContainerGroup
        {
            get;
            set;
        }
        /// <summary>
        /// 客户端版本号
        /// </summary>
        public int? VerNum { get; set; }

        /// <summary>
        /// 系统类别
        /// </summary>
        public string SystemType
        {
            get;
            set;
        }
        /// <summary>
        /// 配置名
        /// </summary>
        public string ConfigName
        {
            get;
            set;
        }
        /// <summary>
        /// 配置类型
        /// </summary>
        public RpcConfigType? ConfigType { get; set; }
    }
}
