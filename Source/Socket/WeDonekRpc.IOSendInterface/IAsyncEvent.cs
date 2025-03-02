namespace WeDonekRpc.IOSendInterface
{
    public class IAsyncEvent
    {
        /// <summary>
        /// 数据内容
        /// </summary>
        public byte[] Content
        {
            get;
            set;
        }
        /// <summary>
        /// 数据类型
        /// </summary>
        public SendType DataType
        {
            get;
            set;
        }
        /// <summary>
        /// 是否错误
        /// </summary>
        public bool IsError
        {
            get;
            set;
        }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string Error
        {
            get;
            set;
        }
        /// <summary>
        /// 附带参数
        /// </summary>
        public object Arg
        {
            get;
            set;
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        public string GetData ()
        {
            return this.DataType == SendType.字符串 ? ToolsHelper.DeserializeStringData(this.Content) : null;
        }

        public T GetData<T> ()
        {
            return ToolsHelper.DeserializeData<T>(this.Content);
        }
    }
}
