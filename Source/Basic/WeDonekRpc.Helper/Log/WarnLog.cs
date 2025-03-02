namespace WeDonekRpc.Helper.Log
{

    /// <summary>
    /// 警告日志
    /// </summary>
    public class WarnLog : LogInfo
    {
        private const string _DefGroup = "Def";
        private const LogGrade _DefLogGrade = LogGrade.WARN;
        public WarnLog ( string error, string title ) : base(error, _DefLogGrade, _DefGroup)
        {
            this.LogTitle = title;
        }
        public WarnLog () : base(_DefGroup, _DefLogGrade)
        {
        }
        public WarnLog ( string error ) : base(error, _DefLogGrade, _DefGroup)
        {
        }
        public WarnLog ( string error, string title, string group ) : base(error, _DefLogGrade, group)
        {
            this.LogTitle = title;
        }
        public WarnLog ( string error, string title, string show, string group ) : base(error, _DefLogGrade, group)
        {
            this.LogTitle = title;
            this.LogContent = show;
        }
    }
}
