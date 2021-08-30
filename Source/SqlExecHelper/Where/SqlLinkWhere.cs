using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SqlExecHelper
{
        public class SqlLinkWhere : ISqlLinkWhere
        {
                public SqlLinkWhere(string column, QueryType queryType) : this(column)
                {
                        this.Symbol = SqlToolsHelper.GetSymbol(queryType);
                }
                public SqlLinkWhere(string column, string link)
                {
                        this._Column = column;
                        this._LinkColumn = link;
                }
                public SqlLinkWhere(string column)
                {
                        this._Column = column;
                        this._LinkColumn = column;
                }
                private readonly string _LinkColumn = null;

                public string _Column = null;

                public bool IsAnd { get; set; } = true;

                public string Symbol { get; } = "=";

                public void GenerateSql(StringBuilder sql, ISqlRunConfig config, ISqlRunConfig other, List<IDataParameter> param)
                {
                        if (this.IsAnd)
                        {
                                sql.AppendFormat("and {0} {1} {2} ", config.FormatName(this._Column), this.Symbol, other.FormatName(this._LinkColumn));
                        }
                        else
                        {
                                sql.AppendFormat("or {0} {1} {2} ", config.FormatName(this._Column), this.Symbol, other.FormatName(this._LinkColumn));
                        }
                }

                public void GenerateSql(StringBuilder sql, ISqlRunConfig config, List<IDataParameter> param)
                {

                }
        }
}
