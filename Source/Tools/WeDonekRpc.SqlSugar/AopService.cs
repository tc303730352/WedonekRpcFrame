using System.Data;
using WeDonekRpc.Helper;
using SqlSugar;
using WeDonekRpc.SqlSugar.Config;
using WeDonekRpc.Helper.Log;

namespace WeDonekRpc.SqlSugar
{
    internal class AopService : AopEvents
    {
        private readonly string _Title = "Sql执行日志!";
        private readonly ISugarConfig _Config;
        public AopService (ISugarConfig config)
        {
            this._Config = config;
            base.OnError = new Action<SqlSugarException>(this._ErrorLog);
            base.OnLogExecuting = new Action<string, SugarParameter[]>(this._sqlExecuting);
            base.OnExecutingChangeSql = new Func<string, SugarParameter[], KeyValuePair<string, SugarParameter[]>>(this._ExecutingChange);
        }

        private KeyValuePair<string, SugarParameter[]> _ExecutingChange (string sql, SugarParameter[] param)
        {
            if (this._Config.StringIsAutoEmpty && !param.IsNull())
            {
                param.ForEach(c =>
                {
                    if (( c.DbType == System.Data.DbType.String || c.DbType == System.Data.DbType.Object ) && c.Value == null && c.Direction != ParameterDirection.Output)
                    {
                        c.Value = string.Empty;
                    }

                });
            }
            return new KeyValuePair<string, SugarParameter[]>(sql, param);
        }

        private void _sqlExecuting (string sql, SugarParameter[] param)
        {
            if (this._Config.CheckIsRecord("Log", out LogConfig config))
            {
                LogInfo log = new LogInfo(this._Title, config.FormatGroup(sql), config.LogGrade)
                {
                    IsLocal = this._Config.IsLocalLog
                };
                log.Add("sql", sql);
                log.Add("params", param.FormatString());
                log.Save();
            }
        }

        private void _ErrorLog (SqlSugarException e)
        {
            e.WriteErrorLog(this._Config);
        }
    }
}
