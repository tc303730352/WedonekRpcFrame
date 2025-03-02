using System;
using WeDonekRpc.Helper;
using WeDonekRpc.Modular.SysEvent.Model;
using WeDonekRpc.ModularModel;
using WeDonekRpc.ModularModel.SysEvent.Model;

namespace WeDonekRpc.HttpApiGateway.SysEvent.Model
{
    internal class HttpTimeEvent : BasicEvent
    {
        public HttpTimeEvent ( ServiceSysEvent obj ) : base(obj)
        {
            RequestDurationConfig config = obj.EventConfig.Json<RequestDurationConfig>();
            this.Threshold = config.Threshold;
            this.IsIgnoreUpFile = config.IsIgnoreUpFile;
            this.IgnoreApi = config.IgnoreApi ?? Array.Empty<string>();
            this.Method = config.Method;
            this.RecordRange = config.RecordRange;
        }

        /// <summary>
        /// 发送超时阈值
        /// </summary>
        public uint Threshold
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
        /// 是否忽略上传文件的接口
        /// </summary>
        public bool IsIgnoreUpFile
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