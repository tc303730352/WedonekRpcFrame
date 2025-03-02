using WeDonekRpc.Model;

namespace RpcStore.RemoteModel.SysConfig.Model
{
    /// <summary>
    /// 配置资料
    /// </summary>
    public class SysConfigDatum
    {
        public long Id { get; set; }
        /// <summary>
        /// 集群Id
        /// </summary>
        public long RpcMerId
        {
            get;
            set;
        }
        /// <summary>
        /// 服务类型
        /// </summary>
        public RpcServerType ServiceType
        {
            get;
            set;
        }
        /// <summary>
        /// 服务节点Id
        /// </summary>
        public long ServerId
        {
            get;
            set;
        }
        /// <summary>
        /// 机房ID
        /// </summary>
        public int RegionId
        {
            get;
            set;
        }
        /// <summary>
        /// 容器组
        /// </summary>
        public long ContainerGroup
        {
            get;
            set;
        }
        /// <summary>
        /// 客户端版本号
        /// </summary>
        public int VerNum { get; set; }
        /// <summary>
        /// 系统类目
        /// </summary>
        public string SystemType
        {
            get;
            set;
        }
        /// <summary>
        /// 配置名
        /// </summary>
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 值类型
        /// </summary>
        public SysConfigValueType ValueType
        {
            get;
            set;
        }
        /// <summary>
        /// 值
        /// </summary>
        public string Value
        {
            get;
            set;
        }
        public string Show
        {
            get;
            set;
        }
        /// <summary>
        /// 配置权限
        /// </summary>
        public int Prower
        {
            get;
            set;
        }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable
        {
            get;
            set;
        }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime ToUpdateTime
        {
            get;
            set;
        }
        /// <summary>
        /// 配置摸版
        /// </summary>
        public string TemplateKey
        {
            get;
            set;
        }
    }
}
