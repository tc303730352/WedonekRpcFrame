using System.Collections.Generic;
using System.Data;
using System.Text;

using SqlExecHelper.Cache;
using SqlExecHelper.Column;
using SqlExecHelper.Config;

namespace SqlExecHelper.Merge
{
        internal class BatchMerge : ISqlBasic, IBatchMerge
        {
                private readonly IDAL _MyDAL = null;

                private readonly Batch.BatchSql _BatchExcel = null;
                public BatchMerge(string table, IDAL myDAL)
                {
                        this._MyDAL = myDAL;
                        this._BatchExcel = new Batch.BatchSql();
                        this.TableName = table;
                        this.Config = new SqlBatchConfig(table, "t");
                        this._Name = SqlTools.FormatTableName(table, "t");
                }
                private readonly string _Name = "";
                private ISqlWhere[] _Where = null;

                private SqlUpdateColumn[] _ReturnCol = null;
                public ISqlRunConfig Config { get; }

                public string TableName { get; }
                public SqlTableColumn[] Column
                {
                        get => this._BatchExcel.Column;
                        set => this._BatchExcel.Column = value;
                }
                private short _MergeType = 2;

                public int RowCount => this._BatchExcel.RowCount;
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
                        StringBuilder sql = this._BatchExcel.GenerateSql(out param);
                        sql.AppendFormat("merge into {0} using {1} on ", this._Name, this._BatchExcel.TableName);
                        SqlTools.InitWhereColumn(this._BatchExcel, sql, this.Config);
                        if ((4 & this._MergeType) == 4)
                        {
                                if (!this._Where.IsNull())
                                {
                                        List<IDataParameter> list = new List<IDataParameter>(this._Where.Length);
                                        sql.AppendFormat(" when matched and {0} then update set ", SqlTools.GetWhere(this._Where, this.Config, this._BatchExcel.Config, list));
                                        param = param.Add(list);
                                }
                                else
                                {
                                        sql.Append(" when matched then update set ");
                                }
                                SqlTools.InitSetColumn(this._BatchExcel, sql, this.Config);
                        }
                        if ((2 & this._MergeType) == 2)
                        {
                                sql.AppendFormat(" when not matched then insert({0}) values(", SqlTools.GetInsertColumn(this._BatchExcel));
                                SqlTools.InitInsertColumn(this._BatchExcel, sql);
                                sql.Append(")");
                        }
                        this._InitOutput(sql);
                        sql.Append(";");
                        return sql;
                }


                public void AddRow(object[] datas)
                {
                        this._BatchExcel.AddRow(datas);
                }
                public bool Insert()
                {
                        this._MergeType = 2;
                        return SqlHelper.ExecuteNonQuery(this, this._MyDAL) != -2;
                }
                public bool Insert<T>(out T[] datas)
                {
                        this._MergeType = 2;
                        this._ReturnCol = ClassStructureCache.GetReturnColumn(typeof(T));
                        return SqlHelper.GetTable(this, this._MyDAL, out datas);
                }
                public bool Insert<T>(string column, out T[] datas)
                {
                        this._MergeType = 2;
                        this._ReturnCol = new SqlUpdateColumn[]
                        {
                                new SqlUpdateColumn(column,SqlEventPrefix.inserted)
                        };
                        return SqlHelper.GetTable(this, this._MyDAL, out datas);
                }
                public bool InsertOrUpdate(params ISqlWhere[] where)
                {
                        if (this.RowCount == 0)
                        {
                                return false;
                        }
                        this._MergeType = 6;
                        this._Where = where;
                        return SqlHelper.ExecuteNonQuery(this, this._MyDAL) != -2;
                }
                public bool InsertOrUpdate<T>(out T[] datas, params ISqlWhere[] where)
                {
                        this._MergeType = 6;
                        this._Where = where;
                        this._ReturnCol = ClassStructureCache.GetReturnColumn(typeof(T));
                        return SqlHelper.GetTable(this, this._MyDAL, out datas);
                }
                public bool InsertOrUpdate<T>(string column, SqlEventPrefix prefix, out T[] datas, params ISqlWhere[] where)
                {
                        this._MergeType = 6;
                        this._Where = where;
                        this._ReturnCol = new SqlUpdateColumn[]
                        {
                                new SqlUpdateColumn(column,prefix)
                        };
                        return SqlHelper.GetTable(this, this._MyDAL, out datas);
                }
                public bool InsertOrUpdate<T>(SqlEventPrefix prefix, out T[] datas, params ISqlWhere[] where)
                {
                        this._MergeType = 6;
                        this._Where = where;
                        this._ReturnCol = ClassStructureCache.GetReturnColumn(typeof(T), prefix);
                        return SqlHelper.GetTable(this, this._MyDAL, out datas);
                }
                public bool Update(params ISqlWhere[] where)
                {
                        this._MergeType = 4;
                        this._Where = where;
                        return SqlHelper.ExecuteNonQuery(this, this._MyDAL) > 0;
                }
                public bool Update<T>(out T[] datas, params ISqlWhere[] where)
                {
                        this._MergeType = 4;
                        this._Where = where;
                        this._ReturnCol = ClassStructureCache.GetReturnColumn(typeof(T));
                        return SqlHelper.GetTable(this, this._MyDAL, out datas);
                }
                public void Dispose()
                {

                }
        }
}
