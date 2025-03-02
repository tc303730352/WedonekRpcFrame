using WeDonekRpc.Helper.Validate;
using WeDonekRpc.Model;

namespace RpcStore.RemoteModel.SysConfig.Model
{
    public class BasicConfigSet
    {
        /// <summary>
        /// 集群Id
        /// </summary>
        [NumValidate("rpc.store.config.id.error", 1)]
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
        /// 配置摸版
        /// </summary>
        public string TemplateKey
        {
            get;
            set;
        }
    }
}
