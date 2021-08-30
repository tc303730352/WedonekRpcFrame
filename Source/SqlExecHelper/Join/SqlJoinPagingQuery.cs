using System.Collections.Generic;
using System.Data;
using System.Text;

using SqlExecHelper.Cache;
using SqlExecHelper.Interface;

namespace SqlExecHelper.Join
{
        internal class SqlJoinPagingQuery : SqlJoinQuery
        {
                public SqlJoinPagingQuery(string table, RunParam param, int index, int size, JoinTable[] tables, ClassStructure column, IOrderBy[] orderBy, params ISqlWhere[] where) : base(table, param, tables, column, orderBy, where)
                {
                        this.Size = size;
                        this.Skip = (index - 1) * size;
                }
                public SqlJoinPagingQuery(string table, RunParam param, int index, int size, bool isHide, JoinTable[] tables, ClassStructure column, IOrderBy[] orderBy, params ISqlWhere[] where) : this(table, param, index, size, tables, column, orderBy, where)
                {
                        this._IsHideCount = isHide;
                }
                public SqlJoinPagingQuery(string table, RunParam param, int index, int size, JoinTable[] tables, JoinColumn column, IOrderBy[] orderBy, params ISqlWhere[] where) : base(table, param, tables, column, orderBy, where)
                {
                        this.Size = size;
                        this.Skip = (index - 1) * size;
                }
                public SqlJoinPagingQuery(string table, RunParam param, int index, int size, bool isHide, JoinTable[] tables, JoinColumn column, IOrderBy[] orderBy, params ISqlWhere[] where) : this(table, param, index, size, tables, column, orderBy, where)
                {
                        this._IsHideCount = isHide;
                }
                public int Size
                {
                        get;
                }
                public int Skip
                {
                        get;
                }
                private readonly bool _IsHideCount = false;
                private string _GetSqlBody(List<IDataParameter> param)
                {
                        StringBuilder body = new StringBuilder(" from ", 256);
                        base._InitTableName(body);
                        this._InitWhere(body, param);
                        return body.ToString();
                }
                public override StringBuilder GenerateSql(out IDataParameter[] param)
                {
                        List<IDataParameter> list = new List<IDataParameter>();
                        string main = this._GetSqlBody(list);
                        StringBuilder sql = new StringBuilder("select ", 512);
                        base._InitColumn(sql);
                        sql.Append(main);
                        if (this._IsHideCount)
                        {
                                SqlTools.InitPaging(sql, this.OrderBy, this.Main, this.Table, this.Skip, this.Size, list);
                        }
                        else
                        {
                                SqlTools.InitPaging(sql, this.OrderBy, this.Main, this.Table, this.Column[0].Name, main, this.Skip, this.Size, list);
                        }
                        param = list.ToArray();
                        return sql;
                }
        }
}
