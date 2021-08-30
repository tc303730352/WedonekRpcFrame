using System.Collections.Generic;
using System.Data;
using System.Text;

using SqlExecHelper.Cache;
using SqlExecHelper.Column;
using SqlExecHelper.Interface;

namespace SqlExecHelper.Join
{
        internal class SqlJoinQuery : ISqlBasic
        {
                private readonly JoinTable _Main = null;
                private readonly JoinTable[] _Table = null;
                public SqlJoinQuery(string table, RunParam param, JoinTable[] tables, ClassStructure column, params ISqlWhere[] where)
                {
                        this._Main = new JoinTable(table, "a", param);
                        this._Table = tables;
                        this.Column = column.GetQueryColumn(this._Main, tables);
                        this._Where = where;
                }
                public SqlJoinQuery(string table, RunParam param, JoinTable[] tables, JoinColumn column, params ISqlWhere[] where)
                {
                        this._Main = new JoinTable(table, "a", param);
                        this._Table = tables;
                        this.Column = column.GetQueryColumn(this._Main, tables);
                        this._Where = where;
                }
                public SqlJoinQuery(string table, RunParam param, JoinTable[] tables, ClassStructure column, IOrderBy[] orderBy, params ISqlWhere[] where) : this(table, param, tables, column, where)
                {
                        this.OrderBy = orderBy;
                }
                public SqlJoinQuery(string table, RunParam param, JoinTable[] tables, JoinColumn column, IOrderBy[] orderBy, params ISqlWhere[] where) : this(table, param, tables, column, where)
                {
                        this.OrderBy = orderBy;
                }

                private readonly ISqlWhere[] _Where = null;

                public IOrderBy[] OrderBy
                {
                        get;
                }

                public SqlColumn[] Column { get; }

                public string TableName { get; }

                internal JoinTable Main => this._Main;

                internal JoinTable[] Table => this._Table;

                protected virtual void _InitWhere(StringBuilder sql, List<IDataParameter> param)
                {
                        if (this._Where != null && this._Where.Length > 0)
                        {
                                SqlTools.InitWhere(sql, this._Where, this._Main, this._Table, param);
                        }
                }
                protected virtual void _InitHead(StringBuilder sql)
                {

                }
                protected virtual void _InitColumn(StringBuilder sql)
                {
                        SqlTools.InitColumn(sql, this.Column);
                }
                protected virtual void _InitOrderBy(StringBuilder sql)
                {
                        if (this.OrderBy != null)
                        {
                                SqlTools.InitOrderBy(sql, this.OrderBy, this._Main, this._Table);
                        }
                }
                protected virtual void _InitFoot(StringBuilder sql, List<IDataParameter> param)
                {
                        this._InitOrderBy(sql);
                }
                protected virtual void _InitTableName(StringBuilder sql)
                {
                        this._Main.InitTable(sql);
                        this._Table.ForEach(a =>
                        {
                                sql.Append(",");
                                a.InitTable(sql);
                        });
                }
                public virtual StringBuilder GenerateSql(out IDataParameter[] param)
                {
                        StringBuilder sql = new StringBuilder(512);
                        sql.Append("select ");
                        this._InitHead(sql);
                        this._InitColumn(sql);
                        sql.Append(" from ");
                        this._InitTableName(sql);
                        List<IDataParameter> list = new List<IDataParameter>();
                        this._InitWhere(sql, list);
                        this._InitFoot(sql, list);
                        param = list.ToArray();
                        return sql;
                }
        }
}
