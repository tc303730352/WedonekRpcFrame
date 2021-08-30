using RpcHelper.Validate;

namespace Wedonek.RpcStore.Service.Model
{
        public class ErrorAddDatum
        {
                /// <summary>
                /// 错误码
                /// </summary>
                [NullValidate("rpc.error.code.null")]
                [LenValidate("rpc.error.code.len", 2, 50)]
                [FormatValidate("rpc.error.code.error", ValidateFormat.字母点)]
                public string ErrorCode
                {
                        get;
                        set;
                }
                /// <summary>
                /// 语言
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
