using SqlExecHelper.Insert;

namespace SqlExecHelper
{
        public interface IInsertTable
        {
                TableColumn[] Column { get; set; }
                string TableName { get; }
                int RowCount { get; }
                void AddRow(params object[] datas);
                bool Save();
        }
}