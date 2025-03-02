using SqlSugar;

namespace RpcSync.Model.DB
{
    [SugarTable("ServerVisitCensus")]
    public class ServerVisitCensusModel
    {
        [SugarColumn(IsPrimaryKey =true)]
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 服务节点
        /// </summary>
        public long ServiceId
        {
            get;
            set;
        }
        /// <summary>
        /// 指令集
        /// </summary>
        public string Dictate
        {
            get;
            set;
        }
        /// <summary>
        /// 说明
        /// </summary>
        public string Show
        {
            get;
            set;
        }
        /// <summary>
        /// 访问量
        /// </summary>
        public long VisitNum
        {
            get;
            set;
        }
        /// <summary>
        /// 成功数
        /// </summary>
        public long SuccessNum
        {
            get;
            set;
        }
        /// <summary>
        /// 失败数
        /// </summary>
        public long FailNum
        {
            get;
            set;
        }
        /// <summary>
        /// 今日访问数
        /// </summary>
        public long TodayVisit
        {
            get;
            set;
        }
        /// <summary>
        /// 今日失败数
        /// </summary>
        public long TodayFail
        {
            get;
            set;
        }
        /// <summary>
        /// 今日成功数
        /// </summary>
        public long TodaySuccess
        {
            get;
            set;
        }
    }
}
