namespace RpcStore.RemoteModel.Error.Model
{
    /// <summary>
    /// 错误信息设置
    /// </summary>
    public class ErrorSet
    {
        /// <summary>
        /// 错误ID
        /// </summary>
        public long ErrorId
        {
            get;
            set;
        }
        /// <summary>
        /// 语言
        /// </summary>
        public string Lang { get; set; }
        /// <summary>
        /// 错误描述信息
        /// </summary>
        public string Msg { get; set; }
    }
}
