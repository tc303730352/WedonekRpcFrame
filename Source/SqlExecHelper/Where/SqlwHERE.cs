using System.Data;

namespace SqlExecHelper
{
        public class SqlWhere : BasicSqlWhere
        {
                public SqlWhere(string name, SqlDbType type) : base(name, type, "=")
                {

                }
                public SqlWhere(string name, SqlDbType type, int size) : base(name, type, size, "=")
                {
                }
                public SqlWhere(string name, SqlDbType type, QueryType queryType) : base(name, type, SqlToolsHelper.GetSymbol(queryType))
                {
                }
                public SqlWhere(string name, SqlDbType type, object value, int size) : base(name, type, value, size, "=")
                {
                }
                public SqlWhere(string name, SqlDbType type, object value, int size, QueryType queryType) : base(name, type, value, size, SqlToolsHelper.GetSymbol(queryType))
                {
                }
        }
}
