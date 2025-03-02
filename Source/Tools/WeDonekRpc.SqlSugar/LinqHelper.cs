using System.Text;
using WeDonekRpc.Helper;
using SqlSugar;
using WeDonekRpc.SqlSugar.Config;
using WeDonekRpc.Helper.Log;

namespace WeDonekRpc.SqlSugar
{
    internal static class LinqHelper
    {
        private static readonly string _Title = "Sql执行错误!";

        public static string FormatGroup (this LogConfig config, string sql)
        {
            return config.LogGroup + "_" + sql.Substring(0, sql.IndexOf(" "));
        }
        public static void WriteErrorLog (this SqlSugarException e, ISugarConfig config)
        {
            if (!config.CheckIsRecord("Error", out LogConfig set))
            {
                return;
            }
            string group = set.FormatGroup(e.Sql);
            ErrorException ex = ErrorException.FormatError(e, set.LogGrade);
            LogInfo log = new LogInfo(ex, group)
            {
                LogTitle = _Title,
                IsLocal = config.IsLocalLog
            };
            log.Add("StackTrace", e.StackTrace);
            log.Add("sql", e.Sql);
            log.Add("params", _FormatString(e.Parametres));
            log.Save();
        }
        public static string FormatString (this SugarParameter[] param)
        {
            if (param.IsNull())
            {
                return string.Empty;
            }
            StringBuilder str = new StringBuilder();
            param.ForEach(c =>
            {
                _ = str.AppendFormat("{0}={1} {2}({4}) {3}\r\n", c.ParameterName, c.Value, c.DbType, c.Direction, c.Size);
            });
            return str.ToString();
        }
        private static string _FormatString (object parametres)
        {
            if (parametres == null)
            {
                return string.Empty;
            }
            SugarParameter[] param = (SugarParameter[])parametres;
            return param.FormatString();
        }
    }
}
