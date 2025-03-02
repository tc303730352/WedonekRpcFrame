namespace RpcStore.RemoteModel.Error.Model
{
    public class ErrorData
    {
        /// <summary>
        /// 错误Id
        /// </summary>
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 错误码
        /// </summary>
        public string ErrorCode
        {
            get;
            set;
        }
        /// <summary>
        /// 中文
        /// </summary>
        public string Zh
        {
            get;
            set;
        }
        /// <summary>
        /// 英文
        /// </summary>
        public string En
        {
            get;
            set;
        }
    }
}
