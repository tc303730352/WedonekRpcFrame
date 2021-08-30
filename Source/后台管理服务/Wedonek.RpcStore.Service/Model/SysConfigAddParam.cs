using RpcHelper.Validate;

namespace Wedonek.RpcStore.Service.Model
{
        /// <summary>
        /// 配置信息
        /// </summary>
        public class SysConfigAddParam
        {
                /// <summary>
                /// 集群Id
                /// </summary>
                [NumValidate("rpc.config.id.error", 0)]
                public long RpcMerId
                {
                        get;
                        set;
                }
                /// <summary>
                /// 服务节点Id
                /// </summary>
                [NumValidate("rpc.server.id.error", 0)]
                public long ServerId
                {
                        get;
                        set;
                }
                /// <summary>
                /// 系统类目ID
                /// </summary>
                [NumValidate("rpc.server.type.id.error", 0)]
                public long SystemTypeId
                {
                        get;
                        set;
                }
                /// <summary>
                /// 配置名
                /// </summary>
                [NullValidate("rpc.config.name.null")]
                [LenValidate("rpc.config.name.len", 2, 50)]
                public string Name
                {
                        get;
                        set;
                }
                /// <summary>
                /// 值类型
                /// </summary>
                [EnumValidate("rpc.config.valueType.error", typeof(SysConfigValueType))]
                public SysConfigValueType ValueType
                {
                        get;
                        set;
                }
                /// <summary>
                /// 值
                /// </summary>
                [NullValidate("rpc.config.value.null")]
                [LenValidate("rpc.config.value.error", 0, 8000)]
                public string Value
                {
                        get;
                        set;
                }
        }
}
