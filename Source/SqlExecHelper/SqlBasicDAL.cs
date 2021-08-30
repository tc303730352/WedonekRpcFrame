using System.Data;

using SqlExecHelper.Config;
namespace SqlExecHelper
{
        public class SqlBasicDAL
        {
                protected readonly IDAL _MyDAL = null;

                internal ITransaction CurrentTran = null;

                public int RowNum => SqlHelper.RowNum;
                public SqlBasicDAL()
                {
                        this._MyDAL = SqlConfig.SqlFactory.GetDAL();
                }
                public SqlBasicDAL(string conName)
                {
                        this._MyDAL = SqlConfig.SqlFactory.GetDAL(conName);
                }
                #region 事务
                public IDAL BeginTrans()
                {
                        return this._MyDAL.BeginTrans();
                }

                public void CommitTrans()
                {
                        this._MyDAL.CommitTrans();
                }
                public void RollbackTrans()
                {
                        this._MyDAL.RollbackTrans();
                }
                #endregion

                #region 执行SQL

                public IBatchExecSql BatchSql(int row, int col)
                {
                        return SqlExecTool.BatchSql(this._MyDAL, row, col);
                }
                public bool ExecuteNonQuery(string sql, params SqlBasicParameter[] param)
                {
                        return SqlExecTool.ExecuteNonQuery(sql, this._MyDAL, param);
                }

                public bool ExecuteScalar<T>(string sql, out T value, params SqlBasicParameter[] param)
                {
                        return SqlExecTool.ExecuteScalar(sql, this._MyDAL, out value, param);
                }
                public bool GetRow<T>(string sql, out T data, params SqlBasicParameter[] param)
                {
                        return SqlExecTool.GetRow(sql, this._MyDAL, out data, param);
                }
                public bool GetTable<T>(string sql, out T[] datas, params SqlBasicParameter[] param)
                {
                        return SqlExecTool.GetTable(sql, this._MyDAL, out datas, param);
                }
                public bool GetDataSet(string sql, out DataSet dataSet, params SqlBasicParameter[] param)
                {
                        return SqlExecTool.GetDataSet(sql, this._MyDAL, out dataSet, param);
                }
                #endregion
        }
}
