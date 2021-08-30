using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

using RpcHelper.Config;
namespace SqlExecHelper
{
        public enum SqlExecType
        {
                全部 = 254,
                close = 0,
                delete = 4,
                drop = 8,
                insert = 16,
                SqlBulkCopy = 32,
                query = 64,
                update = 128,
                proc = 2
        }
        #region "数据库执行类"
        public class MyDAL : IDAL
        {
                static MyDAL()
                {
                        string con = LocalConfig.Local["database:defSqlCon"];
                        if (con != null)
                        {
                                connectionString = _FormatConStr(con);
                        }
                }
                //静态只读的连接字符串。
                public static readonly string connectionString = null;
                //SQL数据库的连接实例。
                private readonly SqlConnection objCon;
                //判断当前执行的是否是一个事务。
                private volatile bool isTrans = false;
                //事务的实例。
                private SqlTransaction objTrans;

                private static SqlExecType _AuditRange = SqlExecType.close;

                public event BeginTranEvent TranBegin;
                public event EndTranEvent TranEnd;

                public string ConName { get; }

                /// <summary>
                /// 默认的构造方法，初始化数据库连接对象。
                /// </summary>
                public MyDAL()
                {
                        this.ConName = "defSqlCon";
                        this.objCon = new SqlConnection(connectionString);
                }

                public static void SetAuditRange(SqlExecType set)
                {
                        _AuditRange = set;
                }
                private static string _FormatConStr(string conStr)
                {
                        int index = conStr.IndexOf("\"");
                        if (index != -1)
                        {
                                index += 1;
                                return conStr[index..conStr.IndexOf("\"", index)];
                        }
                        return conStr;
                }
                public MyDAL(string conName)
                {
                        string set = LocalConfig.Local[string.Concat("database:", conName)];
                        this.ConName = conName;
                        if (set != null)
                        {
                                string con = _FormatConStr(set);
                                this.objCon = new SqlConnection(con);
                        }
                }
                /// <summary>
                /// 获取连接的数据库名
                /// </summary>
                /// <returns></returns>
                public string GetDataBase()
                {
                        return this.objCon.Database;
                }
                #region "事务"
                /// <summary>
                /// 根据现有连接,开始一个事务
                /// </summary>
                public IDAL BeginTrans()
                {
                        if (!this.isTrans)
                        {
                                this.objCon.Open();
                                this.objTrans = this.objCon.BeginTransaction();
                                this.isTrans = true;
                                if (this.TranBegin != null)
                                {
                                        this.TranBegin(this);
                                }
                        }
                        return this;
                }

                /// <summary>
                /// 根据现有连接,提交事务
                /// </summary>
                public void CommitTrans()
                {
                        if (this.isTrans)
                        {
                                this.objTrans.Commit();
                                this.isTrans = false;
                                this.objCon.Close();
                                if (this.TranEnd != null)
                                {
                                        this.TranEnd(this);
                                }
                        }
                }

                /// <summary>
                /// 根据现有连接,回滚事务
                /// </summary>
                public void RollbackTrans()
                {
                        if (this.isTrans)
                        {
                                this.objTrans.Rollback();
                                this.isTrans = false;
                                this.objCon.Close();
                                if (this.TranEnd != null)
                                {
                                        this.TranEnd(this);
                                }
                        }
                }
                #endregion;

                #region "执行一条SQL语句，返回结果集(SqlDataReader)"

                /// <summary>
                /// 根据指定的连接和参数执行SQL语句或存储过程返回(SqlDataReader)
                /// </summary>
                /// <param name="sql">sql语句或存储过程名</param>
                /// <param name="parameter">参数</param>
                /// <returns>返回SqlDataReader</returns>
                public bool ExecuteReader(string sql, out SqlDataReader reader, params IDataParameter[] parameter)
                {
                        SqlCommand objCom = new SqlCommand(sql, this.objCon);
                        this.InitParameter(ref objCom, parameter);
                        try
                        {
                                if (!this.isTrans)
                                {
                                        this.objCon.Open();
                                }
                                else
                                {
                                        objCom.Transaction = this.objTrans;
                                }
                                reader = objCom.ExecuteReader(CommandBehavior.CloseConnection);
                                return true;
                        }
                        catch (Exception e)
                        {
                                if (this.objCon.State == ConnectionState.Connecting)
                                {
                                        this.objCon.Close();
                                }
                                _SaveSqlError(sql, parameter, e);
                                reader = null;
                                return false;
                        }
                }

                #endregion;

                #region "执行一条返回影响行数的SQL语句"


