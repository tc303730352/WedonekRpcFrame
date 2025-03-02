using System;

namespace WeDonekRpc.Helper.Log
{
    /// <summary>
    /// 严重错误日志
    /// </summary>
    public class CriticalLog : LogInfo
    {
        private const string _DefGroup = "Def";
        private const LogGrade _DefLogGrade = LogGrade.Critical;
        public CriticalLog ( string error, string title ) : base(error, _DefLogGrade, _DefGroup)
        {
            this.LogTitle = title;
        }
        public CriticalLog ( Exception e, string title ) : base(e, _DefGroup, _DefLogGrade)
        {
            this.LogTitle = title;
        }
        public CriticalLog () : base(_DefGroup, _DefLogGrade)
        {
        }
        public CriticalLog ( string error ) : base(error, _DefLogGrade, _DefGroup)
        {
        }
        public CriticalLog ( string error, string title, string group ) : base(error, _DefLogGrade, group)
        {
            this.LogTitle = title;
        }
        public CriticalLog ( string error, string title, string show, string group ) : base(error, _DefLogGrade, group)
        {
            this.LogTitle = title;
            this.LogContent = show;
        }
    }
}
