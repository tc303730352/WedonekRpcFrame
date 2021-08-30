using RpcHelper.Validate;

namespace Wedonek.RpcStore.Gateway.Model
{
        /// <summary>
        /// 设置错误信息
        /// </summary>
        internal class SetErrorParam
        {
                /// <summary>
                /// 错误Id
                /// </summary>
                [NumValidate("rpc.error.Id.error", 1)]
                public long ErrorId
                {
                        get;
                        set;
                }
                /// <summary>
                /// 错误信息语言
                /// </summary>
                [NullValidate("rpc.error.lang.null")]
                [LenValidate("rpc.error.lang.len", 2, 20)]
                [FormatValidate("rpc.error.lang.error", ValidateFormat.纯字母)]
                public string Lang
                {
                        get;
                        set;
                }
                /// <summary>
                /// 错误友好提示
                /// </summary>
                [NullValidate("rpc.error.msg.null")]
                [LenValidate("rpc.error.msg.len", 2, 100)]
                public string Msg
                {
                        get;
                        set;
                }
        }
}
