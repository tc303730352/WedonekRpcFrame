using System.Collections.Generic;
using System.Data;
using System.Text;

using SqlExecHelper.Cache;
using SqlExecHelper.Column;
using SqlExecHelper.Config;

namespace SqlExecHelper.Update
{
        internal class BatchUpdate : ISqlBasic, IBatchUpdate
        {
                private readonly IDAL _MyDAL = null;

                private readonly Batch.BatchSql _BatchExcel = null;
                public BatchUpdate(string table, RunParam param, IDAL myDAL)
                {
                        this._MyDAL = myDAL;
                        this._BatchExcel = new Batch.BatchSql();
                        this.TableName = table;
                        this.Config = new SqlBatchConfig(table, table);
                        this._Name = SqlTools.GetTableName(table, param);
                }
                private readonly string _Name = "";
                public ISqlRunConfig Config { get; }

                public string TableName { get; }
                public SqlTableColumn[] Column
                {
                        get => this._BatchExcel.Column;
                        set => this._BatchExcel.Column = value;
                }

                private ISqlWhere[] _Where = null;
                private ISqlSetColumn[] _SetColum = null;

                private SqlUpdateColumn[] _ReturnCol = null;
                public int RowCount => this._BatchExcel.RowCount;
                public bool Update(params ISqlWhere[] where)
                {
                        this._Where = where;
                        return SqlHelper.ExecuteNonQuery(this, this._MyDAL) != -2;
                }
                public bool Update<T>(out T[] datas, params ISqlWhere[] where)
                {
                        this._Where = where;
                        this._ReturnCol = ClassStructureCache.GetReturnColumn(typeof(T));
                        return SqlHelper.GetTable(this, this._MyDAL, out datas);
                }
                public bool Update<T>(out T[] datas, SqlEventPrefix prefix, params ISqlWhere[] where)
                {
                        this._Where = where;
                        this._ReturnCol = ClassStructureCache.GetReturnColumn(typeof(T), prefix);
                        return SqlHelper.GetTable(this, this._MyDAL, out datas);
                }
                public bool Update<T>(string column, out T[] datas, params ISqlWhere[] where)
                {
                        this._Where = where;
                        this._ReturnCol = new SqlUpdateColumn[]
                        {
                                new SqlUpdateColumn(column, SqlEventPrefix.deleted)
                        };
                        return SqlHelper.GetTable(this, this._MyDAL, out datas);
                }
                public bool Update<T>(string column, SqlEventPrefix prefix, out T[] datas, params ISqlWhere[] where)
                {
                        this._Where = where;
                        this._ReturnCol = new SqlUpdateColumn[]
                        {
                                new SqlUpdateColumn(column, prefix)
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
                                sql.Append(a.ToString());
                        });
                        sql.Remove(sql.Length - 1, 1);
                }
                public StringBuilder GenerateSql(out IDataParameter[] param)
                {
                        this._InitColumn();
                        StringBuilder sql = this._BatchExcel.GenerateSql(out param);
                        sql.AppendFormat("update {0} set ", this._Name);
                        List<IDataParameter> adds = new List<IDataParameter>();
                        SqlTools.InitSetColumn(this._BatchExcel, sql, this.Config, this._SetColum, adds);
                        this._InitOutput(sql);
                        sql.AppendFormat(" from {0} where ", this._BatchExcel.TableName);
                        SqlTools.InitWhereColumn(this._BatchExcel, sql, this.Config);
                        if (!this._Where.IsNull())
                        {
                                sql.Append(" and ");
                                sql.Append(SqlTools.GetWhere(this._Where, this.Config, adds));
                        }
                        if (adds.Count > 0)
                        {
                                param = param.Join(adds);
                        }
                        return sql;
                }

                private void _InitColumn()
                {
                        this.Column.ForEach(a =>
                        {
                                if (a.ColType == SqlColType.通用)
                                {
                                        a.ColType = SqlColType.修改;
                                }
                        });
                }

                public void AddRow(params object[] datas)
                {
                        this._BatchExcel.AddRow(datas);
                }

                public void Dispose()
                {

                }

                public bool Update(out int rowNum, params ISqlWhere[] where)
                {
                        this._Where = where;
                        rowNum = SqlHelper.ExecuteNonQuery(this, this._MyDAL);
                        return rowNum != -2;
                }

                public bool Update(ISqlSetColumn[] columns, out int rowNum, params ISqlWhere[] where)
                {
                        this._Where = where;
                        this._SetColum = columns;
                        rowNum = SqlHelper.ExecuteNonQuery(this, this._MyDAL);
                        return rowNum != -2;
                }

                public bool Update(ISqlSetColumn[] columns, params ISqlWhere[] where)
                {
                        this._Where = where;
                        this._SetColum = columns;
                        return SqlHelper.ExecuteNonQuery(this, this._MyDAL) != -2;
                }

                public bool Update<Result>(string column, SqlEventPrefix prefix, out Result[] results, ISqlSetColumn[] columns, params ISqlWhere[] where)
                {
                        this._Where = where;
                        this._ReturnCol = new SqlUpdateColumn[]
                        {
                        new SqlUpdateColumn(column, prefix)
                         };
                        this._SetColum = columns;
                        return SqlHelper.GetTable(this, this._MyDAL, out results);
                }
        }
}
