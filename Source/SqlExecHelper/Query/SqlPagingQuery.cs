using System.Collections.Generic;
using System.Data;
using System.Text;

using SqlExecHelper.Cache;

namespace SqlExecHelper.Query
{
        internal class SqlPagingQuery : SqlQuery
        {
                public SqlPagingQuery(string table, RunParam param, int index, int size, ClassStructure column, string orderBy, bool isHide, params ISqlWhere[] where) : base(table, param, column, orderBy, where)
                {
                        this._IsHideCount = isHide;
                        this.Size = size;
                        this.Skip = (index - 1) * size;
                }
                public SqlPagingQuery(string table, RunParam param, int index, int size, string column, string orderBy, bool isHide, params ISqlWhere[] where) : base(table, param, column, orderBy, where)
                {
                        this._IsHideCount = isHide;
                        this.Size = size;
                        this.Skip = (index - 1) * size;
                }
                public SqlPagingQuery(string table, int index, int size, ClassStructure column, string orderBy, bool isHide, params ISqlWhere[] where) : base(table, column, orderBy, where)
                {
                        this._IsHideCount = isHide;
                        this.Size = size;
                        this.Skip = (index - 1) * size;
                }
                public SqlPagingQuery(string table, int index, int size, string[] column, string orderBy, bool isHide, params ISqlWhere[] where) : base(table, column, orderBy, where)
                {
                        this._IsHideCount = isHide;
                        this.Size = size;
                        this.Skip = (index - 1) * size;
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
                        StringBuilder body = new StringBuilder(" from ", 64);
                        base._InitTableName(body, param);
                        this._InitWhere(body, param);
                        return body.ToString();
                }
                public override StringBuilder GenerateSql(out IDataParameter[] param)
                {
                        List<IDataParameter> list = new List<IDataParameter>();
                        string main = this._GetSqlBody(list);
                        StringBuilder sql = new StringBuilder("select ", 128);
                        base._InitColumn(sql);
                        sql.Append(main);
                        if (this._IsHideCount)
                        {
                                SqlTools.InitPaging(sql, this.OrderBy, this.Skip, this.Size, this.Config, list);
                        }
                        else
                        {
                                SqlTools.InitPaging(sql, this.OrderBy, this.Skip, this.Size, this.Column[0].Name, this.Config, main, list);
                        }
                        param = list.ToArray();
                        return sql;
                }

        }
}
