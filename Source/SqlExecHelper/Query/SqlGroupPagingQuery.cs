using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SqlExecHelper.Query
{
        internal class SqlGroupPagingQuery : SqlPagingQuery
        {
                private readonly ISqlBasic _GroupSql = null;

                public SqlGroupPagingQuery(SqlGroupQuery group, int index, int size, string orderBy, bool isHide) : base("t", index, size, group.GroupBy, orderBy, isHide)
                {
                        this._GroupSql = group;
                }


                protected override void _InitTableName(StringBuilder sql, List<IDataParameter> param)
                {
                        StringBuilder table = this._GroupSql.GenerateSql(out IDataParameter[] t);
                        param.AddRange(t);
                        sql.Append("(");
                        sql.Append(table.ToString());
                        sql.Append(") as t ");
                }
        }
}
