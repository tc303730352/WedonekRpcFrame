using System.Collections.Generic;
using System.Data;
using System.Text;

using SqlExecHelper.Cache;
using SqlExecHelper.Column;
using SqlExecHelper.TempTable;

namespace SqlExecHelper.Update
{
        internal class TempTableUpdate : BatchTempTable, ISqlBasic, IBatchUpdate
        {
                public TempTableUpdate(string table, RunParam param, IDAL myDAL) : base(table, param, null, myDAL)
                {

                }
                private ISqlSetColumn[] _SetCoumn = null;
                private SqlUpdateColumn[] _ReturnCol = null;
                private ISqlWhere[] _Where = null;
                public StringBuilder GenerateSql(out IDataParameter[] param)
                {
                        this._InitColumn();
                        StringBuilder sql = new StringBuilder(128);
                        sql.AppendFormat("update {0} set ", this.FullTableName);
                        List<IDataParameter> adds = new List<IDataParameter>();
                        SqlTools.InitSetColumn(this._TempTable, sql, this.Config, this._SetCoumn, adds);
                        this._InitOutput(sql);
                        sql.AppendFormat(" from {0} where ", this._TempTable.TableName);
                        SqlTools.InitWhereColumn(this._TempTable, sql, this.Config);
                        if (!this._Where.IsNull())
                        {
                                SqlTools.AppendWhere(sql, this.Config, this._Where, adds);
                        }
                        param = adds.ToArray();
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

                public bool Update(params ISqlWhere[] where)
                {
                        if (!this._TempTable.Save())
                        {
                                return false;
                        }
                        this._Where = where;
                        return SqlHelper.ExecuteNonQuery(this, this._MyDAL) != -2;
                }
                public bool Update(out int rowNum, params ISqlWhere[] where)
                {
                        if (!this._TempTable.Save())
                        {
                                rowNum = 0;
                                return false;
                        }
                        this._Where = where;
                        rowNum = SqlHelper.ExecuteNonQuery(this, this._MyDAL);
                        return rowNum != -2;
                }

                public bool Update<T>(out T[] datas, params ISqlWhere[] where)
                {
                        if (!this._TempTable.Save())
                        {
                                datas = null;
                                return false;
                        }
                        this._Where = where;
                        this._ReturnCol = ClassStructureCache.GetReturnColumn(typeof(T));
                        return SqlHelper.GetTable(this, this._MyDAL, out datas);
                }

                public bool Update<T>(out T[] datas, SqlEventPrefix prefix, params ISqlWhere[] where)
                {
                        if (!this._TempTable.Save())
                        {
                                datas = null;
                                return false;
                        }
                        this._Where = where;
                        this._ReturnCol = ClassStructureCache.GetReturnColumn(typeof(T), prefix);
                        return SqlHelper.GetTable(this, this._MyDAL, out datas);
                }

                public bool Update<T>(string column, out T[] datas, params ISqlWhere[] where)
                {
                        if (!this._TempTable.Save())
                        {
                                datas = null;
                                return false;
                        }
                        this._Where = where;
                        this._ReturnCol = new SqlUpdateColumn[]
                        {
                                new SqlUpdateColumn(column, SqlEventPrefix.deleted)
                        };
                        return SqlHelper.GetTable(this, this._MyDAL, out datas);
                }

                public bool Update<T>(string column, SqlEventPrefix prefix, out T[] datas, params ISqlWhere[] where)
                {
                        if (!this._TempTable.Save())
                        {
                                datas = null;
                                return false;
                        }
                        this._Where = where;
                        this._ReturnCol = new SqlUpdateColumn[]
                        {
                                new SqlUpdateColumn(column, prefix)
                        };
                        return SqlHelper.GetTable(this, this._MyDAL, out datas);
                }

                public bool Update(ISqlSetColumn[] columns, out int rowNum, params ISqlWhere[] where)
                {
                        if (!this._TempTable.Save())
                        {
                                rowNum = -2;
                                return false;
                        }
                        this._SetCoumn = columns;
                        this._Where = where;
                        rowNum = SqlHelper.ExecuteNonQuery(this, this._MyDAL);
                        return rowNum != -2;
                }

                public bool Update(ISqlSetColumn[] columns, params ISqlWhere[] where)
                {
                        if (!this._TempTable.Save())
                        {
                                return false;
                        }
                        this._SetCoumn = columns;
                        this._Where = where;
                        return SqlHelper.ExecuteNonQuery(this, this._MyDAL) != -2;
                }

                public bool Update<Result>(string column, SqlEventPrefix prefix, out Result[] results, ISqlSetColumn[] columns, params ISqlWhere[] where)
                {
                        if (!this._TempTable.Save())
                        {
                                results = null;
                                return false;
                        }
                        this._SetCoumn = columns;
                        this._Where = where;
                        this._ReturnCol = new SqlUpdateColumn[]
                       {
                        new SqlUpdateColumn(column, prefix)
                       };
                        return SqlHelper.GetTable(this, this._MyDAL, out results);
                }
        }
}
