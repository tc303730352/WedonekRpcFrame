using SqlExecHelper.Interface;

namespace SqlExecHelper
{
        public class SqlOrderBy : IOrderBy
        {
                public SqlOrderBy(string column, bool isDesc)
                {
                        this.Column = column;
                        this.IsDesc = isDesc;
                }
                public SqlOrderBy(string table, string column, bool isDesc)
                {
                        this.Table = table;
                        this.Column = column;
                        this.IsDesc = isDesc;
                }
                public string Table
                {
                        get;
                }

                public string Column
                {
                        get;
                }

                public bool IsDesc
                {
                        get;
                }

                public string GetOrderBy(ISqlRunConfig config)
                {
                        return this.IsDesc ? string.Concat(config.FormatName(this.Column), " desc") : config.FormatName(this.Column);
                }
        }
}
