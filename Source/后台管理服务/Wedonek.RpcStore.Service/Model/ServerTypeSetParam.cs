using RpcModel;

using RpcHelper.Validate;

namespace Wedonek.RpcStore.Service.Model
{
        /// <summary>
        /// 服务类型修改信息
        /// </summary>
        public class ServerTypeSetParam
        {

                /// <summary>
                /// 服务名
                /// </summary>
                [NullValidate("rpc.sys.name.null")]
                [LenValidate("rpc.sys.name.len", 2, 50)]
                [FormatValidate("rpc.sys.name.error", ValidateFormat.中文字母数字)]
                public string SystemName
                {
                        get;
                        set;
                }

                /// <summary>
                /// 负载均衡的方式
                /// </summary>
                [EnumValidate("rpc.balancedType.error", typeof(BalancedType))]
                public BalancedType BalancedType
                {
                        get;
                        set;
                }
        }
}
