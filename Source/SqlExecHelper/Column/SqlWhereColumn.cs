using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SqlExecHelper.Column
{
        public class SqlWhereColumn : SqlColumn, ISqlWhere
        {
                public SqlWhereColumn(string name, object value) : this(name, QueryType.等号, value)
                {
                }
                public SqlWhereColumn(string name, QueryType queryType, object value) : this(name, queryType)
                {
                        this.Value = new BasicSqlValue(value);
                }
                public SqlWhereColumn(string name, SqlFuncType func, object value) : this(name, func, QueryType.等号, value)
                {
                }
                public SqlWhereColumn(string name, SqlFuncType func, QueryType queryType, object value) : this(name, func, queryType)
                {
                        this.Value = new BasicSqlValue(value);
                }
                public SqlWhereColumn(string name, string func, QueryType queryType, object value) : this(name, func, queryType)
                {
                        this.Value = new BasicSqlValue(value);
                }
                public SqlWhereColumn(string name, string func, object value) : this(name, func, QueryType.等号, value)
                {
                }
                public SqlWhereColumn(string name, QueryType queryType) : base(name)
                {
                        this.Symbol = SqlToolsHelper.GetSymbol(queryType);
                }
                public SqlWhereColumn(string name, SqlFuncType func, QueryType queryType) : base(name, func)
                {
                        this.Symbol = SqlToolsHelper.GetSymbol(queryType);
                }
                public SqlWhereColumn(string name, string func, QueryType queryType) : base(name, name, func)
                {
                        this.Symbol = SqlToolsHelper.GetSymbol(queryType);
                }
                /// <summary>
                /// 符号
                /// </summary>
                public string Symbol
                {
                        get;
                        protected set;
                }
                public ISqlColumnVal Value
                {
                        get;
                        set;
                }
                public bool IsAnd { get; set; } = true;

                public void GenerateSql(StringBuilder sql, ISqlRunConfig config, List<IDataParameter> param)
                {
                        if (this.IsAnd)
                        {
                                sql.Append("and ");
                        }
                        else
                        {
                                sql.Append("or ");
                        }
                        sql.AppendFormat("{0} {1} {2} ", base.ToString(), this.Symbol, this.Value.GetValue(config, param));
                }
        }
}
