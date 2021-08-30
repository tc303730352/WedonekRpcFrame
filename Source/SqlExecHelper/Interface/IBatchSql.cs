namespace SqlExecHelper
{
        public interface IBatchSql
        {
                int RowCount { get; }
                ISqlRunConfig Config { get; }
                SqlTableColumn[] Column { get; set; }
                string TableName { get; }

                void AddRow(params object[] datas);
        }
}