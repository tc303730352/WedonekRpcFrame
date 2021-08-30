namespace SqlExecHelper
{
        public class SqlJoinWhere : SqlLinkWhere, ISqlJoinWhere
        {
                public SqlJoinWhere(string column, string table) : base(column)
                {
                        this.Table = table;
                }
                public SqlJoinWhere(string column, QueryType queryType) : base(column, queryType)
                {

                }
                public SqlJoinWhere(string column, string table, string other) : base(column, other)
                {
                        this.Table = table;
                }
                public SqlJoinWhere(string column) : base(column)
                {
                }
                public string Table { get; set; }
        }
}
