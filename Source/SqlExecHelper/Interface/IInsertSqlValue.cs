namespace SqlExecHelper.SqlValue
{
        public interface IInsertSqlValue : ISqlColumnVal
        {
                string ColumnName { get; }
        }
}