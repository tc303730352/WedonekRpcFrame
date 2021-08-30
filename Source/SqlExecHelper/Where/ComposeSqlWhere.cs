using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SqlExecHelper
{
        /// <summary>
        /// 括号组合查询
        /// </summary>
        public class ComposeSqlWhere : ISqlWhere
        {
                private readonly ISqlWhere[] _Where = null;
                public ComposeSqlWhere(ISqlWhere[] wheres, bool isAnd = true)
                {
                        this.IsAnd = isAnd;
                        this._Where = wheres;
                }
                public bool IsAnd { get; set; } = false;

                public void GenerateSql(StringBuilder sql, ISqlRunConfig config, List<IDataParameter> param)
                {
                        if (this.IsAnd)
                        {
                                sql.Append("and (");
                        }
                        else
                        {
                                sql.Append("or (");
                        }
                        StringBuilder group = new StringBuilder();
                        this._Where.ForEach(a =>
                        {
                                a.GenerateSql(group, config, param);
                        });
                        group.Remove(0, 3);
                        sql.Append(group.ToString());
                        sql.Append(") ");
                }
        }
}
