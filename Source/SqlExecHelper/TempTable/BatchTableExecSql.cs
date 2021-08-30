using System.Data;
using System.Text;

namespace SqlExecHelper.TempTable
{
        public class BatchTableExecSql : IBatchExecSql, ISqlBasic
        {
                private readonly SqlTempTable _TempTable = null;

                private string _Sql = null;

                private SqlBasicParameter[] _Param = null;


                protected readonly IDAL _MyDAL = null;
                public BatchTableExecSql(IDAL myDAL)
                {
                        this._MyDAL = myDAL;
                        this._TempTable = new SqlTempTable(myDAL);
                }
                public SqlTableColumn[] Column
                {
                        get => this._TempTable.Column;
                        set => this._TempTable.Column = value;
                }
                public string TableName => this._TempTable.TableName;

                public void AddRow(params object[] datas)
                {
                        this._TempTable.AddRow(datas);
                }
                public void Dispose()
                {
                        this._TempTable.Dispose();
                }

                public bool ExecuteNonQuery(string sql, params SqlBasicParameter[] param)
                {
                        if (!this._TempTable.Save())
                        {
                                return false;
                        }
                        this._Param = param;
                        this._Sql = sql;
                        return SqlHelper.ExecuteNonQuery(this, this._MyDAL) != -2;
                }

                public StringBuilder GenerateSql(out IDataParameter[] param)
                {
                        param = this._Param.ConvertAll(a => a.GetParameter());
                        return new StringBuilder(this._Sql);
                }

                public bool Get<T>(string sql, out T[] datas, params SqlBasicParameter[] param)
                {
                        if (!this._TempTable.Save())
                        {
                                datas = null;
                                return false;
                        }
                        this._Param = param;
                        this._Sql = sql;
                        return SqlHelper.GetTable(this, this._MyDAL, out datas);
                }
        }
}
