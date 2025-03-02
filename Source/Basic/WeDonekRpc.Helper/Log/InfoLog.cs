namespace WeDonekRpc.Helper.Log
{
    /// <summary>
    /// 消息日志
    /// </summary>
    public class InfoLog : LogInfo
    {
        private const string _DefGroup = "Def";
        public InfoLog ( string title, string content ) : base(title, content, _DefGroup, LogGrade.Information)
        {
        }
        public InfoLog ( string title ) : base(title, _DefGroup, LogGrade.Information)
        {
        }
        public InfoLog ( string title, string content, string group ) : base(title, content, group, LogGrade.Information)
        {
        }
    }
}
