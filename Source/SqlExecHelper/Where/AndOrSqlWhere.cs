using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SqlExecHelper
{
        public class AndOrSqlWhere : ISqlWhere
        {
                private readonly ISqlWhere[] _Param = null;
                public AndOrSqlWhere(ISqlWhere[] param, bool isAnd = true)
                {
                        this.IsAnd = isAnd;
                        this._Param = param;
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
                        this._Param.ForEach(a =>
                        {
                                a.GenerateSql(group, config, param);
                        });
                        group.Remove(0, 3);
                        sql.Append(group.ToString());
                        sql.Append(") ");
                }
        }
}
