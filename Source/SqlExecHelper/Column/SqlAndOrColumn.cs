using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SqlExecHelper.Column
{
        public class SqlAndOrColumn : ISqlWhere
        {
                private readonly ISqlWhere[] _Columns = null;
                public SqlAndOrColumn(ISqlWhere[] column)
                {
                        this._Columns = column;
                }
                public bool IsAnd { get; set; } = true;

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
                        StringBuilder str = new StringBuilder();
                        this._Columns.ForEach(a =>
                        {
                                a.GenerateSql(str, config, param);
                        });
                        str.Remove(0, 3);
                        sql.Append(str.ToString());
                        sql.Append(")");
                }

        }
}
