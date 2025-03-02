using WeDonekRpc.Model;

namespace RpcSync.Model
{
    public class SysConfig
    {
        /// <summary>
        /// 集群ID
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
        /// 所属节点
        /// </summary>
        public long ServerId
        {
            get;
            set;
        }
        /// <summary>
        /// 区域ID
        /// </summary>
        public int RegionId { get; set; }
        /// <summary>
        /// 容器组
        /// </summary>
        public long ContainerGroup { get; set; }

        /// <summary>
        /// Api版本
        /// </summary>
        public int VerNum { get; set; }
        /// <summary>
        /// 配置名
        /// </summary>
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 数据类型
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
        /// <summary>
        /// 权限
        /// </summary>
        public int Prower
        {
            get;
            set;
        }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime ToUpdateTime
        {
            get;
            set;
        }
    }
}
