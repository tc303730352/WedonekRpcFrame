using System.Collections.Generic;
using System.Data;
using System.Text;

using SqlExecHelper.Cache;
using SqlExecHelper.Column;
using SqlExecHelper.TempTable;

namespace SqlExecHelper.Drop
{
        internal class TempTableDrop : BatchTempTable, ISqlBasic, IBatchDrop
        {
                public TempTableDrop(string table, RunParam param, IDAL myDAL) : base(table, param, null, myDAL)
                {

                }

                private BasicSqlColumn[] _ReturnCol = null;

                private ISqlWhere[] _Where = null;

                public int Drop(params ISqlWhere[] where)
                {
                        this._Where = where;
                        return !this._TempTable.Save() ? -2 : SqlHelper.ExecuteNonQuery(this, this._MyDAL);
                }

                public bool Drop<T>(out T[] datas, params ISqlWhere[] where)
                {
                        if (!this._TempTable.Save())
                        {
                                datas = null;
                                return false;
                        }
                        this._Where = where;
                        this._ReturnCol = ClassStructureCache.GetBasicColumn(typeof(T));
                        return SqlHelper.GetTable(this, this._MyDAL, out datas);
                }

                public bool Drop<T>(string column, out T[] datas, params ISqlWhere[] where)
                {
                        if (!this._TempTable.Save())
                        {
                                datas = null;
                                return false;
                        }
                        this._Where = where;
                        this._ReturnCol = new BasicSqlColumn[]
                        {
                                new BasicSqlColumn(column)
                        };
                        return SqlHelper.GetTable(this, this._MyDAL, out datas);
                }

                public StringBuilder GenerateSql(out IDataParameter[] param)
                {
                        StringBuilder sql = new StringBuilder(128);
                        sql.AppendFormat("delete from {0}", this.FullTableName);
                        this._InitOutput(sql);
                        sql.AppendFormat(" from {0} where ", this._TempTable.TableName);
                        SqlTools.InitColumn(this._TempTable, sql, this.Config);
                        if (!this._Where.IsNull())
                        {
                                List<IDataParameter> p = new List<IDataParameter>();
                                sql.Append(" and ");
                                sql.Append(SqlTools.GetWhere(this._Where, this.Config, p));
                                param = p.ToArray();
                        }
                        else
                        {
                                param = null;
                        }
                        return sql;
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

        }
}
