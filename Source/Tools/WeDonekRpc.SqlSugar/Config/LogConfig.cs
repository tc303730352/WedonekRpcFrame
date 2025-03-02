using WeDonekRpc.Helper;

namespace WeDonekRpc.SqlSugar.Config
{
    public class LogConfig
    {
        public LogConfig ()
        {
        }
        public LogConfig (string group, LogGrade grade)
        {
            this.LogGroup = group;
            this.LogGrade = grade;
        }
        public string LogGroup { get; set; }

        public LogGrade LogGrade { get; set; }
    }
}
