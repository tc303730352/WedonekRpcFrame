namespace WeDonekRpc.Helper.Log
{
    public class TraceLog : LogInfo
    {
        private const string _DefGroup = "Def";
        public TraceLog(string title, string group) : base(title, string.Empty, group, LogGrade.Trace)
        {
        }
        public TraceLog(string title) : base(title, _DefGroup, LogGrade.Trace)
        {
        }
        public TraceLog(string title, string content, string group) : base(title, content, group, LogGrade.Trace)
        {
        }
    }
}
