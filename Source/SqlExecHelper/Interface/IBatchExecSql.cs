namespace SqlExecHelper
{
        public interface IBatchExecSql : System.IDisposable
        {
                SqlTableColumn[] Column { get; set; }
                string TableName { get; }

                void AddRow(params object[] datas);
                bool ExecuteNonQuery(string sql, params SqlBasicParameter[] param);

                bool Get<T>(string sql, out T[] datas, params SqlBasicParameter[] param);
        }
}