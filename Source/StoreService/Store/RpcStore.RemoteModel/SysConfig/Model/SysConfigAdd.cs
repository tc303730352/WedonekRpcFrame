using WeDonekRpc.Helper.Validate;
using WeDonekRpc.Model;

namespace RpcStore.RemoteModel.SysConfig.Model
{
    /// <summary>
    /// 配置信息
    /// </summary>
    public class SysConfigAdd
    {
        /// <summary>
        /// 集群Id
        /// </summary>
        [NumValidate("rpc.store.config.id.error", 0)]
        public long RpcMerId
        {
            get;
            set;
        }
        /// <summary>
        /// 服务类型
        /// </summary>
        [EnumValidate("rpc.store.service.type.error")]
        public RpcServerType ServiceType
        {
            get;
            set;
        }
        /// <summary>
        /// 服务节点Id
        /// </summary>
        [NumValidate("rpc.store.server.id.error", 0)]
        public long ServerId
        {
            get;
            set;
        }
        /// <summary>
        /// 机房ID
        /// </summary>
        [NumValidate("rpc.store.region.id.error", 0)]
        public int RegionId
        {
            get;
            set;
        }
        /// <summary>
        /// 容器组
        /// </summary>
        [NumValidate("rpc.store.container.group.id.error", 0)]
        public long ContainerGroup
        {
            get;
            set;
        }
        /// <summary>
        /// 客户端版本号
        /// </summary>
        [NumValidate("rpc.store.ver.num.error", 0)]
        public int VerNum { get; set; }
        /// <summary>
        /// 系统类目ID
        /// </summary>
        [LenValidate("rpc.store.server.type.len", 0, 50)]
        [FormatValidate("rpc.store.server.type.error", ValidateFormat.字母点)]
        public string SystemType
        {
            get;
            set;
        }
        /// <summary>
        /// 配置名
        /// </summary>
        [NullValidate("rpc.store.config.name.null")]
        [LenValidate("rpc.store.config.name.len", 2, 50)]
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 值类型
        /// </summary>
        [EnumValidate("rpc.store.config.valueType.error", typeof(SysConfigValueType))]
        public SysConfigValueType ValueType
        {
            get;
            set;
        }
        /// <summary>
        /// 值
        /// </summary>
        [NullValidate("rpc.store.config.value.null")]
        [LenValidate("rpc.store.config.value.error", 0, 8000)]
        public string Value
        {
            get;
            set;
        }
        /// <summary>
        /// 配置说明
        /// </summary>
        [LenValidate("rpc.store.config.show.len", 0, 50)]
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
        /// 配置类型
        /// </summary>
        [EnumValidate("rpc.store.config.type.error", typeof(RpcConfigType))]
        public RpcConfigType ConfigType { get; set; }

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
