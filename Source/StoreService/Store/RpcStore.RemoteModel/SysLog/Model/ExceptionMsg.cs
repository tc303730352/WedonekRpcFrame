namespace RpcStore.RemoteModel.SysLog.Model
{
    public class ExceptionMsg
    {
        /// <summary>
        /// 获取描述当前异常的消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 获取调用堆栈上的即时框架字符串表示形式
        /// </summary>
        public string StackTrace { get; set; }
        /// <summary>
        /// 获取或设置导致错误的应用程序或对象的名称
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// 获取或设置指向与此异常关联的帮助文件链接
        /// </summary>
        public string HelpLink { get; set; }

        /// <summary>
        /// 获取或设置 HRESULT（一个分配给特定异常的编码数字值）。
        /// </summary>
        public int HResult { get; set; }

        /// <summary>
        /// 应发异常的方法
        /// </summary>
        public Dictionary<string, string> Method { get; set; }

        /// <summary>
        /// 获取提供有关异常的其他用户定义信息的键/值对集合。
        /// </summary>
        public Dictionary<string, string> Data { get; set; }
        /// <summary>
        /// 下级错误
        /// </summary>
        public ExceptionMsg InnerException { get; set; }
    }
}
