using System;
using System.Data;

namespace SqlExecHelper
{
        public interface ISqlLogSystem
        {
                #region "消息日志"

                void AddSqlLog(string sql, IDataParameter[] param, SqlExecType type, object res);

                void AddSqlLog(string sql, IDataParameter[] param, SqlExecType type);
                void AddSqlLog(DataTable table);
                #endregion

                #region 错误日志
                void AddErrorLog(string sql, IDataParameter[] param, SqlExecType type, Exception e);
                void AddErrorLog(DataTable table, Exception e);
                #endregion
        }
}
