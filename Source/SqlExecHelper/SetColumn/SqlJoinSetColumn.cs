using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SqlExecHelper.SetColumn
{
        public class SqlJoinSetColumn : ISqlJoinSetColumn
        {
                public SqlJoinSetColumn(string colName, SqlSetType type)
                {
                        this._SetType = type;
                        this._Column = colName;
                        this._JoinColumn = colName;
                }
                public SqlJoinSetColumn(string colName, string joinColumn, SqlSetType type)
                {
                        this._SetType = type;
                        this._Column = colName;
                        this._JoinColumn = joinColumn;
                }
                public SqlJoinSetColumn(string colName)
                {
                        this._Column = colName;
                        this._JoinColumn = colName;
                }
                public SqlJoinSetColumn(string colName, string joinColumn)
                {
                        this._Column = colName;
                        this._JoinColumn = joinColumn;
                }

                private readonly string _Column = null;
                private readonly string _JoinColumn = null;
                private readonly SqlSetType _SetType = SqlSetType.等于;
                public void GenerateSql(StringBuilder sql, ISqlRunConfig main, ISqlRunConfig join)
                {
                        string one = main.FormatName(this._Column);
                        string two = join.FormatName(this._JoinColumn);
                        if (this._SetType == SqlSetType.等于)
                        {
                                sql.AppendFormat("{0}={1},", one, two);
                        }
                        else if (this._SetType == SqlSetType.递加)
                        {
                                sql.AppendFormat("{0}={0}+{1},", one, two);
                        }
                        else
                        {
                                sql.AppendFormat("{0}={0}-{1},", one, two);
                        }
                }

                public void GenerateSql(StringBuilder sql, ISqlRunConfig config, List<IDataParameter> param)
                {

                }
        }
}
