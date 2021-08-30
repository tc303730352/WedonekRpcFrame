using RpcHelper.Validate;

using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Gateway.Model
{
        public class SysConfigSet
        {
                /// <summary>
                /// 配置Id
                /// </summary>
                [NullValidate("rpc.config.id.null")]
                public long Id
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
