using System.Collections.Generic;
using System.Data;
using System.Text;

using SqlExecHelper.Cache;
using SqlExecHelper.Column;
using SqlExecHelper.Config;

namespace SqlExecHelper.Drop
{
        internal class BatchDrop : ISqlBasic, IBatchDrop
        {
                private readonly IDAL _MyDAL = null;

                private readonly Batch.BatchSql _BatchExcel = null;
                public BatchDrop(string table, RunParam param, IDAL myDAL)
                {
                        this._MyDAL = myDAL;
                        this._BatchExcel = new Batch.BatchSql();
                        this.TableName = table;
                        this.Config = new SqlBatchConfig(table, table);
                        this._Name = SqlTools.GetTableName(table, param);
                }
                protected readonly string _Name = "";
                public ISqlRunConfig Config { get; }

                public string TableName { get; }

                public SqlTableColumn[] Column
                {
                        get => this._BatchExcel.Column;
                        set => this._BatchExcel.Column = value;
                }

                public int RowCount => this._BatchExcel.RowCount;

                private ISqlWhere[] _Where = null;

                private BasicSqlColumn[] _ReturnCol = null;

                public int Drop(params ISqlWhere[] where)
                {
                        this._Where = where;
                        return SqlHelper.ExecuteNonQuery(this, this._MyDAL);
                }
                public bool Drop<T>(out T[] datas, params ISqlWhere[] where)
                {
                        this._Where = where;
                        this._ReturnCol = ClassStructureCache.GetBasicColumn(typeof(T));
                        return SqlHelper.GetTable(this, this._MyDAL, out datas);
                }
                public bool Drop<T>(string column, out T[] datas, params ISqlWhere[] where)
                {
                        this._Where = where;
                        this._ReturnCol = new BasicSqlColumn[]
                        {
                                new BasicSqlColumn(column)
                        };
                        return SqlHelper.GetTable(this, this._MyDAL, out datas);
                }
                private void _InitOutput(StringBuilder sql)
                {
                        if (this._ReturnCol.IsNull())
                        {
                                return;
                        }
                        sql.Append(" output ");
                        this._ReturnCol.ForEach(a =>
                        {
                                if (a.AliasName != null)
                                {
                                        sql.AppendFormat("deleted.{0} as {1},", a.Name, a.AliasName);
                                }
                                else
                                {
                                        sql.AppendFormat("deleted.{0},", a.Name);
                                }
                        });
                        sql.Remove(sql.Length - 1, 1);
                }
                public StringBuilder GenerateSql(out IDataParameter[] param)
                {
                        StringBuilder sql = this._BatchExcel.GenerateSql(out param);
                        sql.AppendFormat("delete from {0}", this._Name);
                        this._InitOutput(sql);
                        sql.AppendFormat(" from {0} where ", this._BatchExcel.TableName);
                        SqlTools.InitColumn(this._BatchExcel, sql, this.Config);
                        if (!this._Where.IsNull())
                        {
                                List<IDataParameter> p = new List<IDataParameter>();
                                sql.Append(" and ");
                                sql.Append(SqlTools.GetWhere(this._Where, this.Config, p));
                                param = param.Join(p);
                        }
                        return sql;
                }

                public void AddRow(object[] datas)
                {
                        this._BatchExcel.AddRow(datas);
                }

                public void Dispose()
                {

                }
        }
}
