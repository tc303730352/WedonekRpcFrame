using WeDonekRpc.Model;
using RpcStore.RemoteModel;

namespace RpcStore.Model.SysConfig
{
    public class SysConfigBasic
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
        /// 系统类目ID
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
        /// 是否是基础配置项
        /// </summary>
        public bool IsBasicConfig
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
    }
}