                /// <summary>
                /// 根据传入的连接和参数执行一条SQL语句或存储过程,返回受影响的行数
                /// </summary>
                /// <param name="sql">sql语句或存储过程名</param>
                /// <param name="param">参数信息</param>
                /// <returns>受影响的行数</returns>
                public int ExecuteNonQuery(string sql, params IDataParameter[] param)
                {
                        SqlCommand objCom = new SqlCommand(sql, this.objCon);
                        this.InitParameter(ref objCom, param);
                        try
                        {
                                if (this.isTrans)
                                {
                                        objCom.Transaction = this.objTrans;
                                }
                                else
                                {
                                        this.objCon.Open();
                                }
                                int res = objCom.ExecuteNonQuery();
                                _SaveSqlLog(sql, param, res);
                                return res;
                        }
                        catch (Exception e)
                        {
                                _SaveSqlError(sql, param, e);
                                return -2;
                        }
                        finally
                        {
                                if (!this.isTrans)
                                {
                                        this.objCon.Close();
                                }
                        }
                }
                #endregion;


                private void InitParameter(ref SqlCommand objCom, IDataParameter[] parameter)
                {
                        if (parameter == null || parameter.Length == 0)
                        {
                                return;
                        }
                        objCom.CommandType = this.GetCommandType(objCom.CommandText);
                        foreach (IDataParameter param in parameter)
                        {
                                objCom.Parameters.Add(param);
                        }
                }
                private CommandType GetCommandType(string sql)
                {
                        if (sql.Trim().IndexOf(" ") == -1)
                        {
                                return CommandType.StoredProcedure;
                        }
                        string temp = sql.Trim().Substring(0, sql.IndexOf(" ")).ToLower();
                        return temp != "exec" ? CommandType.Text : CommandType.StoredProcedure;
                }


                #region "返回一列数据"

                /// <summary>
                /// 根据传入的连接和参数执行一条SQL语句或存储过程,返回第一行第一列的数据
                /// </summary>
                /// <param name="con">传入的连接</param>
                /// <param name="sql">sql语句或存储过程名</param>
                /// <param name="parameter">参数</param>
                /// <returns>第一行第一列的数据</returns>
                public bool ExecuteScalar(string sql, out object res, params IDataParameter[] parameter)
                {
                        SqlCommand objCom = new SqlCommand(sql, this.objCon);
                        this.InitParameter(ref objCom, parameter);
                        try
                        {
                                if (!this.isTrans)
                                {
                                        this.objCon.Open();
                                }
                                else
                                {
                                        objCom.Transaction = this.objTrans;
                                }
                                res = objCom.ExecuteScalar();
                                _SaveSqlLog(sql, parameter, res);
                                return true;
                        }
                        catch (Exception e)
                        {
                                _SaveSqlError(sql, parameter, e);
                                res = null;
                                return false;
                        }
                        finally
                        {
                                if (!this.isTrans)
                                {
                                        this.objCon.Close();
                                }
                        }
                }

                #endregion

                public bool BackUpDatabase(string name, string path, string show)
                {
                        string sql = string.Format("backup database {0} to disk='{1}' with format, name='{2}'", name, path, show);
                        return this.ExecuteNonQuery(sql) != -2;
                }


                #region "返回DataRow"
                /// <summary>
                /// 根据现有的连接和参数执行SQL语句或存储过程，返回一行数据
                /// </summary>
                /// <param name="sql">SQL语句或存储过程名</param>
                /// <param name="param">行</param>
                /// <param name="parameter">参数</param>
                /// <returns>是否成功</returns>
                public bool GetDataRow(string sql, out DataRow row, params IDataParameter[] param)
                {
                        if (!this.GetDataTable(sql, out DataTable table, param))
                        {
                                row = null;
                                return false;
                        }
                        else if (table != null && table.Rows.Count != 0)
                        {
                                row = table.Rows[0];
                                return true;
                        }
                        else
                        {
                                row = null;
                                return true;
                        }
                }

                #endregion


                #region "返回DataTable"


                public bool GetDataTable(string sql, out DataTable table, params IDataParameter[] parameter)
                {
                        try
                        {
                                SqlCommand objCom = new SqlCommand(sql, this.objCon, this.objTrans);
                                this.InitParameter(ref objCom, parameter);
                                SqlDataAdapter objAdapter = new SqlDataAdapter(objCom);
                                table = new DataTable();
                                if (!this.isTrans)
                                {
                                        this.objCon.Open();
                                }
                                objAdapter.Fill(table);
                                _SaveSqlLog(sql, parameter);
                                return true;
                        }
                        catch (Exception e)
                        {
                                _SaveSqlError(sql, parameter, e);
                                table = null;
                                return false;
                        }
                        finally
                        {
                                if (!this.isTrans)
                                {
                                        this.objCon.Close();
                                }
                        }
                }

                #endregion

