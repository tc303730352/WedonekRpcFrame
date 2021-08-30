using System.Collections.Generic;
using System.Data;
using System.Text;

using SqlExecHelper.Cache;

namespace SqlExecHelper.Query
{
        internal class SqlGroupQuery : SqlQuery
        {
                public SqlGroupQuery(string table, RunParam param, string[] groups, ClassStructure column, string orderBy, params ISqlWhere[] where) : base(table, param, column, orderBy, where)
                {
                        this.GroupBy = groups;
                }
                public SqlGroupQuery(string table, RunParam param, string group, params ISqlWhere[] where) : base(table, param, group, where)
                {
                        this.GroupBy = new string[] {
                                group
                        };
                }
                public string[] GroupBy
                {
                        get;
                }
                public ISqlWhere[] Having
                {
                        get;
                        set;
                }
                protected override void _InitFoot(StringBuilder sql, List<IDataParameter> param)
                {
                        SqlTools.InitGroup(sql, this.OrderBy, this.GroupBy, this.Having, this.Config, param);
                }

        }
}
