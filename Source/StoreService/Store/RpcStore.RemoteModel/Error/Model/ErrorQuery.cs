namespace RpcStore.RemoteModel.Error.Model
{
    /// <summary>
    /// 错误信息查询
    /// </summary>
    public class ErrorQuery
    {
        /// <summary>
        /// 错误码
        /// </summary>
        public string QueryKey
        {
            get;
            set;
        }
        /// <summary>
        /// 是否完善了错误信息
        /// </summary>
        public bool? IsPerfect
        {
            get;
            set;
        }
    }
}
