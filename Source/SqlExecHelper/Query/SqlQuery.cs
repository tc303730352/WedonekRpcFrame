using System.Collections.Generic;
using System.Data;
using System.Text;

using SqlExecHelper.Cache;
using SqlExecHelper.Column;

namespace SqlExecHelper.Query
{
        /// <summary>
        /// 
        /// </summary>
        internal class SqlQuery : ISqlBasic
        {
                public SqlQuery(string table, string[] column, params ISqlWhere[] where)
                {
                        this.TableName = table;
                        this._Name = table;
                        this.Column = column.ConvertAll(a => new SqlColumn(a));
                        this.Where = where;
                        this.Config = new Config.SqlBasicConfig(table);
                }
                public SqlQuery(string table, ClassStructure column, params ISqlWhere[] where)
                {
                        this.TableName = table;
                        this._Name = table;
                        this.Column = column.GetQueryColumn();
                        this.Where = where;
                        this.Config = new Config.SqlBasicConfig(table);
                }
                public SqlQuery(string table, RunParam param, ClassStructure column, params ISqlWhere[] where)
                {
                        this.TableName = table;
                        this._Name = SqlTools.GetTableName(table, param);
                        this.Column = column.GetQueryColumn();
                        this.Where = where;
                        this.Config = new Config.SqlBasicConfig(table);
                }
                public SqlQuery(string table, RunParam param,string prefix, ClassStructure column, params ISqlWhere[] where)
                {
                        this.TableName = table;
                        this._Name = SqlTools.GetTableName(table, param);
                        this.Column = column.GetQueryColumn();
                        this.Where = where;
                        this.Config = new Config.SqlBasicConfig(table,prefix);
                }
                public SqlQuery(string table, RunParam param, params ISqlWhere[] where)
                {
                        this.TableName = table;
                        this._Name = SqlTools.GetTableName(table, param);
                        this.Where = where;
                        this.Config = new Config.SqlBasicConfig(table);
                }
                public SqlQuery(string table, RunParam param, string column, params ISqlWhere[] where)
                {
                        this.TableName = table;
                        this._Name = SqlTools.GetTableName(table, param);
                        this.Column = new SqlColumn[]
                        {
                                new SqlColumn(column)
                        };
                        this.Where = where;
                        this.Config = new Config.SqlBasicConfig(table);
                }
                internal SqlQuery(string table, string prefix, RunParam param, string column, params ISqlWhere[] where)
                {
                        this.TableName = table;
                        this._Name = SqlTools.GetTableName(table, param);
                        this.Column = new SqlColumn[]
                        {
                                new SqlColumn(column)
                        };
                        this.Where = where;
                        this.Config = new Config.SqlBasicConfig(table, prefix);
                }
                public SqlQuery(string table, ClassStructure column, string orderBy, params ISqlWhere[] where) : this(table, column, where)
                {
                        this.OrderBy = orderBy;
                }
                public SqlQuery(string table, string[] column, string orderBy, params ISqlWhere[] where) : this(table, column, where)
                {
                        this.OrderBy = orderBy;
                }
                public SqlQuery(string table, RunParam param, ClassStructure column, string orderBy, params ISqlWhere[] where) : this(table, param, column, where)
                {
                        this.OrderBy = orderBy;
                }
                public SqlQuery(string table, RunParam param, string column, string orderBy, params ISqlWhere[] where) : this(table, param, column, where)
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
                public SqlColumn[] Column
                {
                        get;
                }
                public string OrderBy
                {
                        get;
                        set;
                }

                protected virtual void _InitTableName(StringBuilder sql, List<IDataParameter> param)
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
                protected virtual void _InitHead(StringBuilder sql)
                {

                }
                protected virtual void _InitColumn(StringBuilder sql)
                {
                        if (this.Column == null)
                        {
                                sql.Append("*");
                        }
                        else
                        {
                                SqlTools.InitColumn(sql, this.Column, this.Config);
                        }
                }
                protected virtual void _InitFoot(StringBuilder sql, List<IDataParameter> param)
                {
                        this._InitOrderBy(sql);
                }
                public virtual StringBuilder GenerateSql(out IDataParameter[] param)
                {
                        StringBuilder sql = new StringBuilder(256);
                        sql.Append("select ");
                        this._InitHead(sql);
                        this._InitColumn(sql);
                        sql.Append(" from ");
                        List<IDataParameter> list = new List<IDataParameter>();
                        this._InitTableName(sql, list);
                        this._InitWhere(sql, list);
                        this._InitFoot(sql, list);
                        param = list.ToArray();
                        return sql;
                }
        }
}
