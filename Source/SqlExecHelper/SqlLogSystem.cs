using System;
using System.Data;
using System.Text;

using RpcHelper;
namespace SqlExecHelper
{

        public class LocalLogSystem : ISqlLogSystem
        {
                private readonly string _GroupName = "Sql_Server";

                public LocalLogSystem()
                {

                }

                private static string _Format(IDataParameter[] param)
                {
                        StringBuilder str = new StringBuilder();
                        param.ForEach(a =>
                        {
                                str.AppendLine("__________________________");
                                str.AppendFormat("Name:{0}\r\nDirection:{1}\r\nValue:{2}\r\nDataType:{3}\r\n", a.ParameterName, a.Direction.ToString(), a.Value, a.DbType.ToString());
                        });
                        return str.ToString();
                }
                public virtual void AddSqlLog(string sql, IDataParameter[] param, SqlExecType type, object res)
                {
                        new DebugLog("SQL执行日志", this._GroupName)
                        {
                                { "Sql",sql},
                                { "SqlType",type.ToString()},
                                { "Param",_Format(param)},
                                { "Result",res}
                        }.Save();
                }
                public virtual void AddSqlLog(string sql, IDataParameter[] param, SqlExecType type)
                {
                        new DebugLog("SQL执行日志", this._GroupName)
                        {
                                { "Sql",sql},
                                { "SqlType",type.ToString()},
                                { "Param",_Format(param)}
                        }.Save();
                }
                public virtual void AddErrorLog(string sql, IDataParameter[] param, SqlExecType type, Exception e)
                {
                        new ErrorLog(e, "SQL错误日志", this._GroupName)
                        {
                                { "Sql",sql},
                                { "Param",_Format(param)},
                                { "SqlType",type.ToString()}
                        }.Save();
                }
                public void AddErrorLog(DataTable table, Exception e)
                {
                        new ErrorLog(e, "SQL批量插入错误", this._GroupName)
                        {
                                { "TableName",table.TableName},
                                { "Rows",table.Rows.Count},
                                { "Columns",table.Columns.Count},
                                { "Datas",table.Rows.ToJson()}
                        }.Save();
                }
                public void AddSqlLog(DataTable table)
                {
                        new DebugLog("SQL批量插入日志", this._GroupName)
                        {
                                { "TableName",table.TableName},
                                { "Rows",table.Rows.Count},
                                { "Columns",table.Columns.Count},
                                { "Datas",table.Rows.ToJson()}
                        }.Save();
                }
        }
        public class SqlLogSystem
        {
                private static ISqlLogSystem _Log = new LocalLogSystem();
                public static void SetLog(ISqlLogSystem log)
                {
                        _Log = log;
                }

                #region "消息日志"

                public static void AddSqlLog(string sql, IDataParameter[] param, SqlExecType type, object res)
                {
                        _Log.AddSqlLog(sql, param, type, res);
                }
                public static void AddSqlLog(DataTable table)
                {
                        _Log.AddSqlLog(table);
                }
                public static void AddSqlLog(string sql, IDataParameter[] param, SqlExecType type)
                {
                        _Log.AddSqlLog(sql, param, type);
                }

                #endregion

                #region 错误日志

                public static void AddErrorLog(DataTable table, Exception e)
                {
                        _Log.AddErrorLog(table, e);
                }
                public static void AddErrorLog(string sql, IDataParameter[] param, SqlExecType type, Exception e)
                {
                        _Log.AddErrorLog(sql, param, type, e);
                }
                #endregion

        }
}
