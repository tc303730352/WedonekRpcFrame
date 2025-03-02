namespace WeDonekRpc.Helper.Log
{
    /// <summary>
    /// Debug日志
    /// </summary>
    public class DebugLog : LogInfo
    {
        private const string _DefGroup = "Def";
        private const LogGrade _DefLogGrade = LogGrade.DEBUG;
        public DebugLog ( string title, string group ) : base(group, _DefLogGrade)
        {
            this.LogTitle = title;
        }
        public DebugLog ( string title, string show, string group ) : base(title, show, group, _DefLogGrade)
        {
        }
        public DebugLog ( string title ) : base(_DefGroup, _DefLogGrade)
        {
            this.LogTitle = title;
        }
    }
}
