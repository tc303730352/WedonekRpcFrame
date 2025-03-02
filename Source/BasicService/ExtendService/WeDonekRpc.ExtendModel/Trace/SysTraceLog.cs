using System;
using System.Collections.Generic;

namespace WeDonekRpc.ExtendModel.Trace
{
    public class SysTraceLog
    {
        /// <summary>
        /// 指令
        /// </summary>
        public string Dictate { get; set; }
        /// <summary>
        ///指令类型
        /// </summary>
        public StageType StageType { get; set; }
        /// <summary>
        /// 执行时间
        /// </summary>
        public int Duration { get; set; }
        /// <summary>
        /// 远程服务名
        /// </summary>
        public string Show { get; set; }
        public string TraceId { get; set; }
        public long SpanId { get; set; }
        /// <summary>
        /// 远端服务ID(接收或返回的)
        /// </summary>
        public long? RemoteId { get; set; }
        public DateTime Begin { get; set; }
        public Dictionary<string, string> Args { get; set; }
        public long? ParentId { get; set; }
    }
}
