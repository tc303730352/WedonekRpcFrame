using WeDonekRpc.Helper;

namespace WeDonekRpc.Client.Model
{
    public class LocalEventArgs
    {
        internal LocalEventArgs (string name, ErrorException error)
        {
            this.Name = name;
            if (error != null)
            {
                this.IsSuccess = false;
                this.Error = error.ErrorCode;
            }
            else
            {
                this.IsSuccess = true;
            }
        }
        /// <summary>
        /// 事件名称
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string Error { get; }

    }
}
