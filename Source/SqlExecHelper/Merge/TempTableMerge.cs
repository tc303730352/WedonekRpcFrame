using System.Collections.Generic;
using System.Data;
using System.Text;

using SqlExecHelper.Cache;
using SqlExecHelper.Column;
using SqlExecHelper.TempTable;

namespace SqlExecHelper.Merge
{
        internal class TempTableMerge : BatchTempTable, ISqlBasic, IBatchMerge
        {
                public TempTableMerge(string table, IDAL myDAL) : base(table, myDAL, "t")
                {

                }

                private SqlUpdateColumn[] _ReturnCol = null;

                private ISqlWhere[] _Where = null;

                private short _MergeType = 2;
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
                        StringBuilder sql = new StringBuilder(512);
                        sql.AppendFormat("merge into {0} using {1} on ", this.FullTableName, this._TempTable.TableName);
                        SqlTools.InitWhereColumn(this._TempTable, sql, this.Config);
                        param = null;
                        if ((4 & this._MergeType) == 4)
                        {
                                if (this._Where.IsNull())
                                {
                                        sql.Append(" when matched then update set ");
                                }
                                else
                                {
                                        List<IDataParameter> list = new List<IDataParameter>(this._Where.Length);
                                        sql.AppendFormat(" when matched and {0} then update set ", SqlTools.GetWhere(this._Where, this.Config, this._TempTable.Config, list));
                                        param = list.ToArray();
                                }
                                SqlTools.InitSetColumn(this._TempTable, sql, this.Config);
                        }
                        if ((2 & this._MergeType) == 2)
                        {
                                sql.AppendFormat(" when not matched then insert({0}) values(", SqlTools.GetInsertColumn(this._TempTable));
                                SqlTools.InitInsertColumn(this._TempTable, sql);
                                sql.Append(")");
                        }
                        this._InitOutput(sql);
                        sql.Append(";");
                        return sql;
                }

                public bool Insert()
                {
                        if (!this._TempTable.Save())
                        {
                                return false;
                        }
                        this._MergeType = 2;
                        return SqlHelper.ExecuteNonQuery(this, this._MyDAL) != -2;
                }
                public bool Insert<T>(out T[] datas)
                {
                        if (!this._TempTable.Save())
                        {
                                datas = null;
                                return false;
                        }
                        this._MergeType = 2;
                        this._ReturnCol = ClassStructureCache.GetReturnColumn(typeof(T));
                        return SqlHelper.GetTable(this, this._MyDAL, out datas);
                }
                public bool Insert<T>(string column, out T[] datas)
                {
                        if (!this._TempTable.Save())
                        {
                                datas = null;
                                return false;
                        }
                        this._MergeType = 2;
                        this._ReturnCol = new SqlUpdateColumn[]
                        {
                                new SqlUpdateColumn(column,SqlEventPrefix.inserted)
                        };
                        return SqlHelper.GetTable(this, this._MyDAL, out datas);
                }
                public bool InsertOrUpdate(params ISqlWhere[] where)
                {
                        if (!this._TempTable.Save())
                        {
                                return false;
                        }
                        this._Where = where;
                        this._MergeType = 6;
                        return SqlHelper.ExecuteNonQuery(this, this._MyDAL) != -2;
                }
                public bool InsertOrUpdate<T>(out T[] datas, params ISqlWhere[] where)
                {
                        if (!this._TempTable.Save())
                        {
                                datas = null;
                                return false;
                        }
                        this._MergeType = 6;
                        this._Where = where;
                        this._ReturnCol = ClassStructureCache.GetReturnColumn(typeof(T));
                        return SqlHelper.GetTable(this, this._MyDAL, out datas);
                }
                public bool InsertOrUpdate<T>(SqlEventPrefix prefix, out T[] datas, params ISqlWhere[] where)
                {
                        if (!this._TempTable.Save())
                        {
                                datas = null;
                                return false;
                        }
                        this._MergeType = 6;
                        this._Where = where;
                        this._ReturnCol = ClassStructureCache.GetReturnColumn(typeof(T), prefix);
                        return SqlHelper.GetTable(this, this._MyDAL, out datas);
                }
                public bool InsertOrUpdate<T>(string column, SqlEventPrefix prefix, out T[] datas, params ISqlWhere[] where)
                {
                        if (!this._TempTable.Save())
                        {
                                datas = null;
                                return false;
                        }
                        this._Where = where;
                        this._MergeType = 6;
                        this._ReturnCol = new SqlUpdateColumn[]
                        {
                                new SqlUpdateColumn(column,prefix)
                        };
                        return SqlHelper.GetTable(this, this._MyDAL, out datas);
                }
                public bool Update(params ISqlWhere[] where)
                {
                        if (!this._TempTable.Save())
                        {
                                return false;
                        }
                        this._Where = where;
                        this._MergeType = 4;
                        return SqlHelper.ExecuteNonQuery(this, this._MyDAL) > 0;
                }
                public bool Update<T>(out T[] datas, params ISqlWhere[] where)
                {
                        if (!this._TempTable.Save())
                        {
                                datas = null;
                                return false;
                        }
                        this._Where = where;
                        this._MergeType = 4;
                        this._ReturnCol = ClassStructureCache.GetReturnColumn(typeof(T));
                        return SqlHelper.GetTable(this, this._MyDAL, out datas);
                }
        }
}
