using RpcHelper.Validate;

namespace Wedonek.RpcStore.Service.Model
{
        /// <summary>
        /// 服务组
        /// </summary>
        public class ServerGroupDatum
        {
                /// <summary>
                /// 组类目值
                /// </summary>
                [NullValidate("rpc.group.type.null")]
                [LenValidate("rpc.group.type.len", 2, 50)]
                [FormatValidate("rpc.group.type.error", ValidateFormat.字母点)]
                public string TypeVal
                {
                        get;
                        set;
                }
                /// <summary>
                /// 组名
                /// </summary>
                [NullValidate("rpc.group.name.null")]
                [LenValidate("rpc.group.name.len", 2, 50)]
                public string GroupName
                {
                        get;
                        set;
                }
        }
}
