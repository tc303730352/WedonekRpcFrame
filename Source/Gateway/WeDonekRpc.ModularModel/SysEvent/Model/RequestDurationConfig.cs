namespace WeDonekRpc.ModularModel.SysEvent.Model
{
    /// <summary>
    /// Http请求时长配置
    /// </summary>
    public class RequestDurationConfig
    {
        /// <summary>
        /// 发送超时阈值 {"Threshold":100,"IsIgnoreUpFile":true,"RecordRange":2}
        /// </summary>
        public uint Threshold
        {
            get;
            set;
        }
        /// <summary>
        /// 是否忽略上传文件的接口
        /// </summary>
        public bool IsIgnoreUpFile
        {
            get;
            set;
        }
        /// <summary>
        /// 限定的请求谓词
        /// </summary>
        public string[] Method
        {
            get;
            set;
        }
        /// <summary>
        /// 忽略的Api
        /// </summary>
        public string[] IgnoreApi
        {
            get;
            set;
        }
        /// <summary>
        /// 日志记录范围
        /// </summary>
        public LogRecordRange RecordRange
        {
            get;
            set;
        }
    }
}