                #region "返回DataSet"
                public bool GetDataSet(string sql, out DataSet dataSet, params IDataParameter[] parameter)
                {
                        try
                        {
                                SqlCommand objCom = new SqlCommand(sql, this.objCon, this.objTrans);
                                this.InitParameter(ref objCom, parameter);
                                SqlDataAdapter objAdapter = new SqlDataAdapter(objCom);
                                dataSet = new DataSet();
                                if (!this.isTrans)
                                {
                                        this.objCon.Open();
                                }
                                objAdapter.Fill(dataSet);
                                _SaveSqlLog(sql, parameter);
                                return true;
                        }
                        catch (Exception e)
                        {
                                dataSet = null;
                                _SaveSqlError(sql, parameter, e);
                                return false;
                        }
                        finally
                        {
                                if (!this.isTrans)
                                {
                                        this.objCon.Close();
                                }
                        }
                }
                #endregion
                public bool InsertTable(DataTable table, SqlBulkCopyOptions options = SqlBulkCopyOptions.Default)
                {
                        try
                        {
                                if (!this.isTrans)
                                {
                                        this.objCon.Open();
                                }
                                using (SqlBulkCopy sqlBulk = new SqlBulkCopy(this.objCon, options, this.objTrans))
                                {
                                        sqlBulk.BatchSize = 10000;
                                        sqlBulk.BulkCopyTimeout = 1200;
                                        sqlBulk.DestinationTableName = table.TableName;
                                        sqlBulk.WriteToServer(table);
                                }
                                _SaveSqlLog(table);
                                return true;
                        }
                        catch (Exception e)
                        {
                                _SaveSqlErrorLog(table, e);
                                return false;
                        }
                        finally
                        {
                                if (!this.isTrans)
                                {
                                        this.objCon.Close();
                                }
                        }
                }

                public void Dispose()
                {
                        if (this.isTrans)
                        {
                                this.RollbackTrans();
                        }
                        else if (this.objCon != null)
                        {
                                this.objCon.Dispose();
                        }
                }

                #region SQL日志
                private static SqlExecType _GetSqlExecType(string sql)
                {
                        if (sql.StartsWith("insert"))
                        {
                                return SqlExecType.insert;
                        }
                        else if (sql.StartsWith("update"))
                        {
                                return SqlExecType.update;
                        }
                        else if (sql.StartsWith("select"))
                        {
                                return SqlExecType.query;
                        }
                        else if (sql.StartsWith("delete"))
                        {
                                return SqlExecType.delete;
                        }
                        else if (sql.StartsWith("drop"))
                        {
                                return SqlExecType.drop;
                        }
                        else if (sql.StartsWith("exec"))
                        {
                                return SqlExecType.proc;
                        }
                        else if (sql.StartsWith("with"))
                        {
                                if (sql.LastIndexOf("insert") != -1)
                                {
                                        return SqlExecType.insert;
                                }
                                else if (sql.LastIndexOf("update") != -1)
                                {
                                        return SqlExecType.update;
                                }
                                else if (sql.LastIndexOf("delete") != -1)
                                {
                                        return SqlExecType.delete;
                                }
                                else
                                {
                                        return SqlExecType.query;
                                }
                        }
                        else
                        {
                                return SqlExecType.proc;
                        }
                }
                private static async void _SaveSqlLog(string sql, IDataParameter[] param, object res)
                {
                        if (_AuditRange != SqlExecType.close)
                        {
                                SqlExecType type = _GetSqlExecType(sql);
                                if ((type & _AuditRange) == type)
                                {
                                        await Task.Run(new Action(() =>
                                        {
                                                SqlLogSystem.AddSqlLog(sql, param, type, res);
                                        }));
                                }
                        }
                }
                private static async void _SaveSqlLog(string sql, IDataParameter[] param)
                {
                        if (_AuditRange != SqlExecType.close)
                        {
                                SqlExecType type = _GetSqlExecType(sql);
                                if ((type & _AuditRange) == type)
                                {
                                        await Task.Run(new Action(() =>
                                        {
                                                SqlLogSystem.AddSqlLog(sql, param, type);
                                        }));
                                }
                        }
                }

                private static async void _SaveSqlLog(DataTable table)
                {
                        if ((SqlExecType.SqlBulkCopy & _AuditRange) == SqlExecType.SqlBulkCopy)
                        {
                                await Task.Run(new Action(() =>
                                {
                                        SqlLogSystem.AddSqlLog(table);
                                }));
                        }
                }
                private static async void _SaveSqlErrorLog(DataTable table, Exception e)
                {
                        await Task.Run(new Action(() =>
                        {
                                SqlLogSystem.AddErrorLog(table, e);
                        }));
                }
                private static void _SaveSqlError(string sql, IDataParameter[] param, Exception e)
                {
                        SqlExecType type = _GetSqlExecType(sql);
                        SqlLogSystem.AddErrorLog(sql, param, type, e);
                }

                public void DropTable(string tableName)
                {
                        string sql = string.Concat("drop table " + tableName);
                        this.ExecuteNonQuery(sql, null);
                }
                #endregion
        }
        #endregion;
}
