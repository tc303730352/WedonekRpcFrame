using System.Collections.Generic;
using System.Data;
using System.Text;

using SqlExecHelper.Config;
using SqlExecHelper.Param;

namespace SqlExecHelper.Batch
{
        public class BatchSql : ISqlBasic, IBatchSql
        {
                public BatchSql()
                {
                        this.TableName = "temp";
                        this.Config = new SqlBatchConfig("temp", "temp");
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


                public int RowCount => this._Rows.Count;
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
                        List<IDataParameter> list = new List<IDataParameter>(this._Rows.Count * this.Column.Length);
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
                        param = list.ToArray();
                        return sql;
                }

        }
}
