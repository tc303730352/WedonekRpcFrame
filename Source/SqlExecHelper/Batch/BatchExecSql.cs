using System.Collections.Generic;
using System.Data;
using System.Text;

using SqlExecHelper.Config;
using SqlExecHelper.Param;

namespace SqlExecHelper.Batch
{
        public class BatchExecSql : ISqlBasic, IBatchExecSql
        {

                private string _Sql = null;

                private SqlBasicParameter[] _Param = null;

                private int _ParamLen = 0;

                private readonly IDAL _MyDAL = null;
                public BatchExecSql(IDAL myDAL)
                {
                        this._MyDAL = myDAL;
                        this.TableName = "temp";
                        this.Config = new SqlBasicConfig("temp");
                }
                public ISqlRunConfig Config
                {
                        get;
                }

                public string TableName
                {
                        get;
                }
                public SqlTableColumn[] Column
                {
                        get;
                        set;
                }
                private readonly List<object[]> _Rows = new List<object[]>();
                public void AddRow(params object[] datas)
                {
                        if (datas.Length != this.Column.Length)
                        {
                                return;
                        }
                        this._Rows.Add(datas);
                }
                public StringBuilder GenerateSql(out IDataParameter[] param)
                {
                        StringBuilder sql = new StringBuilder(256);
                        sql.AppendFormat("with {0} as (select ", this.TableName);
                        List<IDataParameter> list = new List<IDataParameter>((this._Rows.Count * this.Column.Length) + this._ParamLen);
                        int end = this._Rows.Count - 1;
                        for (int i = 0; i <= end; i++)
                        {
                                object[] vals = this._Rows[i];
                                this.Column.ForEach((a, k) =>
                                {
                                        BasicParameter t = new BasicParameter(a.SqlDbType, vals[k], a.Size);
                                        t.InitParam(this.Config);
                                        list.Add(t.GetParameter());
                                        if (i == 0)
                                        {
                                                sql.AppendFormat("{0} as {1},", t.ParamName, a.Name);
                                        }
                                        else
                                        {
                                                sql.Append(t.ParamName);
                                                sql.Append(",");
                                        }
                                });
                                sql.Remove(sql.Length - 1, 1);
                                if (i != end)
                                {
                                        sql.Append(" union all select ");
                                }
                        }
                        sql.Append(") ");
                        if (this._ParamLen > 0)
                        {
                                this._Param.ForEach(a => list.Add(a.GetParameter()));
                        }
                        sql.Append(this._Sql);
                        param = list.ToArray();
                        return sql;
                }
                public bool ExecuteNonQuery(string sql, params SqlBasicParameter[] param)
                {
                        this._Sql = sql;
                        if (!param.IsNull())
                        {
                                this._ParamLen = param.Length;
                                this._Param = param;
                        }

                        return SqlHelper.ExecuteNonQuery(this, this._MyDAL) != -2;
                }

                public void Dispose()
                {

                }

                public bool Get<T>(string sql, out T[] datas, params SqlBasicParameter[] param)
                {
                        this._Sql = sql;
                        if (!param.IsNull())
                        {
                                this._ParamLen = param.Length;
                                this._Param = param;
                        }
                        return SqlHelper.GetTable(this, this._MyDAL, out datas);
                }
        }
}
