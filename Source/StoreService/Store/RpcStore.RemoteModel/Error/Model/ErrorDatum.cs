using WeDonekRpc.Helper.Validate;

namespace RpcStore.RemoteModel.Error.Model
{
    public class ErrorDatum
    {
        /// <summary>
        /// 错误码
        /// </summary>
        [NullValidate("rpc.store.error.code.null")]
        [LenValidate("rpc.store.error.code.len", 2, 50)]
        [FormatValidate("rpc.store.error.code.error", ValidateFormat.字母点)]
        public string ErrorCode
        {
            get;
            set;
        }
        /// <summary>
        /// 友好提示
        /// </summary>
        public Dictionary<string, string> LangMsg
        {
            get;
            set;
        }
    }
}
