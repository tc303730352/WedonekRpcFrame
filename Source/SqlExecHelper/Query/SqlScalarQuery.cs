using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SqlExecHelper.Query
{
        internal class SqlScalarQuery : ISqlBasic
        {
                public SqlScalarQuery(string table, RunParam param, string column, params ISqlWhere[] where)
                {
                        this.TableName = table;
                        this._Name = SqlTools.GetTableName(table, param);
                        this.Name = column;
                        this.Where = where;
                        this.Config = new Config.SqlBasicConfig(table);
                }
                public SqlScalarQuery(string table, RunParam param, string column, string func, params ISqlWhere[] where) : this(table, param, column, where)
                {
                        this._ColumnFunc = func;
                }
                public SqlScalarQuery(string table, RunParam param, string column, string orderBy, string func, params ISqlWhere[] where) : this(table, param, column, func, where)
                {
                        this.OrderBy = orderBy;
                }
                private readonly string _Name = null;
                public ISqlRunConfig Config
                {
                        get;
                }
                /// <summary>
                /// 表明
                /// </summary>
                public string TableName
                {
                        get;
                }

                /// <summary>
                /// 查询参数
                /// </summary>
                public ISqlWhere[] Where
                {
                        get;
                }
                public string Name
                {
                        get;
                }
                private readonly string _ColumnFunc = null;
                public string OrderBy
                {
                        get;
                        set;
                }
                protected virtual void _InitTableName(StringBuilder sql)
                {
                        sql.Append(this._Name);
                }
                protected virtual void _InitWhere(StringBuilder sql, List<IDataParameter> param)
                {
                        if (this.Where != null && this.Where.Length > 0)
                        {
                                SqlTools.InitWhere(sql, this.Where, this.Config, param);
                        }
                }

                protected virtual void _InitOrderBy(StringBuilder sql)
                {
                        SqlTools.InitOrderBy(sql, this.OrderBy, this.Config);
                }
                public virtual StringBuilder GenerateSql(out IDataParameter[] param)
                {
                        StringBuilder sql = new StringBuilder(256);
                        sql.Append("select ");
                        if (this._ColumnFunc == null)
                        {
                                sql.Append(this.Name);
                        }
                        else if (this.Name == "*")
                        {
                                sql.Append(string.Format("{0}(*)", this._ColumnFunc));
                        }
                        else
                        {
                                sql.Append(string.Format("{0}({1}) as {2}", this._ColumnFunc, this.Name, this.Name));
                        }
                        sql.Append(" from ");
                        this._InitTableName(sql);
                        List<IDataParameter> list = new List<IDataParameter>();
                        this._InitWhere(sql, list);
                        this._InitOrderBy(sql);
                        param = list.ToArray();
                        return sql;
                }


        }
}
