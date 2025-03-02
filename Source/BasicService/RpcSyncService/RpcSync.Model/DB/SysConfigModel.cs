using WeDonekRpc.Model;
using SqlSugar;

namespace RpcSync.Model.DB
{
    [SugarTable("SysConfig")]
    public class SysConfigModel
    {

        [SugarColumn(IsPrimaryKey = true)]
        public long Id
        {
            get;
            set;
        }
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
        /// 区域ID
        /// </summary>
        public int RegionId { get; set; }
        /// <summary>
        /// 容器组
        /// </summary>
        public long ContainerGroup { get; set; }
        /// <summary>
        /// 节点版本号
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
        /// 配置类型
        /// </summary>
        public RpcConfigType ConfigType { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable
        {
            get;
            set;
        }
        public DateTime ToUpdateTime
        {
            get;
            set;
        }
    }
}
