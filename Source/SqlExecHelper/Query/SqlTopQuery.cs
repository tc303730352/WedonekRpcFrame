using System.Text;

using SqlExecHelper.Cache;

namespace SqlExecHelper.Query
{
        internal class SqlTopQuery : SqlQuery
        {
                public SqlTopQuery(string table, RunParam param, int topNum, ClassStructure column, string orderBy, params ISqlWhere[] where) : base(table, param, column, orderBy, where)
                {
                        this.TopNum = topNum;
                }
                public SqlTopQuery(string table, RunParam param, int topNum, params ISqlWhere[] where) : base(table, param, where)
                {
                        this.TopNum = topNum;
                }
                public SqlTopQuery(string table, RunParam param, int topNum, string column, string orderBy, params ISqlWhere[] where) : base(table, param, column, orderBy, where)
                {
                        this.TopNum = topNum;
                }
                public int TopNum
                {
                        get;
                }
                protected override void _InitHead(StringBuilder sql)
                {
                        SqlTools.InitHead(sql, this.TopNum);
                }
        }
}
