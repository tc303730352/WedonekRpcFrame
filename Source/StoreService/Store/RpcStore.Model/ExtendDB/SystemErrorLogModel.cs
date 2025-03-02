using WeDonekRpc.Helper;
using RpcStore.RemoteModel.SysLog.Model;
using SqlSugar;

namespace RpcStore.Model.ExtendDB
{
    [SugarTable("SystemErrorLog")]
    public class SystemErrorLogModel
    {
        /// <summary>
        /// 日志Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public long Id
        {
            get;
            set;
        }
        public long RpcMerId
        {
            get;
            set;
        }
        /// <summary>
        /// 链路Id
        /// </summary>
        public string TraceId
        {
            get;
            set;
        }

        /// <summary>
        /// 日志标题
        /// </summary>
        public string LogTitle
        {
            get;
            set;
        }

        /// <summary>
        /// 日志说明
        /// </summary>
        public string LogShow
        {
            get;
            set;
        }

        /// <summary>
        /// 所属系统类别ID
        /// </summary>
        public string SystemType
        {
            get;
            set;
        }
        /// <summary>
        /// 服务节点
        /// </summary>
        public long ServerId
        {
            get;
            set;
        }
        /// <summary>
        /// 日志组别
        /// </summary>
        public string LogGroup
        {
            get;
            set;
        }
        /// <summary>
        /// 日志类型
        /// </summary>
        public LogType LogType
        {
            get;
            set;
        }
        /// <summary>
        /// 日志等级
        /// </summary>
        public LogGrade LogGrade
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
        /// 错误信息
        /// </summary>
        [SugarColumn(IsJson = true)]
        public ExceptionMsg Exception
        {
            get;
            set;
        }
        /// <summary>
        /// 属性列表
        /// </summary>
        public string AttrList
        {
            get;
            set;
        }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime
        {
            get;
            set;
        }
    }
}
