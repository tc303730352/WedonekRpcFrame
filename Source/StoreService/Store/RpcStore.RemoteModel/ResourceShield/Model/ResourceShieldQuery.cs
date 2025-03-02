using WeDonekRpc.Model;

namespace RpcStore.RemoteModel.ResourceShield.Model
{
    /// <summary>
    /// 查询参数
    /// </summary>
    public class ResourceShieldQuery
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
        /// 资源ID
        /// </summary>
        public long? ResourceId
        {
            get;
            set;
        }
        /// <summary>
        /// 系统类别
        /// </summary>
        public string SystemType
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
        /// 资源路径
        /// </summary>
        public string Path
        {
            get;
            set;
        }
        /// <summary>
        /// 是否已过期
        /// </summary>
        public bool? IsOverTime
        {
            get;
            set;
        }
        public ShieldType? ShieldType
        {
            get;
            set;
        }
    }
}
