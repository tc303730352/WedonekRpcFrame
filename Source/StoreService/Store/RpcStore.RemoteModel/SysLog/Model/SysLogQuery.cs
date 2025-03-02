using WeDonekRpc.Helper;

namespace RpcStore.RemoteModel.SysLog.Model
{
    public class SysLogQuery
    {
        /// <summary>
        /// 集群Id
        /// </summary>
        public long? RpcMerId { get; set; }
        /// <summary>
        /// 查询Key
        /// </summary>
        public string QueryKey { get; set; }

        /// <summary>
        /// 服务类型ID
        /// </summary>
        public string SystemType { get; set; }
        /// <summary>
        /// 日志组
        /// </summary>
        public string LogGroup { get; set; }
        /// <summary>
        /// 日志类型
        /// </summary>
        public LogType? LogType { get; set; }
        /// <summary>
        /// 日志等级
        /// </summary>
        public LogGrade? LogGrade { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? Begin { get; set; }


        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? End { get; set; }
    }
}
